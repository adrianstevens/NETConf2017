using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace AccelClient.ViewModels
{
    public enum DeviceType
    {
        Mobile,
        Desktop,
        IoT,
        Console,
        Unknown,
    }

    public class DeviceDetailViewModel
    {
        DeviceManager deviceManager;
        DeviceStatus deviceStatus;

        public string Name => deviceStatus.Device;

        public string TypeOfDevice => GetDeviceType(deviceStatus.Device).ToString();

        public DateTime LastSeen => deviceStatus.Timestamp;

        public DateTime FirstSeen { get; private set; }

        public ObservableCollection<DeviceData> Data { get { return _data; } }
        ObservableCollection<DeviceData> _data = new ObservableCollection<DeviceData>();

        public double AvgAcceleration { get; private set; }

        public double MaxAcceleration { get; private set; }


        public DeviceDetailViewModel(DeviceStatus deviceStatus)
        {
            this.deviceStatus = deviceStatus;

            deviceManager = new DeviceManager();

            LoadData(deviceStatus);
        }

        async void LoadData(DeviceStatus deviceStatus)
        {
            var data = await deviceManager.GetData(deviceStatus.Device);

            FirstSeen = data.LastOrDefault().Timestamp;

            var listAccel = data.Select(d => d.Acceleration).ToList();

            MaxAcceleration = listAccel.Max();
            AvgAcceleration = listAccel.Average();

            foreach(var d in data)
            {
                _data.Add(d);
            }
        }

        DeviceType GetDeviceType(string device)
        {
            device = device.ToLower();

            if (device.Contains("things") || device.Contains("iot"))
                return DeviceType.IoT;

            if (device.Contains("android"))
                return DeviceType.Mobile;

            if (device.Contains("uwp"))
                return DeviceType.Desktop;

            if (device.Contains("console"))
                return DeviceType.Console;

            return DeviceType.Unknown;
        }
    }
}
