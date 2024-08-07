using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

namespace _TDK
{
    public static class Extensions
    {
        public static string ConvertBytesToMegabytes(this long bytes)
        {
            return $"{(bytes / 1024f) / 1024f:0.##}";
        }


        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        public static float Map(float x, float inMin, float inMax, float outMin, float outMax)
        {
            return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        }

        public static float GetTimeDifferenceToNextDayInSeconds(DateTime startDate)
        {
            return (float)(startDate.Date.AddDays(1) - DateTime.Now).TotalSeconds;
        }
    }
}