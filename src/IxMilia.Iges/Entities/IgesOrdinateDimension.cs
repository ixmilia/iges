using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesOrdinateDimension : IgesDimensionBase
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.OrdinateDimension; } }

        public IgesCopiousData WitnessLine { get; set; }

        public bool HasSupplementalLeader { get { return WitnessLine != null && FirstLeader != null; } }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var index = 0;
            binder.BindEntity(Integer(parameters, index++), generalNote => GeneralNote = generalNote as IgesGeneralNote);
            if (FormNumber == 0)
            {
                binder.BindEntity(Integer(parameters, index++), witness =>
                {
                    if (witness == null)
                    {
                        return;
                    }
                    else if (witness is IgesCopiousData)
                    {
                        WitnessLine = witness as IgesCopiousData;
                    }
                    else if (witness is IgesLeader)
                    {
                        FirstLeader = witness as IgesLeader;
                    }
                });
            }
            else if (FormNumber == 1)
            {
                binder.BindEntity(Integer(parameters, index++), witness => WitnessLine = witness as IgesCopiousData);
                binder.BindEntity(Integer(parameters, index++), leader => FirstLeader = leader as IgesLeader);
            }

            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            foreach (var referenced in base.GetReferencedEntities())
            {
                yield return referenced;
            }

            yield return WitnessLine;
        }

        internal override void OnBeforeWrite()
        {
            FormNumber = HasSupplementalLeader ? 1 : 0;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(binder.GetEntityId(GeneralNote));
            parameters.Add(binder.GetEntityId((IgesEntity)WitnessLine ?? FirstLeader));
            if (HasSupplementalLeader)
            {
                parameters.Add(binder.GetEntityId(FirstLeader));
            }
        }
    }
}
