using System.Collections.Generic;
using System.Linq;

namespace IxMilia.Iges.Entities
{
    public partial class IgesSubfigureDefinition : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.SubfigureDefinition; } }

        // properties
        public int Depth { get; set; }
        public string Name { get; set; }

        // custom properties
        public List<IgesEntity> Entities { get; private set; }

        public IgesSubfigureDefinition()
            : base()
        {
            this.Depth = 0;
            this.Name = null;
            this.EntityUseFlag = IgesEntityUseFlag.Definition;
            Entities = new List<IgesEntity>();
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            this.Depth = Integer(parameters, 0);
            this.Name = String(parameters, 1);
            var entityCount = Integer(parameters, 2);
            for (int i = 0; i < entityCount; i++)
            {
                binder.BindEntity(Integer(parameters, i + 3), e => Entities.Add(e));
            }

            return entityCount + 3;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            return Entities;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(this.Depth);
            parameters.Add(this.Name);
            parameters.Add(this.Entities.Count);
            parameters.AddRange(Entities.Select(binder.GetEntityId).Cast<object>());
        }
    }
}
