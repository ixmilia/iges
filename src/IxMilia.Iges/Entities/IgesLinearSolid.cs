namespace IxMilia.Iges.Entities
{
    public class IgesLinearSolid : IgesFiniteElement
    {
        public override IgesElementEdgeOrder EdgeOrder { get { return IgesElementEdgeOrder.Linear; } }

        public IgesPoint P1 { get; set; }
        public IgesPoint P2 { get; set; }
        public IgesPoint P3 { get; set; }
        public IgesPoint P4 { get; set; }
        public IgesPoint P5 { get; set; }
        public IgesPoint P6 { get; set; }
        public IgesPoint P7 { get; set; }
        public IgesPoint P8 { get; set; }

        public IgesLinearSolid(
            IgesPoint p1,
            IgesPoint p2,
            IgesPoint p3,
            IgesPoint p4,
            IgesPoint p5,
            IgesPoint p6,
            IgesPoint p7,
            IgesPoint p8)
            : base(IgesTopologyType.LinearSolid)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            P4 = p4;
            P5 = p5;
            P6 = p6;
            P7 = p7;
            P8 = p8;
        }

        protected override void AddNodes()
        {
            InternalNodes.Add(new IgesNode(P1));
            InternalNodes.Add(new IgesNode(P2));
            InternalNodes.Add(new IgesNode(P3));
            InternalNodes.Add(new IgesNode(P4));
            InternalNodes.Add(new IgesNode(P5));
            InternalNodes.Add(new IgesNode(P6));
            InternalNodes.Add(new IgesNode(P7));
            InternalNodes.Add(new IgesNode(P8));
        }

        internal static IgesLinearSolid FromDummy(IgesFiniteElementDummy dummy)
        {
            return new IgesLinearSolid(
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
