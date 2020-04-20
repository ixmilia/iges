using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesRightCircularConeFrustrum : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.RightCircularConeFrustrum; } }

        public double Height { get; set; }
        public double LargeFaceRadius { get; set; }
        public double SmallFaceRadius { get; set; }
        public IgesPoint LargeFaceCenter { get; set; } = IgesPoint.Origin;
        public IgesVector AxisDirection { get; set; } = IgesVector.ZAxis;

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            int index = 0;
            Height = Double(parameters, index++);
            LargeFaceRadius = Double(parameters, index++);
            SmallFaceRadius = Double(parameters, index++);
            LargeFaceCenter = Point3(parameters, ref index);
            AxisDirection = Vector(parameters, ref index);
            return index;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(Height);
            parameters.Add(LargeFaceRadius);
            parameters.Add(SmallFaceRadius);
            parameters.Add(LargeFaceCenter.X);
            parameters.Add(LargeFaceCenter.Y);
            parameters.Add(LargeFaceCenter.Z);
            parameters.Add(AxisDirection.X);
            parameters.Add(AxisDirection.Y);
            parameters.Add(AxisDirection.Z);
        }
    }
}
