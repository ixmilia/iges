using System.Collections.Generic;
using System.Linq;

namespace IxMilia.Iges.Entities
{
    public class IgesTrimmedParametricSurface : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.TrimmedParametricSurface; } }

        public IgesEntity Surface { get; set; }
        public bool IsOuterBoundaryD { get; set; }
        public List<IgesEntity> BoundaryEntities { get; private set; }
        public IgesEntity OuterBoundary { get; set; }

        public IgesTrimmedParametricSurface()
        {
            BoundaryEntities = new List<IgesEntity>();
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            int index = 0;
            binder.BindEntity(Integer(parameters, index++), e => Surface = e);
            IsOuterBoundaryD = !Boolean(parameters, index++);
            var boundaryEntityCount = Integer(parameters, index++);
            binder.BindEntity(Integer(parameters, index++), e => OuterBoundary = e);
            for (int i = 0; i < boundaryEntityCount; i++)
            {
                binder.BindEntity(Integer(parameters, index++), e => BoundaryEntities.Add(e));
            }

            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return Surface;
            yield return OuterBoundary;
            foreach (var boundary in BoundaryEntities)
            {
                yield return boundary;
            }
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(binder.GetEntityId(Surface));
            parameters.Add(!IsOuterBoundaryD);
            parameters.Add(BoundaryEntities.Count);
            parameters.Add(binder.GetEntityId(OuterBoundary));
            parameters.AddRange(BoundaryEntities.Select(binder.GetEntityId).Cast<object>());
        }
    }
}
