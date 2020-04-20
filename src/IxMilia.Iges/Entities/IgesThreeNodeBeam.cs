namespace IxMilia.Iges.Entities
{
    public class IgesThreeNodeBeam : IgesFiniteElement
    {
        public override IgesElementEdgeOrder EdgeOrder { get { return IgesElementEdgeOrder.Parabolic; } }

        public IgesPoint P1 { get; set; }
        public IgesPoint P2 { get; set; }
        public IgesPoint P3 { get; set; }

        public IgesThreeNodeBeam(
            IgesPoint p1,
            IgesPoint p2,
            IgesPoint p3)
            : base(IgesTopologyType.ThreeNodeBeam)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
        }

        protected override void AddNodes()
        {
            InternalNodes.Add(new IgesNode(P1));
            InternalNodes.Add(new IgesNode(P2));
            InternalNodes.Add(new IgesNode(P3));
        }

        internal static IgesThreeNodeBeam FromDummy(IgesFiniteElementDummy dummy)
        {
            return new IgesThreeNodeBeam(
                GetNodeOffset(dummy, 0),
                GetNodeOffset(dummy, 1),
                GetNodeOffset(dummy, 2));
        }
    }
}
