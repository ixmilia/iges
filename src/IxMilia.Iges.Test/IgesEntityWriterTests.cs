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
       0       1       0       0       0                        00000000D      1
       0       0       0       1       0                                D      2
0;                                                                     1P      1");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WriteCircularArcTest()
        {
            var circle = new IgesCircularArc(new IgesPoint(22, 33, 11), new IgesPoint(44, 55, 11), new IgesPoint(66, 77, 11));
            circle.Color = IgesColorNumber.Green;
            VerifyEntity(circle, @"
     100       1       0       0       0                        00000000D      1
     100       0       3       1       0                                D      2
100,11.,22.,33.,44.,55.,66.,77.;                                       1P      1
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WriteCompositeCurveTest()
        {
            var curve = new IgesCompositeCurve();
            curve.Entities.Add(new IgesLine(new IgesPoint(11, 22, 33), new IgesPoint(44, 55, 66)));
            curve.Entities.Add(new IgesCircularArc(new IgesPoint(11, 22, 33), new IgesPoint(11, 22, 33), new IgesPoint(11, 22, 33)));
            VerifyEntity(curve, @"
     110       1       0       0       0                        00000000D      1
     110       0       0       1       0                                D      2
     100       2       0       0       0                        00000000D      3
     100       0       0       1       0                                D      4
     102       3       0       0       0                        00000000D      5
     102       0       0       1       0                                D      6
110,11.,22.,33.,44.,55.,66.;                                           1P      1
100,33.,11.,22.,11.,22.,11.,22.;                                       3P      2
102,2,1,3;                                                             5P      3
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WritePlaneTest()
        {
            // unbounded
            var plane = new IgesPlane()
            {
                PlaneCoefficientA = 11,
                PlaneCoefficientB = 22,
                PlaneCoefficientC = 33,
                PlaneCoefficientD = 44,
                DisplaySymbolLocation = new IgesPoint(55, 66, 77),
                DisplaySymbolSize = 88,
                Bounding = IgesPlaneBounding.Unbounded
            };
            VerifyEntity(plane, @"
     108       1       0       0       0                        00000000D      1
     108       0       0       1       0                                D      2
108,11.,22.,33.,44.,0,55.,66.,77.,88.;                                 1P      1
");

            // bounded
            plane.Bounding = IgesPlaneBounding.BoundedPositive;
            plane.ClosedCurveBoundingEntity = new IgesCircularArc();
            VerifyEntity(plane, @"
     100       1       0       0       0                        00000000D      1
     100       0       0       1       0                                D      2
     108       2       0       0       0                        00000000D      3
     108       0       0       1       1                                D      4
100,0.,0.,0.,0.,0.,0.,0.;                                              1P      1
108,11.,22.,33.,44.,1,55.,66.,77.,88.;                                 3P      2
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WriteLineTest()
        {
            var line = new IgesLine(new IgesPoint(11, 22, 33), new IgesPoint(44, 55, 66));
            line.Bounding = IgesLineBounding.Unbounded;
            VerifyEntity(line, @"
     110       1       0       0       0                        00000000D      1
     110       0       0       1       2                                D      2
110,11.,22.,33.,44.,55.,66.;                                           1P      1
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WriteLocationTest()
        {
            VerifyEntity(new IgesLocation(11, 22, 33), @"
     116       1       0       0       0                        00000000D      1
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
     124       1       0       0       0                        00000000D      1
     124       0       0       1       0                                D      2
124,1.,2.,3.,4.,5.,6.,7.,8.,9.,10.,11.,12.;                            1P      1
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WriteSphereTest()
        {
            // regular case
            var sphere = new IgesSphere(11, new IgesPoint(22, 33, 44));
            VerifyEntity(sphere, @"
     158       1       0       0       0                        00000000D      1
     158       0       0       1       0                                D      2
158,11.,22.,33.,44.;                                                   1P      1
");

            // default center
            VerifyEntity(new IgesSphere() { Radius = 1 }, @"
     158       1       0       0       0                        00000000D      1
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
        public void WriteTemplateLineFontDefinitionTest()
        {
            // regular case
            var sfd = new IgesSubfigureDefinition();
            sfd.Entities.Add(new IgesNull());
            var lfd = new IgesTemplateLineFontDefinition(sfd, 1.0, 2.0);
            VerifyEntity(lfd, @"
       0       1       0       0       0                        00000000D      1
       0       0       0       1       0                                D      2
     308       2       0       0       0                        00000200D      3
     308       0       0       1       0                                D      4
     304       3       0       0       0                        00000200D      5
     304       0       0       1       1                                D      6
0;                                                                     1P      1
308,0,,1,1;                                                            3P      2
304,0,3,1.,2.;                                                         5P      3
");

            // default values
            lfd = new IgesTemplateLineFontDefinition();
            VerifyEntity(lfd, @"
     308       1       0       0       0                        00000200D      1
     308       0       0       1       0                                D      2
     304       2       0       0       0                        00000200D      3
     304       0       0       1       1                                D      4
308,0,,0;                                                              1P      1
304,0,1,0.,0.;                                                         3P      2
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WritePatternLineFontDefinitionTest()
        {
            // regular case
            var lfd = new IgesPatternLineFontDefinition();
            lfd.SegmentLengths.Add(1);
            lfd.SegmentLengths.Add(2);
            lfd.DisplayMask = 0x34;
            VerifyEntity(lfd, @"
     304       1       0       0       0                        00000200D      1
     304       0       0       1       2                                D      2
304,2,1.,2.,2H34;                                                      1P      1
");

            // default values
            lfd = new IgesPatternLineFontDefinition();
            VerifyEntity(lfd, @"
     304       1       0       0       0                        00000200D      1
     304       0       0       1       2                                D      2
304,0,1H0;                                                             1P      1
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
     124       1       0       0       0                        00000000D      1
     124       0       0       1       0                                D      2
     110       2       0       0       0               1        00000000D      3
     110       0       0       1       0                                D      4
     110       3       0       0       0                        00000000D      5
     110       0       0       1       0                                D      6
     308       4       0       0       0                        00000200D      7
     308       0       0       2       0                                D      8
124,1.,2.,3.,4.,5.,6.,7.,8.,9.,10.,11.,12.;                            1P      1
110,1.,2.,3.,4.,5.,6.;                                                 3P      2
110,7.,8.,9.,10.,11.,12.;                                              5P      3
308,0,78Hthis is a really long string that should span many line       7P      4
s because it is so huge,2,3,5;                                         7P      5
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WriteColorDefinitionTest()
        {
            // regular case
            var color = new IgesColorDefinition(11, 22, 33, "name");
            VerifyEntity(color, @"
     314       1       0       0       0                        00000200D      1
     314       0       0       1       0                                D      2
314,11.,22.,33.,4Hname;                                                1P      1
");

            // default values
            color = new IgesColorDefinition();
            VerifyEntity(color, @"
     314       1       0       0       0                        00000200D      1
     314       0       0       1       0                                D      2
314,1.,1.,1.,;                                                         1P      1
");

            // line with custom color
            var line = new IgesLine() { CustomColor = new IgesColorDefinition(11, 22, 33, "name") };
            VerifyEntity(line, @"
     314       1       0       0       0                        00000200D      1
     314       0       0       1       0                                D      2
     110       2       0       0       0                        00000000D      3
     110       0      -1       1       0                                D      4
314,11.,22.,33.,4Hname;                                                1P      1
110,0.,0.,0.,0.,0.,0.;                                                 3P      2
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WriteViewTest()
        {
            // regular values
            var view = new IgesView(
                1,
                2,
                new IgesPlane() { PlaneCoefficientA = 3 },
                new IgesPlane() { PlaneCoefficientA = 4 },
                new IgesPlane() { PlaneCoefficientA = 5 },
                new IgesPlane() { PlaneCoefficientA = 6 },
                new IgesPlane() { PlaneCoefficientA = 7 },
                new IgesPlane() { PlaneCoefficientA = 8 });
            VerifyEntity(view, @"
     108       1       0       0       0                        00000000D      1
     108       0       0       1       0                                D      2
     108       2       0       0       0                        00000000D      3
     108       0       0       1       0                                D      4
     108       3       0       0       0                        00000000D      5
     108       0       0       1       0                                D      6
     108       4       0       0       0                        00000000D      7
     108       0       0       1       0                                D      8
     108       5       0       0       0                        00000000D      9
     108       0       0       1       0                                D     10
     108       6       0       0       0                        00000000D     11
     108       0       0       1       0                                D     12
     410       7       0       0       0                        00000100D     13
     410       0       0       1       0                                D     14
108,3.,0.,0.,0.,0,0.,0.,0.,0.;                                         1P      1
108,4.,0.,0.,0.,0,0.,0.,0.,0.;                                         3P      2
108,5.,0.,0.,0.,0,0.,0.,0.,0.;                                         5P      3
108,6.,0.,0.,0.,0,0.,0.,0.,0.;                                         7P      4
108,7.,0.,0.,0.,0,0.,0.,0.,0.;                                         9P      5
108,8.,0.,0.,0.,0,0.,0.,0.,0.;                                        11P      6
410,1,2.,1,3,5,7,9,11;                                                13P      7
");

            // default values
            view = new IgesView();
            VerifyEntity(view, @"
     410       1       0       0       0                        00000100D      1
     410       0       0       1       0                                D      2
410,0,0.,0,0,0,0,0,0;                                                  1P      1
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Writing)]
        public void WritePerspectiveViewTest()
        {
            // regular values
            var view = new IgesPerspectiveView(
                1,
                2,
                new IgesVector(3, 0, 0),
                new IgesPoint(4, 0, 0),
                new IgesPoint(5, 0, 0),
                new IgesVector(6, 0, 0),
                7,
                8,
                9,
                10,
                11,
                IgesDepthClipping.BackClipping,
                12,
                13);
            VerifyEntity(view, @"
     410       1       0       0       0                        00000100D      1
     410       0       0       2       1                                D      2
410,1,2.,3.,0.,0.,4.,0.,0.,5.,0.,0.,6.,0.,0.,7.,8.,9.,10.,11.,1,       1P      1
12.,13.;                                                               1P      2
");

            // default values
            view = new IgesPerspectiveView();
            VerifyEntity(view, @"
     410       1       0       0       0                        00000100D      1
     410       0       0       2       1                                D      2
410,0,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0,         1P      1
0.,0.;                                                                 1P      2
");
        }
    }
}
