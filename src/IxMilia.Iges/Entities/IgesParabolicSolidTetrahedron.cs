namespace IxMilia.Iges.Entities
{
    public class IgesParabolicSolidTetrahedron : IgesFiniteElement
    {
        public override IgesElementEdgeOrder EdgeOrder { get { return IgesElementEdgeOrder.Parabolic; } }

        public IgesPoint P1 { get; set; }
        public IgesPoint P1P2Control { get; set; }
        public IgesPoint P2 { get; set; }
        public IgesPoint P2P3Control { get; set; }
        public IgesPoint P3 { get; set; }
        public IgesPoint P3P1Control { get; set; }
        public IgesPoint P4P1Control { get; set; }
        public IgesPoint P4P2Control { get; set; }
        public IgesPoint P4P3Control { get; set; }
        public IgesPoint P4 { get; set; }

        public IgesParabolicSolidTetrahedron(
            IgesPoint p1,
            IgesPoint p1P2Control,
            IgesPoint p2,
            IgesPoint p2P3Control,
            IgesPoint p3,
            IgesPoint p3P1Control,
            IgesPoint p4P1Control,
            IgesPoint p4P2Control,
            IgesPoint p4P3Control,
            IgesPoint p4)
            : base(IgesTopologyType.ParabolicSolidTetrahedron)
        {
            P1 = p1;
            P1P2Control = p1P2Control;
            P2 = p2;
            P2P3Control = p2P3Control;
            P3 = p3;
            P3P1Control = p3P1Control;
            P4P1Control = p4P1Control;
            P4P2Control = p4P2Control;
            P4P3Control = p4P3Control;
            P4 = p4;
        }

        protected override void AddNodes()
        {
            InternalNodes.Add(new IgesNode(P1));
            InternalNodes.Add(new IgesNode(P1P2Control));
            InternalNodes.Add(new IgesNode(P2));
            InternalNodes.Add(new IgesNode(P2P3Control));
            InternalNodes.Add(new IgesNode(P3));
            InternalNodes.Add(new IgesNode(P3P1Control));
            InternalNodes.Add(new IgesNode(P4P1Control));
            InternalNodes.Add(new IgesNode(P4P2Control));
            InternalNodes.Add(new IgesNode(P4P3Control));
            InternalNodes.Add(new IgesNode(P4));
        }

        internal static IgesParabolicSolidTetrahedron FromDummy(IgesFiniteElementDummy dummy)
        {
            return new IgesParabolicSolidTetrahedron(
                GetNodeOffset(dummy, 0),
                GetNodeOffset(dummy, 1),
                GetNodeOffset(dummy, 2),
                GetNodeOffset(dummy, 3),
                GetNodeOffset(dummy, 4),
                GetNodeOffset(dummy, 5),
                GetNodeOffset(dummy, 6),
                GetNodeOffset(dummy, 7),
                GetNodeOffset(dummy, 8),
                GetNodeOffset(dummy, 9));
        }
    }
}
