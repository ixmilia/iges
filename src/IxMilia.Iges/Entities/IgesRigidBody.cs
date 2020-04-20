namespace IxMilia.Iges.Entities
{
    public class IgesRigidBody : IgesFiniteElement
    {
        public override IgesElementEdgeOrder EdgeOrder { get { return IgesElementEdgeOrder.NotApplicable; } }

        public IgesPoint P1 { get; set; }
        public IgesPoint P2 { get; set; }

        public IgesRigidBody(
            IgesPoint p1,
            IgesPoint p2)
            : base(IgesTopologyType.RigidBody)
        {
            P1 = p1;
            P2 = p2;
        }

        protected override void AddNodes()
        {
            InternalNodes.Add(new IgesNode(P1));
            InternalNodes.Add(new IgesNode(P2));
        }

        internal static IgesRigidBody FromDummy(IgesFiniteElementDummy dummy)
        {
            return new IgesRigidBody(
                GetNodeOffset(dummy, 0),
                GetNodeOffset(dummy, 1));
        }
    }
}
