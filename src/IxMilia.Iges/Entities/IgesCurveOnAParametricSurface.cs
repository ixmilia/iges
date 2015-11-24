// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public enum IgesCurveCreationType
    {
        Unspecified = 0,
        Projection = 1,
        Intersection = 2,
        Isoparametric = 3
    }

    public enum IgesCurvePreferredRepresentation
    {
        Unspecified = 0,
        SurfaceAndB = 1,
        C = 2,
        CAndSurfaceAndB = 3
    }

    public class IgesCurveOnAParametricSurface : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.CurveOnAParametricSurface; } }

        public IgesCurveCreationType CurveCreationType { get; set; }
        public IgesEntity Surface { get; set; }
        public IgesEntity CurveDefinitionB { get; set; }
        public IgesEntity CurveDefinitionC { get; set; }
        public IgesCurvePreferredRepresentation PreferredRepresentation { get; set; }

        protected override int ReadParameters(List<string> parameters)
        {
            CurveCreationType = (IgesCurveCreationType)Integer(parameters, 0);
            SubEntityIndices.Add(Integer(parameters, 1)); // Surface
            SubEntityIndices.Add(Integer(parameters, 2)); // CurveDefinitionB
            SubEntityIndices.Add(Integer(parameters, 3)); // CurveDefinitionC
            PreferredRepresentation = (IgesCurvePreferredRepresentation)Integer(parameters, 4);
            return 5;
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            Surface = SubEntities[0];
            CurveDefinitionB = SubEntities[1];
            CurveDefinitionC = SubEntities[2];
        }

        internal override void OnBeforeWrite()
        {
            SubEntities.Add(Surface);
            SubEntities.Add(CurveDefinitionB);
            SubEntities.Add(CurveDefinitionC);
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add((int)CurveCreationType);
            parameters.Add(SubEntityIndices[0]); // Surface
            parameters.Add(SubEntityIndices[1]); // CurveDefinitionB
            parameters.Add(SubEntityIndices[2]); // CurveDefinitionC
            parameters.Add((int)PreferredRepresentation);
        }
    }
}
