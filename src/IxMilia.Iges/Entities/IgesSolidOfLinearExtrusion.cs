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

        protected override int ReadParameters(List<string> parameters)
        {
            SubEntityIndices.Add(Integer(parameters, 0));
            ExtrusionLength = Double(parameters, 1);
            ExtrusionDirection.X = Double(parameters, 2);
            ExtrusionDirection.Y = Double(parameters, 3);
            ExtrusionDirection.Z = Double(parameters, 4);
            return 5;
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            Curve = SubEntities[0];
        }

        internal override void OnBeforeWrite()
        {
            SubEntities.Add(Curve);
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(SubEntityIndices[0]);
            parameters.Add(ExtrusionLength);
            parameters.Add(ExtrusionDirection?.X ?? 0.0);
            parameters.Add(ExtrusionDirection?.Y ?? 0.0);
            parameters.Add(ExtrusionDirection?.Z ?? 1.0);
        }
    }
}
