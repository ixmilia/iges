namespace IxMilia.Iges.Entities
{
    public class IgesParabolicThickShellWedge : IgesFiniteElement
    {
        public override IgesElementEdgeOrder EdgeOrder { get { return IgesElementEdgeOrder.Parabolic; } }

        public IgesPoint P1 { get; set; }
        public IgesPoint P1P2Control { get; set; }
        public IgesPoint P2 { get; set; }
        public IgesPoint P2P3Control { get; set; }
        public IgesPoint P3 { get; set; }
        public IgesPoint P3P1Control { get; set; }
        public IgesPoint P4 { get; set; }
        public IgesPoint P4P5Control { get; set; }
        public IgesPoint P5 { get; set; }
        public IgesPoint P5P6Control { get; set; }
        public IgesPoint P6 { get; set; }
        public IgesPoint P6P4Control { get; set; }

        public IgesParabolicThickShellWedge(
            IgesPoint p1,
            IgesPoint p1P2Control,
            IgesPoint p2,
            IgesPoint p2P3Control,
            IgesPoint p3,
            IgesPoint p3P1Control,
            IgesPoint p4,
            IgesPoint p4P5Control,
            IgesPoint p5,
            IgesPoint p5P6Control,
            IgesPoint p6,
            IgesPoint p6P4Control)
            : base(IgesTopologyType.ParabolicThickShellWedge)
        {
            P1 = p1;
            P1P2Control = p1P2Control;
            P2 = p2;
            P2P3Control = p2P3Control;
            P3 = p3;
            P3P1Control = p3P1Control;
            P4 = p4;
            P4P5Control = p4P5Control;
            P5 = p5;
            P5P6Control = p5P6Control;
            P6 = p6;
            P6P4Control = p6P4Control;
        }

        protected override void AddNodes()
        {
            InternalNodes.Add(new IgesNode(P1));
            InternalNodes.Add(new IgesNode(P1P2Control));
            InternalNodes.Add(new IgesNode(P2));
            InternalNodes.Add(new IgesNode(P2P3Control));
            InternalNodes.Add(new IgesNode(P3));
            InternalNodes.Add(new IgesNode(P3P1Control));
            InternalNodes.Add(new IgesNode(P4));
            InternalNodes.Add(new IgesNode(P4P5Control));
            InternalNodes.Add(new IgesNode(P5));
            InternalNodes.Add(new IgesNode(P5P6Control));
            InternalNodes.Add(new IgesNode(P6));
            InternalNodes.Add(new IgesNode(P6P4Control));
        }

        internal static IgesParabolicThickShellWedge FromDummy(IgesFiniteElementDummy dummy)
        {
            return new IgesParabolicThickShellWedge(
                GetNodeOffset(dummy, 0),
                GetNodeOffset(dummy, 1),
                GetNodeOffset(dummy, 2),
                GetNodeOffset(dummy, 3),
                GetNodeOffset(dummy, 4),
                GetNodeOffset(dummy, 5),
                GetNodeOffset(dummy, 6),
                GetNodeOffset(dummy, 7),
                GetNodeOffset(dummy, 8),
                GetNodeOffset(dummy, 9),
                GetNodeOffset(dummy, 10),
                GetNodeOffset(dummy, 11));
        }
    }
}
