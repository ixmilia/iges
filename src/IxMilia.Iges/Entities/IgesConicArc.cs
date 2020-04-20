using System.Collections.Generic;
using System.Diagnostics;

namespace IxMilia.Iges.Entities
{
    public enum IgesArcType
    {
        Unknown = 0,
        Ellipse = 1,
        Hyperbola = 2,
        Parabola = 3
    }

    public class IgesConicArc : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.ConicArc; } }

        public double CoefficientA { get; set; }
        public double CoefficientB { get; set; }
        public double CoefficientC { get; set; }
        public double CoefficientD { get; set; }
        public double CoefficientE { get; set; }
        public double CoefficientF { get; set; }
        public IgesPoint StartPoint { get; set; }
        public IgesPoint EndPoint { get; set; }

        public double Q1
        {
            get
            {
                return ((CoefficientA * CoefficientC) - ((CoefficientB * CoefficientB) / 4.0)) * CoefficientF
                    + ((CoefficientB * CoefficientD * CoefficientE) / 4.0)
                    - ((CoefficientC * CoefficientD * CoefficientD) / 4.0)
                    - ((CoefficientA * CoefficientE * CoefficientE) / 4.0);
            }
        }

        public double Q2
        {
            get
            {
                return (CoefficientA * CoefficientC) - ((CoefficientB * CoefficientB) / 4.0);
            }
        }

        public double Q3
        {
            get
            {
                return CoefficientA + CoefficientC;
            }
        }

        public IgesArcType ArcType
        {
            get
            {
                if (CoefficientB == 0.0 && CoefficientD == 0.0 && CoefficientE == 0.0 &&
                    CoefficientA != 0.0 && CoefficientC != 0.0 && CoefficientF != 0.0 &&
                    CoefficientF == -CoefficientA * CoefficientC)
                {
                    return IgesArcType.Ellipse;
                }
                else if (Q2 < 0.0)
                {
                    return IgesArcType.Hyperbola;
                }
                else if (Q2 == 0.0 && Q1 != 0.0)
                {
                    return IgesArcType.Parabola;
                }
                else
                {
                    return IgesArcType.Unknown;
                }
            }
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            this.CoefficientA = Double(parameters, 0);
            this.CoefficientB = Double(parameters, 1);
            this.CoefficientC = Double(parameters, 2);
            this.CoefficientD = Double(parameters, 3);
            this.CoefficientE = Double(parameters, 4);
            this.CoefficientF = Double(parameters, 5);
            var planeOffset = Double(parameters, 6);
            var x = Double(parameters, 7);
            var y = Double(parameters, 8);
            StartPoint = new IgesPoint(x, y, planeOffset);
            x = Double(parameters, 9);
            y = Double(parameters, 10);
            EndPoint = new IgesPoint(x, y, planeOffset);
            return 11;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(CoefficientA);
            parameters.Add(CoefficientB);
            parameters.Add(CoefficientC);
            parameters.Add(CoefficientD);
            parameters.Add(CoefficientE);
            parameters.Add(CoefficientF);
            parameters.Add(StartPoint.Z);
            parameters.Add(StartPoint.X);
            parameters.Add(StartPoint.Y);
            parameters.Add(EndPoint.X);
            parameters.Add(EndPoint.Y);
        }

        internal override void OnBeforeWrite()
        {
            base.OnBeforeWrite();
            Debug.Assert(StartPoint.Z == EndPoint.Z, "start and end point must have same z values");
            FormNumber = (int)ArcType;
        }

        public static IgesConicArc MakeEllipse(double majorAxis, double minorAxis)
        {
            var arc = new IgesConicArc();
            arc.CoefficientA = minorAxis * minorAxis;
            arc.CoefficientB = 0.0;
            arc.CoefficientC = majorAxis * majorAxis;
            arc.CoefficientD = 0.0;
            arc.CoefficientE = 0.0;
            arc.CoefficientF = -arc.CoefficientA * arc.CoefficientC;
            Debug.Assert(arc.ArcType == IgesArcType.Ellipse);
            return arc;
        }
    }
}
