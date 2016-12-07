// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

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
                var result = 0.0;
                double.TryParse(values[index], out result);
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        public static int Integer(List<string> values, int index)
        {
            return IntegerOrDefault(values, index, 0);
        }

        public static int IntegerOrDefault(List<string> values, int index, int defaultValue)
        {
            if (index < values.Count)
            {
                var result = 0;
                int.TryParse(values[index], out result);
                return result;
            }
            else
            {
                return defaultValue;
            }
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
    }
}
