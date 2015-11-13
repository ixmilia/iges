// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace IxMilia.Iges.Entities
{
    public class IgesTabulatedCylinder : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.TabulatedCylinder; } }

        public IgesEntity Directrix { get; set; }
        public IgesPoint GeneratrixTerminatePoint { get; set; }

        protected override int ReadParameters(List<string> parameters)
        {
            SubEntityIndices.Add(Integer(parameters, 0));
            var x = Double(parameters, 1);
            var y = Double(parameters, 2);
            var z = Double(parameters, 3);
            GeneratrixTerminatePoint = new IgesPoint(x, y, z);
            return 4;
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(SubEntityIndices[0]);
            parameters.Add(GeneratrixTerminatePoint?.X ?? 0.0);
            parameters.Add(GeneratrixTerminatePoint?.Y ?? 0.0);
            parameters.Add(GeneratrixTerminatePoint?.Z ?? 0.0);
        }

        internal override void OnBeforeWrite()
        {
            SubEntities.Add(Directrix);
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            Debug.Assert(FormNumber == 0);
            Directrix = SubEntities[0];
        }
    }
}
