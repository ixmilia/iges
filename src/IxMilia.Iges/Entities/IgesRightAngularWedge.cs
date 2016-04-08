// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesRightAngularWedge : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.RightAngularWedge; } }

        public double XAxisSize { get; set; }
        public double YAxisSize { get; set; }
        public double ZAxisSize { get; set; }
        public double XAxisSizeAtYDistance { get; set; }
        public IgesPoint Corner { get; set; } = IgesPoint.Origin;
        public IgesVector XAxis { get; set; } = IgesVector.XAxis;
        public IgesVector ZAxis { get; set; } = IgesVector.ZAxis;

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            XAxisSize = Double(parameters, 0);
            YAxisSize = Double(parameters, 1);
            ZAxisSize = Double(parameters, 2);
            XAxisSizeAtYDistance = Double(parameters, 3);
            Corner.X = Double(parameters, 4);
            Corner.Y = Double(parameters, 5);
            Corner.Z = Double(parameters, 6);
            XAxis.X = Double(parameters, 7);
            XAxis.Y = Double(parameters, 8);
            XAxis.Z = Double(parameters, 9);
            ZAxis.X = Double(parameters, 10);
            ZAxis.Y = Double(parameters, 11);
            ZAxis.Z = Double(parameters, 12);
            return 13;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(XAxisSize);
            parameters.Add(YAxisSize);
            parameters.Add(ZAxisSize);
            parameters.Add(XAxisSizeAtYDistance);
            parameters.Add(Corner?.X ?? 0.0);
            parameters.Add(Corner?.Y ?? 0.0);
            parameters.Add(Corner?.Z ?? 0.0);
            parameters.Add(XAxis?.X ?? 1.0);
            parameters.Add(XAxis?.Y ?? 0.0);
            parameters.Add(XAxis?.Z ?? 0.0);
            parameters.Add(ZAxis?.X ?? 0.0);
            parameters.Add(ZAxis?.Y ?? 0.0);
            parameters.Add(ZAxis?.Z ?? 1.0);
        }
    }
}
