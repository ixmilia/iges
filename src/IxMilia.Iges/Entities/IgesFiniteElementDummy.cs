using System;
using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    /// <summary>
    /// This class only exists as a placeholder for the parameter values.  The <see cref="PostProcess"/> method creates
    /// the final object that gets added to the file.
    /// </summary>
    internal class IgesFiniteElementDummy : IgesFiniteElement
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.FiniteElement; } }
        public override IgesElementEdgeOrder EdgeOrder { get { throw new NotImplementedException(); } }

        public IgesFiniteElementDummy()
            : base((IgesTopologyType)(-1))
        {
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            int index = 0;
            TopologyType = (IgesTopologyType)Integer(parameters, index++);
            var nodeCount = Integer(parameters, index++);
            for (var i = 0; i < nodeCount; i++)
            {
                binder.BindEntity(Integer(parameters, index++), e => InternalNodes.Add(e as IgesNode));
            }

            ElementTypeName = String(parameters, index++);
            return index;
        }

        protected override void AddNodes()
        {
            throw new NotImplementedException();
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            throw new NotImplementedException();
        }

        internal override IgesEntity PostProcess()
        {
            IgesFiniteElement result;
            switch (TopologyType)
            {
                case IgesTopologyType.Beam:
                    result = IgesBeam.FromDummy(this);
                    break;
                case IgesTopologyType.LinearTriangle:
                    result = IgesLinearTriangle.FromDummy(this);
                    break;
                case IgesTopologyType.ParabolicTriangle:
                    result = IgesParabolicTriangle.FromDummy(this);
                    break;
                case IgesTopologyType.CubicTriangle:
                    result = IgesCubicTriangle.FromDummy(this);
                    break;
                case IgesTopologyType.LinearQuadrilateral:
                    result = IgesLinearQuadrilateral.FromDummy(this);
                    break;
                case IgesTopologyType.ParabolicQuadrilateral:
                    result = IgesParabolicQuadrilateral.FromDummy(this);
                    break;
                case IgesTopologyType.CubicQuadrilateral:
                    result = IgesCubicQuadrilateral.FromDummy(this);
                    break;
                case IgesTopologyType.ParabolicThickShellWedge:
                    result = IgesParabolicThickShellWedge.FromDummy(this);
                    break;
                case IgesTopologyType.CubicThickShellWedge:
                    result = IgesCubicThickShellWedge.FromDummy(this);
                    break;
                case IgesTopologyType.ParabolicThickShell:
                    result = IgesParabolicThickShell.FromDummy(this);
                    break;
                case IgesTopologyType.CubicThickShell:
                    result = IgesCubicThickShell.FromDummy(this);
                    break;
                case IgesTopologyType.LinearSolidTetrahedron:
                    result = IgesLinearSolidTetrahedron.FromDummy(this);
                    break;
                case IgesTopologyType.ParabolicSolidTetrahedron:
                    result = IgesParabolicSolidTetrahedron.FromDummy(this);
                    break;
                case IgesTopologyType.LinearSolidWedge:
                    result = IgesLinearSolidWedge.FromDummy(this);
                    break;
                case IgesTopologyType.ParabolicSolidWedge:
                    result = IgesParabolicSolidWedge.FromDummy(this);
                    break;
                case IgesTopologyType.CubicSolidWedge:
                    result = IgesCubicSolidWedge.FromDummy(this);
                    break;
                case IgesTopologyType.LinearSolid:
                    result = IgesLinearSolid.FromDummy(this);
                    break;
                case IgesTopologyType.ParabolicSolid:
                    result = IgesParabolicSolid.FromDummy(this);
                    break;
                case IgesTopologyType.CubicSolid:
                    result = IgesCubicSolid.FromDummy(this);
                    break;
                case IgesTopologyType.AxisymmetricLinarLine:
                    result = IgesAxisymmetricLinarLine.FromDummy(this);
                    break;
                case IgesTopologyType.AxisymmetricParabolicLine:
                    result = IgesAxisymmetricParabolicLine.FromDummy(this);
                    break;
                case IgesTopologyType.AxisymmetricCubicLine:
                    result = IgesAxisymmetricCubicLine.FromDummy(this);
                    break;
                case IgesTopologyType.AxisymmetricLinearTriangle:
                    result = IgesAxisymmetricLinearTriangle.FromDummy(this);
                    break;
                case IgesTopologyType.AxisymmetricParabolicTriangle:
                    result = IgesAxisymmetricParabolicTriangle.FromDummy(this);
                    break;
                case IgesTopologyType.AxisymmetricLinearQuadrilateral:
                    result = IgesAxisymmetricLinearQuadrilateral.FromDummy(this);
                    break;
                case IgesTopologyType.AxisymmetricParabolicQuadrilateral:
                    result = IgesAxisymmetricParabolicQuadrilateral.FromDummy(this);
                    break;
                case IgesTopologyType.Spring:
                    result = IgesSpring.FromDummy(this);
                    break;
                case IgesTopologyType.GroundedSpring:
                    result = IgesGroundedSpring.FromDummy(this);
                    break;
                case IgesTopologyType.Damper:
                    result = IgesDamper.FromDummy(this);
                    break;
                case IgesTopologyType.GroundedDamper:
                    result = IgesGroundedDamper.FromDummy(this);
                    break;
                case IgesTopologyType.Mass:
                    result = IgesMass.FromDummy(this);
                    break;
                case IgesTopologyType.RigidBody:
                    result = IgesRigidBody.FromDummy(this);
                    break;
                case IgesTopologyType.ThreeNodedBeam:
                    result = IgesThreeNodedBeam.FromDummy(this);
                    break;
                case IgesTopologyType.OffsetMass:
                    result = IgesOffsetMass.FromDummy(this);
                    break;
                case IgesTopologyType.OffsetBeam:
                    result = IgesOffsetBeam.FromDummy(this);
                    break;
                case IgesTopologyType.ThreeNodeBeam:
                    result = IgesThreeNodeBeam.FromDummy(this);
                    break;
                case IgesTopologyType.CurvedBeam:
                    result = IgesCurvedBeam.FromDummy(this);
                    break;
                case IgesTopologyType.CubicParabolicSolidWedge:
                    result = IgesCubicParabolicSolidWedge.FromDummy(this);
                    break;
                default:
                    result = (int)TopologyType >= (int)IgesTopologyType.Custom
                        ? IgesCustomFiniteElement.FromDummy(this)
                        : null;
                    break;
            }

            if (result != null)
            {
                result.ElementTypeName = ElementTypeName;
            }

            return result;
        }
    }
}
