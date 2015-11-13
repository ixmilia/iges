// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace IxMilia.Iges.Entities
{
    public class IgesSurfaceOfRevolution : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.SurfaceOfRevolution; } }

        public IgesLine AxisOfRevolution { get; set; }
        public IgesEntity Generatrix { get; set; }
        public double StartAngle { get; set; }
        public double EndAngle { get; set; }

        protected override int ReadParameters(List<string> parameters)
        {
            SubEntityIndices.Add(Integer(parameters, 0));
            SubEntityIndices.Add(Integer(parameters, 1));
            StartAngle = Double(parameters, 2);
            EndAngle = Double(parameters, 3);
            return 4;
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(SubEntityIndices[0]);
            parameters.Add(SubEntityIndices[1]);
            parameters.Add(StartAngle);
            parameters.Add(EndAngle);
        }

        internal override void OnBeforeWrite()
        {
            SubEntities.Add(AxisOfRevolution);
            SubEntities.Add(Generatrix);
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            Debug.Assert(FormNumber == 0);
            AxisOfRevolution = SubEntities[0] as IgesLine;
            Generatrix = SubEntities[1];
        }
    }
}
