using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Gpio;
using Windows.Devices.I2c;

//https://www.hackster.io/graham_chow/mpu-6050-for-windows-iot-d67793
//https://store.invensense.com/ProductDetail/MPU6050-InvenSense-Inc/422200/
namespace Accel
{
    public class MpuSensorEventArgs : EventArgs
    {
        public byte Status { get; set; }
        public float SamplePeriod { get; set; }
        public MpuSensorValue[] Values { get; set; }

        public override string ToString()
        {
            if (Values.Length < 1)
                return "no data";

            var data = Values[0];
            return $"AccelX:{data.AccelerationX} AccelY:{data.AccelerationY} AccelZ:{data.AccelerationZ}";

            // return $"Status: {Status} SamplePeriod: {SamplePeriod} Number of Values: {Values.Length}";
        }
    }

    public partial class MPU6050 : IDisposable
    {
        public event EventHandler<MpuSensorEventArgs> SensorInterruptEvent;

        const byte ADDRESS      = 0x68;
        const byte PWR_MGMT_1   = 0x6B;
        const byte SMPLRT_DIV   = 0x19;
        const byte CONFIG       = 0x1A;
        const byte GYRO_CONFIG  = 0x1B;
        const byte ACCEL_CONFIG = 0x1C;
        const byte FIFO_EN      = 0x23;
        const byte INT_ENABLE   = 0x38;
        const byte INT_STATUS   = 0x3A;
        const byte USER_CTRL    = 0x6A;
        const byte FIFO_COUNT   = 0x72;
        const byte FIFO_R_W     = 0x74;

        const int SensorBytes = 12;

        const Int32 INTERRUPT_PIN = 18;

        GpioController gpioController;
        GpioPin interruptPin;

        I2cDevice mpu6050;

        byte ReadByte(byte regAddr)
        {
            var buffer = new byte[1];
            buffer[0] = regAddr;

            var value = new byte[1];
            mpu6050.WriteRead(buffer, value);

            return value[0];
        }

        byte[] ReadBytes(byte regAddr, int length)
        {
            var values = new byte[length];
            var buffer = new byte[1];
            buffer[0] = regAddr;

            mpu6050.WriteRead(buffer, values);
            return values;
        }

        public ushort ReadWord(byte address)
        {
            var buffer = ReadBytes(FIFO_COUNT, 2);
            return (ushort)(((int)buffer[0] << 8) | (int)buffer[1]);
        }

        void WriteByte(byte regAddr, byte data)
        {
            var buffer = new byte[2];
            buffer[0] = regAddr;
            buffer[1] = data;
            mpu6050.Write(buffer);
        }

        void WriteBytes(byte regAddr, byte[] values)
        {
            var buffer = new byte[1 + values.Length];
            buffer[0] = regAddr;
            Array.Copy(values, 0, buffer, 1, values.Length);
            mpu6050.Write(buffer);
        }

        public async Task Init()
        {
            try
            {
                gpioController = GpioController.GetDefault();

                interruptPin = gpioController.OpenPin(INTERRUPT_PIN);
                interruptPin.Write(GpioPinValue.Low);
                interruptPin.SetDriveMode(GpioPinDriveMode.Input);
                interruptPin.ValueChanged += Interrupt;

                var aqsFilter = I2cDevice.GetDeviceSelector();
                var collection = await DeviceInformation.FindAllAsync(aqsFilter);

                var settings = new I2cConnectionSettings(ADDRESS)
                {
                    BusSpeed = I2cBusSpeed.FastMode, //400Khz
                    SharingMode = I2cSharingMode.Exclusive,
                };
              
                mpu6050 = await I2cDevice.FromIdAsync(collection[0].Id, settings);

                await Task.Delay(3); // wait power up sequence 

                WriteByte(PWR_MGMT_1, 0x80);// reset the device

                await Task.Delay(100);

                WriteByte(PWR_MGMT_1, 0x2);
                WriteByte(USER_CTRL, 0x04); //reset fifo

                WriteByte(PWR_MGMT_1, 1); // clock source = gyro x
                WriteByte(GYRO_CONFIG, 0); // +/- 250 degrees sec
                WriteByte(ACCEL_CONFIG, 0); // +/- 2g

                WriteByte(CONFIG, 1); // 184 Hz, 2ms delay
                WriteByte(SMPLRT_DIV, 19);  // set rate 50Hz
                WriteByte(FIFO_EN, 0x78); // enable accel and gyro to read into fifo
                WriteByte(USER_CTRL, 0x40); // reset and enable fifo
                WriteByte(INT_ENABLE, 0x1);
            }
            catch (Exception ex)
            {
                var error = ex.ToString();
            }
        }

        void Interrupt(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            if (mpu6050 != null)
            {
                int interrupt_status = ReadByte(INT_STATUS);
                if ((interrupt_status & 0x10) != 0)
                {
                    WriteByte(USER_CTRL, 0x44); // reset and enable fifo
                }
                if ((interrupt_status & 0x1) != 0)
                {
                    var ea = new MpuSensorEventArgs()
                    {
                        Status = (byte)interrupt_status,
                        SamplePeriod = 0.02f,
                    };
                    
                    var values = new List<MpuSensorValue>();

                    int count = ReadWord(FIFO_COUNT);

                    while (count >= SensorBytes)
                    {
                        var data = ReadBytes(FIFO_R_W, (byte)SensorBytes);
                        count -= SensorBytes;

                        short xa = (short)(data[0] << 8  | data[1]);
                        short ya = (short)(data[2] << 8  | data[3]);
                        short za = (short)(data[4] << 8  | data[5]);

                        short xg = (short)(data[6] << 8  | data[7]);
                        short yg = (short)(data[8] << 8  | data[9]);
                        short zg = (short)(data[10] << 8 | data[11]);

                        var sensorValue = new MpuSensorValue();
                        sensorValue.AccelerationX = (float)xa / (float)16384;
                        sensorValue.AccelerationY = (float)ya / (float)16384;
                        sensorValue.AccelerationZ = (float)za / (float)16384;
                        sensorValue.GyroX = (float)xg / (float)131;
                        sensorValue.GyroY = (float)yg / (float)131;
                        sensorValue.GyroZ = (float)zg / (float)131;

                        values.Add(sensorValue);
                    }
                    ea.Values = values.ToArray();

                    if (SensorInterruptEvent != null && ea.Values.Length > 0)
                        SensorInterruptEvent(this, ea);
                }
            }
        }

        bool disposed = false; 
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                interruptPin.Dispose();
                if (mpu6050 != null)
                {
                    mpu6050.Dispose();
                    mpu6050 = null;
                }
                disposed = true;
            }
        }

        ~MPU6050()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}