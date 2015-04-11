// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace IxMilia.Iges.Entities
{
    public class IgesPatternLineFontDefinition : IgesLineFontDefinitionBase
    {
        public List<double> SegmentLengths { get; private set; }
        public int DisplayMask { get; set; }

        public IgesPatternLineFontDefinition()
            : base()
        {
            this.FormNumber = 2;
            SegmentLengths = new List<double>();
        }

        protected override int ReadParameters(List<string> parameters)
        {
            var segmentCount = Integer(parameters, 0);
            for (int i = 0; i < segmentCount; i++)
            {
                SegmentLengths.Add(Double(parameters, i + 1));
            }

            DisplayMask = int.Parse(StringOrDefault(parameters, segmentCount + 1, "0"), NumberStyles.HexNumber);
            return segmentCount + 2;
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(SegmentLengths.Count);
            parameters.AddRange(SegmentLengths.Cast<object>());
            parameters.Add(DisplayMask.ToString("X"));
        }
    }
}
