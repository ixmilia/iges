using System.Collections.Generic;
using System.Linq;

namespace IxMilia.Iges.Entities
{
    public class IgesGeneralLabel : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.GeneralLabel; } }

        public IgesGeneralNote GeneralNote { get; set; }
        public IList<IgesLeader> Leaders { get; private set; }

        public IgesGeneralLabel()
        {
            EntityUseFlag = IgesEntityUseFlag.Annotation;
            Leaders = new List<IgesLeader>();
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var index = 0;
            binder.BindEntity(Integer(parameters, index++), note => GeneralNote = note as IgesGeneralNote);

            var leaderCount = Integer(parameters, index++);
            Leaders = new IgesLeader[leaderCount].ToList();
            for (int i = 0; i < leaderCount; i++)
            {
                var idx = i;
                binder.BindEntity(Integer(parameters, index++), leader => Leaders[idx] = leader as IgesLeader);
            }

            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return GeneralNote;
            foreach (var leader in Leaders)
            {
                yield return leader;
            }
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(binder.GetEntityId(GeneralNote));
            parameters.Add(Leaders.Count);
            foreach (var leader in Leaders)
            {
                parameters.Add(binder.GetEntityId(leader));
            }
        }
    }
}
