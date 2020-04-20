using System.Collections.Generic;
using System.Linq;

namespace IxMilia.Iges.Entities
{
    public partial class IgesCompositeCurve : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.CompositeCurve; } }

        // custom properties
        public List<IgesEntity> Entities { get; private set; }

        public IgesCompositeCurve()
            : base()
        {
            Entities = new List<IgesEntity>();
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var entityCount = Integer(parameters, 0);
            for (int i = 0; i < entityCount; i++)
            {
                binder.BindEntity(Integer(parameters, i + 1), e => Entities.Add(e));
            }

            return entityCount + 1;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            return Entities;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(this.Entities.Count);
            parameters.AddRange(Entities.Select(binder.GetEntityId).Cast<object>());
        }
    }
}
