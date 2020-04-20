namespace IxMilia.Iges.Entities
{
    public class IgesAxisymmetricParabolicTriangle : IgesFiniteElement
    {
        public override IgesElementEdgeOrder EdgeOrder { get { return IgesElementEdgeOrder.Parabolic; } }

        public IgesPoint P1 { get; set; }
        public IgesPoint P2 { get; set; }
        public IgesPoint P3 { get; set; }
        public IgesPoint P4 { get; set; }
        public IgesPoint P5 { get; set; }

        public IgesAxisymmetricParabolicTriangle(
            IgesPoint p1,
            IgesPoint p2,
            IgesPoint p3,
            IgesPoint p4,
            IgesPoint p5)
            : base(IgesTopologyType.AxisymmetricParabolicTriangle)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            P4 = p4;
            P5 = p5;
        }

        protected override void AddNodes()
        {
            InternalNodes.Add(new IgesNode(P1));
            InternalNodes.Add(new IgesNode(P2));
            InternalNodes.Add(new IgesNode(P3));
            InternalNodes.Add(new IgesNode(P4));
            InternalNodes.Add(new IgesNode(P5));
        }

        internal static IgesAxisymmetricParabolicTriangle FromDummy(IgesFiniteElementDummy dummy)
        {
            return new IgesAxisymmetricParabolicTriangle(
                GetNodeOffset(dummy, 0),
                GetNodeOffset(dummy, 1),
                GetNodeOffset(dummy, 2),
                GetNodeOffset(dummy, 3),
                GetNodeOffset(dummy, 4));
        }
    }
}
