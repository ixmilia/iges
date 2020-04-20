namespace IxMilia.Iges.Entities
{
    public class IgesGroundedSpring : IgesFiniteElement
    {
        public override IgesElementEdgeOrder EdgeOrder { get { return IgesElementEdgeOrder.NotApplicable; } }

        public IgesPoint Location { get; set; }

        public IgesGroundedSpring(
            IgesPoint location)
            : base(IgesTopologyType.GroundedSpring)
        {
            Location = location;
        }

        protected override void AddNodes()
        {
            InternalNodes.Add(new IgesNode(Location));
        }

        internal static IgesGroundedSpring FromDummy(IgesFiniteElementDummy dummy)
        {
            return new IgesGroundedSpring(
                GetNodeOffset(dummy, 0));
        }
    }
}
