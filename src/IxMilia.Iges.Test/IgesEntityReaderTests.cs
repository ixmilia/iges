using System.Collections.Generic;
using System.Linq;
using IxMilia.Iges.Entities;
using Xunit;

namespace IxMilia.Iges.Test
{
    public class IgesEntityReaderTests
    {

        #region Private methods

        private static List<IgesEntity> ParseEntities(string content)
        {
            return IgesReaderTests.ParseEntities(content);
        }

        private static IgesEntity ParseLastEntity(string content)
        {
            var last = ParseEntities(content).Last();
            Assert.NotNull(last);
            return last;
        }

        private static IgesFile CreateFile(string content)
        {
            return IgesReaderTests.CreateFile(content);
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
            Assert.Empty(file.Entities);
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
            var circle = (IgesCircularArc)ParseLastEntity(@"
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
            circle = (IgesCircularArc)ParseLastEntity(@"
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
            var entity = ParseEntities(@"
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
")[1];
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
            compositeCurve = (IgesCompositeCurve)ParseLastEntity(@"
     102       1       0       0       0                               0D      1
     102       0       0       1       0                               0D      2
102;                                                                    P      1
");
            Assert.Empty(compositeCurve.Entities);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadConicArcTest()
        {
            // fully-specified values
            var arc = (IgesConicArc)ParseLastEntity(@"
     104       1       0       0       0                        00000000D      1
     104       0       0       1       1                                D      2
104,16.,0.,4.,0.,0.,-64.,7.,8.,9.,10.,11.;                             1P      1
");
            Assert.Equal(IgesArcType.Ellipse, arc.ArcType);
            Assert.Equal(16.0, arc.CoefficientA);
            Assert.Equal(0.0, arc.CoefficientB);
            Assert.Equal(4.0, arc.CoefficientC);
            Assert.Equal(0.0, arc.CoefficientD);
            Assert.Equal(0.0, arc.CoefficientE);
            Assert.Equal(-64.0, arc.CoefficientF);
            Assert.Equal(new IgesPoint(8, 9, 7), arc.StartPoint);
            Assert.Equal(new IgesPoint(10, 11, 7), arc.EndPoint);

            // type-default values
            arc = (IgesConicArc)ParseLastEntity(@"
     104       1       0       0       0                        00000000D      1
     104       0       0       1       0                                D      2
104;                                                                   1P      1
");
            Assert.Equal(IgesArcType.Unknown, arc.ArcType);
            Assert.Equal(0.0, arc.CoefficientA);
            Assert.Equal(0.0, arc.CoefficientB);
            Assert.Equal(0.0, arc.CoefficientC);
            Assert.Equal(0.0, arc.CoefficientD);
            Assert.Equal(0.0, arc.CoefficientE);
            Assert.Equal(0.0, arc.CoefficientF);
            Assert.Equal(IgesPoint.Origin, arc.StartPoint);
            Assert.Equal(IgesPoint.Origin, arc.EndPoint);

            // real-world examples
            arc = (IgesConicArc)ParseLastEntity(@"
     104       1       0       1       0       0               000000000D      1
     104       2       3       1       1                               9D      2
104,0.5,0.,1.,0.,0.,-0.5,0.,0.7575,-0.4616,-0.8971,0.3125;             1P      1
");
            Assert.Equal(IgesArcType.Ellipse, arc.ArcType);

            arc = (IgesConicArc)ParseLastEntity(@"
     104       1       0       1       0       0               000000000D      1
     104       2       4       1       2                              44D      2
104,-0.49,0.,1.96,0.,0.,0.9603999,0.,1.9799,-0.7,1.9799,0.7;           1P      1
");
            Assert.Equal(IgesArcType.Hyperbola, arc.ArcType);

            arc = (IgesConicArc)ParseLastEntity(@"
     104       1       0       1       0       0               000000000D      1
     104       2       2       1       3                              28D      2
104,0.,0.,1.,-4.,0.,0.,0.,0.25,-1.,0.25,1.;                            1P      1
");
            Assert.Equal(IgesArcType.Parabola, arc.ArcType);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadCopiousDataTest()
        {
            var cd = (IgesCopiousData)ParseLastEntity(@"
     106       1       0       0       0                        00000100D      1
     106       0       0       1      40                                D      2
106,1,3,3.,1.,2.,4.,5.,6.,7.;                                          1P      1
");
            Assert.Equal(IgesCopiousDataType.WitnessLine, cd.DataType);
            Assert.Empty(cd.DataVectors);
            Assert.Equal(3, cd.DataPoints.Count);
            Assert.Equal(new IgesPoint(1, 2, 3), cd.DataPoints[0]);
            Assert.Equal(new IgesPoint(4, 5, 3), cd.DataPoints[1]);
            Assert.Equal(new IgesPoint(6, 7, 3), cd.DataPoints[2]);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadPlaneTest()
        {
            // unbounded
            var plane = (IgesPlane)ParseLastEntity(@"
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
            plane = (IgesPlane)ParseLastEntity(@"
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
            plane = (IgesPlane)ParseLastEntity(@"
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
            var line = (IgesLine)ParseLastEntity(@"
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
            line = (IgesLine)ParseLastEntity(@"
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
            var line = (IgesLine)ParseLastEntity(@"
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
        public void ReadParametricSplineCurveTest()
        {
            var psc = (IgesParametricSplineCurve)ParseLastEntity(@"
     112       1       0       0       0                        00000000D      1
     112       0       0       2       0                                D      2
112,6,1,2,1,3.,4.,5.,6.,7.,8.,9.,10.,11.,12.,13.,14.,15.,16.,          1P      1
17.,18.,19.,20.,21.,22.,23.,24.,25.,26.,27.;                           1P      2
");
            Assert.Equal(IgesSplineType.BSpline, psc.SplineType);
            Assert.Equal(1, psc.DegreeOfContinuity);
            Assert.Equal(2, psc.NumberOfDimensions);
            var segment = psc.Segments.Single();
            Assert.Equal(3.0, segment.BreakPoint);
            Assert.Equal(4.0, segment.AX);
            Assert.Equal(5.0, segment.BX);
            Assert.Equal(6.0, segment.CX);
            Assert.Equal(7.0, segment.DX);
            Assert.Equal(8.0, segment.AY);
            Assert.Equal(9.0, segment.BY);
            Assert.Equal(10.0, segment.CY);
            Assert.Equal(11.0, segment.DY);
            Assert.Equal(12.0, segment.AZ);
            Assert.Equal(13.0, segment.BZ);
            Assert.Equal(14.0, segment.CZ);
            Assert.Equal(15.0, segment.DZ);
            Assert.Equal(16.0, segment.XValue);
            Assert.Equal(17.0, segment.XFirstDerivative);
            Assert.Equal(18.0, segment.XSecondDerivative);
            Assert.Equal(19.0, segment.XThirdDerivative);
            Assert.Equal(20.0, segment.YValue);
            Assert.Equal(21.0, segment.YFirstDerivative);
            Assert.Equal(22.0, segment.YSecondDerivative);
            Assert.Equal(23.0, segment.YThirdDerivative);
            Assert.Equal(24.0, segment.ZValue);
            Assert.Equal(25.0, segment.ZFirstDerivative);
            Assert.Equal(26.0, segment.ZSecondDerivative);
            Assert.Equal(27.0, segment.ZThirdDerivative);
        }

        [Fact(Skip = "need additional spec"), Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadParametricSplineSurfaceTest()
        {
            var pss = (IgesParametricSplineSurface)ParseLastEntity(@"
     114       1       0       1       0       0       0       000000001D      1
     114       2       2      77       0                                D      2
114,3,1,1,3,0.,1.,0.,1.,2.,3.,10.5,0.,0.,0.,-0.166666,                 1P      1
8.583070000000001E-006,-8.583070000000001E-006,0.,                     1P      2
-1.430510000000000E-006,-1.502040000000000E-005,                       1P      3
1.502040000000000E-005,0.,-0.0833325,6.437300000000000E-006,           1P      4
-6.437300000000000E-006,0.,11.5,-0.75,0.,0.,-0.0999956,                1P      5
8.583070000000001E-006,-8.583070000000001E-006,0.,                     1P      6
-4.529950000000000E-006,-8.583070000000001E-006,                       1P      7
8.583070000000001E-006,0.,0.100001,2.861020000000000E-006,             1P      8
-2.861020000000000E-006,0.,1.,-1.,-1.192090000000000E-007,             1P      9
5.960460000000000E-008,0.,0.,0.,0.,0.,5.364420000000000E-007,          1P     10
-7.152560000000000E-007,1.788140000000000E-007,0.,                     1P     11
-3.576280000000000E-007,4.768370000000000E-007,                        1P     12
-1.192090000000000E-007,10.25,0.,0.,0.,-0.416667,                      1P     13
-2.145770000000000E-006,2.145770000000000E-006,0.,-0.249998,           1P     14
2.145770000000000E-006,-2.145770000000000E-006,0.,0.166666,0.,         1P     15
0.,0.,11.5,-0.749997,-2.861020000000000E-006,0.,0.199999,0.,0.,        1P     16
0.,0.299999,0.,0.,0.,-0.249999,0.,0.,0.,1.,-1.,                        1P     17
-3.576280000000000E-007,1.192090000000000E-007,0.,0.,0.,0.,0.,         1P     18
0.,0.,0.,0.,0.,0.,0.,9.75,0.,0.,0.,-0.416666,                          1P     19
2.145770000000000E-006,-2.145770000000000E-006,0.,0.249998,            1P     20
4.291530000000000E-006,-4.291530000000000E-006,0.,-0.0833328,          1P     21
-6.437300000000000E-006,6.437300000000000E-006,0.,11.75,               1P     22
-0.749997,-2.861020000000000E-006,0.,0.0500009,0.,0.,0.,               1P     23
-0.449999,0.,0.,0.,0.149998,-2.861020000000000E-006,                   1P     24
2.861020000000000E-006,0.,1.,-1.,-3.576280000000000E-007,              1P     25
1.192090000000000E-007,0.,0.,0.,0.,0.,-5.364420000000000E-007,         1P     26
7.152560000000000E-007,-1.788140000000000E-007,0.,                     1P     27
3.576280000000000E-007,-4.768370000000000E-007,                        1P     28
1.192090000000000E-007,9.5,0.,0.,0.,-0.166669,                         1P     29
-8.583070000000001E-006,8.583070000000001E-006,0.,0.500007,            1P     30
2.574920000000000E-005,-2.574920000000000E-005,0.,-0.333338,           1P     31
-1.716610000000000E-005,1.716610000000000E-005,0.,11.5,-0.75,0.,       1P     32
0.,-0.400002,-8.583070000000001E-006,8.583070000000001E-006,0.,        1P     33
1.20001,2.574920000000000E-005,-2.574920000000000E-005,0.,             1P     34
-0.800005,-1.716610000000000E-005,1.716610000000000E-005,0.,1.,        1P     35
-1.,-1.192090000000000E-007,5.960460000000000E-008,0.,0.,0.,0.,        1P     36
0.,0.,0.,0.,0.,0.,0.,0.,10.5,0.,0.,0.,-0.166666,                       1P     37
-8.583070000000001E-006,2.574920000000000E-005,                        1P     38
-1.716610000000000E-005,-1.430510000000000E-006,                       1P     39
1.502040000000000E-005,-4.506110000000000E-005,                        1P     40
3.004070000000000E-005,-0.0833325,-6.437300000000000E-006,             1P     41
1.931190000000000E-005,-1.287460000000000E-005,10.75,-0.75,2.25,       1P     42
-1.5,-0.0999956,-8.583070000000001E-006,2.574920000000000E-005,        1P     43
-1.716610000000000E-005,-4.529950000000000E-006,                       1P     44
8.583070000000001E-006,-2.574920000000000E-005,                        1P     45
1.716610000000000E-005,0.100001,-2.861020000000000E-006,               1P     46
8.583070000000001E-006,-5.722050000000000E-006,0.,-1.,3.,-2.,0.,       1P     47
0.,0.,0.,0.,-3.576280000000000E-007,1.072880000000000E-006,            1P     48
-7.152560000000000E-007,0.,2.384190000000000E-007,                     1P     49
-7.152560000000000E-007,4.768370000000000E-007,10.25,0.,0.,0.,         1P     50
-0.416667,2.145770000000000E-006,-6.437300000000000E-006,              1P     51
4.291530000000000E-006,-0.249998,-2.145770000000000E-006,              1P     52
6.437300000000000E-006,-4.291530000000000E-006,0.166666,0.,0.,         1P     53
0.,10.75,-0.750003,2.25001,-1.50001,0.199999,0.,0.,0.,0.299999,        1P     54
0.,0.,0.,-0.249999,0.,0.,0.,0.,-1.,3.,-2.,0.,0.,0.,0.,0.,0.,0.,        1P     55
0.,0.,0.,0.,0.,9.75,0.,0.,0.,-0.416666,-2.145770000000000E-006,        1P     56
6.437300000000000E-006,-4.291530000000000E-006,0.249998,               1P     57
-4.291530000000000E-006,1.287460000000000E-005,                        1P     58
-8.583070000000001E-006,-0.0833328,6.437300000000000E-006,             1P     59
-1.931190000000000E-005,1.287460000000000E-005,11.,-0.750003,          1P     60
2.25001,-1.50001,0.0500009,0.,0.,0.,-0.449999,0.,0.,0.,0.149998,       1P     61
2.861020000000000E-006,-8.583070000000001E-006,                        1P     62
5.722050000000000E-006,0.,-1.,3.,-2.,0.,0.,0.,0.,0.,                   1P     63
3.576280000000000E-007,-1.072880000000000E-006,                        1P     64
7.152560000000000E-007,0.,-2.384190000000000E-007,                     1P     65
7.152560000000000E-007,-4.768370000000000E-007,9.5,0.,0.,0.,           1P     66
-0.166669,8.583070000000001E-006,-2.574920000000000E-005,              1P     67
1.716610000000000E-005,0.500007,-2.574920000000000E-005,               1P     68
7.724760000000000E-005,-5.149840000000000E-005,-0.333338,              1P     69
1.716610000000000E-005,-5.149840000000000E-005,                        1P     70
3.433230000000000E-005,10.75,-0.75,2.25,-1.5,-0.400002,                1P     71
8.583070000000001E-006,-2.574920000000000E-005,                        1P     72
1.716610000000000E-005,1.20001,-2.574920000000000E-005,                1P     73
7.724760000000000E-005,-5.149840000000000E-005,-0.800005,              1P     74
1.716610000000000E-005,-5.149840000000000E-005,                        1P     75
3.433230000000000E-005,0.,-1.,3.,-2.,0.,0.,0.,0.,0.,0.,0.,0.,0.,       1P     76
0.,0.,0.;                                                              1P     77
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadLocationTest()
        {
            var location = (IgesLocation)ParseLastEntity(@"
     116       1       0       0       0                               0D      1
     116       0       0       1       0                                D      2
116,11.,22.,33.;                                                       1P      1
");
            Assert.Equal(11.0, location.X);
            Assert.Equal(22.0, location.Y);
            Assert.Equal(33.0, location.Z);
            Assert.Equal(IgesColorNumber.Default, location.Color);

            // read type-default values
            location = (IgesLocation)ParseLastEntity(@"
     116       1       0       0       0                               0D      1
     116       0       0       1       0                                D      2
116;                                                                   1P      1
");
            Assert.Equal(0.0, location.X);
            Assert.Equal(0.0, location.Y);
            Assert.Equal(0.0, location.Z);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadRuledSurfaceTest()
        {
            var ruledSurface = (IgesRuledSurface)ParseLastEntity(@"
     110       1       0       0       0                        00000000D      1
     110       0       0       1       0                                D      2
     110       2       0       0       0                        00000000D      3
     110       0       0       1       0                                D      4
     118       3       0       0       0                        00000000D      5
     118       0       0       1       0                                D      6
110,1.,0.,0.,0.,0.,0.;                                                 1P      1
110,2.,0.,0.,0.,0.,0.;                                                 3P      2
118,1,3,1,1;                                                           5P      3
");
            Assert.Equal(1.0, ((IgesLine)ruledSurface.FirstCurve).P1.X);
            Assert.Equal(2.0, ((IgesLine)ruledSurface.SecondCurve).P1.X);
            Assert.Equal(IgesRuledSurfaceDirection.FirstToLast_LastToFirst, ruledSurface.Direction);
            Assert.True(ruledSurface.IsDevelopable);

            // read type-default values
            ruledSurface = (IgesRuledSurface)ParseLastEntity(@"
     118       1       0       0       0                        00000000D      1
     118       0       0       1       0                                D      2
118;                                                                   1P      1
");
            Assert.Null(ruledSurface.FirstCurve);
            Assert.Null(ruledSurface.SecondCurve);
            Assert.Equal(IgesRuledSurfaceDirection.FirstToFirst_LastToLast, ruledSurface.Direction);
            Assert.False(ruledSurface.IsDevelopable);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadSurfaceOfRevolutionTest()
        {
            var surface = (IgesSurfaceOfRevolution)ParseLastEntity(@"
     110       1       0       0       0                        00000000D      1
     110       0       0       1       0                                D      2
     110       2       0       0       0                        00000000D      3
     110       0       0       1       0                                D      4
     120       3       0       0       0                        00000000D      5
     120       0       0       1       0                                D      6
110,1.,0.,0.,0.,0.,0.;                                                 1P      1
110,2.,0.,0.,0.,0.,0.;                                                 3P      2
120,1,3,3.,4.;                                                         5P      3
");
            Assert.Equal(1.0, surface.AxisOfRevolution.P1.X);
            Assert.Equal(2.0, ((IgesLine)surface.Generatrix).P1.X);
            Assert.Equal(3.0, surface.StartAngle);
            Assert.Equal(4.0, surface.EndAngle);

            // read type-default values
            surface = (IgesSurfaceOfRevolution)ParseLastEntity(@"
     120       1       0       0       0                        00000000D      1
     120       0       0       1       0                                D      2
120;                                                                   1P      1
");
            Assert.Null(surface.AxisOfRevolution);
            Assert.Null(surface.Generatrix);
            Assert.Equal(0.0, surface.StartAngle);
            Assert.Equal(0.0, surface.EndAngle);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadTabulatedCylinderTest()
        {
            var tab = (IgesTabulatedCylinder)ParseLastEntity(@"
     110       1       0       0       0                        00000000D      1
     110       0       0       1       0                                D      2
     122       2       0       0       0                        00000000D      3
     122       0       0       1       0                                D      4
110,0.,0.,0.,0.,0.,0.;                                                 1P      1
122,1,1.,2.,3.;                                                        3P      2
");
            Assert.IsType<IgesLine>(tab.Directrix);
            Assert.Equal(new IgesPoint(1, 2, 3), tab.GeneratrixTerminatePoint);

            // read type-default values
            tab = (IgesTabulatedCylinder)ParseLastEntity(@"
     122       1       0       0       0                        00000000D      1
     122       0       0       1       0                                D      2
122;                                                                   1P      1
");
            Assert.Null(tab.Directrix);
            Assert.Equal(IgesPoint.Origin, tab.GeneratrixTerminatePoint);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadDirectionTest()
        {
            var direction = (IgesDirection)ParseLastEntity(@"
     123       1       0       0       0                        00010200D      1
     123       0       0       1       0                                D      2
123,11.,22.,33.;                                                       1P      1
");
            Assert.Equal(11.0, direction.X);
            Assert.Equal(22.0, direction.Y);
            Assert.Equal(33.0, direction.Z);

            // read type-default values
            direction = (IgesDirection)ParseLastEntity(@"
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
            var matrix = (IgesTransformationMatrix)ParseLastEntity(@"
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
            matrix = (IgesTransformationMatrix)ParseLastEntity(@"
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
        public void ReadFlashTest()
        {
            // regular case
            var flash = (IgesFlash)ParseLastEntity(@"
     125       1       0       0       0                        00000000D      1
     125       0       0       1       3                                D      2
125,1.,2.,3.,4.,5.,0;                                                  1P      1
");
            Assert.Equal(1.0, flash.XOffset);
            Assert.Equal(2.0, flash.YOffset);
            Assert.Equal(3.0, flash.SizeParameter1);
            Assert.Equal(4.0, flash.SizeParameter2);
            Assert.Equal(5.0, flash.RotationAngle);
            Assert.Equal(IgesClosedAreaType.Donut, flash.AreaType);
            Assert.Null(flash.ReferenceEntity);

            // referenced entity case
            flash = (IgesFlash)ParseLastEntity(@"
     100       1       0       0       0                        00000000D      1
     100       0       0       1       0                                D      2
     125       2       0       0       0                        00000000D      3
     125       0       0       1       0                                D      4
100,0.,0.,0.,0.,0.,0.,0.;                                              1P      1
125,1.,2.,3.,4.,5.,1;                                                  3P      2
");
            Assert.Equal(IgesClosedAreaType.ReferencedEntity, flash.AreaType);
            Assert.IsType<IgesCircularArc>(flash.ReferenceEntity);

            // read type-default values
            flash = (IgesFlash)ParseLastEntity(@"
     125       1       0       0       0                        00000000D      1
     125       0       0       1       3                                D      2
125;                                                                   1P      1
");
            Assert.Equal(0.0, flash.XOffset);
            Assert.Equal(0.0, flash.YOffset);
            Assert.Equal(0.0, flash.SizeParameter1);
            Assert.Equal(0.0, flash.SizeParameter2);
            Assert.Equal(0.0, flash.RotationAngle);
            Assert.Equal(IgesClosedAreaType.Donut, flash.AreaType);
            Assert.Null(flash.ReferenceEntity);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadRationalBSplineCurveTest()
        {
            var curve = (IgesRationalBSplineCurve)ParseLastEntity(@"
     126       1       0       1       2       0       0       000000001D      1
     126       3       2       3       0                                D      2
126,5,3,1,0,1,0,0.,0.,0.,0.,0.333333,0.666667,1.,1.,1.,1.,1.,1.,       1P      1
1.,1.,1.,1.,-178.,109.,0.,-166.,128.,0.,-144.,109.,0.,-109.,           1P      2
112.,0.,-106.,134.,0.,-119.,138.,0.,0.,1.,0.,0.,1.;                    1P      3
");
            Assert.Equal(IgesSplineCurveType.Custom, curve.CurveType);
            Assert.True(curve.IsPlanar);
            Assert.False(curve.IsClosed);
            Assert.True(curve.IsPolynomial);
            Assert.False(curve.IsPeriodic);
            Assert.Equal(new List<double>() { 0.0, 0.0, 0.0, 0.0, 0.333333, 0.666667, 1.0, 1.0, 1.0, 1.0 }, curve.KnotValues);
            Assert.Equal(new List<double>() { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 }, curve.Weights);
            Assert.Equal(6, curve.ControlPoints.Count);
            Assert.Equal(new IgesPoint(-178, 109, 0), curve.ControlPoints.First());
            Assert.Equal(new IgesPoint(-119, 138, 0), curve.ControlPoints.Last());
            Assert.Equal(0.0, curve.StartParameter);
            Assert.Equal(1.0, curve.EndParameter);
            Assert.Equal(IgesVector.ZAxis, curve.Normal);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadRationalBSplineSurfaceTest()
        {
            var surface = (IgesRationalBSplineSurface)ParseLastEntity(@"
     128       1       0       0       0                        00000000D      1
     128       0       0       2       0                                D      2
128,1,1,-2,-2,1,1,1,0,0,-1.,-2.,1.,3.,2.,4.,5.,6.,0.,9.,10.,0.,        1P      1
7.,8.,0.,11.,12.,0.,0.,1.,0.,1.;                                       1P      2
");
            Assert.Equal(IgesSplineSurfaceType.Custom, surface.SurfaceType);
            Assert.True(surface.IsClosedInFirstParametricVariable);
            Assert.True(surface.IsClosedInSecondParametricVariable);
            Assert.True(surface.IsPolynomial);
            Assert.False(surface.IsPeriodicInFirstParametricVariable);
            Assert.False(surface.IsPeriodicInSecondParametricVariable);
            Assert.Equal(0.0, surface.FirstParametricStartingValue);
            Assert.Equal(1.0, surface.FirstParametricEndingValue);
            Assert.Equal(0.0, surface.SecondParametricStartingValue);
            Assert.Equal(1.0, surface.SecondParametricEndingValue);
            Assert.Equal(2, surface.Weights.GetLength(0));
            Assert.Equal(2, surface.Weights.GetLength(1));
            Assert.Equal(1.0, surface.Weights[0, 0]);
            Assert.Equal(3.0, surface.Weights[1, 0]);
            Assert.Equal(2.0, surface.Weights[0, 1]);
            Assert.Equal(4.0, surface.Weights[1, 1]);
            Assert.Equal(new IgesPoint(5, 6, 0), surface.ControlPoints[0, 0]);
            Assert.Equal(new IgesPoint(9, 10, 0), surface.ControlPoints[1, 0]);
            Assert.Equal(new IgesPoint(7, 8, 0), surface.ControlPoints[0, 1]);
            Assert.Equal(new IgesPoint(11, 12, 0), surface.ControlPoints[1, 1]);
            Assert.Equal(new[] { -1.0 }, surface.FirstKnotValueSequence);
            Assert.Equal(new[] { -2.0 }, surface.SecondKnotValueSequence);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadOffsetCurveTest()
        {
            var curve = (IgesOffsetCurve)ParseLastEntity(@"
     110       1       0       0       0                        00000000D      1
     110       0       0       1       0                                D      2
     130       2       0       0       0                        00000000D      3
     130       0       0       1       0                                D      4
110,0.,0.,0.,0.,0.,0.;                                                 1P      1
130,1,1,0,0,0,1.,2.,3.,4.,0.,0.,1.,5.,6.;                              3P      2
");
            Assert.IsType<IgesLine>(curve.CurveToOffset);
            Assert.Equal(IgesOffsetDistanceType.SingleUniformOffset, curve.DistanceType);
            Assert.Null(curve.EntityOffsetCurveFunction);
            Assert.Equal(0, curve.ParameterIndexOfFunctionEntityCurve);
            Assert.Equal(IgesTaperedOffsetType.None, curve.TaperedOffsetType);
            Assert.Equal(1.0, curve.FirstOffsetDistance);
            Assert.Equal(2.0, curve.FirstOffsetDistanceValue);
            Assert.Equal(3.0, curve.SecondOffsetDistance);
            Assert.Equal(4.0, curve.SecondOffsetDistanceValue);
            Assert.Equal(IgesVector.ZAxis, curve.EntityNormal);
            Assert.Equal(5.0, curve.StartingParameterValue);
            Assert.Equal(6.0, curve.EndingParameterValue);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadConnectPointTest()
        {
            var point = (IgesConnectPoint)ParseLastEntity(@"
     132       1       0       0       0                        00000400D      1
     132       0       0       1       0                                D      2
132,1.,2.,3.,0,102,1,7Hfunc-id,0,9Hfunc-name,0,42,20,0,0;              1P      1
");
            Assert.Equal(new IgesPoint(1, 2, 3), point.Location);
            Assert.Null(point.DisplaySymbolGeometry);
            Assert.Equal(IgesConnectionType.LogicalPortConnector, point.ConnectionType);
            Assert.Equal(IgesConnectionFunctionType.ElectricalSignal, point.FunctionType);
            Assert.Equal("func-id", point.FunctionIdentifier);
            Assert.Null(point.FunctionIdentifierTextDisplayTemplate);
            Assert.Equal("func-name", point.FunctionName);
            Assert.Null(point.FunctionNameTextDisplayTemplate);
            Assert.Equal(42, point.UniqueIdentifier);
            Assert.Equal(IgesConnectionFunctionCode.InvertingOutput, point.FunctionCode);
            Assert.True(point.ConnectPointMayBeSwapped);
            Assert.Null(point.Owner);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadNodeTest()
        {
            var node = (IgesNode)ParseLastEntity(@"
     134       1       0       0       0                        00000400D      1
     134       0       0       1       0                              17D      2
134,1.,2.,3.,0;                                                        1P      1
");
            Assert.Equal(new IgesPoint(1, 2, 3), node.Offset);
            Assert.Equal(17u, node.NodeNumber);
            Assert.Null(node.DisplacementCoordinateSystem);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadFiniteElementBeamTest()
        {
            var beam = (IgesBeam)ParseLastEntity(@"
     134       1       0       0       0                        00000400D      1
     134       0       0       1       0                                D      2
     134       2       0       0       0                        00000400D      3
     134       0       0       1       0                                D      4
     136       3       0       0       0                        00000000D      5
     136       0       0       1       0                                D      6
134,1.,2.,3.,0;                                                        1P      1
134,4.,5.,6.,0;                                                        3P      2
136,1,2,1,3,4Hname;                                                    5P      3
");
            Assert.Equal(new IgesPoint(1, 2, 3), beam.P1);
            Assert.Equal(new IgesPoint(4, 5, 6), beam.P2);
            Assert.Equal("name", beam.ElementTypeName);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadCustomFiniteElementTest()
        {
            var custom = (IgesCustomFiniteElement)ParseLastEntity(@"
     134       1       0       0       0                        00000400D      1
     134       0       0       1       0                                D      2
     136       2       0       0       0                        00000000D      3
     136       0       0       1       0                                D      4
134,1.,2.,3.,0;                                                        1P      1
136,5002,1,1,6Hcustom;                                                 3P      2
");
            Assert.Equal(5001, (int)custom.TopologyType);
            Assert.Equal(5002, custom.CustomTopologyNumber);
            Assert.Equal(new IgesPoint(1, 2, 3), custom.Nodes.Single().Offset);
            Assert.Equal("custom", custom.ElementTypeName);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadNodalDisplacementAndRotationTest()
        {
            var file = CreateFile(@"
     212       1       0       0       0                        00000100D      1
     212       0       0       1       0                                D      2
     134       2       0       0       0                        00000400D      3
     134       0       0       1       0                                D      4
     134       3       0       0       0                        00000400D      5
     134       0       0       1       0                                D      6
     136       4       0       0       0                        00000000D      7
     136       0       0       1       0                                D      8
     138       5       0       0       0                        00000000D      9
     138       0       0       1       0                                D     10
212,0;                                                                 1P      1
134,8.,9.,10.,0;                                                       3P      2
134,11.,12.,13.,0;                                                     5P      3
136,1,2,3,5,;                                                          7P      4
138,1,1,1,42,7,2.,3.,4.,5.,6.,7.;                                      9P      5
");
            Assert.Single(file.Entities.OfType<IgesBeam>());
            Assert.Single(file.Entities.OfType<IgesNodalDisplacementAndRotation>());
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadOffsetSurfaceTest()
        {
            var offset = (IgesOffsetSurface)ParseLastEntity(@"
     118       1       0       0       0                        00000000D      1
     118       0       0       1       0                                D      2
     140       2       0       0       0                        00000000D      3
     140       0       0       1       0                                D      4
118,0,0,0,0;                                                           1P      1
140,1.,2.,3.,4.,1;                                                     3P      2
");
            Assert.Equal(new IgesVector(1, 2, 3), offset.Direction);
            Assert.Equal(4.0, offset.Distance);
            Assert.IsType<IgesRuledSurface>(offset.Surface);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadTrimmedParametricSurfaceTest()
        {
            var surface = (IgesTrimmedParametricSurface)ParseLastEntity(@"
     118       1       0       0       0                        00000000D      1
     118       0       0       1       0                                D      2
     118       2       0       0       0                        00000000D      3
     118       0       0       1       0                                D      4
     118       3       0       0       0                        00000000D      5
     118       0       0       1       0                                D      6
     144       4       0       0       0                        00000000D      7
     144       0       0       1       0                                D      8
118,0,0,0,0;                                                           1P      1
118,0,0,0,0;                                                           3P      2
118,0,0,0,0;                                                           5P      3
144,1,1,1,3,5;                                                         7P      4
");
            Assert.IsType<IgesRuledSurface>(surface.Surface);
            Assert.False(surface.IsOuterBoundaryD);
            Assert.IsType<IgesRuledSurface>(surface.OuterBoundary);
            Assert.IsType<IgesRuledSurface>(surface.BoundaryEntities.Single());
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadNodalResultsTest()
        {
            var nodalResults = (IgesNodalResults)ParseLastEntity(@"
     146       1       0       0       0                        00000000D      1
     146       0       0       1       1                                D      2
146,0,0,18H15H20000101.000000,1,1,0,0,42.;                             1P      1
");
            Assert.Equal(new System.DateTime(2000, 1, 1, 0, 0, 0), nodalResults.AnalysisTime);
            Assert.Equal(IgesResultType.Temperature, nodalResults.ResultsType);
            Assert.Equal(42.0, nodalResults.Results.Single().Values.Single());
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadElementResultsTest()
        {
            var elementResults = (IgesElementResults)ParseLastEntity(@"
     148       1       0       0       0                        00000000D      1
     148       0       0       1       0                                D      2
148,0,0,15H00010101.000000,0,1,0;                                      1P      1
");
            Assert.Equal(IgesResultsReportingType.ElementCentroid, elementResults.ReportingType);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadBlockTest()
        {
            var block = (IgesBlock)ParseLastEntity(@"
     150       1       0       0       0                        00000000D      1
     150       0       0       1       0                                D      2
150,1.,2.,3.,4.,5.,6.,1.,0.,0.,0.,0.,1.;                               1P      1
");
            Assert.Equal(1.0, block.XLength);
            Assert.Equal(2.0, block.YLength);
            Assert.Equal(3.0, block.ZLength);
            Assert.Equal(new IgesPoint(4.0, 5.0, 6.0), block.Corner);
            Assert.Equal(IgesVector.XAxis, block.XAxis);
            Assert.Equal(IgesVector.ZAxis, block.ZAxis);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadRightAngularWedgeTest()
        {
            var wedge = (IgesRightAngularWedge)ParseLastEntity(@"
     152       1       0       0       0                        00000000D      1
     152       0       0       1       0                                D      2
152,1.,2.,3.,0.,0.,0.,0.,1.,0.,0.,0.,0.,1.;                            1P      1
");
            Assert.Equal(1.0, wedge.XAxisSize);
            Assert.Equal(2.0, wedge.YAxisSize);
            Assert.Equal(3.0, wedge.ZAxisSize);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadRightCircularCylinderTest()
        {
            var cylinder = (IgesRightCircularCylinder)ParseLastEntity(@"
     154       1       0       0       0                        00000000D      1
     154       0       0       1       0                                D      2
154,1.,2.,0.,0.,0.,0.,0.,1.;                                           1P      1
");
            Assert.Equal(1.0, cylinder.Height);
            Assert.Equal(2.0, cylinder.Radius);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadRightCircularConeFrustrumTest()
        {
            var cone = (IgesRightCircularConeFrustrum)ParseLastEntity(@"
     156       1       0       0       0                        00000000D      1
     156       0       0       1       0                                D      2
156,1.,2.,3.,0.,0.,0.,0.,0.,1.;                                        1P      1
");
            Assert.Equal(1.0, cone.Height);
            Assert.Equal(2.0, cone.LargeFaceRadius);
            Assert.Equal(3.0, cone.SmallFaceRadius);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadSphereTest()
        {
            // fully-specified values
            var sphere = (IgesSphere)ParseLastEntity(@"
     158       1       0       0       0                            0000D      1
     158       0       0       1       0                                D      2
158,11.,22.,33.,44.;                                                   1P      1
");
            Assert.Equal(11.0, sphere.Radius);
            Assert.Equal(new IgesPoint(22, 33, 44), sphere.Center);

            // read type-default values
            sphere = (IgesSphere)ParseLastEntity(@"
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
            var torus = (IgesTorus)ParseLastEntity(@"
     160       1       0       0       0                        00000000D      1
     160       0       0       1       0                                D      2
160,11.,22.,33.,44.,55.,66.,77.,88.;                                   1P      1
");
            Assert.Equal(11.0, torus.RingRadius);
            Assert.Equal(22.0, torus.DiscRadius);
            Assert.Equal(new IgesPoint(33, 44, 55), torus.Center);
            Assert.Equal(new IgesVector(66, 77, 88), torus.Normal);

            // read type-default values
            torus = (IgesTorus)ParseLastEntity(@"
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
        public void ReadSolidOfRevolutionTest()
        {
            var solid = (IgesSolidOfRevolution)ParseLastEntity(@"
     110       1       0       0       0                        00000000D      1
     110       0       0       1       0                                D      2
     162       2       0       0       0                        00000000D      3
     162       0       0       1       0                                D      4
110,1.,2.,3.,0.,0.,0.;                                                 1P      1
162,1,0.5,0.,0.,0.,1.,0.,0.;                                           3P      2
");
            Assert.Equal(new IgesPoint(1.0, 2.0, 3.0), ((IgesLine)solid.Curve).P1);
            Assert.Equal(0.5, solid.RevolutionAmount);
            Assert.Equal(IgesVector.XAxis, solid.AxisDirection);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadSolidOfLinearExtrusionTest()
        {
            var solid = (IgesSolidOfLinearExtrusion)ParseLastEntity(@"
     100       1       0       0       0                        00000000D      1
     100       0       0       1       0                                D      2
     164       2       0       0       0                        00000000D      3
     164       0       0       1       0                                D      4
100,0.,0.,0.,0.,0.,0.,0.;                                              1P      1
164,1,3.,0.,0.,1.;                                                     3P      2
");
            Assert.IsType<IgesCircularArc>(solid.Curve);
            Assert.Equal(3.0, solid.ExtrusionLength);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadEllipsoidTest()
        {
            var el = (IgesEllipsoid)ParseLastEntity(@"
     168       1       0       0       0                        00000000D      1
     168       0       0       1       0                                D      2
168,1.,2.,3.,4.,5.,6.,7.,8.,9.,10.,11.,12.;                            1P      1
");
            Assert.Equal(1.0, el.XAxisLength);
            Assert.Equal(2.0, el.YAxisLength);
            Assert.Equal(3.0, el.ZAxisLength);
            Assert.Equal(new IgesPoint(4.0, 5.0, 6.0), el.Center);
            Assert.Equal(new IgesVector(7.0, 8.0, 9.0), el.XAxis);
            Assert.Equal(new IgesVector(10.0, 11.0, 12.0), el.ZAxis);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadBooleanTreeTest()
        {
            //       U
            //    /     \
            //   -       I
            //  / \     / \
            // A   U   D   E
            //    / \
            //   B   C
            var tree = (IgesBooleanTree)ParseLastEntity(@"
     110       1       0       0       0                        00000000D      1
     110       0       0       1       0                       A        D      2
     110       2       0       0       0                        00000000D      3
     110       0       0       1       0                       B        D      4
     110       3       0       0       0                        00000000D      5
     110       0       0       1       0                       C        D      6
     110       4       0       0       0                        00000000D      7
     110       0       0       1       0                       D        D      8
     110       5       0       0       0                        00000000D      9
     110       0       0       1       0                       E        D     10
     180       6       0       0       0                        00000000D     11
     180       0       0       1       0                                D     12
110,0.,0.,0.,0.,0.,0.;                                                 1P      1
110,0.,0.,0.,0.,0.,0.;                                                 3P      2
110,0.,0.,0.,0.,0.,0.;                                                 5P      3
110,0.,0.,0.,0.,0.,0.;                                                 7P      4
110,0.,0.,0.,0.,0.,0.;                                                 9P      5
180,9,-1,-3,-5,1,3,-7,-9,2,1;                                         11P      6
");
            var root = (IgesBooleanTreeOperation)tree.RootNode;
            Assert.Equal(IgesBooleanTreeOperationKind.Union, root.OperationKind);

            var left = (IgesBooleanTreeOperation)root.LeftChild;
            Assert.Equal(IgesBooleanTreeOperationKind.Difference, left.OperationKind);
            Assert.Equal("A", ((IgesBooleanTreeEntity)left.LeftChild).Entity.EntityLabel);

            var leftRight = (IgesBooleanTreeOperation)left.RightChild;
            Assert.Equal(IgesBooleanTreeOperationKind.Union, leftRight.OperationKind);
            Assert.Equal("B", ((IgesBooleanTreeEntity)leftRight.LeftChild).Entity.EntityLabel);
            Assert.Equal("C", ((IgesBooleanTreeEntity)leftRight.RightChild).Entity.EntityLabel);

            var right = (IgesBooleanTreeOperation)root.RightChild;
            Assert.Equal(IgesBooleanTreeOperationKind.Intersection, right.OperationKind);
            Assert.Equal("D", ((IgesBooleanTreeEntity)right.LeftChild).Entity.EntityLabel);
            Assert.Equal("E", ((IgesBooleanTreeEntity)right.RightChild).Entity.EntityLabel);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadSelectedComponentTest()
        {
            var selected = (IgesSelectedComponent)ParseLastEntity(@"
     180       1       0       0       0                        00000000D      1
     180       0       0       1       0                                D      2
     182       2       0       0       0                        00000300D      3
     182       0       0       1       0                                D      4
180,0;                                                                 1P      1
182,1,1.,2.,3.;                                                        3P      2
");
            Assert.NotNull(selected.BooleanTree);
            Assert.Equal(new IgesPoint(1.0, 2.0, 3.0), selected.SelectionPoint);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadSolidAssemblyTest()
        {
            var solid = (IgesSolidAssembly)ParseLastEntity(@"
     110       1       0       0       0                        00000000D      1
     110       0       0       1       0                                D      2
     110       2       0       0       0                        00000000D      3
     110       0       0       1       0                                D      4
     124       3       0       0       0                        00000000D      5
     124       0       0       1       0                                D      6
     184       4       0       0       0                        00000200D      7
     184       0       0       1       0                                D      8
110,0.,0.,0.,0.,0.,0.;                                                 1P      1
110,0.,0.,0.,0.,0.,0.;                                                 3P      2
124,1.,0.,0.,0.,0.,1.,0.,0.,0.,0.,1.,0.;                               5P      3
184,2,1,3,0,5;                                                         7P      4
");
            Assert.Equal(2, solid.Solids.Count);
            Assert.IsType<IgesLine>(solid.Solids[0].Solid);
            Assert.IsType<IgesLine>(solid.Solids[1].Solid);
            Assert.True(solid.Solids[0].TransformationMatrix.IsIdentity); // null pointer, defaulted to identity
            Assert.True(solid.Solids[1].TransformationMatrix.IsIdentity); // actually an identity matrix
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadManifestSolidBRepObjectTest()
        {
            var solid = (IgesManifestSolidBRepObject)ParseLastEntity(@"
     110       1       0       0       0                        00000000D      1
     110       0       0       1       0                                D      2
     100       2       0       0       0                        00000000D      3
     100       0       0       1       0                                D      4
     186       3       0       0       0                        00000000D      5
     186       0       0       1       0                                D      6
110,0.,0.,0.,0.,0.,0.;                                                 1P      1
100,0.,0.,0.,0.,0.,0.,0.;                                              3P      2
186,1,1,1,3,1;                                                         5P      3
");
            Assert.IsType<IgesLine>(solid.Shell);
            Assert.True(solid.IsOriented);
            Assert.Single(solid.Voids);
            Assert.IsType<IgesCircularArc>(solid.Voids[0].Shell);
            Assert.True(solid.Voids[0].IsOriented);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadPlaneSurfaceTest()
        {
            var plane = (IgesPlaneSurface)ParseLastEntity(@"
     116       1       0       0       0                        00000000D      1
     116       0       0       1       0                                D      2
     123       2       0       0       0                        00010200D      3
     123       0       0       1       0                                D      4
     123       3       0       0       0                        00010200D      5
     123       0       0       1       0                                D      6
     190       4       0       0       0                        00000000D      7
     190       0       0       1       1                                D      8
116,1.,2.,3.;                                                          1P      1
123,0.,0.,1.;                                                          3P      2
123,1.,0.,0.;                                                          5P      3
190,1,3,5;                                                             7P      4
");
            Assert.Equal(new IgesPoint(1.0, 2.0, 3.0), plane.Point.ToPoint());
            Assert.Equal(IgesVector.ZAxis, plane.Normal.ToVector());
            Assert.Equal(IgesVector.XAxis, plane.ReferenceDirection.ToVector());
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadRightCircularCylindricalSurfaceTest()
        {
            var cylinder = (IgesRightCircularCylindricalSurface)ParseLastEntity(@"
     116       1       0       0       0                        00000000D      1
     116       0       0       1       0                                D      2
     123       2       0       0       0                        00010200D      3
     123       0       0       1       0                                D      4
     123       3       0       0       0                        00010200D      5
     123       0       0       1       0                                D      6
     192       4       0       0       0                        00010000D      7
     192       0       0       1       1                                D      8
116,1.,2.,3.;                                                          1P      1
123,0.,0.,1.;                                                          3P      2
123,1.,0.,0.;                                                          5P      3
192,1,3,13.,5;                                                         7P      4
");
            Assert.Equal(new IgesPoint(1.0, 2.0, 3.0), cylinder.Point.ToPoint());
            Assert.Equal(IgesVector.ZAxis, cylinder.AxisDirection.ToVector());
            Assert.Equal(13.0, cylinder.Radius);
            Assert.Equal(IgesVector.XAxis, cylinder.ReferenceDirection.ToVector());
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadRightCircularConicalSurfaceTest()
        {
            var cone = (IgesRightCircularConicalSurface)ParseLastEntity(@"
     116       1       0       0       0                        00000000D      1
     116       0       0       1       0                                D      2
     123       2       0       0       0                        00010200D      3
     123       0       0       1       0                                D      4
     123       3       0       0       0                        00010200D      5
     123       0       0       1       0                                D      6
     194       4       0       0       0                        00010000D      7
     194       0       0       1       1                                D      8
116,1.,2.,3.;                                                          1P      1
123,0.,0.,1.;                                                          3P      2
123,1.,0.,0.;                                                          5P      3
194,1,3,13.,45.,5;                                                     7P      4
");
            Assert.Equal(new IgesPoint(1.0, 2.0, 3.0), cone.Point.ToPoint());
            Assert.Equal(IgesVector.ZAxis, cone.AxisDirection.ToVector());
            Assert.Equal(13.0, cone.Radius);
            Assert.Equal(45.0, cone.SemiAngle);
            Assert.Equal(IgesVector.XAxis, cone.ReferenceDirection.ToVector());
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadSphericalSurfaceTest()
        {
            var sphere = (IgesSphericalSurface)ParseLastEntity(@"
     116       1       0       0       0                        00000000D      1
     116       0       0       1       0                                D      2
     123       2       0       0       0                        00010200D      3
     123       0       0       1       0                                D      4
     123       3       0       0       0                        00010200D      5
     123       0       0       1       0                                D      6
     196       4       0       0       0                        00010000D      7
     196       0       0       1       1                                D      8
116,1.,2.,3.;                                                          1P      1
123,0.,0.,1.;                                                          3P      2
123,1.,0.,0.;                                                          5P      3
196,1,13.,3,5;                                                         7P      4
");
            Assert.Equal(new IgesPoint(1.0, 2.0, 3.0), sphere.Center.ToPoint());
            Assert.Equal(13.0, sphere.Radius);
            Assert.Equal(IgesVector.ZAxis, sphere.AxisDirection.ToVector());
            Assert.Equal(IgesVector.XAxis, sphere.ReferenceDirection.ToVector());
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadToroidalSurfaceTest()
        {
            var torus = (IgesToroidalSurface)ParseLastEntity(@"
     116       1       0       0       0                        00000000D      1
     116       0       0       1       0                                D      2
     123       2       0       0       0                        00010200D      3
     123       0       0       1       0                                D      4
     123       3       0       0       0                        00010200D      5
     123       0       0       1       0                                D      6
     198       4       0       0       0                        00010000D      7
     198       0       0       1       1                                D      8
116,1.,2.,3.;                                                          1P      1
123,0.,0.,1.;                                                          3P      2
123,1.,0.,0.;                                                          5P      3
198,1,3,12.,6.,5;                                                      7P      4
");
            Assert.Equal(new IgesPoint(1.0, 2.0, 3.0), torus.Center.ToPoint());
            Assert.Equal(IgesVector.ZAxis, torus.AxisDirection.ToVector());
            Assert.Equal(12.0, torus.MajorRadius);
            Assert.Equal(6.0, torus.MinorRadius);
            Assert.Equal(IgesVector.XAxis, torus.ReferenceDirection.ToVector());
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadAngularDimensionTest()
        {
            var angular = (IgesAngularDimension)ParseLastEntity(@"
     202       1       0       0       0                        00000100D      1
     202       0       0       1       0                                D      2
202,0,0,0,0.,0.,0.,0,0;                                                1P      1
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadCurveDimensionTest()
        {
            var curve = (IgesCurveDimension)ParseLastEntity(@"
     204       1       0       0       0                        00000100D      1
     204       0       0       1       0                                D      2
204,0,0,0,0,0,0,0;                                                     1P      1
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadDiameterDimensionTest()
        {
            var diameter = (IgesDiameterDimension)ParseLastEntity(@"
     206       1       0       0       0                        00000100D      1
     206       0       0       1       0                                D      2
206,0,0,0,0.,0.;                                                       1P      1
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadFlagNoteTest()
        {
            var flag = (IgesFlagNote)ParseLastEntity(@"
     212       1       0       0       0                        00000100D      1
     212       0       0       1       0                                D      2
     214       2       0       0       0                        00000100D      3
     214       0       0       1       1                                D      4
     208       3       0       0       0                        00000100D      5
     208       0       0       1       0                                D      6
212,0;                                                                 1P      1
214,0,0.,0.,0.,0.,0.;                                                  3P      2
208,1.,2.,3.,0.1,1,1,3;                                                5P      3
");
            Assert.Equal(new IgesPoint(1.0, 2.0, 3.0), flag.Location);
            Assert.Equal(0.1, flag.RotationAngle);
            Assert.Single(flag.Leaders);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadGeneralLabelTest()
        {
            var label = (IgesGeneralLabel)ParseLastEntity(@"
     212       1       0       0       0                        00000100D      1
     212       0       0       1       0                                D      2
     214       2       0       0       0                        00000100D      3
     214       0       0       1       1                                D      4
     210       3       0       0       0                        00000100D      5
     210       0       0       1       0                                D      6
212,0;                                                                 1P      1
214,0,0.,0.,0.,0.,0.;                                                  3P      2
210,1,1,3;                                                             5P      3
");
            Assert.NotNull(label);
            Assert.NotNull(label.GeneralNote);
            Assert.NotNull(label.Leaders.Single());
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadGeneralNoteTest()
        {
            // fully-specified values
            var note = (IgesGeneralNote)ParseLastEntity(@"
     212       1       0       0       0                        00000100D      1
     212       0       0       1       6                                D      2
212,1,11,1.,2.,3,4.,5.,1,1,6.,7.,8.,11Htest string;                    1P      1
");
            Assert.Equal(IgesGeneralNoteType.MultipleStackLeftJustified, note.NoteType);
            var str = note.Strings.Single();
            Assert.Equal(1.0, str.BoxWidth);
            Assert.Equal(2.0, str.BoxHeight);
            Assert.Equal(3, str.FontCode);
            Assert.Null(str.TextFontDefinition);
            Assert.Equal(4.0, str.SlantAngle);
            Assert.Equal(5.0, str.RotationAngle);
            Assert.Equal(IgesTextMirroringAxis.PerpendicularToTextBase, str.MirroringAxis);
            Assert.Equal(IgesTextRotationType.Vertical, str.RotationType);
            Assert.Equal(6.0, str.Location.X);
            Assert.Equal(7.0, str.Location.Y);
            Assert.Equal(8.0, str.Location.Z);
            Assert.Equal("test string", str.Value);

            // with a text font definition
            note = (IgesGeneralNote)ParseLastEntity(@"
     310       1       0       0       0                        00000200D      1
     310       0       0       1       0                                D      2
     212       2       0       0       0                        00000100D      3
     212       0       0       1       6                                D      4
310,0,,,0,0;                                                           1P      1
212,1,11,1.,2.,-1,4.,5.,1,1,6.,7.,8.,11Htest string;                   3P      2
");
            Assert.NotNull(note.Strings.Single().TextFontDefinition);

            // type-default values
            note = (IgesGeneralNote)ParseLastEntity(@"
     212       1       0       0       0                        00000100D      1
     212       0       0       1       0                                D      2
212;                                                                   1P      1
");
            Assert.Equal(IgesGeneralNoteType.Simple, note.NoteType);
            Assert.Empty(note.Strings);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadNewGeneralNoteTest()
        {
            var note = (IgesNewGeneralNote)ParseLastEntity(@"
     213       1       0       0       0                        00000100D      1
     213       0       0       2       0                                D      2
213,1.,2.,2,0.,0.,0.,3.,4.,5.,6.,7.,1,1,11.,12.,13.,14.,3,15.,,        1P      1
3,16.,17.,3,18.,19.,0,0,20.,21.,22.,3Hfoo;                             1P      2
");
            Assert.Equal(1.0, note.TextContainmentAreaWidth);
            Assert.Equal(2.0, note.TextContainmentAreaHeight);
            Assert.Equal(IgesTextJustification.Center, note.TextJustification);
            Assert.Equal(IgesPoint.Origin, note.TextContainmentAreaLocation);
            Assert.Equal(3.0, note.TextContainmentAreaRotation);
            Assert.Equal(new IgesPoint(4.0, 5.0, 6.0), note.FirstBaseLineLocation);
            Assert.Equal(7.0, note.NormalInterLineSpacing);
            var str = note.Strings.Single();
            Assert.Equal(IgesCharacterDisplay.Variable, str.CharacterDisplay);
            Assert.Equal(11.0, str.CharacterWidth);
            Assert.Equal(12.0, str.CharacterHeight);
            Assert.Equal(13.0, str.InterCharacterSpacing);
            Assert.Equal(14.0, str.InterLineSpacing);
            Assert.Equal(IgesFontStyle.Futura, str.FontStyle);
            Assert.Equal(15.0, str.CharacterAngle);
            Assert.Equal(string.Empty, str.ControlCode);
            Assert.Equal(16.0, str.BoxWidth);
            Assert.Equal(17.0, str.BoxHeight);
            Assert.Equal(3, str.FontCode);
            Assert.Equal(18.0, str.SlantAngle);
            Assert.Equal(19.0, str.RotationAngle);
            Assert.Equal(IgesTextMirroringAxis.None, str.MirroringAxis);
            Assert.Equal(IgesTextRotationType.Horizontal, str.RotationType);
            Assert.Equal(new IgesPoint(20.0, 21.0, 22.0), str.Location);
            Assert.Equal("foo", str.Value);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadLinearDimensionTest()
        {
            var linear = (IgesLinearDimension)ParseLastEntity(@"
     216       1       0       0       0                        00000100D      1
     216       0       0       1       0                                D      2
216,0,0,0,0,0;                                                         1P      1
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadOrdinateDimensionTest()
        {
            var ord = (IgesOrdinateDimension)ParseLastEntity(@"
     218       1       0       0       0                        00000100D      1
     218       0       0       1       0                                D      2
218,0,0;                                                               1P      1
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadPointDimensionTest()
        {
            var point = (IgesPointDimension)ParseLastEntity(@"
     220       1       0       0       0                        00000100D      1
     220       0       0       1       0                                D      2
220,0,0,0;                                                             1P      1
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadRadiusDimensionTest()
        {
            var rad = (IgesRadiusDimension)ParseLastEntity(@"
     222       1       0       0       0                        00000100D      1
     222       0       0       1       0                                D      2
222,0,0,0.,0.;                                                         1P      1
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadGeneralSymbolTest()
        {
            var symbol = (IgesGeneralSymbol)ParseLastEntity(@"
     212       1       0       0       0                        00000100D      1
     212       0       0       1       0                                D      2
     110       2       0       0       0                        00000000D      3
     110       0       0       1       0                                D      4
     228       3       0       0       0                        00000100D      5
     228       0       0       1       0                                D      6
212,0;                                                                 1P      1
110,0.,0.,0.,0.,0.,0.;                                                 3P      2
228,1,1,3,0;                                                           5P      3
");
            Assert.NotNull(symbol.Note);
            Assert.Equal(IgesEntityType.Line, symbol.Geometries.Single().EntityType);
            Assert.Empty(symbol.Leaders);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadSectionedAreaTest()
        {
            var sectioned = (IgesSectionedArea)ParseLastEntity(@"
     100       1       0       0       0                        00000000D      1
     100       0       0       1       0                                D      2
     100       2       0       0       0                        00000000D      3
     100       0       0       1       0                                D      4
     230       3       0       0       0                        00000000D      5
     230       0       0       1       0                                D      6
100,0.,0.,0.,0.,0.,0.,0.;                                              1P      1
100,0.,0.,0.,0.,0.,0.,0.;                                              3P      2
230,1,2,3.,4.,5.,6.,7.,1,3;                                            5P      3
");
            Assert.IsType<IgesCircularArc>(sectioned.ExteriorDefinitionCurve);
            Assert.Equal(2, sectioned.FillPattern);
            Assert.Equal(new IgesPoint(3.0, 4.0, 0.0), sectioned.ReferenceLocation);
            Assert.Equal(5.0, sectioned.PatternZDepth);
            Assert.Equal(6.0, sectioned.NormalDistance);
            Assert.Equal(7.0, sectioned.RotationAngle);
            Assert.IsType<IgesCircularArc>(sectioned.InteriorDefinitionCurves.Single());
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadTextDisplayTemplateTest()
        {
            // fully-specified values
            var tdt = (IgesTextDisplayTemplate)ParseLastEntity(@"
     312       1       0       0       0                        00000200D      1
     312       0       0       1       0                                D      2
312,1.,2.,3,4.,5.,1,1,6.,7.,8.;                                        1P      1
");
            Assert.Equal(1.0, tdt.CharacterBoxWidth);
            Assert.Equal(2.0, tdt.CharacterBoxHeight);
            Assert.Equal(3, tdt.FontCode);
            Assert.Null(tdt.TextFontDefinition);
            Assert.Equal(4.0, tdt.SlantAngle);
            Assert.Equal(5.0, tdt.RotationAngle);
            Assert.Equal(IgesTextMirroringAxis.PerpendicularToTextBase, tdt.MirroringAxis);
            Assert.Equal(IgesTextRotationType.Vertical, tdt.RotationType);
            Assert.Equal(new IgesVector(6, 7, 8), tdt.LocationOrOffset);
            Assert.False(tdt.IsIncrementalDisplayTemplate);
            Assert.True(tdt.IsAbsoluteDisplayTemplate);

            // with a text font definition and incremental display
            tdt = (IgesTextDisplayTemplate)ParseLastEntity(@"
     310       1       0       0       0                        00000200D      1
     310       0       0       1       0                                D      2
     312       2       0       0       0                        00000200D      3
     312       0       0       1       1                                D      4
310,0,,,0,0;                                                           1P      1
312,1.,2.,-1,4.,5.,1,1,6.,7.,8.;                                       3P      2
");
            Assert.Equal(0, tdt.FontCode);
            Assert.NotNull(tdt.TextFontDefinition);
            Assert.True(tdt.IsIncrementalDisplayTemplate);
            Assert.False(tdt.IsAbsoluteDisplayTemplate);

            // type-default values
            tdt = (IgesTextDisplayTemplate)ParseLastEntity(@"
     312       1       0       0       0                        00000200D      1
     312       0       0       1       0                                D      2
312;                                                                   1P      1
");
            Assert.Equal(0.0, tdt.CharacterBoxWidth);
            Assert.Equal(0.0, tdt.CharacterBoxHeight);
            Assert.Equal(0, tdt.FontCode);
            Assert.Null(tdt.TextFontDefinition);
            Assert.Equal(0.0, tdt.SlantAngle);
            Assert.Equal(0.0, tdt.RotationAngle);
            Assert.Equal(IgesTextMirroringAxis.None, tdt.MirroringAxis);
            Assert.Equal(IgesTextRotationType.Horizontal, tdt.RotationType);
            Assert.Equal(IgesVector.Zero, tdt.LocationOrOffset);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadLeaderTest()
        {
            // fully-specified values
            var leader = (IgesLeader)ParseLastEntity(@"
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
            leader = (IgesLeader)ParseLastEntity(@"
     214       1       0       0       0                        00000100D      1
     214       0       0       1       1                                D      2
214;                                                                   1P      1
");
            Assert.Equal(IgesArrowType.Wedge, leader.ArrowType);
            Assert.Equal(0.0, leader.ArrowHeight);
            Assert.Equal(0.0, leader.ArrowWidth);
            Assert.Equal(IgesPoint.Origin, leader.ArrowheadCoordinates);
            Assert.Empty(leader.LineSegments);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadTemplateLineFontDefinitionTest()
        {
            var lfd = (IgesTemplateLineFontDefinition)ParseLastEntity(@"
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
            var lfd = (IgesPatternLineFontDefinition)ParseLastEntity(@"
     304       1       0       0       0                        00000200D      1
     304       0       0       1       2                                D      2
304,2,1.,2.,2H34;                                                      1P      1
");
            Assert.Equal(2, lfd.SegmentLengths.Count);
            Assert.Equal(1.0, lfd.SegmentLengths[0]);
            Assert.Equal(2.0, lfd.SegmentLengths[1]);
            Assert.Equal(0x34, lfd.DisplayMask);

            // default values
            lfd = (IgesPatternLineFontDefinition)ParseLastEntity(@"
     304       1       0       0       0                        00000200D      1
     304       0       0       1       2                                D      2
304;                                                                   1P      1
");
            Assert.Empty(lfd.SegmentLengths);
            Assert.Equal(0x00, lfd.DisplayMask);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadSubfigureTest()
        {
            var entity = ParseEntities(@"
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
")[1];
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
            subfigure = (IgesSubfigureDefinition)ParseLastEntity(@"
     308       1       0       0       0                        00000200D      1
     308       0       0       1       0                                D      2
308;                                                                    P      1
");
            Assert.Equal(0, subfigure.Depth);
            Assert.Null(subfigure.Name);
            Assert.Empty(subfigure.Entities);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadTextFontDefinitionTest()
        {
            // fully-specified values with supercedes code
            var tfd = (IgesTextFontDefinition)ParseLastEntity(@"
     310       1       0       0       0                        00000200D      1
     310       0       0       1       0                                D      2
310,1,8HSTANDARD,42,8,1,65,11,0,4,,4,8,,8,0,1,2,4,,6,4;                1P      1
");
            Assert.Equal(1, tfd.FontCode);
            Assert.Equal("STANDARD", tfd.Name);
            Assert.Equal(42, tfd.SupercedesCode);
            Assert.Equal(8, tfd.Scale);
            var character = tfd.Characters.Single();
            Assert.Equal(65, character.ASCIICode);
            Assert.Equal(11, character.CharacterOrigin.X);
            Assert.Equal(0, character.CharacterOrigin.Y);
            Assert.Equal(4, character.CharacterMovements.Count);
            Assert.False(character.CharacterMovements[0].IsUp);
            Assert.Equal(4, character.CharacterMovements[0].Location.X);
            Assert.Equal(8, character.CharacterMovements[0].Location.Y);
            Assert.False(character.CharacterMovements[1].IsUp);
            Assert.Equal(8, character.CharacterMovements[1].Location.X);
            Assert.Equal(0, character.CharacterMovements[1].Location.Y);
            Assert.True(character.CharacterMovements[2].IsUp);
            Assert.Equal(2, character.CharacterMovements[2].Location.X);
            Assert.Equal(4, character.CharacterMovements[2].Location.Y);
            Assert.False(character.CharacterMovements[3].IsUp);
            Assert.Equal(6, character.CharacterMovements[3].Location.X);
            Assert.Equal(4, character.CharacterMovements[3].Location.Y);

            // with no supercedes value
            tfd = (IgesTextFontDefinition)ParseLastEntity(@"
     310       1       0       0       0                        00000200D      1
     310       0       0       1       0                                D      2
310,1,8HSTANDARD,,8,1,65,11,0,4,,4,8,,8,0,1,2,4,,6,4;                  1P      1
");
            Assert.Equal(0, tfd.SupercedesCode);

            // with supercedes pointer
            tfd = (IgesTextFontDefinition)ParseLastEntity(@"
     310       1       0       0       0                        00000200D      1
     310       0       0       1       0                                D      2
     310       2       0       0       0                        00000200D      3
     310       0       0       1       0                                D      4
310,0,,,0,0;                                                           1P      1
310,1,8HSTANDARD,-1,8,1,65,11,0,4,,4,8,,8,0,1,2,4,,6,4;                3P      2
");
            Assert.IsType<IgesTextFontDefinition>(tfd.SupercedesFont);

            // type-default values
            tfd = (IgesTextFontDefinition)ParseLastEntity(@"
     310       1       0       0       0                        00000200D      1
     310       0       0       1       0                                D      2
310;                                                                   1P      1
");
            Assert.Equal(0, tfd.FontCode);
            Assert.Null(tfd.Name);
            Assert.Equal(0, tfd.SupercedesCode);
            Assert.Equal(0, tfd.Scale);
            Assert.Empty(tfd.Characters);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadColorDefinitionTest()
        {
            // fully-specified values
            var color = (IgesColorDefinition)ParseLastEntity(@"
     314       1       0       0       0                        00000200D      1
     314       0       0       1       0                                D      2
314,11.,22.,33.,4Hname;                                                1P      1
");
            Assert.Equal(11.0, color.RedIntensity);
            Assert.Equal(22.0, color.GreenIntensity);
            Assert.Equal(33.0, color.BlueIntensity);
            Assert.Equal("name", color.Name);

            // read type-default values
            color = (IgesColorDefinition)ParseLastEntity(@"
     314       1       0       0       0                        00000200D      1
     314       0       0       1       0                                D      2
314;                                                                   1P      1
");
            Assert.Equal(0.0, color.RedIntensity);
            Assert.Equal(0.0, color.GreenIntensity);
            Assert.Equal(0.0, color.BlueIntensity);
            Assert.Null(color.Name);

            // read line with custom color
            var line = (IgesLine)ParseLastEntity(@"
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
        public void ReadSingularSubfigureInstanceTest()
        {
            var sub = (IgesSingularSubfigureInstance)ParseLastEntity(@"
       0       1       0       0       0                        00000000D      1
       0       0       0       1       0                                D      2
     308       2       0       0       0                        00000200D      3
     308       0       0       1       0                                D      4
     408       3       0       0       0                        00000000D      5
     408       0       0       1       0                                D      6
0;                                                                     1P      1
308,0,,1,1;                                                            3P      2
408,3,1.,2.,3.,4.;                                                     5P      3
");
            Assert.NotNull(sub.SubfigureDefinition);
            Assert.Equal(new IgesVector(1.0, 2.0, 3.0), sub.Translation);
            Assert.Equal(4.0, sub.Scale);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadLabelDisplayAssociativityTest()
        {
//            // fully-specified values
//            var file = IgesReaderTests.CreateFile(@"
//     410       1       0       0       0                        00000100D      1
//     410       0       0       2       1                                D      2
//     214       3       0       0       0                        00000100D      3
//     214       0       0       1       6                                D      4
//     110       4       0       0       0                        00000000D      5
//     110       0       0       1       0                                D      6
//     402       5       0       0       0                        00000000D      7
//     402       0       0       1       5                                D      8
//410,0,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0,         1P      1
//0.,0.;                                                                 1P      2
//214,0,0.,0.,0.,0.,0.;                                                  3P      3
//110,1.,2.,3.,4.,5.,6.;                                                 5P      4
//402,1,1,1.,2.,3.,3,7,5;                                                7P      5
//");
//            Assert.Equal(2, file.Entities.Count);
//            var disp = file.Entities.OfType<IgesLabelDisplayAssociativity>().Single();
//            var line = file.Entities.OfType<IgesLine>().Single();
//            Assert.Equal(1, disp.Count);
//            var placement = disp[0];
//            Assert.IsType(typeof(IgesPerspectiveView), placement.View);
//            Assert.Equal(new IgesPoint(1, 2, 3), placement.Location);
//            Assert.Equal(IgesArrowType.FilledCircle, placement.Leader.ArrowType);
//            Assert.Equal(7, placement.Level);
//            line = (IgesLine)placement.Entity;
//            Assert.Equal(new IgesPoint(1, 2, 3), line.P1);
//            Assert.Equal(new IgesPoint(4, 5, 6), line.P2);

            // read type-defaule values
            var disp = (IgesLabelDisplayAssociativity)ParseLastEntity(@"
     402       1       0       0       0                        00000000D      1
     402       0       0       1       5                                D      2
402;                                                                   1P      1
");
            Assert.Empty(disp.LabelPlacements);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadNamePropertyTest()
        {
            var np = (IgesNameProperty)ParseLastEntity(@"
     406       1       0       0       0                        00000000D      1
     406       0       0       1      15                                D      2
406,1,4Htest;                                                          1P      1
");
            Assert.Equal("test", np.Name);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Reading)]
        public void ReadViewTest()
        {
            // fully-specified values
            var view = (IgesView)ParseLastEntity(@"
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
            view = (IgesView)ParseLastEntity(@"
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
            view = (IgesView)ParseLastEntity(@"
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
            var view = (IgesPerspectiveView)ParseLastEntity(@"
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
            view = (IgesPerspectiveView)ParseLastEntity(@"
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
