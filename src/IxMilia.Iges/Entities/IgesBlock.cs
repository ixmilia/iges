using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesBlock : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.Block; } }

        public double XLength { get; set; }
        public double YLength { get; set; }
        public double ZLength { get; set; }
        public IgesPoint Corner { get; set; } = IgesPoint.Origin;
        public IgesVector XAxis { get; set; } = IgesVector.XAxis;
        public IgesVector ZAxis { get; set; } = IgesVector.ZAxis;

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            int index = 0;
            XLength = Double(parameters, index++);
            YLength = Double(parameters, index++);
            ZLength = Double(parameters, index++);
            Corner = Point3(parameters, ref index);
            XAxis = VectorOrDefault(parameters, ref index, IgesVector.XAxis);
            ZAxis = VectorOrDefault(parameters, ref index, IgesVector.ZAxis);
            return index;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(XLength);
            parameters.Add(YLength);
            parameters.Add(ZLength);
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
