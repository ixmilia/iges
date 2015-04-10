// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace IxMilia.Iges.Entities
{
    public partial class IgesTorus : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.Torus; } }

        // properties
        public double RingRadius { get; set; }
        public double DiscRadius { get; set; }
        public IgesPoint Center { get; set; }
        public IgesVector Normal { get; set; }

        public IgesTorus()
            : this(0.0, 0.0, IgesPoint.Origin, IgesVector.ZAxis)
        {
        }

        public IgesTorus(double ringRadius, double discRadius, IgesPoint center, IgesVector normal)
            : base()
        {
            this.LineCount = 1;
            this.BlankStatus = IgesBlankStatus.Visible;
            this.SubordinateEntitySwitchType = IgesSubordinateEntitySwitchType.Independent;
            this.EntityUseFlag = IgesEntityUseFlag.Geometry;
            this.Hierarchy = IgesHierarchy.GlobalTopDown;
            this.RingRadius = ringRadius;
            this.DiscRadius = discRadius;
            this.Center = center;
            this.Normal = normal;
        }

        protected override int ReadParameters(List<string> parameters)
        {
            this.RingRadius = Double(parameters, 0);
            this.DiscRadius = Double(parameters, 1);
            this.Center.X = Double(parameters, 2);
            this.Center.Y = Double(parameters, 3);
            this.Center.Z = Double(parameters, 4);
            this.Normal.X = Double(parameters, 5);
            this.Normal.Y = Double(parameters, 6);
            this.Normal.Z = DoubleOrDefault(parameters, 7, 1.0);
            return 8;
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(this.RingRadius);
            parameters.Add(this.DiscRadius);

            if (Center != IgesPoint.Origin || Normal != IgesVector.ZAxis)
            {
                parameters.Add(this.Center.X);
                parameters.Add(this.Center.Y);
                parameters.Add(this.Center.Z);
                if (Normal != IgesVector.ZAxis)
                {
                    parameters.Add(this.Normal.X);
                    parameters.Add(this.Normal.Y);
                    parameters.Add(this.Normal.Z);
                }
            }
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            Debug.Assert(BlankStatus == IgesBlankStatus.Visible);
            Debug.Assert(SubordinateEntitySwitchType == IgesSubordinateEntitySwitchType.Independent);
            Debug.Assert(EntityUseFlag == IgesEntityUseFlag.Geometry);
            Debug.Assert(Hierarchy == IgesHierarchy.GlobalTopDown);
        }
    }
}
