using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public partial class IgesSphere : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.Sphere; } }

        // properties
        public double Radius { get; set; }
        public IgesPoint Center { get; set; }

        public IgesSphere()
            : this(0.0, IgesPoint.Origin)
        {
        }

        public IgesSphere(double radius, IgesPoint center)
            : base()
        {
            this.EntityUseFlag = IgesEntityUseFlag.Geometry;
            this.Radius = radius;
            this.Center = center;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            int index = 0;
            this.Radius = Double(parameters, index++);
            this.Center = Point3(parameters, ref index);
            return index;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(this.Radius);
            if (Center != IgesPoint.Origin)
            {
                parameters.Add(this.Center.X);
                parameters.Add(this.Center.Y);
                parameters.Add(this.Center.Z);
            }
        }
    }
}
