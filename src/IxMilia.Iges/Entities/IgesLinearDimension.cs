using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public enum IgesLinearDimensionType
    {
        Undetermined = 0,
        Diameter = 1,
        Radius = 2
    }

    public class IgesLinearDimension : IgesDimensionBase
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.LinearDimension; } }

        public IgesCopiousData FirstWitnessLine { get; set; }
        public IgesCopiousData SecondWitnessLine { get; set; }

        public IgesLinearDimensionType LinearDimensionType
        {
            get { return (IgesLinearDimensionType)FormNumber; }
            set { FormNumber = (int)value; }
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var index = 0;
            binder.BindEntity(Integer(parameters, index++), generalNote => GeneralNote = generalNote as IgesGeneralNote);
            binder.BindEntity(Integer(parameters, index++), leader => FirstLeader = leader as IgesLeader);
            binder.BindEntity(Integer(parameters, index++), leader => SecondLeader = leader as IgesLeader);
            binder.BindEntity(Integer(parameters, index++), witness => FirstWitnessLine = witness as IgesCopiousData);
            binder.BindEntity(Integer(parameters, index++), witness => SecondWitnessLine = witness as IgesCopiousData);
            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            foreach (var referenced in base.GetReferencedEntities())
            {
                yield return referenced;
            }

            yield return FirstWitnessLine;
            yield return SecondWitnessLine;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(binder.GetEntityId(GeneralNote));
            parameters.Add(binder.GetEntityId(FirstLeader));
            parameters.Add(binder.GetEntityId(SecondLeader));
            parameters.Add(binder.GetEntityId(FirstWitnessLine));
            parameters.Add(binder.GetEntityId(SecondWitnessLine));
        }
    }
}
