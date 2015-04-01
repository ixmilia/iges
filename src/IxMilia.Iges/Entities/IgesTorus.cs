// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

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

        public override int LineCount
        {
            get
            {
                return 1;
            }
        }

        public override string StatusNumber
        {
            get
            {
                return "00000000";
            }
        }

        public IgesTorus()
            : this(0.0, 0.0, IgesPoint.Origin, IgesVector.ZAxis)
        {
        }

        public IgesTorus(double ringRadius, double discRadius, IgesPoint center, IgesVector normal)
            : base()
        {
            this.RingRadius = ringRadius;
            this.DiscRadius = discRadius;
            this.Center = center;
            this.Normal = normal;
        }

        protected override void ReadParameters(List<string> parameters)
        {
            int index = 0;
            this.RingRadius = Double(parameters[index++]);
            this.DiscRadius = Double(parameters[index++]);
            this.Center.X = Double(ReadParameterOrDefault(parameters, index++, "0.0"));
            this.Center.Y = Double(ReadParameterOrDefault(parameters, index++, "0.0"));
            this.Center.Z = Double(ReadParameterOrDefault(parameters, index++, "0.0"));
            this.Normal.X = Double(ReadParameterOrDefault(parameters, index++, "0.0"));
            this.Normal.Y = Double(ReadParameterOrDefault(parameters, index++, "0.0"));
            this.Normal.Z = Double(ReadParameterOrDefault(parameters, index++, "1.0"));
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
    }
}
