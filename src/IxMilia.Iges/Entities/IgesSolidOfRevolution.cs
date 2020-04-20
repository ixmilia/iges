using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesSolidOfRevolution : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.SolidOfRevolution; } }

        public IgesEntity Curve { get; set; }
        public double RevolutionAmount { get; set; }
        public IgesPoint PointOnAxis { get; set; }
        public IgesVector AxisDirection { get; set; }

        public bool IsClosedToAxis
        {
            get { return FormNumber == 0; }
            set { FormNumber = value ? 0 : 1; }
        }

        public bool IsClosedToSelf
        {
            get { return FormNumber == 1; }
            set { FormNumber = value ? 1 : 0; }
        }

        public IgesSolidOfRevolution()
        {
            RevolutionAmount = 1.0;
            PointOnAxis = IgesPoint.Origin;
            AxisDirection = IgesVector.ZAxis;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            int index = 0;
            binder.BindEntity(Integer(parameters, index++), e => Curve = e);
            RevolutionAmount = Double(parameters, index++);
            PointOnAxis = Point3(parameters, ref index);
            AxisDirection = Vector(parameters, ref index);
            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return Curve;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(binder.GetEntityId(Curve));
            parameters.Add(RevolutionAmount);
            parameters.Add(PointOnAxis.X);
            parameters.Add(PointOnAxis.Y);
            parameters.Add(PointOnAxis.Z);
            parameters.Add(AxisDirection.X);
            parameters.Add(AxisDirection.Y);
            parameters.Add(AxisDirection.Z);
        }
    }
}
