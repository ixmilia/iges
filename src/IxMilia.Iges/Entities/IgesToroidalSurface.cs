using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesToroidalSurface : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.ToroidalSurface; } }

        public IgesLocation Center { get; set; }
        public IgesDirection AxisDirection { get; set; }

        /// <summary>
        /// The value of the major radius (0.0, <see cref="double.PositiveInfinity"/>)
        /// </summary>
        public double MajorRadius { get; set; }

        /// <summary>
        /// The value of the minor radius (0.0, <see cref="MajorRadius"/>)
        /// </summary>
        public double MinorRadius { get; set; }
        public IgesDirection ReferenceDirection { get; set; }

        public bool IsParameterized
        {
            get { return FormNumber != 0; }
            set { FormNumber = value ? 1 : 0; }
        }

        public IgesToroidalSurface()
        {
            SubordinateEntitySwitchType = IgesSubordinateEntitySwitchType.PhysicallyDependent;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var index = 0;
            binder.BindEntity(Integer(parameters, index++), center => Center = center as IgesLocation);
            binder.BindEntity(Integer(parameters, index++), axis => AxisDirection = axis as IgesDirection);
            MajorRadius = Double(parameters, index++);
            MinorRadius = Double(parameters, index++);
            if (IsParameterized)
            {
                binder.BindEntity(Integer(parameters, index++), refDir => ReferenceDirection = refDir as IgesDirection);
            }

            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return Center;
            yield return AxisDirection;
            yield return ReferenceDirection;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(binder.GetEntityId(Center));
            parameters.Add(binder.GetEntityId(AxisDirection));
            parameters.Add(MajorRadius);
            parameters.Add(MinorRadius);
            if (IsParameterized)
            {
                parameters.Add(binder.GetEntityId(ReferenceDirection));
            }
        }
    }
}
