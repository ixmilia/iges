// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesSolidOfLinearExtrusion : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.SolidOfLinearExtrusion; } }

        public IgesEntity Curve { get; set; }
        public double ExtrusionLength { get; set; }
        public IgesVector ExtrusionDirection { get; set; } = IgesVector.ZAxis;

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            int index = 0;
            binder.BindEntity(Integer(parameters, index++), e => Curve = e);
            ExtrusionLength = Double(parameters, index++);
            ExtrusionDirection = Vector(parameters, ref index);
            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return Curve;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(binder.GetEntityId(Curve));
            parameters.Add(ExtrusionLength);
            parameters.Add(ExtrusionDirection.X);
            parameters.Add(ExtrusionDirection.Y);
            parameters.Add(ExtrusionDirection.Z);
        }
    }
}
