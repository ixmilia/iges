namespace IxMilia.Iges.Entities
{
    public class IgesAxisymmetricLinarLine : IgesFiniteElement
    {
        public override IgesElementEdgeOrder EdgeOrder { get { return IgesElementEdgeOrder.Linear; } }

        public IgesPoint P1 { get; set; }
        public IgesPoint P2 { get; set; }

        public IgesAxisymmetricLinarLine(
            IgesPoint p1,
            IgesPoint p2)
            : base(IgesTopologyType.AxisymmetricLinarLine)
        {
            P1 = p1;
            P2 = p2;
        }

        protected override void AddNodes()
        {
            InternalNodes.Add(new IgesNode(P1));
            InternalNodes.Add(new IgesNode(P2));
        }

        internal static IgesAxisymmetricLinarLine FromDummy(IgesFiniteElementDummy dummy)
        {
            return new IgesAxisymmetricLinarLine(
                GetNodeOffset(dummy, 0),
                GetNodeOffset(dummy, 1));
        }
    }
}
