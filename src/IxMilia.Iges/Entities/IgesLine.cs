using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public enum IgesLineBounding
    {
        BoundOnBothSides = 0,
        BoundOnStart = 1,
        Unbounded = 2
    }

    public class IgesLine : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.Line; } }

        // properties
        public IgesPoint P1 { get; set; }
        public IgesPoint P2 { get; set; }

        // custom properties
        public IgesLineBounding Bounding
        {
            get
            {
                return (IgesLineBounding)FormNumber;
            }
            set
            {
                FormNumber = (int)value;
            }
        }

        public IgesLine()
            : this(IgesPoint.Origin, IgesPoint.Origin)
        {
        }

        public IgesLine(IgesPoint p1, IgesPoint p2)
            : base()
        {
            this.P1 = p1;
            this.P2 = p2;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            int index = 0;
            this.P1 = Point3(parameters, ref index);
            this.P2 = Point3(parameters, ref index);
            return index;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(this.P1.X);
            parameters.Add(this.P1.Y);
            parameters.Add(this.P1.Z);
            parameters.Add(this.P2.X);
            parameters.Add(this.P2.Y);
            parameters.Add(this.P2.Z);
        }
    }
}
