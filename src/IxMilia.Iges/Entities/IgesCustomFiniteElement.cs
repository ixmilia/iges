using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesCustomFiniteElement : IgesFiniteElement
    {
        public int CustomTopologyNumber { get; set; }
        public List<IgesNode> Nodes { get; set; }
        protected override int TopologyNumber { get { return CustomTopologyNumber; } }
        public override IgesElementEdgeOrder EdgeOrder { get { return IgesElementEdgeOrder.NotApplicable; } }

        public IgesCustomFiniteElement(int customTopologyNumber)
            : base(IgesTopologyType.Custom)
        {
            CustomTopologyNumber = customTopologyNumber;
            Nodes = new List<IgesNode>();
        }

        protected override void AddNodes()
        {
            InternalNodes.AddRange(Nodes);
        }

        internal static IgesCustomFiniteElement FromDummy(IgesFiniteElementDummy dummy)
        {
            var custom = new IgesCustomFiniteElement((int)dummy.TopologyType);
            custom.Nodes.AddRange(dummy.InternalNodes);
            return custom;
        }
    }
}
