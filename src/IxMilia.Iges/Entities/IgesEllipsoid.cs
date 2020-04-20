using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesEllipsoid : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.Ellipsoid; } }

        public double XAxisLength { get; set; }
        public double YAxisLength { get; set; }
        public double ZAxisLength { get; set; }
        public IgesPoint Center { get; set; }
        public IgesVector XAxis { get; set; }
        public IgesVector ZAxis { get; set; }

        public IgesEllipsoid()
        {
            EntityUseFlag = IgesEntityUseFlag.Geometry;
            Center = IgesPoint.Origin;
            XAxis = IgesVector.XAxis;
            ZAxis = IgesVector.ZAxis;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            int index = 0;
            XAxisLength = Double(parameters, index++);
            YAxisLength = Double(parameters, index++);
            ZAxisLength = Double(parameters, index++);
            Center = Point3(parameters, ref index);
            XAxis = VectorOrDefault(parameters, ref index, IgesVector.XAxis);
            ZAxis = VectorOrDefault(parameters, ref index, IgesVector.ZAxis);
            return 12;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(XAxisLength);
            parameters.Add(YAxisLength);
            parameters.Add(ZAxisLength);
            parameters.Add(Center.X);
            parameters.Add(Center.Y);
            parameters.Add(Center.Z);
            parameters.Add(XAxis.X);
            parameters.Add(XAxis.Y);
            parameters.Add(XAxis.Z);
            parameters.Add(ZAxis.X);
            parameters.Add(ZAxis.Y);
            parameters.Add(ZAxis.Z);
        }
    }
}
