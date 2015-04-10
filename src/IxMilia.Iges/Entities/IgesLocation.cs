// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public partial class IgesLocation : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.Point; } }

        // properties
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public IgesLocation()
            : this(0.0, 0.0, 0.0)
        {
        }

        public IgesLocation(double x, double y, double z)
            : base()
        {
            this.LineCount = 1;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        protected override int ReadParameters(List<string> parameters)
        {
            this.X = Double(parameters, 0);
            this.Y = Double(parameters, 1);
            this.Z = Double(parameters, 2);
            return 3;
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(this.X);
            parameters.Add(this.Y);
            parameters.Add(this.Z);
        }
    }
}
