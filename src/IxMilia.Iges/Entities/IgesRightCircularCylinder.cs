using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesRightCircularCylinder : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.RightCircularCylinder; } }

        public double Height { get; set; }
        public double Radius { get; set; }
        public IgesPoint FirstFaceCenter { get; set; } = IgesPoint.Origin;
        public IgesVector AxisDirection { get; set; } = IgesVector.ZAxis;

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            int index = 0;
            Height = Double(parameters, index++);
            Radius = Double(parameters, index++);
            FirstFaceCenter = Point3(parameters, ref index);
            AxisDirection = Vector(parameters, ref index);
            return index;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(Height);
            parameters.Add(Radius);
            parameters.Add(FirstFaceCenter.X);
            parameters.Add(FirstFaceCenter.Y);
            parameters.Add(FirstFaceCenter.Z);
            parameters.Add(AxisDirection.X);
            parameters.Add(AxisDirection.Y);
            parameters.Add(AxisDirection.Z);
        }
    }
}
