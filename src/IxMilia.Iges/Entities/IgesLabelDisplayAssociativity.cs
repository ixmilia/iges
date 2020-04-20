using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesLabelPlacement
    {
        public IgesViewBase View { get; set; }
        public IgesPoint Location { get; set; }
        public IgesLeader Leader { get; set; }
        public int Level { get; set; }
        public IgesEntity Label { get; set; }

        internal IgesLabelPlacement()
        {
        }

        public IgesLabelPlacement(IgesViewBase view, IgesPoint location, IgesLeader leader, int level, IgesEntity label)
        {
            View = view;
            Location = location;
            Leader = leader;
            Level = level;
            Label = label;
        }
    }

    public class IgesLabelDisplayAssociativity : IgesAssociativity
    {
        public List<IgesLabelPlacement> LabelPlacements { get; private set; }
        public IgesEntity AssociatedEntity { get; internal set; }

        public IgesLabelDisplayAssociativity()
            : base()
        {
            FormNumber = 5;
            LabelPlacements = new List<IgesLabelPlacement>();
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            int index = 0;
            var labelPlacementCount = Integer(parameters, index++);
            for (int i = 0; i < labelPlacementCount; i++)
            {
                var labelPlacement = new IgesLabelPlacement();
                binder.BindEntity(Integer(parameters, index++), e => labelPlacement.View = e as IgesViewBase);
                var x = Double(parameters, index++);
                var y = Double(parameters, index++);
                var z = Double(parameters, index++);
                labelPlacement.Location = new IgesPoint(x, y, z);
                binder.BindEntity(Integer(parameters, index++), e => labelPlacement.Leader = e as IgesLeader);
                labelPlacement.Level = Integer(parameters, index++);
                binder.BindEntity(Integer(parameters, index++), e => labelPlacement.Label = e);
                LabelPlacements.Add(labelPlacement);
            }

            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            foreach (var labelPlacement in LabelPlacements)
            {
                yield return labelPlacement.View;
                yield return labelPlacement.Leader;
                yield return labelPlacement.Label;
            }
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(LabelPlacements.Count);
            foreach (var labelPlacement in LabelPlacements)
            {
                parameters.Add(binder.GetEntityId(labelPlacement.View));
                parameters.Add(labelPlacement.Location.X);
                parameters.Add(labelPlacement.Location.Y);
                parameters.Add(labelPlacement.Location.Z);
                parameters.Add(binder.GetEntityId(labelPlacement.Leader));
                parameters.Add(labelPlacement.Level);
                parameters.Add(binder.GetEntityId(labelPlacement.Label));
            }
        }
    }
}
