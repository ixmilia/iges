namespace IxMilia.Iges.Entities
{
    public class IgesCurvedBeam : IgesFiniteElement
    {
        public override IgesElementEdgeOrder EdgeOrder { get { return IgesElementEdgeOrder.Parabolic; } }

        public IgesPoint P1 { get; set; }
        public IgesPoint P2 { get; set; }

        public IgesCurvedBeam(
            IgesPoint p1,
            IgesPoint p2)
            : base(IgesTopologyType.CurvedBeam)
        {
            P1 = p1;
            P2 = p2;
        }

        protected override void AddNodes()
        {
            InternalNodes.Add(new IgesNode(P1));
            InternalNodes.Add(new IgesNode(P2));
        }

        internal static IgesCurvedBeam FromDummy(IgesFiniteElementDummy dummy)
        {
            return new IgesCurvedBeam(
                GetNodeOffset(dummy, 0),
                GetNodeOffset(dummy, 1));
        }
    }
}
