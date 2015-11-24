// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public abstract partial class IgesEntity
    {
        internal static IgesEntity FromData(IgesDirectoryData directoryData, List<string> parameters)
        {
            IgesEntity entity = null;
            switch (directoryData.EntityType)
            {
                case IgesEntityType.AssociativityInstance:
                    switch (directoryData.FormNumber)
                    {
                        case 5:
                            entity = new IgesLabelDisplayAssociativity();
                            break;
                    }
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
                case IgesEntityType.CurveOnAParametricSurface:
                    entity = new IgesCurveOnAParametricSurface();
                    break;
                case IgesEntityType.Direction:
                    entity = new IgesDirection();
                    break;
                case IgesEntityType.Flash:
                    entity = new IgesFlash();
                    break;
                case IgesEntityType.FiniteElement:
                    entity = new IgesFiniteElementDummy();
                    break;
                case IgesEntityType.GeneralNote:
                    entity = new IgesGeneralNote();
                    break;
                case IgesEntityType.Leader:
                    entity = new IgesLeader();
                    break;
                case IgesEntityType.Line:
                    entity = new IgesLine();
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
                case IgesEntityType.NodalDisplacementAndRotation:
                    entity = new IgesNodalDisplacementAndRotation();
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
                case IgesEntityType.ParametricSplineCurve:
                    entity = new IgesParametricSplineCurve();
                    break;
                case IgesEntityType.ParametricSplineSurface:
                    entity = new IgesParametricSplineSurface();
                    break;
                case IgesEntityType.Plane:
                    entity = new IgesPlane();
                    break;
                case IgesEntityType.Point:
                    entity = new IgesLocation();
                    break;
                case IgesEntityType.Property:
                    switch (directoryData.FormNumber)
                    {
                        case 1:
                            entity = new IgesDefinitionLevelsProperty();
                            break;
                    }
                    break;
                case IgesEntityType.RationalBSplineCurve:
                    entity = new IgesRationalBSplineCurve();
                    break;
                case IgesEntityType.RationalBSplineSurface:
                    entity = new IgesRationalBSplineSurface();
                    break;
                case IgesEntityType.RuledSurface:
                    entity = new IgesRuledSurface();
                    break;
                case IgesEntityType.Sphere:
                    entity = new IgesSphere();
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
                case IgesEntityType.Torus:
                    entity = new IgesTorus();
                    break;
                case IgesEntityType.TransformationMatrix:
                    entity = new IgesTransformationMatrix();
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
                int nextIndex = entity.ReadParameters(parameters);
                entity.ReadCommonPointers(parameters, nextIndex);
            }

            return entity;
        }
    }
}
