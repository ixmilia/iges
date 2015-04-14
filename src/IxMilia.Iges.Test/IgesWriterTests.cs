// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text;
using IxMilia.Iges.Entities;
using Xunit;

namespace IxMilia.Iges.Test
{
    public class IgesWriterTests
    {
        private static void VerifyFileText(IgesFile file, string expected, Action<string, string> verifier)
        {
            var stream = new MemoryStream();
            file.Save(stream);
            stream.Seek(0, SeekOrigin.Begin);
            var bytes = stream.ToArray();
            var actual = Encoding.ASCII.GetString(bytes);
            verifier(expected.Trim('\r', '\n'), actual.Trim('\r', '\n'));
        }

        private static void VerifyFileExactly(IgesFile file, string expected)
        {
            VerifyFileText(file, expected, (ex, ac) => Assert.Equal(ex, ac));
        }

        private static void VerifyFileContains(IgesFile file, string expected)
        {
            VerifyFileText(file, expected, (ex, ac) => Assert.True(ac.Contains(ex)));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WriteEmptyFileTest()
        {
            var date = new DateTime(2000, 12, 25, 13, 8, 5);
            var file = new IgesFile()
            {
                ModifiedTime = date,
                TimeStamp = date
            };
            VerifyFileExactly(file, @"
                                                                        S      1
1H,,1H;,,,,,32,8,23,11,52,,1.,1,,0,1.,15H20001225.130805,1E-10,0.,,,11, G      1
0,15H20001225.130805,;                                                  G      2
S      1G      2D      0P      0                                        T      1
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WriteLineWithSpanningParametersTest()
        {
            var file = new IgesFile();
            file.Entities.Add(new IgesLine()
            {
                P1 = new IgesPoint(1.1234512345, 2.1234512345, 3.1234512345),
                P2 = new IgesPoint(4.1234512345, 5.1234512345, 6.1234512345),
                Color = IgesColorNumber.Green
            });
            VerifyFileContains(file, @"
     110       1       0       0       0                        00000000D      1
     110       0       3       2       0                                D      2
110,1.1234512345,2.1234512345,3.1234512345,4.1234512345,               1P      1
5.1234512345,6.1234512345;                                             1P      2
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WriteLineWithViewTest()
        {
            var file = new IgesFile();
            var line = new IgesLine() { View = new IgesView() };
            file.Entities.Add(line);
            VerifyFileContains(file, @"
     410       1       0       0       0                        00000100D      1
     410       0       0       1       0                                D      2
     110       2       0       0       0       1                00000000D      3
     110       0       0       1       0                                D      4
410,0,0.,0,0,0,0,0,0;                                                  1P      1
110,0.,0.,0.,0.,0.,0.;                                                 3P      2
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WriteLineWithTransformationMatrixTest()
        {
            var file = new IgesFile();
            var trans = new IgesTransformationMatrix()
            {
                R11 = 1.0,
                R12 = 2.0,
                R13 = 3.0,
                T1 = 4.0,
                R21 = 5.0,
                R22 = 6.0,
                R23 = 7.0,
                T2 = 8.0,
                R31 = 9.0,
                R32 = 10.0,
                R33 = 11.0,
                T3 = 12.0
            };
            var line = new IgesLine()
            {
                P1 = new IgesPoint(1, 2, 3),
                P2 = new IgesPoint(4, 5, 6),
                TransformationMatrix = trans,
            };
            file.Entities.Add(line);
            VerifyFileContains(file, @"
     124       1       0       0       0                        00000000D      1
     124       0       0       1       0                                D      2
     110       2       0       0       0               1        00000000D      3
     110       0       0       1       0                                D      4
124,1.,2.,3.,4.,5.,6.,7.,8.,9.,10.,11.,12.;                            1P      1
110,1.,2.,3.,4.,5.,6.;                                                 3P      2
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WriteLineWithStructureTest()
        {
            var file = new IgesFile();
            var circle = new IgesCircularArc() { StructureEntity = new IgesLine() };
            file.Entities.Add(circle);
            VerifyFileContains(file, @"
     110       1       0       0       0                        00000000D      1
     110       0       0       1       0                                D      2
     100       2      -1       0       0                        00000000D      3
     100       0       0       1       0                                D      4
110,0.,0.,0.,0.,0.,0.;                                                 1P      1
100,0.,0.,0.,0.,0.,0.,0.;                                              3P      2
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WriteLineWithLineFontPatternTest()
        {
            var file = new IgesFile();
            var line = new IgesLine() { LineFont = IgesLineFontPattern.Phantom };
            file.Entities.Add(line);
            VerifyFileContains(file, @"
     110       1       0       3       0                        00000000D      1
     110       0       0       1       0                                D      2
110,0.,0.,0.,0.,0.,0.;                                                 1P      1
");
            line.CustomLineFont = new IgesPatternLineFontDefinition();
            VerifyFileContains(file, @"
     304       1       0       0       0                        00000200D      1
     304       0       0       1       2                                D      2
     110       2       0      -1       0                        00000000D      3
     110       0       0       1       0                                D      4
304,0,1H0;                                                             1P      1
110,0.,0.,0.,0.,0.,0.;                                                 3P      2
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WriteLineWithLevelsTest()
        {
            var file = new IgesFile();
            var line = new IgesLine();
            file.Entities.Add(line);

            // no levels defined
            line.Levels.Clear();
            VerifyFileContains(file, @"
     110       1       0       0       0                        00000000D      1
     110       0       0       1       0                                D      2
110,0.,0.,0.,0.,0.,0.;                                                 1P      1
");

            // one level defined
            line.Levels.Add(13);
            VerifyFileContains(file, @"
     110       1       0       0      13                        00000000D      1
     110       0       0       1       0                                D      2
110,0.,0.,0.,0.,0.,0.;                                                 1P      1
");

            // multiple levels defined
            line.Levels.Add(23);
            VerifyFileContains(file, @"
     406       1       0       0       0                        00000000D      1
     406       0       0       1       1                                D      2
     110       2       0       0      -1                        00000000D      3
     110       0       0       1       0                                D      4
406,2,13,23;                                                           1P      1
110,0.,0.,0.,0.,0.,0.;                                                 3P      2
");

            // multiple entities referencing different multiple levels
            var otherLine = new IgesLine();
            otherLine.Levels.Add(40);
            otherLine.Levels.Add(41);
            file.Entities.Add(otherLine);
            VerifyFileContains(file, @"
     406       1       0       0       0                        00000000D      1
     406       0       0       1       1                                D      2
     110       2       0       0      -1                        00000000D      3
     110       0       0       1       0                                D      4
     406       3       0       0       0                        00000000D      5
     406       0       0       1       1                                D      6
     110       4       0       0      -5                        00000000D      7
     110       0       0       1       0                                D      8
406,2,13,23;                                                           1P      1
110,0.,0.,0.,0.,0.,0.;                                                 3P      2
406,2,40,41;                                                           5P      3
110,0.,0.,0.,0.,0.,0.;                                                 7P      4
");

            // multiple entities referencing the same multiple levels
            otherLine.Levels.Clear();
            foreach (var level in line.Levels)
            {
                otherLine.Levels.Add(level);
            }

            VerifyFileContains(file, @"
     406       1       0       0       0                        00000000D      1
     406       0       0       1       1                                D      2
     110       2       0       0      -1                        00000000D      3
     110       0       0       1       0                                D      4
     110       3       0       0      -1                        00000000D      5
     110       0       0       1       0                                D      6
406,2,13,23;                                                           1P      1
110,0.,0.,0.,0.,0.,0.;                                                 3P      2
110,0.,0.,0.,0.,0.,0.;                                                 5P      3
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WriteSpecificGlobalValuesTest()
        {
            var file = new IgesFile()
            {
                FieldDelimiter = ',',
                RecordDelimiter = ';',
                Identification = "identifier",
                FullFileName = @"C:\path\to\full\filename.igs",
                SystemIdentifier = "abcd",
                SystemVersion = "1.0",
                IntegerSize = 16,
                SingleSize = 7,
                DecimalDigits = 22,
                DoubleMagnitude = 10,
                DoublePrecision = 51,
                Identifier = "ident2",
                ModelSpaceScale = 0.75,
                ModelUnits = IgesUnits.Centimeters,
                CustomModelUnits = null,
                MaxLineWeightGraduations = 4,
                MaxLineWeight = 0.8,
                TimeStamp = new DateTime(2000, 12, 25, 13, 8, 11),
                MinimumResolution = 0.001,
                MaxCoordinateValue = 500.0,
                Author = "Brett",
                Organization = "IxMilia",
                IgesVersion = IgesVersion.v5_0,
                DraftingStandard = IgesDraftingStandard.BSI,
                ModifiedTime = new DateTime(1987, 5, 8, 12, 34, 56),
                ApplicationProtocol = "protocol"
            };
            VerifyFileExactly(file, @"
                                                                        S      1
1H,,1H;,10Hidentifier,28HC:\path\to\full\filename.igs,4Habcd,3H1.0,16,7,G      1
22,10,51,6Hident2,0.75,10,,4,0.8,15H20001225.130811,0.001,500.,5HBrett, G      2
7HIxMilia,8,4,15H19870508.123456,8Hprotocol;                            G      3
S      1G      3D      0P      0                                        T      1
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WriteCommonPointersTest()
        {
            // TODO: implement this once type (402, 212, or 312) is implemented
        }
    }
}
