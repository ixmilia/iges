namespace IxMilia.Iges.Entities
{
    public class IgesOffsetBeam : IgesFiniteElement
    {
        public override IgesElementEdgeOrder EdgeOrder { get { return IgesElementEdgeOrder.Linear; } }

        public IgesPoint Offset1 { get; set; }
        public IgesPoint Offset2 { get; set; }
        public IgesPoint P1 { get; set; }
        public IgesPoint P2 { get; set; }

        public IgesOffsetBeam(
            IgesPoint offset1,
            IgesPoint offset2,
            IgesPoint p1,
            IgesPoint p2)
            : base(IgesTopologyType.OffsetBeam)
        {
            Offset1 = offset1;
            Offset2 = offset2;
            P1 = p1;
            P2 = p2;
        }

        protected override void AddNodes()
        {
            InternalNodes.Add(new IgesNode(Offset1));
            InternalNodes.Add(new IgesNode(Offset2));
            InternalNodes.Add(new IgesNode(P1));
            InternalNodes.Add(new IgesNode(P2));
        }

        internal static IgesOffsetBeam FromDummy(IgesFiniteElementDummy dummy)
        {
            return new IgesOffsetBeam(
                GetNodeOffset(dummy, 0),
                GetNodeOffset(dummy, 1),
                GetNodeOffset(dummy, 2),
                GetNodeOffset(dummy, 3));
        }
    }
}
