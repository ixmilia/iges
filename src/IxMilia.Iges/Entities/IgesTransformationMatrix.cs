using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public partial class IgesTransformationMatrix : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.TransformationMatrix; } }

        // properties
        public double R11 { get; set; }
        public double R12 { get; set; }
        public double R13 { get; set; }
        public double T1 { get; set; }
        public double R21 { get; set; }
        public double R22 { get; set; }
        public double R23 { get; set; }
        public double T2 { get; set; }
        public double R31 { get; set; }
        public double R32 { get; set; }
        public double R33 { get; set; }
        public double T3 { get; set; }

        public IgesTransformationMatrix()
            : this(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0)
        {
        }

        public IgesTransformationMatrix(double r11, double r12, double r13, double t1, double r21, double r22, double r23, double t2, double r31, double r32, double r33, double t3)
            : base()
        {
            this.R11 = r11;
            this.R12 = r12;
            this.R13 = r13;
            this.T1 = t1;
            this.R21 = r21;
            this.R22 = r22;
            this.R23 = r23;
            this.T2 = t2;
            this.R31 = r31;
            this.R32 = r32;
            this.R33 = r33;
            this.T3 = t3;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            this.R11 = Double(parameters, 0);
            this.R12 = Double(parameters, 1);
            this.R13 = Double(parameters, 2);
            this.T1 = Double(parameters, 3);
            this.R21 = Double(parameters, 4);
            this.R22 = Double(parameters, 5);
            this.R23 = Double(parameters, 6);
            this.T2 = Double(parameters, 7);
            this.R31 = Double(parameters, 8);
            this.R32 = Double(parameters, 9);
            this.R33 = Double(parameters, 10);
            this.T3 = Double(parameters, 11);
            return 12;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(this.R11);
            parameters.Add(this.R12);
            parameters.Add(this.R13);
            parameters.Add(this.T1);
            parameters.Add(this.R21);
            parameters.Add(this.R22);
            parameters.Add(this.R23);
            parameters.Add(this.T2);
            parameters.Add(this.R31);
            parameters.Add(this.R32);
            parameters.Add(this.R33);
            parameters.Add(this.T3);
        }

        public IgesPoint Transform(IgesPoint point)
        {
            return new IgesPoint(
                (R11 * point.X + R12 * point.Y + R13 * point.Z) + T1,
                (R21 * point.X + R22 * point.Y + R23 * point.Z) + T2,
                (R31 * point.X + R32 * point.Y + R33 * point.Z) + T3);
        }

        public bool IsIdentity
        {
            get
            {
                return
                    R11 == 1.0 &&
                    R12 == 0.0 &&
                    R13 == 0.0 &&
                    T1 == 0.0 &&
                    R21 == 0.0 &&
                    R22 == 1.0 &&
                    R23 == 0.0 &&
                    T2 == 0.0 &&
                    R31 == 0.0 &&
                    R32 == 0.0 &&
                    R33 == 1.0 &&
                    T3 == 0.0;
            }
        }

        public static IgesTransformationMatrix Identity
        {
            get
            {
                return new IgesTransformationMatrix()
                {
                    R11 = 1.0,
                    R12 = 0.0,
                    R13 = 0.0,
                    T1 = 0.0,
                    R21 = 0.0,
                    R22 = 1.0,
                    R23 = 0.0,
                    T2 = 0.0,
                    R31 = 0.0,
                    R32 = 0.0,
                    R33 = 1.0,
                    T3 = 0.0,
                };
            }
        }
    }
}
