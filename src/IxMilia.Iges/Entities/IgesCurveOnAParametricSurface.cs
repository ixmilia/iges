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

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            CurveCreationType = (IgesCurveCreationType)Integer(parameters, 0);
            binder.BindEntity(Integer(parameters, 1), e => Surface = e);
            binder.BindEntity(Integer(parameters, 2), e => CurveDefinitionB = e);
            binder.BindEntity(Integer(parameters, 3), e => CurveDefinitionC = e);
            PreferredRepresentation = (IgesCurvePreferredRepresentation)Integer(parameters, 4);
            return 5;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return Surface;
            yield return CurveDefinitionB;
            yield return CurveDefinitionC;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add((int)CurveCreationType);
            parameters.Add(binder.GetEntityId(Surface));
            parameters.Add(binder.GetEntityId(CurveDefinitionB));
            parameters.Add(binder.GetEntityId(CurveDefinitionC));
            parameters.Add((int)PreferredRepresentation);
        }
    }
}
