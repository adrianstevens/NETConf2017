using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccelerationAndGyro
{
    public class RotationAngles
    {
        public RotationAngles(double pitch, double roll, double yaw)
        {
            Pitch = pitch;
            Roll = roll;
            Yaw = yaw;
        }

        public double gyroXNoiseCorrect { get; set; }
        public double gyroYNoiseCorrect { get; set; }
        public double gyroZNoiseCorrect { get; set; }

        public double Pitch { get; private set; }
        public double Roll { get; private set; }
        public double Yaw { get; private set; }

        public void Update(double pitch, double roll, double yaw)
        {
            Pitch = pitch;
            Roll = roll;
            Yaw = yaw;
        }

        public AccelerationAndGyroModel AccelAndGyro { get; private set; }

        public void UpdateFromGravity(AccelerationAndGyroModel sensors)
        {
            AccelAndGyro = sensors;

            double gravX = sensors.AccelerationX;
            double gravY = sensors.AccelerationY;
            double gravZ = sensors.AccelerationZ;

            double gyroZ = (sensors.GyroZ * sensors.SamplePeriod) - gyroZNoiseCorrect;
            double gyroX = (sensors.GyroX * sensors.SamplePeriod) - gyroXNoiseCorrect;
            double gyroY = (sensors.GyroY * sensors.SamplePeriod) - gyroYNoiseCorrect;

            //roll happens along the Y axis so the Y gravity doesn't change, so we check X and Z changes for roll
            if (!IsSignificantGravity(gravX, gravZ))
            {
                Roll += -1 * gyroY;
                Roll = BoundAngle(Roll);
            }
            else
            {
                Roll = AngleFromGravity(gravX, gravZ);
            }

            //pitch is rotation along the x axis

            if (!IsSignificantGravity(gravY, gravZ))
            {
                Pitch += gyroX;
                Pitch = BoundAngle(Pitch);
            }
            else
            {
                Pitch = BoundAngle(AngleFromGravity(gravY, gravZ));
            }

            //Yaw is buggy so let's keep it simple
            //if (!IsSignificantGravity(gravX, gravY))
            //{
                Yaw += -1 * gyroZ;
                Yaw = BoundAngle(Yaw);
                
            //}
            //else
            //{
            //    Yaw = BoundAngle(AngleFromGravity(gravX, gravY) + 180);
            //}
            
        }

        private double BoundAngle(double angle)
        {
            var newAngle = angle % 360;

            if (newAngle < 0)
            {
                newAngle += 360;
            }
            return newAngle;
        }

        private double radiansToDegrees(double r)
        {
            return r * (180 / Math.PI);
        }

        private bool IsSignificantGravity(double gravA, double gravB)
        {
            var totalGrav = Math.Abs(Math.Sqrt((gravA * gravA) + (gravB * gravB)));

            return 0.8d < totalGrav && totalGrav < 1.1d && Math.Abs(gravA) > 0.05 && Math.Abs(gravB) > 0.05;
        }

        private double AngleFromGravity(double a, double b)
        {
            return radiansToDegrees(Math.Atan2(a, b));
        }
    }
}
