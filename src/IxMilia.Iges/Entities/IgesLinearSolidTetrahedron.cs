namespace IxMilia.Iges.Entities
{
    public class IgesLinearSolidTetrahedron : IgesFiniteElement
    {
        public override IgesElementEdgeOrder EdgeOrder { get { return IgesElementEdgeOrder.Linear; } }

        public IgesPoint P1 { get; set; }
        public IgesPoint P2 { get; set; }
        public IgesPoint P3 { get; set; }
        public IgesPoint P4 { get; set; }

        public IgesLinearSolidTetrahedron(
            IgesPoint p1,
            IgesPoint p2,
            IgesPoint p3,
            IgesPoint p4)
            : base(IgesTopologyType.LinearSolidTetrahedron)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            P4 = p4;
        }

        protected override void AddNodes()
        {
            InternalNodes.Add(new IgesNode(P1));
            InternalNodes.Add(new IgesNode(P2));
            InternalNodes.Add(new IgesNode(P3));
            InternalNodes.Add(new IgesNode(P4));
        }

        internal static IgesLinearSolidTetrahedron FromDummy(IgesFiniteElementDummy dummy)
        {
            return new IgesLinearSolidTetrahedron(
                GetNodeOffset(dummy, 0),
                GetNodeOffset(dummy, 1),
                GetNodeOffset(dummy, 2),
                GetNodeOffset(dummy, 3));
        }
    }
}
