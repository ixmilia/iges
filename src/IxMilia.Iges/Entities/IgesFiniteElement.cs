using System;
using System.Collections.Generic;
using System.Linq;

namespace IxMilia.Iges.Entities
{
    public enum IgesTopologyType
    {
        Beam = 1,
        LinearTriangle = 2,
        ParabolicTriangle = 3,
        CubicTriangle = 4,
        LinearQuadrilateral = 5,
        ParabolicQuadrilateral = 6,
        CubicQuadrilateral = 7,
        ParabolicThickShellWedge = 8,
        CubicThickShellWedge = 9,
        ParabolicThickShell = 10,
        CubicThickShell = 11,
        LinearSolidTetrahedron = 12,
        ParabolicSolidTetrahedron = 13,
        LinearSolidWedge = 14,
        ParabolicSolidWedge = 15,
        CubicSolidWedge = 16,
        LinearSolid = 17,
        ParabolicSolid = 18,
        CubicSolid = 19,
        AxisymmetricLinarLine = 20,
        AxisymmetricParabolicLine = 21,
        AxisymmetricCubicLine = 22,
        AxisymmetricLinearTriangle = 23,
        AxisymmetricParabolicTriangle = 24,
        AxisymmetricLinearQuadrilateral = 25,
        AxisymmetricParabolicQuadrilateral = 26,
        Spring = 27,
        GroundedSpring = 28,
        Damper = 29,
        GroundedDamper = 30,
        Mass = 31,
        RigidBody = 32,
        ThreeNodedBeam = 33,
        OffsetMass = 34,
        OffsetBeam = 35,
        ThreeNodeBeam = 36,
        CurvedBeam = 37,
        CubicParabolicSolidWedge = 38,
        Custom = 5001
    }

    public enum IgesElementEdgeOrder
    {
        NotApplicable = 0,
        Linear = 1,
        Parabolic = 2,
        Cubic = 3
    }

    public abstract class IgesFiniteElement : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.FiniteElement; } }

        public IgesTopologyType TopologyType { get; protected set; }
        protected virtual int TopologyNumber { get { return (int)TopologyType; } }
        internal List<IgesNode> InternalNodes { get; private set; }
        public string ElementTypeName { get; set; }

        public abstract IgesElementEdgeOrder EdgeOrder { get; }

        protected IgesFiniteElement(IgesTopologyType topologyType)
        {
            InternalNodes = new List<IgesNode>();
            TopologyType = topologyType;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            throw new NotImplementedException();
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            InternalNodes.Clear();
            AddNodes();
            return InternalNodes;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(this.TopologyNumber);
            parameters.Add(this.InternalNodes.Count);
            parameters.AddRange(InternalNodes.Select(binder.GetEntityId).Cast<object>());
            parameters.Add(ElementTypeName);

            InternalNodes.Clear();
        }

        protected abstract void AddNodes();

        internal static IgesPoint GetNodeOffset(IgesFiniteElementDummy dummy, int index)
        {
            return dummy.InternalNodes.Count > index
                ? dummy.InternalNodes[index].Offset
                : IgesPoint.Origin;
        }
    }
}
