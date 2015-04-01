// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using IxMilia.Iges.Directory;

namespace IxMilia.Iges.Entities
{
    public partial class IgesSphere : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.Sphere; } }

        // properties
        public double Radius { get; set; }
        public IgesPoint Center { get; set; }

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
                return "0000";
            }
        }

        public IgesSphere()
            : this(0.0, IgesPoint.Origin)
        {
        }

        public IgesSphere(double radius, IgesPoint center)
            : base()
        {
            this.Radius = radius;
            this.Center = center;
        }

        protected override void ReadParameters(List<string> parameters)
        {
            int index = 0;
            this.Radius = Double(parameters[index++]);
            this.Center.X = Double(ReadParameterOrDefault(parameters, index++, "0.0"));
            this.Center.Y = Double(ReadParameterOrDefault(parameters, index++, "0.0"));
            this.Center.Z = Double(ReadParameterOrDefault(parameters, index++, "0.0"));
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
            // status number should match "00**"
            Debug.Assert(Regex.IsMatch(directoryData.StatusNumber, "^.*00..$"));
        }
    }
}
