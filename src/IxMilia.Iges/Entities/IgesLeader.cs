using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public enum IgesArrowType
    {
        Wedge = 1,
        Triangle = 2,
        FilledTriangle = 3,
        None = 4,
        Circle = 5,
        FilledCircle = 6,
        Rectangle = 7,
        FilledRectangle = 8,
        Slash = 9,
        IntegralSign = 10,
        OpenTriangle = 11,
        DimensionOrigin = 12
    }

    public class IgesLeader : IgesEntity
    {
        public override IgesEntityType EntityType {  get { return IgesEntityType.Leader; } }

        public double ArrowHeight { get; set; }
        public double ArrowWidth { get; set; }

        public IgesPoint ArrowheadCoordinates { get; set; }
        public List<IgesPoint> LineSegments { get; private set; }

        public IgesArrowType ArrowType
        {
            get { return (IgesArrowType)FormNumber; }
            set { FormNumber = (int)value; }
        }

        public IgesLeader()
            : base()
        {
            ArrowType = IgesArrowType.Wedge;
            EntityUseFlag = IgesEntityUseFlag.Annotation;
            ArrowheadCoordinates = IgesPoint.Origin;
            LineSegments = new List<IgesPoint>();
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var index = 0;
            var segmentCount = Integer(parameters, index++);
            ArrowHeight = Double(parameters, index++);
            ArrowWidth = Double(parameters, index++);
            var zDepth = Double(parameters, index++);
            var x = Double(parameters, index++);
            var y = Double(parameters, index++);
            ArrowheadCoordinates = new IgesPoint(x, y, zDepth);
            for (int i = 0; i < segmentCount; i++)
            {
                x = Double(parameters, index++);
                y = Double(parameters, index++);
                LineSegments.Add(new IgesPoint(x, y, zDepth));
            }

            return segmentCount * 2 + 6;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(LineSegments.Count);
            parameters.Add(ArrowHeight);
            parameters.Add(ArrowWidth);
            parameters.Add(ArrowheadCoordinates.Z);
            parameters.Add(ArrowheadCoordinates.X);
            parameters.Add(ArrowheadCoordinates.Y);
            foreach (var p in LineSegments)
            {
                parameters.Add(p.X);
                parameters.Add(p.Y);
            }
        }
    }
}
