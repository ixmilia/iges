namespace IxMilia.Iges.Entities
{
    public class IgesOffsetMass : IgesFiniteElement
    {
        public override IgesElementEdgeOrder EdgeOrder { get { return IgesElementEdgeOrder.NotApplicable; } }

        public IgesPoint Location { get; set; }
        public IgesPoint CenterOfMass { get; set; }

        public IgesOffsetMass(
            IgesPoint location,
            IgesPoint centerOfMass)
            : base(IgesTopologyType.OffsetMass)
        {
            Location = location;
            CenterOfMass = centerOfMass;
        }

        protected override void AddNodes()
        {
            InternalNodes.Add(new IgesNode(Location));
            InternalNodes.Add(new IgesNode(CenterOfMass));
        }

        internal static IgesOffsetMass FromDummy(IgesFiniteElementDummy dummy)
        {
            return new IgesOffsetMass(
                GetNodeOffset(dummy, 0),
                GetNodeOffset(dummy, 1));
        }
    }
}
