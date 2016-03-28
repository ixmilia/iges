// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesRightCircularCylinder : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.RightCircularCylinder; } }

        public double Height { get; set; }
        public double Radius { get; set; }
        public IgesPoint FirstFaceCenter { get; set; } = IgesPoint.Origin;
        public IgesVector AxisDirection { get; set; } = IgesVector.ZAxis;

        protected override int ReadParameters(List<string> parameters)
        {
            Height = Double(parameters, 0);
            Radius = Double(parameters, 1);
            FirstFaceCenter.X = Double(parameters, 2);
            FirstFaceCenter.Y = Double(parameters, 3);
            FirstFaceCenter.Z = Double(parameters, 4);
            AxisDirection.X = Double(parameters, 5);
            AxisDirection.Y = Double(parameters, 6);
            AxisDirection.Z = Double(parameters, 7);
            return 8;
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(Height);
            parameters.Add(Radius);
            parameters.Add(FirstFaceCenter?.X ?? 0.0);
            parameters.Add(FirstFaceCenter?.Y ?? 0.0);
            parameters.Add(FirstFaceCenter?.Z ?? 0.0);
            parameters.Add(AxisDirection?.X ?? 0.0);
            parameters.Add(AxisDirection?.Y ?? 0.0);
            parameters.Add(AxisDirection?.Z ?? 1.0);
        }
    }
}
