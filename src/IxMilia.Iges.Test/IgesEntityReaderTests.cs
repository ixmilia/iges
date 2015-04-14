// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Linq;
using IxMilia.Iges.Entities;
using Xunit;

namespace IxMilia.Iges.Test
{
    public class IgesEntityReaderTests
    {

        #region Private methods

        private static IgesEntity ParseSingleEntity(string content)
        {
            return IgesReaderTests.ParseSingleEntity(content);
        }

        #endregion

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadUnsupportedEntityTest()
        {
            // entity id 888 is invalid
            var file = IgesReaderTests.CreateFile(@"
     888       1       0       0       0                               0D      1
     888       0       0       1       0                               0D      2
888,11,22,33,44,55,66;                                                 1P      1
".Trim('\r', '\n'));
            Assert.Equal(0, file.Entities.Count);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadNullEntityTest()
        {
            var file = IgesReaderTests.CreateFile(@"
       0       1       0       0       0                               0D      1
       0       0       0       1       0                               0D      2
0,11,22,33,44,55,66;                                                   1P      1");
            Assert.Equal(IgesEntityType.Null, file.Entities.Single().EntityType);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadCircularArcTest()
        {
            // read specified values
            var circle = (IgesCircularArc)ParseSingleEntity(@"
     100       1       0       0       0                               0D      1
     100       0       3       1       0                               0D      2
100,11,22,33,44,55,66,77;                                              1P      1
");
            Assert.Equal(11.0, circle.PlaneDisplacement);
            Assert.Equal(22.0, circle.Center.X);
            Assert.Equal(33.0, circle.Center.Y);
            Assert.Equal(0.0, circle.Center.Z);
            Assert.Equal(44.0, circle.StartPoint.X);
            Assert.Equal(55.0, circle.StartPoint.Y);
            Assert.Equal(0.0, circle.StartPoint.Z);
            Assert.Equal(66.0, circle.EndPoint.X);
            Assert.Equal(77.0, circle.EndPoint.Y);
            Assert.Equal(0.0, circle.EndPoint.Z);
            Assert.Equal(new IgesPoint(22.0, 33.0, 11.0), circle.ProperCenter);
            Assert.Equal(new IgesPoint(44.0, 55.0, 11.0), circle.ProperStartPoint);
            Assert.Equal(new IgesPoint(66.0, 77.0, 11.0), circle.ProperEndPoint);
            Assert.Equal(IgesColorNumber.Green, circle.Color);

            // read type-default values
            circle = (IgesCircularArc)ParseSingleEntity(@"
     100       1       0       0       0                               0D      1
     100       0       3       1       0                               0D      2
100;                                                                   1P      1"
);
            Assert.Equal(0.0, circle.PlaneDisplacement);
            Assert.Equal(0.0, circle.Center.X);
            Assert.Equal(0.0, circle.Center.Y);
            Assert.Equal(0.0, circle.Center.Z);
            Assert.Equal(0.0, circle.StartPoint.X);
            Assert.Equal(0.0, circle.StartPoint.Y);
            Assert.Equal(0.0, circle.StartPoint.Z);
            Assert.Equal(0.0, circle.EndPoint.X);
            Assert.Equal(0.0, circle.EndPoint.Y);
            Assert.Equal(0.0, circle.EndPoint.Z);
            Assert.Equal(IgesPoint.Origin, circle.ProperCenter);
            Assert.Equal(IgesPoint.Origin, circle.ProperStartPoint);
            Assert.Equal(IgesPoint.Origin, circle.ProperEndPoint);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadCompositeCurve()
        {
            var entity = ParseSingleEntity(@"
// The curve has two lines; one defined before and one after.           S      1
     110       1       0       0       0                               0D      1
     110       0       0       1       0                               0D      2
     102       2       0       0       0                               0D      3
     102       0       0       1       0                               0D      4
     100       3       0       0       0                               0D      5
     100       0       0       1       0                               0D      6
110,1.0,2.0,3.0,4.0,5.0,6.0;                                            P      1
102,2,1,5;                                                              P      2
100,11,22,33,44,55,66,77;                                               P      3
");
            Assert.Equal(IgesEntityType.CompositeCurve, entity.EntityType);
            var compositeCurve = (IgesCompositeCurve)entity;
            Assert.Equal(2, compositeCurve.Entities.Count);
            Assert.Equal(IgesEntityType.Line, compositeCurve.Entities[0].EntityType);
            Assert.Equal(IgesEntityType.CircularArc, compositeCurve.Entities[1].EntityType);
            var line = (IgesLine)compositeCurve.Entities[0];
            Assert.Equal(1.0, line.P1.X);
            Assert.Equal(2.0, line.P1.Y);
            Assert.Equal(3.0, line.P1.Z);
            Assert.Equal(4.0, line.P2.X);
            Assert.Equal(5.0, line.P2.Y);
            Assert.Equal(6.0, line.P2.Z);
            var circle = (IgesCircularArc)compositeCurve.Entities[1];
            Assert.Equal(11.0, circle.PlaneDisplacement);
            Assert.Equal(22.0, circle.Center.X);
            Assert.Equal(33.0, circle.Center.Y);
            Assert.Equal(0.0, circle.Center.Z);
            Assert.Equal(44.0, circle.StartPoint.X);
            Assert.Equal(55.0, circle.StartPoint.Y);
            Assert.Equal(0.0, circle.StartPoint.Z);
            Assert.Equal(66.0, circle.EndPoint.X);
            Assert.Equal(77.0, circle.EndPoint.Y);
            Assert.Equal(0.0, circle.EndPoint.Z);

            // read type-default values
            compositeCurve = (IgesCompositeCurve)ParseSingleEntity(@"
     102       1       0       0       0                               0D      1
     102       0       0       1       0                               0D      2
102;                                                                    P      1
");
            Assert.Equal(0, compositeCurve.Entities.Count);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadPlaneTest()
        {
            // unbounded
            var plane = (IgesPlane)ParseSingleEntity(@"
     108       1       0       0       0                        00000000D      1
     108       0       0       1       0                                D      2
108,11.,22.,33.,44.,0,55.,66.,77.,88.;                                 1P      1
");
            Assert.Equal(11, plane.PlaneCoefficientA);
            Assert.Equal(22, plane.PlaneCoefficientB);
            Assert.Equal(33, plane.PlaneCoefficientC);
            Assert.Equal(44, plane.PlaneCoefficientD);
            Assert.Null(plane.ClosedCurveBoundingEntity);
            Assert.Equal(IgesPlaneBounding.Unbounded, plane.Bounding);
            Assert.Equal(new IgesPoint(55, 66, 77), plane.DisplaySymbolLocation);
            Assert.Equal(88, plane.DisplaySymbolSize);

            // bounded
            plane = (IgesPlane)ParseSingleEntity(@"
     100       1       0       0       0                        00000000D      1
     100       0       0       1       0                                D      2
     108       2       0       0       0                        00000000D      3
     108       0       0       1       1                                D      4
100,0.,0.,0.,0.,0.,0.,0.;                                              1P      1
108,11.,22.,33.,44.,1,55.,66.,77.,88.;                                 3P      2
");
            Assert.Equal(11, plane.PlaneCoefficientA);
            Assert.Equal(22, plane.PlaneCoefficientB);
            Assert.Equal(33, plane.PlaneCoefficientC);
            Assert.Equal(44, plane.PlaneCoefficientD);
            Assert.NotNull(plane.ClosedCurveBoundingEntity as IgesCircularArc);
            Assert.Equal(IgesPlaneBounding.BoundedPositive, plane.Bounding);
            Assert.Equal(new IgesPoint(55, 66, 77), plane.DisplaySymbolLocation);
            Assert.Equal(88, plane.DisplaySymbolSize);

            // default values
            plane = (IgesPlane)ParseSingleEntity(@"
     108       1       0       0       0                        00000000D      1
     108       0       0       1       0                                D      2
108;                                                                   1P      1
");
            Assert.Equal(0, plane.PlaneCoefficientA);
            Assert.Equal(0, plane.PlaneCoefficientB);
            Assert.Equal(0, plane.PlaneCoefficientC);
            Assert.Equal(0, plane.PlaneCoefficientD);
            Assert.Null(plane.ClosedCurveBoundingEntity);
            Assert.Equal(IgesPlaneBounding.Unbounded, plane.Bounding);
            Assert.Equal(IgesPoint.Origin, plane.DisplaySymbolLocation);
            Assert.Equal(0, plane.DisplaySymbolSize);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadLineTest()
        {
            var line = (IgesLine)ParseSingleEntity(@"
     110       1       0       0       0                               0D      1
     110       0       3       1       0                               0D      2
110,11,22,33,44,55,66;                                                 1P      1
");
            Assert.Equal(11.0, line.P1.X);
            Assert.Equal(22.0, line.P1.Y);
            Assert.Equal(33.0, line.P1.Z);
            Assert.Equal(44.0, line.P2.X);
            Assert.Equal(55.0, line.P2.Y);
            Assert.Equal(66.0, line.P2.Z);
            Assert.Equal(IgesColorNumber.Green, line.Color);

            // verify transformation matrix is identity
            Assert.Equal(1.0, line.TransformationMatrix.R11);
            Assert.Equal(0.0, line.TransformationMatrix.R12);
            Assert.Equal(0.0, line.TransformationMatrix.R13);
            Assert.Equal(0.0, line.TransformationMatrix.R21);
            Assert.Equal(1.0, line.TransformationMatrix.R22);
            Assert.Equal(0.0, line.TransformationMatrix.R23);
            Assert.Equal(0.0, line.TransformationMatrix.R31);
            Assert.Equal(0.0, line.TransformationMatrix.R32);
            Assert.Equal(1.0, line.TransformationMatrix.R33);
            Assert.Equal(0.0, line.TransformationMatrix.T1);
            Assert.Equal(0.0, line.TransformationMatrix.T2);
            Assert.Equal(0.0, line.TransformationMatrix.T3);

            // read type-default values
            line = (IgesLine)ParseSingleEntity(@"
     110       1       0       0       0                               0D      1
     110       0       3       1       0                               0D      2
110;                                                                   1P      1
");
            Assert.Equal(0.0, line.P1.X);
            Assert.Equal(0.0, line.P1.Y);
            Assert.Equal(0.0, line.P1.Z);
            Assert.Equal(0.0, line.P2.X);
            Assert.Equal(0.0, line.P2.Y);
            Assert.Equal(0.0, line.P2.Z);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadLineWithNonStandardDelimitersTest()
        {
            var line = (IgesLine)ParseSingleEntity(@"
1H//1H##                                                                G      1
     110       1       0       0       0                               0D      1
     110       0       3       1       0                               0D      2
110/11/22/33/44/55/66#                                                 1P      1
");
            Assert.Equal(11.0, line.P1.X);
            Assert.Equal(22.0, line.P1.Y);
            Assert.Equal(33.0, line.P1.Z);
            Assert.Equal(44.0, line.P2.X);
            Assert.Equal(55.0, line.P2.Y);
            Assert.Equal(66.0, line.P2.Z);
            Assert.Equal(IgesColorNumber.Green, line.Color);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadLocationTest()
        {
            var location = (IgesLocation)ParseSingleEntity(@"
     116       1       0       0       0                               0D      1
     116       0       0       1       0                                D      2
116,11.,22.,33.;                                                       1P      1
");
            Assert.Equal(11.0, location.X);
            Assert.Equal(22.0, location.Y);
            Assert.Equal(33.0, location.Z);
            Assert.Equal(IgesColorNumber.Default, location.Color);

            // read type-default values
            location = (IgesLocation)ParseSingleEntity(@"
     116       1       0       0       0                               0D      1
     116       0       0       1       0                                D      2
116;                                                                   1P      1
");
            Assert.Equal(0.0, location.X);
            Assert.Equal(0.0, location.Y);
            Assert.Equal(0.0, location.Z);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadDirectionTest()
        {
            var direction = (IgesDirection)ParseSingleEntity(@"
     123       1       0       0       0                        00010200D      1
     123       0       0       1       0                                D      2
123,11.,22.,33.;                                                       1P      1
");
            Assert.Equal(11.0, direction.X);
            Assert.Equal(22.0, direction.Y);
            Assert.Equal(33.0, direction.Z);

            // read type-default values
            direction = (IgesDirection)ParseSingleEntity(@"
     123       1       0       0       0                        00010200D      1
     123       0       0       1       0                                D      2
123;                                                                   1P      1
");
            Assert.Equal(0.0, direction.X);
            Assert.Equal(0.0, direction.Y);
            Assert.Equal(0.0, direction.Z);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadTransformationMatrixTest()
        {
            var matrix = (IgesTransformationMatrix)ParseSingleEntity(@"
     124       1       0       0       0                               0D      1
     124       0       0       1       0                               0D      2
124,1,2,3,4,5,6,7,8,9,10,11,12;                                        1P      1
");
            Assert.Equal(1.0, matrix.R11);
            Assert.Equal(2.0, matrix.R12);
            Assert.Equal(3.0, matrix.R13);
            Assert.Equal(4.0, matrix.T1);
            Assert.Equal(5.0, matrix.R21);
            Assert.Equal(6.0, matrix.R22);
            Assert.Equal(7.0, matrix.R23);
            Assert.Equal(8.0, matrix.T2);
            Assert.Equal(9.0, matrix.R31);
            Assert.Equal(10.0, matrix.R32);
            Assert.Equal(11.0, matrix.R33);
            Assert.Equal(12.0, matrix.T3);

            // read type-default values
            matrix = (IgesTransformationMatrix)ParseSingleEntity(@"
     124       1       0       0       0                               0D      1
     124       0       0       1       0                               0D      2
124;                                                                   1P      1
");
            Assert.Equal(0.0, matrix.R11);
            Assert.Equal(0.0, matrix.R12);
            Assert.Equal(0.0, matrix.R13);
            Assert.Equal(0.0, matrix.T1);
            Assert.Equal(0.0, matrix.R21);
            Assert.Equal(0.0, matrix.R22);
            Assert.Equal(0.0, matrix.R23);
            Assert.Equal(0.0, matrix.T2);
            Assert.Equal(0.0, matrix.R31);
            Assert.Equal(0.0, matrix.R32);
            Assert.Equal(0.0, matrix.R33);
            Assert.Equal(0.0, matrix.T3);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadSphereTest()
        {
            // fully-specified values
            var sphere = (IgesSphere)ParseSingleEntity(@"
     158       1       0       0       0                            0000D      1
     158       0       0       1       0                                D      2
158,11.,22.,33.,44.;                                                   1P      1
");
            Assert.Equal(11.0, sphere.Radius);
            Assert.Equal(new IgesPoint(22, 33, 44), sphere.Center);

            // read type-default values
            sphere = (IgesSphere)ParseSingleEntity(@"
     158       1       0       0       0                            0000D      1
     158       0       0       1       0                                D      2
158;                                                                   1P      1
");
            Assert.Equal(0.0, sphere.Radius);
            Assert.Equal(IgesPoint.Origin, sphere.Center);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadTorusTest()
        {
            // fully-specified values
            var torus = (IgesTorus)ParseSingleEntity(@"
     160       1       0       0       0                        00000000D      1
     160       0       0       1       0                                D      2
160,11.,22.,33.,44.,55.,66.,77.,88.;                                   1P      1
");
            Assert.Equal(11.0, torus.RingRadius);
            Assert.Equal(22.0, torus.DiscRadius);
            Assert.Equal(new IgesPoint(33, 44, 55), torus.Center);
            Assert.Equal(new IgesVector(66, 77, 88), torus.Normal);

            // read type-default values
            torus = (IgesTorus)ParseSingleEntity(@"
     160       1       0       0       0                        00000000D      1
     160       0       0       1       0                                D      2
160;                                                                   1P      1
");
            Assert.Equal(0.0, torus.RingRadius);
            Assert.Equal(0.0, torus.DiscRadius);
            Assert.Equal(IgesPoint.Origin, torus.Center);
            Assert.Equal(IgesVector.ZAxis, torus.Normal);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadLeaderTest()
        {
            // fully-specified values
            var leader = (IgesLeader)ParseSingleEntity(@"
     214       1       0       0       0                        00000100D      1
     214       0       0       1       6                                D      2
214,2,8.,9.,3.,1.,2.,4.,5.,6.,7.;                                      1P      1
");
            Assert.Equal(IgesArrowType.FilledCircle, leader.ArrowType);
            Assert.Equal(8.0, leader.ArrowHeight);
            Assert.Equal(9.0, leader.ArrowWidth);
            Assert.Equal(new IgesPoint(1, 2, 3), leader.ArrowheadCoordinates);
            Assert.Equal(2, leader.LineSegments.Count);
            Assert.Equal(new IgesPoint(4, 5, 3), leader.LineSegments.First());
            Assert.Equal(new IgesPoint(6, 7, 3), leader.LineSegments.Last());

            // read type-default values
            leader = (IgesLeader)ParseSingleEntity(@"
     214       1       0       0       0                        00000100D      1
     214       0       0       1       1                                D      2
214;                                                                   1P      1
");
            Assert.Equal(IgesArrowType.Wedge, leader.ArrowType);
            Assert.Equal(0.0, leader.ArrowHeight);
            Assert.Equal(0.0, leader.ArrowWidth);
            Assert.Equal(IgesPoint.Origin, leader.ArrowheadCoordinates);
            Assert.Equal(0, leader.LineSegments.Count);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadTemplateLineFontDefinitionTest()
        {
            var lfd = (IgesTemplateLineFontDefinition)ParseSingleEntity(@"
       0       1       0       0       0                        00000000D      1
       0       0       0       1       0                                D      2
     308       2       0       0       0                        00000200D      3
     308       0       0       1       0                                D      4
     304       3       0       0       0                        00000200D      5
     304       0       0       1       1                                D      6
0;                                                                     1P      1
308,0,3Hfoo,1,1;                                                       3P      2
304,1,3,1.,2.;                                                         5P      3
");
            Assert.Equal(IgesTemplateLineFontOrientation.AlignedToTangent, lfd.Orientation);
            var sub = lfd.Template;
            Assert.Equal("foo", sub.Name);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadPatternLineFontDefinitionTest()
        {
            // fully-specified values
            var lfd = (IgesPatternLineFontDefinition)ParseSingleEntity(@"
     304       1       0       0       0                        00000200D      1
     304       0       0       1       2                                D      2
304,2,1.,2.,2H34;                                                      1P      1
");
            Assert.Equal(2, lfd.SegmentLengths.Count);
            Assert.Equal(1.0, lfd.SegmentLengths[0]);
            Assert.Equal(2.0, lfd.SegmentLengths[1]);
            Assert.Equal(0x34, lfd.DisplayMask);

            // default values
            lfd = (IgesPatternLineFontDefinition)ParseSingleEntity(@"
     304       1       0       0       0                        00000200D      1
     304       0       0       1       2                                D      2
304;                                                                   1P      1
");
            Assert.Equal(0, lfd.SegmentLengths.Count);
            Assert.Equal(0x00, lfd.DisplayMask);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadSubfigureTest()
        {
            var entity = ParseSingleEntity(@"
// The subfigure has two lines; one defined before and one after.       S      1
     110       1       0       0       0                               0D      1
     110       0       0       1       0                                D      2
     308       2       0       0       0                        00000200D      3
     308       0       0       2       0                                D      4
     110       4       0       0       0                               0D      5
     110       0       0       1       0                                D      6
110,1.0,2.0,3.0,4.0,5.0,6.0;                                            P      1
308,0,                                           22Hthis,is;the         P      2
subfigureH,2,1,5;                                                       P      3
110,7.0,8.0,9.0,10.0,11.0,12.0;                                         P      4
");
            Assert.Equal(IgesEntityType.SubfigureDefinition, entity.EntityType);
            var subfigure = (IgesSubfigureDefinition)entity;
            Assert.Equal(0, subfigure.Depth);
            Assert.Equal("this,is;the subfigureH", subfigure.Name);
            Assert.Equal(2, subfigure.Entities.Count);
            Assert.Equal(IgesEntityType.Line, subfigure.Entities[0].EntityType);
            Assert.Equal(IgesEntityType.Line, subfigure.Entities[1].EntityType);
            var line1 = (IgesLine)subfigure.Entities[0];
            Assert.Equal(1.0, line1.P1.X);
            Assert.Equal(2.0, line1.P1.Y);
            Assert.Equal(3.0, line1.P1.Z);
            Assert.Equal(4.0, line1.P2.X);
            Assert.Equal(5.0, line1.P2.Y);
            Assert.Equal(6.0, line1.P2.Z);
            var line2 = (IgesLine)subfigure.Entities[1];
            Assert.Equal(7.0, line2.P1.X);
            Assert.Equal(8.0, line2.P1.Y);
            Assert.Equal(9.0, line2.P1.Z);
            Assert.Equal(10.0, line2.P2.X);
            Assert.Equal(11.0, line2.P2.Y);
            Assert.Equal(12.0, line2.P2.Z);

            // read type-default values
            subfigure = (IgesSubfigureDefinition)ParseSingleEntity(@"
     308       1       0       0       0                        00000200D      1
     308       0       0       1       0                                D      2
308;                                                                    P      1
");
            Assert.Equal(0, subfigure.Depth);
            Assert.Equal(null, subfigure.Name);
            Assert.Equal(0, subfigure.Entities.Count);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadColorDefinitionTest()
        {
            // fully-specified values
            var color = (IgesColorDefinition)ParseSingleEntity(@"
     314       1       0       0       0                        00000200D      1
     314       0       0       1       0                                D      2
314,11.,22.,33.,4Hname;                                                1P      1
");
            Assert.Equal(11.0, color.RedIntensity);
            Assert.Equal(22.0, color.GreenIntensity);
            Assert.Equal(33.0, color.BlueIntensity);
            Assert.Equal("name", color.Name);

            // read type-default values
            color = (IgesColorDefinition)ParseSingleEntity(@"
     314       1       0       0       0                        00000200D      1
     314       0       0       1       0                                D      2
314;                                                                   1P      1
");
            Assert.Equal(0.0, color.RedIntensity);
            Assert.Equal(0.0, color.GreenIntensity);
            Assert.Equal(0.0, color.BlueIntensity);
            Assert.Null(color.Name);

            // read line with custom color
            var line = (IgesLine)ParseSingleEntity(@"
     314       1       0       0       0                        00000200D      1
     314       0       0       1       0                               0D      2
     110       2       0       0       0                               0D      3
     110       0      -1       1       0                               0D      4
314,77,88,99,4Hname;                                                   1P      1
110,11,22,33,44,55,66;                                                 3P      2
");
            Assert.Equal(IgesColorNumber.Custom, line.Color);
            Assert.Equal(77.0, line.CustomColor.RedIntensity);
            Assert.Equal(88.0, line.CustomColor.GreenIntensity);
            Assert.Equal(99.0, line.CustomColor.BlueIntensity);
            Assert.Equal("name", line.CustomColor.Name);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadViewTest()
        {
            // fully-specified values
            var view = (IgesView)ParseSingleEntity(@"
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
            Assert.Equal(1, view.ViewNumber);
            Assert.Equal(2.0, view.ScaleFactor);
            Assert.Equal(3.0, view.ViewVolumeLeft.PlaneCoefficientA);
            Assert.Equal(4.0, view.ViewVolumeTop.PlaneCoefficientA);
            Assert.Equal(5.0, view.ViewVolumeRight.PlaneCoefficientA);
            Assert.Equal(6.0, view.ViewVolumeBottom.PlaneCoefficientA);
            Assert.Equal(7.0, view.ViewVolumeBack.PlaneCoefficientA);
            Assert.Equal(8.0, view.ViewVolumeFront.PlaneCoefficientA);

            // null pointers
            view = (IgesView)ParseSingleEntity(@"
     410       1       0       0       0                        00000100D      1
     410       0       0       1       0                                D      2
410,0,0.,0,0,0,0,0,0;                                                  1P      1
");
            Assert.Null(view.ViewVolumeLeft);
            Assert.Null(view.ViewVolumeTop);
            Assert.Null(view.ViewVolumeRight);
            Assert.Null(view.ViewVolumeBottom);
            Assert.Null(view.ViewVolumeBack);
            Assert.Null(view.ViewVolumeFront);

            // type-default values
            view = (IgesView)ParseSingleEntity(@"
     410       1       0       0       0                        00000100D      1
     410       0       0       1       0                                D      2
410;                                                                   1P      1
");
            Assert.Equal(0, view.ViewNumber);
            Assert.Equal(0.0, view.ScaleFactor);
            Assert.Null(view.ViewVolumeLeft);
            Assert.Null(view.ViewVolumeTop);
            Assert.Null(view.ViewVolumeRight);
            Assert.Null(view.ViewVolumeBottom);
            Assert.Null(view.ViewVolumeBack);
            Assert.Null(view.ViewVolumeFront);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadPerspectiveViewTest()
        {
            // fully-specified values
            var view = (IgesPerspectiveView)ParseSingleEntity(@"
     410       1       0       0       0                        00000100D      1
     410       0       0       2       1                                D      2
410,1,2.,3.,0.,0.,4.,0.,0.,5.,0.,0.,6.,0.,0.,7.,8.,9.,10.,11.,1,       1P      1
12.,13.;                                                               1P      2
");
            Assert.Equal(1, view.ViewNumber);
            Assert.Equal(2.0, view.ScaleFactor);
            Assert.Equal(new IgesVector(3, 0, 0), view.ViewPlaneNormal);
            Assert.Equal(new IgesPoint(4, 0, 0), view.ViewReferencePoint);
            Assert.Equal(new IgesPoint(5, 0, 0), view.CenterOfProjection);
            Assert.Equal(new IgesVector(6, 0, 0), view.ViewUpVector);
            Assert.Equal(7.0, view.ViewPlaneDistance);
            Assert.Equal(8.0, view.ClippingWindowLeftCoordinate);
            Assert.Equal(9.0, view.ClippingWindowRightCoordinate);
            Assert.Equal(10.0, view.ClippingWindowBottomCoordinate);
            Assert.Equal(11.0, view.ClippingWindowTopCoordinate);
            Assert.Equal(IgesDepthClipping.BackClipping, view.DepthClipping);
            Assert.Equal(12.0, view.ClippingWindowBackCoordinate);
            Assert.Equal(13.0, view.ClippingWindowFrontCoordinate);

            // type-default values
            view = (IgesPerspectiveView)ParseSingleEntity(@"
     410       1       0       0       0                        00000100D      1
     410       0       0       1       1                                D      2
410;                                                                   1P      1
");
            Assert.Equal(0, view.ViewNumber);
            Assert.Equal(0.0, view.ScaleFactor);
            Assert.Equal(IgesVector.Zero, view.ViewPlaneNormal);
            Assert.Equal(IgesPoint.Origin, view.ViewReferencePoint);
            Assert.Equal(IgesPoint.Origin, view.CenterOfProjection);
            Assert.Equal(IgesVector.Zero, view.ViewUpVector);
            Assert.Equal(0.0, view.ViewPlaneDistance);
            Assert.Equal(0.0, view.ClippingWindowLeftCoordinate);
            Assert.Equal(0.0, view.ClippingWindowRightCoordinate);
            Assert.Equal(0.0, view.ClippingWindowBottomCoordinate);
            Assert.Equal(0.0, view.ClippingWindowTopCoordinate);
            Assert.Equal(IgesDepthClipping.None, view.DepthClipping);
            Assert.Equal(0.0, view.ClippingWindowBackCoordinate);
            Assert.Equal(0.0, view.ClippingWindowFrontCoordinate);
        }
    }
}
