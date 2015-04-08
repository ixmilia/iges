// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using IxMilia.Iges.Directory;

namespace IxMilia.Iges.Entities
{
    public partial class IgesSphere : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.Sphere; } }

        // properties
        public double Radius { get; set; }
        public IgesPoint Center { get; set; }

        public IgesSphere()
            : this(0.0, IgesPoint.Origin)
        {
        }

        public IgesSphere(double radius, IgesPoint center)
            : base()
        {
            this.LineCount = 1;
            this.EntityUseFlag = IgesEntityUseFlag.Geometry;
            this.Radius = radius;
            this.Center = center;
        }

        protected override void ReadParameters(List<string> parameters)
        {
            this.Radius = Double(parameters, 0);
            this.Center.X = Double(parameters, 1);
            this.Center.Y = Double(parameters, 2);
            this.Center.Z = Double(parameters, 3);
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(this.Radius);
            if (Center != IgesPoint.Origin)
            {
                parameters.Add(this.Center.X);
                parameters.Add(this.Center.Y);
                parameters.Add(this.Center.Z);
            }
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            Debug.Assert(EntityUseFlag == IgesEntityUseFlag.Geometry);
        }
    }
}
