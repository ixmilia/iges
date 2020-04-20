namespace IxMilia.Iges.Entities
{
    public class IgesCubicQuadrilateral : IgesFiniteElement
    {
        public override IgesElementEdgeOrder EdgeOrder { get { return IgesElementEdgeOrder.Cubic; } }

        public IgesPoint P1 { get; set; }
        public IgesPoint P1P2Control1 { get; set; }
        public IgesPoint P1P2Control2 { get; set; }
        public IgesPoint P2 { get; set; }
        public IgesPoint P2P3Control1 { get; set; }
        public IgesPoint P2P3Control2 { get; set; }
        public IgesPoint P3 { get; set; }
        public IgesPoint P3P4Control1 { get; set; }
        public IgesPoint P3P4Control2 { get; set; }
        public IgesPoint P4 { get; set; }
        public IgesPoint P4P1Control1 { get; set; }
        public IgesPoint P4P1Control2 { get; set; }

        public IgesCubicQuadrilateral(
            IgesPoint p1,
            IgesPoint p1P2Control1,
            IgesPoint p1P2Control2,
            IgesPoint p2,
            IgesPoint p2P3Control1,
            IgesPoint p2P3Control2,
            IgesPoint p3,
            IgesPoint p3P4Control1,
            IgesPoint p3P4Control2,
            IgesPoint p4,
            IgesPoint p4P1Control1,
            IgesPoint p4P1Control2)
            : base(IgesTopologyType.CubicQuadrilateral)
        {
            P1 = p1;
            P1P2Control1 = p1P2Control1;
            P1P2Control2 = p1P2Control2;
            P2 = p2;
            P2P3Control1 = p2P3Control1;
            P2P3Control2 = p2P3Control2;
            P3 = p3;
            P3P4Control1 = p3P4Control1;
            P3P4Control2 = p3P4Control2;
            P4 = p4;
            P4P1Control1 = p4P1Control1;
            P4P1Control2 = p4P1Control2;
        }

        protected override void AddNodes()
        {
            InternalNodes.Add(new IgesNode(P1));
            InternalNodes.Add(new IgesNode(P1P2Control1));
            InternalNodes.Add(new IgesNode(P1P2Control2));
            InternalNodes.Add(new IgesNode(P2));
            InternalNodes.Add(new IgesNode(P2P3Control1));
            InternalNodes.Add(new IgesNode(P2P3Control2));
            InternalNodes.Add(new IgesNode(P3));
            InternalNodes.Add(new IgesNode(P3P4Control1));
            InternalNodes.Add(new IgesNode(P3P4Control2));
            InternalNodes.Add(new IgesNode(P4));
            InternalNodes.Add(new IgesNode(P4P1Control1));
            InternalNodes.Add(new IgesNode(P4P1Control2));
        }

        internal static IgesCubicQuadrilateral FromDummy(IgesFiniteElementDummy dummy)
        {
            return new IgesCubicQuadrilateral(
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
