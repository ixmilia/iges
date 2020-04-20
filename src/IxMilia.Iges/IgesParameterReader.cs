using System;
using System.Collections.Generic;

namespace IxMilia.Iges
{
    internal static class IgesParameterReader
    {
        public static double Double(List<string> values, int index)
        {
            return DoubleOrDefault(values, index, 0.0);
        }

        public static double DoubleOrDefault(List<string> values, int index, double defaultValue)
        {
            if (index < values.Count)
            {
                return IgesParser.ParseDoubleOrDefault(values[index], defaultValue);
            }

            return defaultValue;
        }

        public static int Integer(List<string> values, int index)
        {
            return IntegerOrDefault(values, index, 0);
        }

        public static int IntegerOrDefault(List<string> values, int index, int defaultValue)
        {
            if (index < values.Count)
            {
                return IgesParser.ParseIntOrDefault(values[index], defaultValue);
            }

            return defaultValue;
        }

        public static string String(List<string> values, int index)
        {
            return StringOrDefault(values, index, null);
        }

        public static string StringOrDefault(List<string> values, int index, string defaultValue)
        {
            if (index < values.Count)
            {
                return values[index];
            }
            else
            {
                return defaultValue;
            }
        }

        public static bool Boolean(List<string> values, int index)
        {
            return BooleanOrDefault(values, index, false);
        }

        public static bool BooleanOrDefault(List<string> values, int index, bool defaultValue)
        {
            if (index < values.Count)
            {
                return Integer(values, index) != 0;
            }
            else
            {
                return defaultValue;
            }
        }

        public static DateTime DateTime(List<string> values, int index)
        {
            return DateTimeOrDefault(values, index, System.DateTime.MinValue);
        }

        public static DateTime DateTimeOrDefault(List<string> values, int index, DateTime defaultValue)
        {
            if (index < values.Count)
            {
                return IgesFileReader.ParseDateTime(values[index], defaultValue);
            }
            else
            {
                return defaultValue;
            }
        }

        public static IgesPoint Point3(List<string> values, ref int index)
        {
            return PointOrDefault(values, ref index, IgesPoint.Origin);
        }

        public static IgesPoint Point2(List<string> values, ref int index)
        {
            return Point2DOrDefault(values, ref index, IgesPoint.Origin);
        }

        public static IgesPoint PointOrDefault(List<string> values, ref int index, IgesPoint defaultValue)
        {
            var x = DoubleOrDefault(values, index++, defaultValue.X);
            var y = DoubleOrDefault(values, index++, defaultValue.Y);
            var z = DoubleOrDefault(values, index++, defaultValue.Z);
            return new IgesPoint(x, y, z);
        }

        public static IgesPoint Point2DOrDefault(List<string> values, ref int index, IgesPoint defaultValue)
        {
            var x = DoubleOrDefault(values, index++, defaultValue.X);
            var y = DoubleOrDefault(values, index++, defaultValue.Y);
            return new IgesPoint(x, y, 0.0);
        }

        public static IgesVector Vector(List<string> values, ref int index)
        {
            return VectorOrDefault(values, ref index, IgesVector.Zero);
        }

        public static IgesVector VectorOrDefault(List<string> values, ref int index, IgesVector defaultValue)
        {
            var x = DoubleOrDefault(values, index++, defaultValue.X);
            var y = DoubleOrDefault(values, index++, defaultValue.Y);
            var z = DoubleOrDefault(values, index++, defaultValue.Z);
            return new IgesVector(x, y, z);
        }
    }
}
