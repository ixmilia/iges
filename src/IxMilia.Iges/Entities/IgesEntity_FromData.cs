using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public abstract partial class IgesEntity
    {
        internal static IgesEntity FromData(IgesDirectoryData directoryData, List<string> parameters, IgesReaderBinder binder)
        {
            IgesEntity entity = null;
            switch (directoryData.EntityType)
            {
                case IgesEntityType.AngularDimension:
                    entity = new IgesAngularDimension();
                    break;
                case IgesEntityType.AssociativityInstance:
                    switch (directoryData.FormNumber)
                    {
                        case 5:
                            entity = new IgesLabelDisplayAssociativity();
                            break;
                    }
                    break;
                case IgesEntityType.Block:
                    entity = new IgesBlock();
                    break;
                case IgesEntityType.BooleanTree:
                    entity = new IgesBooleanTree();
                    break;
                case IgesEntityType.Boundary:
                    entity = new IgesBoundary();
                    break;
                case IgesEntityType.BoundedSurface:
                    entity = new IgesBoundedSurface();
                    break;
                case IgesEntityType.CircularArc:
                    entity = new IgesCircularArc();
                    break;
                case IgesEntityType.ColorDefinition:
                    entity = new IgesColorDefinition();
                    break;
                case IgesEntityType.CompositeCurve:
                    entity = new IgesCompositeCurve();
                    break;
                case IgesEntityType.ConicArc:
                    entity = new IgesConicArc();
                    break;
                case IgesEntityType.ConnectPoint:
                    entity = new IgesConnectPoint();
                    break;
                case IgesEntityType.CopiousData:
                    entity = new IgesCopiousData();
                    break;
                case IgesEntityType.CurveDimension:
                    entity = new IgesCurveDimension();
                    break;
                case IgesEntityType.CurveOnAParametricSurface:
                    entity = new IgesCurveOnAParametricSurface();
                    break;
                case IgesEntityType.DiameterDimension:
                    entity = new IgesDiameterDimension();
                    break;
                case IgesEntityType.Direction:
                    entity = new IgesDirection();
                    break;
                case IgesEntityType.ElementResults:
                    entity = new IgesElementResults();
                    break;
                case IgesEntityType.Ellipsoid:
                    entity = new IgesEllipsoid();
                    break;
                case IgesEntityType.FlagNote:
                    entity = new IgesFlagNote();
                    break;
                case IgesEntityType.Flash:
                    entity = new IgesFlash();
                    break;
                case IgesEntityType.FiniteElement:
                    entity = new IgesFiniteElementDummy();
                    break;
                case IgesEntityType.GeneralLabel:
                    entity = new IgesGeneralLabel();
                    break;
                case IgesEntityType.GeneralNote:
                    entity = new IgesGeneralNote();
                    break;
                case IgesEntityType.GeneralSymbol:
                    entity = new IgesGeneralSymbol();
                    break;
                case IgesEntityType.Leader:
                    entity = new IgesLeader();
                    break;
                case IgesEntityType.Line:
                    entity = new IgesLine();
                    break;
                case IgesEntityType.LinearDimension:
                    entity = new IgesLinearDimension();
                    break;
                case IgesEntityType.LineFontDefinition:
                    switch (directoryData.FormNumber)
                    {
                        case 1:
                            entity = new IgesTemplateLineFontDefinition();
                            break;
                        case 2:
                            entity = new IgesPatternLineFontDefinition();
                            break;
                    }
                    break;
                case IgesEntityType.ManifestSolidBRepObject:
                    entity = new IgesManifestSolidBRepObject();
                    break;
                case IgesEntityType.NewGeneralNote:
                    entity = new IgesNewGeneralNote();
                    break;
                case IgesEntityType.NodalDisplacementAndRotation:
                    entity = new IgesNodalDisplacementAndRotation();
                    break;
                case IgesEntityType.NodalResults:
                    entity = new IgesNodalResults();
                    break;
                case IgesEntityType.Node:
                    entity = new IgesNode();
                    break;
                case IgesEntityType.Null:
                    entity = new IgesNull();
                    break;
                case IgesEntityType.OffsetCurve:
                    entity = new IgesOffsetCurve();
                    break;
                case IgesEntityType.OffsetSurface:
                    entity = new IgesOffsetSurface();
                    break;
                case IgesEntityType.OrdinateDimension:
                    entity = new IgesOrdinateDimension();
                    break;
                case IgesEntityType.ParametricSplineCurve:
                    entity = new IgesParametricSplineCurve();
                    break;
                case IgesEntityType.ParametricSplineSurface:
                    entity = new IgesParametricSplineSurface();
                    break;
                case IgesEntityType.Plane:
                    entity = new IgesPlane();
                    break;
                case IgesEntityType.PlaneSurface:
                    entity = new IgesPlaneSurface();
                    break;
                case IgesEntityType.Point:
                    entity = new IgesLocation();
                    break;
                case IgesEntityType.PointDimension:
                    entity = new IgesPointDimension();
                    break;
                case IgesEntityType.Property:
                    switch (directoryData.FormNumber)
                    {
                        case 1:
                            entity = new IgesDefinitionLevelsProperty();
                            break;
                        case 15:
                            entity = new IgesNameProperty();
                            break;
                    }
                    break;
                case IgesEntityType.RadiusDimension:
                    entity = new IgesRadiusDimension();
                    break;
                case IgesEntityType.RationalBSplineCurve:
                    entity = new IgesRationalBSplineCurve();
                    break;
                case IgesEntityType.RationalBSplineSurface:
                    entity = new IgesRationalBSplineSurface();
                    break;
                case IgesEntityType.RightAngularWedge:
                    entity = new IgesRightAngularWedge();
                    break;
                case IgesEntityType.RightCircularConeFrustrum:
                    entity = new IgesRightCircularConeFrustrum();
                    break;
                case IgesEntityType.RightCircularConicalSurface:
                    entity = new IgesRightCircularConicalSurface();
                    break;
                case IgesEntityType.RightCircularCylinder:
                    entity = new IgesRightCircularCylinder();
                    break;
                case IgesEntityType.RightCircularCylindricalSurface:
                    entity = new IgesRightCircularCylindricalSurface();
                    break;
                case IgesEntityType.RuledSurface:
                    entity = new IgesRuledSurface();
                    break;
                case IgesEntityType.SectionedArea:
                    entity = new IgesSectionedArea();
                    break;
                case IgesEntityType.SelectedComponent:
                    entity = new IgesSelectedComponent();
                    break;
                case IgesEntityType.SingularSubfigureInstance:
                    entity = new IgesSingularSubfigureInstance();
                    break;
                case IgesEntityType.SolidAssembly:
                    entity = new IgesSolidAssembly();
                    break;
                case IgesEntityType.SolidOfLinearExtrusion:
                    entity = new IgesSolidOfLinearExtrusion();
                    break;
                case IgesEntityType.SolidOfRevolution:
                    entity = new IgesSolidOfRevolution();
                    break;
                case IgesEntityType.Sphere:
                    entity = new IgesSphere();
                    break;
                case IgesEntityType.SphericalSurface:
                    entity = new IgesSphericalSurface();
                    break;
                case IgesEntityType.SubfigureDefinition:
                    entity = new IgesSubfigureDefinition();
                    break;
                case IgesEntityType.SurfaceOfRevolution:
                    entity = new IgesSurfaceOfRevolution();
                    break;
                case IgesEntityType.TabulatedCylinder:
                    entity = new IgesTabulatedCylinder();
                    break;
                case IgesEntityType.TextDisplayTemplate:
                    entity = new IgesTextDisplayTemplate();
                    break;
                case IgesEntityType.TextFontDefinition:
                    entity = new IgesTextFontDefinition();
                    break;
                case IgesEntityType.ToroidalSurface:
                    entity = new IgesToroidalSurface();
                    break;
                case IgesEntityType.Torus:
                    entity = new IgesTorus();
                    break;
                case IgesEntityType.TransformationMatrix:
                    entity = new IgesTransformationMatrix();
                    break;
                case IgesEntityType.TrimmedParametricSurface:
                    entity = new IgesTrimmedParametricSurface();
                    break;
                case IgesEntityType.View:
                    switch (directoryData.FormNumber)
                    {
                        case 0:
                            entity = new IgesView();
                            break;
                        case 1:
                            entity = new IgesPerspectiveView();
                            break;
                    }
                    break;
            }

            if (entity != null)
            {
                entity.PopulateDirectoryData(directoryData);
                int nextIndex = entity.ReadParameters(parameters, binder);
                entity.ReadCommonPointers(parameters, nextIndex, binder);
            }

            return entity;
        }
    }
}
