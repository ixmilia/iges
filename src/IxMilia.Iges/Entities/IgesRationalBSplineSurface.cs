using System.Collections.Generic;
using System.Diagnostics;

namespace IxMilia.Iges.Entities
{
    public enum IgesSplineSurfaceType
    {
        Custom = 0,
        Plane = 1,
        RightCircularCylinder = 2,
        Cone = 3,
        Sphere = 4,
        Torus = 5,
        SurfaceOfRevolution = 6,
        TabulatedCylinder = 7,
        RuledSurface = 8,
        GeneralQuadricSurface = 9
    }

    public class IgesRationalBSplineSurface : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.RationalBSplineSurface; } }

        public bool IsClosedInFirstParametricVariable { get; set; }
        public bool IsClosedInSecondParametricVariable { get; set; }
        public bool IsPolynomial { get; set; }
        public bool IsPeriodicInFirstParametricVariable { get; set; }
        public bool IsPeriodicInSecondParametricVariable { get; set; }

        public List<double> FirstKnotValueSequence { get; private set; }
        public List<double> SecondKnotValueSequence { get; private set; }
        public double[,] Weights { get; set; }
        public IgesPoint[,] ControlPoints { get; set; }

        public double FirstParametricStartingValue { get; set; }
        public double FirstParametricEndingValue { get; set; }
        public double SecondParametricStartingValue { get; set; }
        public double SecondParametricEndingValue { get; set; }

        public IgesSplineSurfaceType SurfaceType
        {
            get { return (IgesSplineSurfaceType)FormNumber; }
            set { FormNumber = (int)value; }
        }

        public IgesRationalBSplineSurface()
            : base()
        {
            FirstKnotValueSequence = new List<double>();
            SecondKnotValueSequence = new List<double>();
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            int index = 0;
            var k1 = Integer(parameters, index++);
            var k2 = Integer(parameters, index++);
            var m1 = Integer(parameters, index++);
            var m2 = Integer(parameters, index++);

            IsClosedInFirstParametricVariable = Boolean(parameters, index++);
            IsClosedInSecondParametricVariable = Boolean(parameters, index++);
            IsPolynomial = Boolean(parameters, index++);
            IsPeriodicInFirstParametricVariable = Boolean(parameters, index++);
            IsPeriodicInSecondParametricVariable = Boolean(parameters, index++);

            var n1 = 1 + k1 - m1;
            var n2 = 1 + k2 - m2;
            var a = n1 + 2 * m1;
            var b = n2 + 2 * m2;

            for (int i = 0; i < a + 1; i++)
            {
                FirstKnotValueSequence.Add(Double(parameters, index++));
            }

            for (int i = 0; i < b + 1; i++)
            {
                SecondKnotValueSequence.Add(Double(parameters, index++));
            }

            Weights = new double[k1 + 1, k2 + 1];
            for (int j = 0; j < k2 + 1; j++)
            {
                for (int i = 0; i < k1 + 1; i++)
                {
                    Weights[i, j] = Double(parameters, index++);
                }
            }

            ControlPoints = new IgesPoint[k1 + 1, k2 + 1];
            for (int j = 0; j < k2 + 1; j++)
            {
                for (int i = 0; i < k1 + 1; i++)
                {
                    var x = Double(parameters, index++);
                    var y = Double(parameters, index++);
                    var z = Double(parameters, index++);
                    ControlPoints[i, j] = new IgesPoint(x, y, z);
                }
            }

            FirstParametricStartingValue = Double(parameters, index++);
            FirstParametricEndingValue = Double(parameters, index++);
            SecondParametricStartingValue = Double(parameters, index++);
            SecondParametricEndingValue = Double(parameters, index++);

            return index;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            Debug.Assert(Weights != null);
            Debug.Assert(ControlPoints != null);
            Debug.Assert(Weights.GetLength(0) == ControlPoints.GetLength(0));
            Debug.Assert(Weights.GetLength(1) == ControlPoints.GetLength(1));

            var k1 = Weights.GetLength(0) - 1;
            var k2 = Weights.GetLength(1) - 1;
            var a = FirstKnotValueSequence.Count - 1;
            var b = SecondKnotValueSequence.Count - 1;
            var m1 = a - 1 - k1;
            var m2 = b - 1 - k2;

            parameters.Add(k1);
            parameters.Add(k2);
            parameters.Add(m1);
            parameters.Add(m2);
            parameters.Add(IsClosedInFirstParametricVariable ? 1 : 0);
            parameters.Add(IsClosedInSecondParametricVariable ? 1 : 0);
            parameters.Add(IsPolynomial ? 1 : 0);
            parameters.Add(IsPeriodicInFirstParametricVariable ? 1 : 0);
            parameters.Add(IsPeriodicInSecondParametricVariable ? 1 : 0);

            for (int i = 0; i < FirstKnotValueSequence.Count; i++)
            {
                parameters.Add(FirstKnotValueSequence[i]);
            }

            for (int i = 0; i < SecondKnotValueSequence.Count; i++)
            {
                parameters.Add(SecondKnotValueSequence[i]);
            }

            for (int j = 0; j < k2 + 1; j++)
            {
                for (int i = 0; i < k1 + 1; i++)
                {
                    parameters.Add(Weights[i, j]);
                }
            }

            for (int j = 0; j < k2 + 1; j++)
            {
                for (int i = 0; i < k1 + 1; i++)
                {
                    var point = ControlPoints[i, j];
                    parameters.Add(point.X);
                    parameters.Add(point.Y);
                    parameters.Add(point.Z);
                }
            }

            parameters.Add(FirstParametricStartingValue);
            parameters.Add(FirstParametricEndingValue);
            parameters.Add(SecondParametricStartingValue);
            parameters.Add(SecondParametricEndingValue);
        }
    }
}
