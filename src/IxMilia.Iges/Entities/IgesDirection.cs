using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public partial class IgesDirection : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.Direction; } }

        // properties
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        internal IgesDirection()
            : this(0.0, 0.0, 0.0)
        {
        }

        public IgesDirection(double x, double y, double z)
            : base()
        {
            this.SubordinateEntitySwitchType = IgesSubordinateEntitySwitchType.PhysicallyDependent;
            this.EntityUseFlag = IgesEntityUseFlag.Definition;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            this.X = Double(parameters, 0);
            this.Y = Double(parameters, 1);
            this.Z = Double(parameters, 2);
            return 3;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(this.X);
            parameters.Add(this.Y);
            parameters.Add(this.Z);
        }

        public IgesVector ToVector()
        {
            return new IgesVector(X, Y, Z);
        }
    }
}
