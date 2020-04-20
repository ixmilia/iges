namespace IxMilia.Iges.Entities
{
    public class IgesLinearSolidWedge : IgesFiniteElement
    {
        public override IgesElementEdgeOrder EdgeOrder { get { return IgesElementEdgeOrder.Linear; } }

        public IgesPoint P1 { get; set; }
        public IgesPoint P2 { get; set; }
        public IgesPoint P3 { get; set; }
        public IgesPoint P4 { get; set; }
        public IgesPoint P5 { get; set; }
        public IgesPoint P6 { get; set; }

        public IgesLinearSolidWedge(
            IgesPoint p1,
            IgesPoint p2,
            IgesPoint p3,
            IgesPoint p4,
            IgesPoint p5,
            IgesPoint p6)
            : base(IgesTopologyType.LinearSolidWedge)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            P4 = p4;
            P5 = p5;
            P6 = p6;
        }

        protected override void AddNodes()
        {
            InternalNodes.Add(new IgesNode(P1));
            InternalNodes.Add(new IgesNode(P2));
            InternalNodes.Add(new IgesNode(P3));
            InternalNodes.Add(new IgesNode(P4));
            InternalNodes.Add(new IgesNode(P5));
            InternalNodes.Add(new IgesNode(P6));
        }

        internal static IgesLinearSolidWedge FromDummy(IgesFiniteElementDummy dummy)
        {
            return new IgesLinearSolidWedge(
                GetNodeOffset(dummy, 0),
                GetNodeOffset(dummy, 1),
                GetNodeOffset(dummy, 2),
                GetNodeOffset(dummy, 3),
                GetNodeOffset(dummy, 4),
                GetNodeOffset(dummy, 5));
        }
    }
}
