// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.IO;
using IxMilia.Iges.Entities;
using Xunit;

namespace IxMilia.Iges.Test
{
    public class IgesEntityWriterTests
    {

        #region Private methods

        public static void VerifyFileContains(IgesFile file, string expectedText)
        {
            using (var ms = new MemoryStream())
            {
                file.Save(ms);
                ms.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(ms))
                {
                    var actual = reader.ReadToEnd();
                    Assert.Contains(expectedText.Trim('\r', '\n'), actual);
                }
            }
        }

        private static void VerifyEntity(IgesEntity entity, string expectedText)
        {
            var file = new IgesFile();
            file.Entities.Add(entity);
            VerifyFileContains(file, expectedText);
        }

        #endregion

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WriteNullEntityTest()
        {
            VerifyEntity(new IgesNull(), @"
       0       1       0       0       0                               0D      1
       0       0       0       0       0                                D      2
0;                                                                     1P      1");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WriteCircularArcTest()
        {
            var circle = new IgesCircularArc(new IgesPoint(22, 33, 11), new IgesPoint(44, 55, 11), new IgesPoint(66, 77, 11));
            circle.Color = IgesColorNumber.Green;
            VerifyEntity(circle, @"
     100       1       0       0       0                               0D      1
     100       0       3       1       0                                D      2
100,11.,22.,33.,44.,55.,66.,77.;                                       1P      1
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WriteLineTest()
        {
            var line = new IgesLine(new IgesPoint(11, 22, 33), new IgesPoint(44, 55, 66));
            line.Bounding = IgesBounding.Unbound;
            VerifyEntity(line, @"
     110       1       0       0       0                               0D      1
     110       0       0       1       2                                D      2
110,11.,22.,33.,44.,55.,66.;                                           1P      1
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WriteLocationTest()
        {
            VerifyEntity(new IgesLocation(11, 22, 33), @"
     116       1       0       0       0                               0D      1
     116       0       0       1       0                                D      2
116,11.,22.,33.;                                                       1P      1
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WriteDirectionTest()
        {
            VerifyEntity(new IgesDirection(11, 22, 33), @"
     123       1       0       0       0                        00010200D      1
     123       0       0       1       0                                D      2
123,11.,22.,33.;                                                       1P      1
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WriteTransformationMatrixTest()
        {
            var matrix = new IgesTransformationMatrix(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            VerifyEntity(matrix, @"
     124       1       0       0       0                               0D      1
     124       0       0       0       0                                D      2
124,1.,2.,3.,4.,5.,6.,7.,8.,9.,10.,11.,12.;                            1P      1
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WriteSphereTest()
        {
            // regular case
            var sphere = new IgesSphere(11, new IgesPoint(22, 33, 44));
            VerifyEntity(sphere, @"
     158       1       0       0       0                            0000D      1
     158       0       0       1       0                                D      2
158,11.,22.,33.,44.;                                                   1P      1
");

            // default center
            VerifyEntity(new IgesSphere() { Radius = 1 }, @"
     158       1       0       0       0                            0000D      1
     158       0       0       1       0                                D      2
158,1.;                                                                1P      1
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WriteTorusTest()
        {
            // regular case
            var torus = new IgesTorus(11, 22, new IgesPoint(33, 44, 55), new IgesVector(66, 77, 88));
            VerifyEntity(torus, @"
     160       1       0       0       0                        00000000D      1
     160       0       0       1       0                                D      2
160,11.,22.,33.,44.,55.,66.,77.,88.;                                   1P      1
");

            // default values; minimal data written
            VerifyEntity(new IgesTorus(), @"
     160       1       0       0       0                        00000000D      1
     160       0       0       1       0                                D      2
160,0.,0.;                                                             1P      1
");

            // non-default center; only center written
            VerifyEntity(new IgesTorus() { Center = new IgesPoint(1, 2, 3) }, @"
     160       1       0       0       0                        00000000D      1
     160       0       0       1       0                                D      2
160,0.,0.,1.,2.,3.;                                                    1P      1
");

            // non-default normal
            VerifyEntity(new IgesTorus() { Normal = new IgesVector(1, 2, 3) }, @"
     160       1       0       0       0                        00000000D      1
     160       0       0       1       0                                D      2
160,0.,0.,0.,0.,0.,1.,2.,3.;                                           1P      1
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WriteSubfigureEntityTest()
        {
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
            var sub = new IgesSubfigureDefinition();
            sub.Name = "this is a really long string that should span many lines because it is so huge";
            sub.Entities.Add(new IgesLine() { P1 = new IgesPoint(1, 2, 3), P2 = new IgesPoint(4, 5, 6), TransformationMatrix = trans });
            sub.Entities.Add(new IgesLine() { P1 = new IgesPoint(7, 8, 9), P2 = new IgesPoint(10, 11, 12) });
            var file = new IgesFile();
            file.Entities.Add(sub);
            VerifyFileContains(file, @"
     124       1       0       0       0                               0D      1
     124       0       0       0       0                                D      2
     110       2       0       0       0               1               0D      3
     110       0       0       1       0                                D      4
     110       3       0       0       0                               0D      5
     110       0       0       1       0                                D      6
     308       4       0       0       0                               0D      7
     308       0       0       0       0                                D      8
124,1.,2.,3.,4.,5.,6.,7.,8.,9.,10.,11.,12.;                            1P      1
110,1.,2.,3.,4.,5.,6.;                                                 3P      2
110,7.,8.,9.,10.,11.,12.;                                              5P      3
308,0,78Hthis is a really long string that should span many line       7P      4
s because it is so huge,2,3,5;                                         7P      5
");
        }
    }
}
