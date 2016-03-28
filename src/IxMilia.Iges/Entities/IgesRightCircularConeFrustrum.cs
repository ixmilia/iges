// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesRightCircularConeFrustrum : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.RightCircularConeFrustrum; } }

        public double Height { get; set; }
        public double LargeFaceRadius { get; set; }
        public double SmallFaceRadius { get; set; }
        public IgesPoint LargeFaceCenter { get; set; } = IgesPoint.Origin;
        public IgesVector AxisDirection { get; set; } = IgesVector.ZAxis;

        protected override int ReadParameters(List<string> parameters)
        {
            Height = Double(parameters, 0);
            LargeFaceRadius = Double(parameters, 1);
            SmallFaceRadius = Double(parameters, 2);
            LargeFaceCenter.X = Double(parameters, 3);
            LargeFaceCenter.Y = Double(parameters, 4);
            LargeFaceCenter.Z = Double(parameters, 5);
            AxisDirection.X = Double(parameters, 6);
            AxisDirection.Y = Double(parameters, 7);
            AxisDirection.Z = Double(parameters, 8);
            return 9;
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(Height);
            parameters.Add(LargeFaceRadius);
            parameters.Add(SmallFaceRadius);
            parameters.Add(LargeFaceCenter?.X ?? 0.0);
            parameters.Add(LargeFaceCenter?.Y ?? 0.0);
            parameters.Add(LargeFaceCenter?.Z ?? 0.0);
            parameters.Add(AxisDirection?.X ?? 0.0);
            parameters.Add(AxisDirection?.Y ?? 0.0);
            parameters.Add(AxisDirection?.Z ?? 1.0);
        }
    }
}
