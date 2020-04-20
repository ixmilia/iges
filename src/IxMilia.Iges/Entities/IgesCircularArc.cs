using System;
using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesCircularArc : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.CircularArc; } }

        // properties
        public double PlaneDisplacement { get; set; }
        public IgesPoint Center { get; set; }
        public IgesPoint StartPoint { get; set; }
        public IgesPoint EndPoint { get; set; }

        // custom properties
        public IgesPoint ProperCenter
        {
            get
            {
                return new IgesPoint(Center.X, Center.Y, PlaneDisplacement);
            }
        }

        public IgesPoint ProperStartPoint
        {
            get
            {
                return new IgesPoint(StartPoint.X, StartPoint.Y, PlaneDisplacement);
            }
        }

        public IgesPoint ProperEndPoint
        {
            get
            {
                return new IgesPoint(EndPoint.X, EndPoint.Y, PlaneDisplacement);
            }
        }

        public IgesCircularArc()
            : this(IgesPoint.Origin, IgesPoint.Origin, IgesPoint.Origin)
        {
        }

        public IgesCircularArc(IgesPoint center, IgesPoint start, IgesPoint end)
            : base()
        {
            if (center.Z != start.Z || center.Z != end.Z)
            {
                throw new ArgumentException("All z values must be equal");
            }

            this.PlaneDisplacement = center.Z;
            this.Center = new IgesPoint(center.X, center.Y, 0.0);
            this.StartPoint = new IgesPoint(start.X, start.Y, 0.0);
            this.EndPoint = new IgesPoint(end.X, end.Y, 0.0);
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            int index = 0;
            this.PlaneDisplacement = Double(parameters, index++);
            this.Center = Point2(parameters, ref index);
            this.StartPoint = Point2(parameters, ref index);
            this.EndPoint = Point2(parameters, ref index);
            return index;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(this.PlaneDisplacement);
            parameters.Add(this.Center.X);
            parameters.Add(this.Center.Y);
            parameters.Add(this.StartPoint.X);
            parameters.Add(this.StartPoint.Y);
            parameters.Add(this.EndPoint.X);
            parameters.Add(this.EndPoint.Y);
        }
    }
}
