using System;
using System.Collections.Generic;
using System.IO;
using IxMilia.Iges.Entities;

namespace IxMilia.Iges
{
    public class IgesFile
    {
        internal const int MaxDataLength = 72;

        internal const int MaxParameterLength = 64;

        public char FieldDelimiter { get; set; }
        public char RecordDelimiter { get; set; }
        public string Identification { get; set; }
        public string FullFileName { get; set; }
        public string SystemIdentifier { get; set; }
        public string SystemVersion { get; set; }
        public int IntegerSize { get; set; }
        public int SingleSize { get; set; }
        public int DecimalDigits { get; set; }
        public int DoubleMagnitude { get; set; }
        public int DoublePrecision { get; set; }
        public string Identifier { get; set; }
        public double ModelSpaceScale { get; set; }
        public IgesUnits ModelUnits { get; set; }
        public string CustomModelUnits { get; set; }
        public int MaxLineWeightGraduations { get; set; }
        public double MaxLineWeight { get; set; }
        public DateTime TimeStamp { get; set; }
        public double MinimumResolution { get; set; }
        public double MaxCoordinateValue { get; set; }
        public string Author { get; set; }
        public string Organization { get; set; }
        public IgesVersion IgesVersion { get; set; }
        public IgesDraftingStandard DraftingStandard { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string ApplicationProtocol { get; set; }

        public List<IgesEntity> Entities { get; private set; }

        public const char DefaultFieldDelimiter = ',';

        public const char DefaultRecordDelimiter = ';';

        public const char StringSentinelCharacter = 'H';

        public IgesFile()
        {
            FieldDelimiter = DefaultFieldDelimiter;
            RecordDelimiter = DefaultRecordDelimiter;
            IntegerSize = 32;
            SingleSize = 8;
            DecimalDigits = 23;
            DoubleMagnitude = 11;
            DoublePrecision = 52;
            ModelSpaceScale = 1.0;
            ModelUnits = IgesUnits.Inches;
            MaxLineWeight = 1.0;
            TimeStamp = DateTime.Now;
            MinimumResolution = 1.0e-10;
            IgesVersion = Iges.IgesVersion.v5_3;
            DraftingStandard = IgesDraftingStandard.None;

            Entities = new List<IgesEntity>();
        }

#if HAS_FILESYSTEM_ACCESS
        public void Save(string path)
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                Save(stream);
            }
        }
#endif

        public void Save(Stream stream)
        {
            new IgesFileWriter().Write(this, stream);
        }

#if HAS_FILESYSTEM_ACCESS
        public static IgesFile Load(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return Load(stream);
            }
        }
#endif

        public static IgesFile Load(Stream stream)
        {
            return IgesFileReader.Load(stream);
        }
    }
}
