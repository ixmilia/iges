using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesRightAngularWedge : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.RightAngularWedge; } }

        public double XAxisSize { get; set; }
        public double YAxisSize { get; set; }
        public double ZAxisSize { get; set; }
        public double XAxisSizeAtYDistance { get; set; }
        public IgesPoint Corner { get; set; } = IgesPoint.Origin;
        public IgesVector XAxis { get; set; } = IgesVector.XAxis;
        public IgesVector ZAxis { get; set; } = IgesVector.ZAxis;

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            int index = 0;
            XAxisSize = Double(parameters, index++);
            YAxisSize = Double(parameters, index++);
            ZAxisSize = Double(parameters, index++);
            XAxisSizeAtYDistance = Double(parameters, index++);
            Corner = Point3(parameters, ref index);
            XAxis = VectorOrDefault(parameters, ref index, IgesVector.XAxis);
            ZAxis = VectorOrDefault(parameters, ref index, IgesVector.ZAxis);
            return index;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(XAxisSize);
            parameters.Add(YAxisSize);
            parameters.Add(ZAxisSize);
            parameters.Add(XAxisSizeAtYDistance);
            parameters.Add(Corner.X);
            parameters.Add(Corner.Y);
            parameters.Add(Corner.Z);
            parameters.Add(XAxis.X);
            parameters.Add(XAxis.Y);
            parameters.Add(XAxis.Z);
            parameters.Add(ZAxis.X);
            parameters.Add(ZAxis.Y);
            parameters.Add(ZAxis.Z);
        }
    }
}
