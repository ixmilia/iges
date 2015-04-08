// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using IxMilia.Iges.Directory;

namespace IxMilia.Iges.Entities
{
    public partial class IgesDirection : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.Direction; } }

        // properties
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        internal IgesDirection()
            : this(0.0, 0.0, 0.0)
        {
        }

        public IgesDirection(double x, double y, double z)
            : base()
        {
            this.LineCount = 1;
            this.SubordinateEntitySwitchType = IgesSubordinateEntitySwitchType.PhysicallyDependent;
            this.EntityUseFlag = IgesEntityUseFlag.Definition;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        protected override void ReadParameters(List<string> parameters)
        {
            this.X = Double(parameters, 0);
            this.Y = Double(parameters, 1);
            this.Z = Double(parameters, 2);
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(this.X);
            parameters.Add(this.Y);
            parameters.Add(this.Z);
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            Debug.Assert(directoryData.TransformationMatrixPointer == 0);

            Debug.Assert(SubordinateEntitySwitchType == IgesSubordinateEntitySwitchType.PhysicallyDependent);
            Debug.Assert(EntityUseFlag == IgesEntityUseFlag.Definition);
        }
    }
}
