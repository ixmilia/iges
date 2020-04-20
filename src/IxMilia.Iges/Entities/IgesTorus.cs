using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public partial class IgesTorus : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.Torus; } }

        // properties
        public double RingRadius { get; set; }
        public double DiscRadius { get; set; }
        public IgesPoint Center { get; set; }
        public IgesVector Normal { get; set; }

        public IgesTorus()
            : this(0.0, 0.0, IgesPoint.Origin, IgesVector.ZAxis)
        {
        }

        public IgesTorus(double ringRadius, double discRadius, IgesPoint center, IgesVector normal)
            : base()
        {
            this.BlankStatus = IgesBlankStatus.Visible;
            this.SubordinateEntitySwitchType = IgesSubordinateEntitySwitchType.Independent;
            this.EntityUseFlag = IgesEntityUseFlag.Geometry;
            this.Hierarchy = IgesHierarchy.GlobalTopDown;
            this.RingRadius = ringRadius;
            this.DiscRadius = discRadius;
            this.Center = center;
            this.Normal = normal;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            int index = 0;
            this.RingRadius = Double(parameters, index++);
            this.DiscRadius = Double(parameters, index++);
            this.Center = Point3(parameters, ref index);
            this.Normal = VectorOrDefault(parameters, ref index, IgesVector.ZAxis);
            return index;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(this.RingRadius);
            parameters.Add(this.DiscRadius);

            if (Center != IgesPoint.Origin || Normal != IgesVector.ZAxis)
            {
                parameters.Add(this.Center.X);
                parameters.Add(this.Center.Y);
                parameters.Add(this.Center.Z);
                if (Normal != IgesVector.ZAxis)
                {
                    parameters.Add(this.Normal.X);
                    parameters.Add(this.Normal.Y);
                    parameters.Add(this.Normal.Z);
                }
            }
        }
    }
}
