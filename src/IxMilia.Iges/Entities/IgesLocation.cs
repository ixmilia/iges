// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace IxMilia.Iges.Entities
{
    public partial class IgesLocation : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.Point; } }

        // properties
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public override int LineCount
        {
            get
            {
                return 1;
            }
        }

        public IgesLocation()
            : this(0.0, 0.0, 0.0)
        {
        }

        public IgesLocation(double x, double y, double z)
            : base()
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        protected override void ReadParameters(List<string> parameters)
        {
            int index = 0;
            this.X = Double(parameters[index++]);
            this.Y = Double(parameters[index++]);
            this.Z = Double(parameters[index++]);
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(this.X);
            parameters.Add(this.Y);
            parameters.Add(this.Z);
        }
    }
}
