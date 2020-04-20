using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesAngularDimension : IgesDimensionBase
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.AngularDimension; } }

        public IgesCopiousData FirstWitnessLine { get; set; }
        public IgesCopiousData SecondWitnessLine { get; set; }
        public IgesPoint Vertex { get; set; } = IgesPoint.Origin;
        public double LeaderArcRadius { get; set; }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var index = 0;
            binder.BindEntity(Integer(parameters, index++), generalNote => GeneralNote = generalNote as IgesGeneralNote);
            binder.BindEntity(Integer(parameters, index++), witness => FirstWitnessLine = witness as IgesCopiousData);
            binder.BindEntity(Integer(parameters, index++), witness => SecondWitnessLine = witness as IgesCopiousData);
            Vertex = new IgesPoint(
                Double(parameters, index++),
                Double(parameters, index++),
                0.0);
            LeaderArcRadius = Double(parameters, index++);
            binder.BindEntity(Integer(parameters, index++), leader => FirstLeader = leader as IgesLeader);
            binder.BindEntity(Integer(parameters, index++), leader => SecondLeader = leader as IgesLeader);
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
            parameters.Add(binder.GetEntityId(FirstWitnessLine));
            parameters.Add(binder.GetEntityId(SecondWitnessLine));
            parameters.Add(Vertex.X);
            parameters.Add(Vertex.Y);
            parameters.Add(LeaderArcRadius);
            parameters.Add(binder.GetEntityId(FirstLeader));
            parameters.Add(binder.GetEntityId(SecondLeader));
        }
    }
}
