// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
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

        public override string StatusNumber
        {
            get
            {
                return "00010200";
            }
        }

        public override int LineCount
        {
            get
            {
                return 1; ;
            }
        }

        internal IgesDirection()
            : this(0.0, 0.0, 0.0)
        {
        }

        public IgesDirection(double x, double y, double z)
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

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            Debug.Assert(directoryData.TransformationMatrixPointer == 0);

            // status number should be of the form '**0102**'
            Debug.Assert(Regex.IsMatch(directoryData.StatusNumber, "^..0102..$"));
        }
    }
}
