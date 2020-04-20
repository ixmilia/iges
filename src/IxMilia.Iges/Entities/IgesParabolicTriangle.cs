namespace IxMilia.Iges.Entities
{
    public class IgesParabolicTriangle : IgesFiniteElement
    {
        public override IgesElementEdgeOrder EdgeOrder { get { return IgesElementEdgeOrder.Parabolic; } }

        public IgesPoint P1 { get; set; }
        public IgesPoint P1P2Control { get; set; }
        public IgesPoint P2 { get; set; }
        public IgesPoint P2P3Control { get; set; }
        public IgesPoint P3 { get; set; }
        public IgesPoint P3P1Control { get; set; }

        public IgesParabolicTriangle(
            IgesPoint p1,
            IgesPoint p1P2Control,
            IgesPoint p2,
            IgesPoint p2P3Control,
            IgesPoint p3,
            IgesPoint p3P1Control)
            : base(IgesTopologyType.ParabolicTriangle)
        {
            P1 = p1;
            P1P2Control = p1P2Control;
            P2 = p2;
            P2P3Control = p2P3Control;
            P3 = p3;
            P3P1Control = p3P1Control;
        }

        protected override void AddNodes()
        {
            InternalNodes.Add(new IgesNode(P1));
            InternalNodes.Add(new IgesNode(P1P2Control));
            InternalNodes.Add(new IgesNode(P2));
            InternalNodes.Add(new IgesNode(P2P3Control));
            InternalNodes.Add(new IgesNode(P3));
            InternalNodes.Add(new IgesNode(P3P1Control));
        }

        internal static IgesParabolicTriangle FromDummy(IgesFiniteElementDummy dummy)
        {
            return new IgesParabolicTriangle(
                GetNodeOffset(dummy, 0),
                GetNodeOffset(dummy, 1),
                GetNodeOffset(dummy, 2),
                GetNodeOffset(dummy, 3),
                GetNodeOffset(dummy, 4),
                GetNodeOffset(dummy, 5));
        }
    }
}
