// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.IO;
using Xunit;

namespace IxMilia.Iges.Test
{
    public class IgesReaderTests
    {
        internal static IgesFile CreateFile(string content)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content.Trim('\r', '\n'));
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            var file = IgesFile.Load(stream);
            return file;
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void GlobalParseTest()
        {
            var file = CreateFile(@"
0123456789012345678901234567890123456789012345678901234567890123456789--S      1
1H,,1H;,10Hidentifier,28HC:\path\to\full\filename.igs,4Habcd,3H1.0,16,7,G      1
22,10,51,6Hident2,0.75,10,,4,0.8,15H20001225.130811,1.0E-003,500,5HBrettG      2
,7HIxMilia,8,4,13H870508.123456,8Hprotocol;                             G      3
S      1G      3D      0P      0                                        T      1
");
            Assert.Equal(',', file.FieldDelimiter);
            Assert.Equal(';', file.RecordDelimiter);
            Assert.Equal("identifier", file.Identification);
            Assert.Equal(@"C:\path\to\full\filename.igs", file.FullFileName);
            Assert.Equal(@"abcd", file.SystemIdentifier);
            Assert.Equal(@"1.0", file.SystemVersion);
            Assert.Equal(16, file.IntegerSize);
            Assert.Equal(7, file.SingleSize);
            Assert.Equal(22, file.DecimalDigits);
            Assert.Equal(10, file.DoubleMagnitude);
            Assert.Equal(51, file.DoublePrecision);
            Assert.Equal("ident2", file.Identifier);
            Assert.Equal(0.75, file.ModelSpaceScale);
            Assert.Equal(IgesUnits.Centimeters, file.ModelUnits);
            Assert.Null(file.CustomModelUnits);
            Assert.Equal(4, file.MaxLineWeightGraduations);
            Assert.Equal(0.8, file.MaxLineWeight);
            Assert.Equal(new DateTime(2000, 12, 25, 13, 08, 11), file.TimeStamp);
            Assert.Equal(0.001, file.MinimumResolution);
            Assert.Equal(500.0, file.MaxCoordinateValue);
            Assert.Equal("Brett", file.Author);
            Assert.Equal("IxMilia", file.Organization);
            Assert.Equal(IgesVersion.v5_0, file.IgesVersion);
            Assert.Equal(IgesDraftingStandard.BSI, file.DraftingStandard);
            Assert.Equal(new DateTime(1987, 5, 8, 12, 34, 56), file.ModifiedTime);
            Assert.Equal("protocol", file.ApplicationProtocol);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void GlobalParseWithLeadingWhitespaceTest()
        {
            var file = CreateFile(@"
0123456789012345678901234567890123456789012345678901234567890123456789--S      1
1H,,1H;,10Hidentifier,28HC:\path\to\full\filename.igs,4Habcd,3H1.0,16,7,G      1
22,10,51,6Hident2,0.75,10,,4,0.8,15H20001225.130811, 1.0E-003,500,5HBretG      2
t,7HIxMilia, 8,4,13H870508.123456, 8Hprotocol;                          G      3
S      1G      3D      0P      0                                        T      1
");
            Assert.Equal(0.001, file.MinimumResolution); // leading space on double
            Assert.Equal(IgesVersion.v5_0, file.IgesVersion); // leading space on int
            Assert.Equal("protocol", file.ApplicationProtocol); // leading space on string
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void GlobalParseWithMissingStringField()
        {
            var file = CreateFile(@"
0123456789012345678901234567890123456789012345678901234567890123456789--S      1
1H,,1H;,10Hidentifier,28HC:\path\to\full\filename.igs,4Habcd,3H1.0,16,7,G      1
22,10,51,6Hident2,0.75,10,,4,0.8,15H20001225.130811,1.0E-003,500,5HBrettG      2
,7HIxMilia,8,4,13H870508.123456,;                                       G      3
S      1G      3D      0P      0                                        T      1
");
            Assert.Equal(null, file.ApplicationProtocol);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void GlobalParseWithMissingIntField()
        {
            var file = CreateFile(@"
0123456789012345678901234567890123456789012345678901234567890123456789--S      1
1H,,1H;,10Hidentifier,28HC:\path\to\full\filename.igs,4Habcd,3H1.0,16,7,G      1
22,10,51,6Hident2,0.75,10,,4,0.8,15H20001225.130811,1.0E-003,500,5HBrettG      2
,7HIxMilia,8,,13H870508.123456,8Hprotocol;                              G      3
S      1G      3D      0P      0                                        T      1
");
            Assert.Equal(IgesDraftingStandard.None, file.DraftingStandard);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void GlobalParseWithMissingDoubleField()
        {
            var file = CreateFile(@"
0123456789012345678901234567890123456789012345678901234567890123456789--S      1
1H,,1H;,10Hidentifier,28HC:\path\to\full\filename.igs,4Habcd,3H1.0,16,7,G      1
22,10,51,6Hident2,0.75,10,,4,0.8,15H20001225.130811,,500,5HBrett,7HIxMilG      2
ia,8,4,13H870508.123456,8Hprotocol;                                     G      3
S      1G      3D      0P      0                                        T      1
");
            Assert.Equal(0.0, file.MinimumResolution);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void FileWithNonStandardDelimitersTest()
        {
            var file = CreateFile(@"
                                                                        S      1
1H//1H#/10Hidentifier/12Hfilename.igs#                                  G      1
");
            Assert.Equal('/', file.FieldDelimiter);
            Assert.Equal('#', file.RecordDelimiter);
            Assert.Equal("identifier", file.Identification);
            Assert.Equal("filename.igs", file.FullFileName);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void FileWithEmptyFieldOrRecordSpecifierTest()
        {
            var file = CreateFile(@"
,;                                                                      G      1
");
            Assert.Equal(',', file.FieldDelimiter);
            Assert.Equal(';', file.RecordDelimiter);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void StringContainingDelimiterValuesTest()
        {
            var file = CreateFile(@"
                                                                        S      1
1H,,1H;,6H,;,;,;;                                                       G      1
");
            Assert.Equal(",;,;,;", file.Identification);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void StringWithLeadingAndTrailingWhitespaceTest()
        {
            var file = CreateFile(@"
                                                                        S      1
1H,,1H;,                                                        7H  foo G      1
 ;                                                                      G      2
");
            Assert.Equal("  foo  ", file.Identification);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void MissingStartSectionTest()
        {
            var file = CreateFile(@"
1H,,1H;,10Hidentifier,28HC:\path\to\full\filename.igs,4Habcd,3H1.0,16,7,G      1
22,10,51,6Hident2,0.75,10,,4,0.8,15H20001225.130811,,500,5HBrett,7HIxMilG      2
ia,8,4,13H870508.123456,8Hprotocol;                                     G      3
S      0G      3D      0P      0                                        T      1
");
            Assert.Equal(',', file.FieldDelimiter);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void MissingGlobalSectionTest()
        {
            var file = CreateFile(@"
                                                                        S      1
S      1G      0D      0P      0                                        T      1
");
            Assert.Equal(',', file.FieldDelimiter);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void OnlyTerminateLineTest()
        {
            var file = CreateFile(@"
S      0G      0D      0P      0                                        T      1
");
            Assert.Equal(',', file.FieldDelimiter);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void EmptyFileTest()
        {
            var file = CreateFile(string.Empty);
            Assert.Equal(',', file.FieldDelimiter);
        }
    }
}
