// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesEllipsoid : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.Ellipsoid; } }

        public double XAxisLength { get; set; }
        public double YAxisLength { get; set; }
        public double ZAxisLength { get; set; }
        public IgesPoint Center { get; set; }
        public IgesVector XAxis { get; set; }
        public IgesVector ZAxis { get; set; }

        public IgesEllipsoid()
        {
            EntityUseFlag = IgesEntityUseFlag.Geometry;
            Center = IgesPoint.Origin;
            XAxis = IgesVector.XAxis;
            ZAxis = IgesVector.ZAxis;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            XAxisLength = Double(parameters, 0);
            YAxisLength = Double(parameters, 1);
            ZAxisLength = Double(parameters, 2);
            Center.X = Double(parameters, 3);
            Center.Y = Double(parameters, 4);
            Center.Z = Double(parameters, 5);
            XAxis.X = Double(parameters, 6);
            XAxis.Y = Double(parameters, 7);
            XAxis.Z = Double(parameters, 8);
            ZAxis.X = Double(parameters, 9);
            ZAxis.Y = Double(parameters, 10);
            ZAxis.Z = Double(parameters, 11);
            return 12;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(XAxisLength);
            parameters.Add(YAxisLength);
            parameters.Add(ZAxisLength);
            parameters.Add(Center?.X ?? 0.0);
            parameters.Add(Center?.Y ?? 0.0);
            parameters.Add(Center?.Z ?? 0.0);
            parameters.Add(XAxis?.X ?? 1.0);
            parameters.Add(XAxis?.Y ?? 0.0);
            parameters.Add(XAxis?.Z ?? 0.0);
            parameters.Add(ZAxis?.X ?? 0.0);
            parameters.Add(ZAxis?.Y ?? 0.0);
            parameters.Add(ZAxis?.Z ?? 1.0);
        }
    }
}
