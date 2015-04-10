// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public enum IgesLineBounding
    {
        BoundOnBothSides = 0,
        BoundOnStart = 1,
        Unbounded = 2
    }

    public class IgesLine : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.Line; } }

        // properties
        public IgesPoint P1 { get; set; }
        public IgesPoint P2 { get; set; }

        // custom properties
        public IgesLineBounding Bounding
        {
            get
            {
                return (IgesLineBounding)FormNumber;
            }
            set
            {
                FormNumber = (int)value;
            }
        }

        public IgesLine()
            : this(IgesPoint.Origin, IgesPoint.Origin)
        {
        }

        public IgesLine(IgesPoint p1, IgesPoint p2)
            : base()
        {
            this.P1 = p1;
            this.P2 = p2;
        }

        protected override int ReadParameters(List<string> parameters)
        {
            this.P1.X = Double(parameters, 0);
            this.P1.Y = Double(parameters, 1);
            this.P1.Z = Double(parameters, 2);
            this.P2.X = Double(parameters, 3);
            this.P2.Y = Double(parameters, 4);
            this.P2.Z = Double(parameters, 5);
            return 6;
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(this.P1.X);
            parameters.Add(this.P1.Y);
            parameters.Add(this.P1.Z);
            parameters.Add(this.P2.X);
            parameters.Add(this.P2.Y);
            parameters.Add(this.P2.Z);
        }
    }
}
