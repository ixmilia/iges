// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesCircularArc : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.CircularArc; } }

        // properties
        public double PlaneDisplacement { get; set; }
        public IgesPoint Center { get; set; }
        public IgesPoint StartPoint { get; set; }
        public IgesPoint EndPoint { get; set; }

        // custom properties
        public IgesPoint ProperCenter
        {
            get
            {
                return new IgesPoint(Center.X, Center.Y, PlaneDisplacement);
            }
        }

        public IgesPoint ProperStartPoint
        {
            get
            {
                return new IgesPoint(StartPoint.X, StartPoint.Y, PlaneDisplacement);
            }
        }

        public IgesPoint ProperEndPoint
        {
            get
            {
                return new IgesPoint(EndPoint.X, EndPoint.Y, PlaneDisplacement);
            }
        }

        public IgesCircularArc()
            : this(IgesPoint.Origin, IgesPoint.Origin, IgesPoint.Origin)
        {
        }

        public IgesCircularArc(IgesPoint center, IgesPoint start, IgesPoint end)
            : base()
        {
            if (center.Z != start.Z || center.Z != end.Z)
            {
                throw new ArgumentException("All z values must be equal");
            }

            this.PlaneDisplacement = center.Z;
            this.Center = new IgesPoint(center.X, center.Y, 0.0);
            this.StartPoint = new IgesPoint(start.X, start.Y, 0.0);
            this.EndPoint = new IgesPoint(end.X, end.Y, 0.0);
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            this.PlaneDisplacement = Double(parameters, 0);
            this.Center.X = Double(parameters, 1);
            this.Center.Y = Double(parameters, 2);
            this.StartPoint.X = Double(parameters, 3);
            this.StartPoint.Y = Double(parameters, 4);
            this.EndPoint.X = Double(parameters, 5);
            this.EndPoint.Y = Double(parameters, 6);
            return 7;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(this.PlaneDisplacement);
            parameters.Add(this.Center.X);
            parameters.Add(this.Center.Y);
            parameters.Add(this.StartPoint.X);
            parameters.Add(this.StartPoint.Y);
            parameters.Add(this.EndPoint.X);
            parameters.Add(this.EndPoint.Y);
        }
    }
}
