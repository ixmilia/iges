using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesPointDimension : IgesDimensionBase
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.PointDimension; } }

        public IgesEntity Geometry { get; set; }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var index = 0;
            binder.BindEntity(Integer(parameters, index++), generalNote => GeneralNote = generalNote as IgesGeneralNote);
            binder.BindEntity(Integer(parameters, index++), leader => FirstLeader = leader as IgesLeader);
            binder.BindEntity(Integer(parameters, index++), geometry => Geometry = geometry);
            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            foreach (var referenced in base.GetReferencedEntities())
            {
                yield return referenced;
            }

            yield return Geometry;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(binder.GetEntityId(GeneralNote));
            parameters.Add(binder.GetEntityId(FirstLeader));
            parameters.Add(binder.GetEntityId(Geometry));
        }
    }
}
