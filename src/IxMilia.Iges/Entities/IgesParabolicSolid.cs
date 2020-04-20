namespace IxMilia.Iges.Entities
{
    public class IgesParabolicSolid : IgesFiniteElement
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
        public IgesPoint P5P1Control { get; set; }
        public IgesPoint P6P2Control { get; set; }
        public IgesPoint P7P3Control { get; set; }
        public IgesPoint P8P4Control { get; set; }
        public IgesPoint P5 { get; set; }
        public IgesPoint P5P6Control { get; set; }
        public IgesPoint P6 { get; set; }
        public IgesPoint P6P7Control { get; set; }
        public IgesPoint P7 { get; set; }
        public IgesPoint P7P8Control { get; set; }
        public IgesPoint P8 { get; set; }
        public IgesPoint P8P5Control { get; set; }

        public IgesParabolicSolid(
            IgesPoint p1,
            IgesPoint p1P2Control,
            IgesPoint p2,
            IgesPoint p2P3Control,
            IgesPoint p3,
            IgesPoint p3P4Control,
            IgesPoint p4,
            IgesPoint p4P1Control,
            IgesPoint p5P1Control,
            IgesPoint p6P2Control,
            IgesPoint p7P3Control,
            IgesPoint p8P4Control,
            IgesPoint p5,
            IgesPoint p5P6Control,
            IgesPoint p6,
            IgesPoint p6P7Control,
            IgesPoint p7,
            IgesPoint p7P8Control,
            IgesPoint p8,
            IgesPoint p8P5Control)
            : base(IgesTopologyType.ParabolicSolid)
        {
            P1 = p1;
            P1P2Control = p1P2Control;
            P2 = p2;
            P2P3Control = p2P3Control;
            P3 = p3;
            P3P4Control = p3P4Control;
            P4 = p4;
            P4P1Control = p4P1Control;
            P5P1Control = p5P1Control;
            P6P2Control = p6P2Control;
            P7P3Control = p7P3Control;
            P8P4Control = p8P4Control;
            P5 = p5;
            P5P6Control = p5P6Control;
            P6 = p6;
            P6P7Control = p6P7Control;
            P7 = p7;
            P7P8Control = p7P8Control;
            P8 = p8;
            P8P5Control = p8P5Control;
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
            InternalNodes.Add(new IgesNode(P5P1Control));
            InternalNodes.Add(new IgesNode(P6P2Control));
            InternalNodes.Add(new IgesNode(P7P3Control));
            InternalNodes.Add(new IgesNode(P8P4Control));
            InternalNodes.Add(new IgesNode(P5));
            InternalNodes.Add(new IgesNode(P5P6Control));
            InternalNodes.Add(new IgesNode(P6));
            InternalNodes.Add(new IgesNode(P6P7Control));
            InternalNodes.Add(new IgesNode(P7));
            InternalNodes.Add(new IgesNode(P7P8Control));
            InternalNodes.Add(new IgesNode(P8));
            InternalNodes.Add(new IgesNode(P8P5Control));
        }

        internal static IgesParabolicSolid FromDummy(IgesFiniteElementDummy dummy)
        {
            return new IgesParabolicSolid(
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
                GetNodeOffset(dummy, 11),
                GetNodeOffset(dummy, 12),
                GetNodeOffset(dummy, 13),
                GetNodeOffset(dummy, 14),
                GetNodeOffset(dummy, 15),
                GetNodeOffset(dummy, 16),
                GetNodeOffset(dummy, 17),
                GetNodeOffset(dummy, 18),
                GetNodeOffset(dummy, 19));
        }
    }
}
