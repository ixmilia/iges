using System.Globalization;

namespace IxMilia.Iges
{
    public static class IgesParser
    {
        public static bool TryParseDouble(string s, out double result)
        {
            return double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
        }

        public static double ParseDoubleOrDefault(string s, double defaultValue)
        {
            if (TryParseDouble(s, out var result))
            {
                return result;
            }

            return defaultValue;
        }

        public static double ParseDoubleStrict(string s)
        {
            return double.Parse(s, NumberStyles.Float, CultureInfo.InvariantCulture);
        }

        public static bool TryParseInt(string s, out int result)
        {
            return int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
        }

        public static int ParseIntOrDefault(string s, int defaultValue)
        {
            if (TryParseInt(s, out var result))
            {
                return result;
            }

            return defaultValue;
        }

        public static int ParseIntStrict(string s, NumberStyles numberStyle = NumberStyles.Integer)
        {
            return int.Parse(s, numberStyle, CultureInfo.InvariantCulture);
        }

        public static uint ParseUIntStrict(string s, NumberStyles numberStyle = NumberStyles.Integer)
        {
            return uint.Parse(s, numberStyle, CultureInfo.InvariantCulture);
        }
    }
}
