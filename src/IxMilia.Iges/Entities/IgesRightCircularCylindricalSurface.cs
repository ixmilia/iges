using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesRightCircularCylindricalSurface : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.RightCircularCylindricalSurface; } }

        public IgesLocation Point { get; set; }
        public IgesDirection AxisDirection { get; set; }
        public double Radius { get; set; }
        public IgesDirection ReferenceDirection { get; set; }

        public bool IsParameterized
        {
            get { return FormNumber != 0; }
            set { FormNumber = value ? 1 : 0; }
        }

        public IgesRightCircularCylindricalSurface()
        {
            SubordinateEntitySwitchType = IgesSubordinateEntitySwitchType.PhysicallyDependent;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var index = 0;
            binder.BindEntity(Integer(parameters, index++), point => Point = point as IgesLocation);
            binder.BindEntity(Integer(parameters, index++), axis => AxisDirection = axis as IgesDirection);
            Radius = Double(parameters, index++);
            if (IsParameterized)
            {
                binder.BindEntity(Integer(parameters, index++), refDir => ReferenceDirection = refDir as IgesDirection);
            }

            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return Point;
            yield return AxisDirection;
            yield return ReferenceDirection;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(binder.GetEntityId(Point));
            parameters.Add(binder.GetEntityId(AxisDirection));
            parameters.Add(Radius);
            if (IsParameterized)
            {
                parameters.Add(binder.GetEntityId(ReferenceDirection));
            }
        }
    }
}
