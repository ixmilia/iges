namespace IxMilia.Iges.Entities
{
    public class IgesParabolicQuadrilateral : IgesFiniteElement
    {
        public override IgesElementEdgeOrder EdgeOrder { get { return IgesElementEdgeOrder.Parabolic; } }

        public IgesPoint P1 { get; set; }
        public IgesPoint P1P2Control { get; set; }
        public IgesPoint P2 { get; set; }
        public IgesPoint P2P3Control { get; set; }
        public IgesPoint P3 { get; set; }
        public IgesPoint P3P4Control { get; set; }
        public IgesPoint P4 { get; set; }
        public IgesPoint P4P1Control { get; set; }

        public IgesParabolicQuadrilateral(
            IgesPoint p1,
            IgesPoint p1P2Control,
            IgesPoint p2,
            IgesPoint p2P3Control,
            IgesPoint p3,
            IgesPoint p3P4Control,
            IgesPoint p4,
            IgesPoint p4P1Control)
            : base(IgesTopologyType.ParabolicQuadrilateral)
        {
            P1 = p1;
            P1P2Control = p1P2Control;
            P2 = p2;
            P2P3Control = p2P3Control;
            P3 = p3;
            P3P4Control = p3P4Control;
            P4 = p4;
            P4P1Control = p4P1Control;
        }

        protected override void AddNodes()
        {
            InternalNodes.Add(new IgesNode(P1));
            InternalNodes.Add(new IgesNode(P1P2Control));
            InternalNodes.Add(new IgesNode(P2));
            InternalNodes.Add(new IgesNode(P2P3Control));
            InternalNodes.Add(new IgesNode(P3));
            InternalNodes.Add(new IgesNode(P3P4Control));
            InternalNodes.Add(new IgesNode(P4));
            InternalNodes.Add(new IgesNode(P4P1Control));
        }

        internal static IgesParabolicQuadrilateral FromDummy(IgesFiniteElementDummy dummy)
        {
            return new IgesParabolicQuadrilateral(
                GetNodeOffset(dummy, 0),
                GetNodeOffset(dummy, 1),
                GetNodeOffset(dummy, 2),
                GetNodeOffset(dummy, 3),
                GetNodeOffset(dummy, 4),
                GetNodeOffset(dummy, 5),
                GetNodeOffset(dummy, 6),
                GetNodeOffset(dummy, 7));
        }
    }
}
