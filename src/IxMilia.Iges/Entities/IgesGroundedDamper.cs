namespace IxMilia.Iges.Entities
{
    public class IgesGroundedDamper : IgesFiniteElement
    {
        public override IgesElementEdgeOrder EdgeOrder { get { return IgesElementEdgeOrder.NotApplicable; } }

        public IgesPoint Location { get; set; }

        public IgesGroundedDamper(
            IgesPoint location)
            : base(IgesTopologyType.GroundedDamper)
        {
            Location = location;
        }

        protected override void AddNodes()
        {
            InternalNodes.Add(new IgesNode(Location));
        }

        internal static IgesGroundedDamper FromDummy(IgesFiniteElementDummy dummy)
        {
            return new IgesGroundedDamper(
                GetNodeOffset(dummy, 0));
        }
    }
}
