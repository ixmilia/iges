// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public enum IgesBounding
    {
        BoundOnBothSides = 0,
        BoundOnStart = 1,
        Unbound = 2
    }

    public class IgesLine : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.Line; } }

        // properties
        public IgesPoint P1 { get; set; }
        public IgesPoint P2 { get; set; }

        // custom properties
        public override int LineCount
        {
            get
            {
                return 1;
            }
        }

        public IgesBounding Bounding
        {
            get
            {
                return (IgesBounding)FormNumber;
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

        protected override void ReadParameters(List<string> parameters)
        {
            int index = 0;
            this.P1.X = Double(parameters[index++]);
            this.P1.Y = Double(parameters[index++]);
            this.P1.Z = Double(parameters[index++]);
            this.P2.X = Double(parameters[index++]);
            this.P2.Y = Double(parameters[index++]);
            this.P2.Z = Double(parameters[index++]);
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
