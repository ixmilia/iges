using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesSplinePolynomialPatchSegment
    {
        public double BreakPoint { get; set; }
        public double AX { get; set; }
        public double BX { get; set; }
        public double CX { get; set; }
        public double DX { get; set; }
        public double EX { get; set; }
        public double FX { get; set; }
        public double GX { get; set; }
        public double HX { get; set; }
        public double KX { get; set; }
        public double LX { get; set; }
        public double MX { get; set; }
        public double NX { get; set; }
        public double PX { get; set; }
        public double QX { get; set; }
        public double RX { get; set; }
        public double SX { get; set; }
        public double AY { get; set; }
        public double BY { get; set; }
        public double CY { get; set; }
        public double DY { get; set; }
        public double EY { get; set; }
        public double FY { get; set; }
        public double GY { get; set; }
        public double HY { get; set; }
        public double KY { get; set; }
        public double LY { get; set; }
        public double MY { get; set; }
        public double NY { get; set; }
        public double PY { get; set; }
        public double QY { get; set; }
        public double RY { get; set; }
        public double SY { get; set; }
        public double AZ { get; set; }
        public double BZ { get; set; }
        public double CZ { get; set; }
        public double DZ { get; set; }
        public double EZ { get; set; }
        public double FZ { get; set; }
        public double GZ { get; set; }
        public double HZ { get; set; }
        public double KZ { get; set; }
        public double LZ { get; set; }
        public double MZ { get; set; }
        public double NZ { get; set; }
        public double PZ { get; set; }
        public double QZ { get; set; }
        public double RZ { get; set; }
        public double SZ { get; set; }
    }

    public class IgesParametricSplineSurface : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.ParametricSplineSurface; } }

        public IgesSplineType SplineType { get; set; }
        public bool IsCartesianProduct { get; set; }
        public List<double> UBreakpoints { get; private set; }
        public List<double> VBreakpoints { get; private set; }
        public IgesSplinePolynomialPatchSegment[,] SegmentCoefficients { get; set; }

        public IgesParametricSplineSurface()
            : base()
        {
            SplineType = IgesSplineType.Linear;
            UBreakpoints = new List<double>();
            VBreakpoints = new List<double>();
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            int index = 0;
            this.SplineType = (IgesSplineType)Integer(parameters, index++);
            this.IsCartesianProduct = Boolean(parameters, index++);
            var m = Integer(parameters, index++);
            var n = Integer(parameters, index++);
            for (int i = 0; i < m; i++)
            {
                UBreakpoints.Add(Double(parameters, index++));
            }

            for (int i = 0; i < n; i++)
            {
                VBreakpoints.Add(Double(parameters, index++));
            }

            SegmentCoefficients = new IgesSplinePolynomialPatchSegment[m, n];
            for (int u = 0; u < m; u++)
            {
                for (int v = 0; v < n; v++)
                {
                    SegmentCoefficients[u, v] = new IgesSplinePolynomialPatchSegment();
                    SegmentCoefficients[u, v].AX = Double(parameters, index++);
                    SegmentCoefficients[u, v].BX = Double(parameters, index++);
                    SegmentCoefficients[u, v].CX = Double(parameters, index++);
                    SegmentCoefficients[u, v].DX = Double(parameters, index++);
                    SegmentCoefficients[u, v].EX = Double(parameters, index++);
                    SegmentCoefficients[u, v].FX = Double(parameters, index++);
                    SegmentCoefficients[u, v].GX = Double(parameters, index++);
                    SegmentCoefficients[u, v].HX = Double(parameters, index++);
                    SegmentCoefficients[u, v].KX = Double(parameters, index++);
                    SegmentCoefficients[u, v].LX = Double(parameters, index++);
                    SegmentCoefficients[u, v].MX = Double(parameters, index++);
                    SegmentCoefficients[u, v].NX = Double(parameters, index++);
                    SegmentCoefficients[u, v].PX = Double(parameters, index++);
                    SegmentCoefficients[u, v].QX = Double(parameters, index++);
                    SegmentCoefficients[u, v].RX = Double(parameters, index++);
                    SegmentCoefficients[u, v].SX = Double(parameters, index++);
                    SegmentCoefficients[u, v].AY = Double(parameters, index++);
                    SegmentCoefficients[u, v].BY = Double(parameters, index++);
                    SegmentCoefficients[u, v].CY = Double(parameters, index++);
                    SegmentCoefficients[u, v].DY = Double(parameters, index++);
                    SegmentCoefficients[u, v].EY = Double(parameters, index++);
                    SegmentCoefficients[u, v].FY = Double(parameters, index++);
                    SegmentCoefficients[u, v].GY = Double(parameters, index++);
                    SegmentCoefficients[u, v].HY = Double(parameters, index++);
                    SegmentCoefficients[u, v].KY = Double(parameters, index++);
                    SegmentCoefficients[u, v].LY = Double(parameters, index++);
                    SegmentCoefficients[u, v].MY = Double(parameters, index++);
                    SegmentCoefficients[u, v].NY = Double(parameters, index++);
                    SegmentCoefficients[u, v].PY = Double(parameters, index++);
                    SegmentCoefficients[u, v].QY = Double(parameters, index++);
                    SegmentCoefficients[u, v].RY = Double(parameters, index++);
                    SegmentCoefficients[u, v].SY = Double(parameters, index++);
                    SegmentCoefficients[u, v].AZ = Double(parameters, index++);
                    SegmentCoefficients[u, v].BZ = Double(parameters, index++);
                    SegmentCoefficients[u, v].CZ = Double(parameters, index++);
                    SegmentCoefficients[u, v].DZ = Double(parameters, index++);
                    SegmentCoefficients[u, v].EZ = Double(parameters, index++);
                    SegmentCoefficients[u, v].FZ = Double(parameters, index++);
                    SegmentCoefficients[u, v].GZ = Double(parameters, index++);
                    SegmentCoefficients[u, v].HZ = Double(parameters, index++);
                    SegmentCoefficients[u, v].KZ = Double(parameters, index++);
                    SegmentCoefficients[u, v].LZ = Double(parameters, index++);
                    SegmentCoefficients[u, v].MZ = Double(parameters, index++);
                    SegmentCoefficients[u, v].NZ = Double(parameters, index++);
                    SegmentCoefficients[u, v].PZ = Double(parameters, index++);
                    SegmentCoefficients[u, v].QZ = Double(parameters, index++);
                    SegmentCoefficients[u, v].RZ = Double(parameters, index++);
                    SegmentCoefficients[u, v].SZ = Double(parameters, index++);
                    index += 48 * (u + 1);
                }

                // arbitrary values
                index += 48 * (u + 1);
            }

            return index;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add((int)SplineType);
            parameters.Add(IsCartesianProduct);
        }
    }
}
