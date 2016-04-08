// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesBlock : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.Block; } }

        public double XLength { get; set; }
        public double YLength { get; set; }
        public double ZLength { get; set; }
        public IgesPoint Corner { get; set; } = IgesPoint.Origin;
        public IgesVector XAxis { get; set; } = IgesVector.XAxis;
        public IgesVector ZAxis { get; set; } = IgesVector.ZAxis;

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            int index = 0;
            XLength = Double(parameters, index++);
            YLength = Double(parameters, index++);
            ZLength = Double(parameters, index++);
            Corner.X = Double(parameters, index++);
            Corner.Y = Double(parameters, index++);
            Corner.Z = Double(parameters, index++);
            XAxis.X = DoubleOrDefault(parameters, index++, 1.0);
            XAxis.Y = Double(parameters, index++);
            XAxis.Z = Double(parameters, index++);
            ZAxis.X = Double(parameters, index++);
            ZAxis.Y = Double(parameters, index++);
            ZAxis.Z = DoubleOrDefault(parameters, index++, 1.0);
            return index;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(XLength);
            parameters.Add(YLength);
            parameters.Add(ZLength);
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
