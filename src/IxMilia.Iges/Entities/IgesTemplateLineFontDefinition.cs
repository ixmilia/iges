// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public enum IgesTemplateLineFontOrientation
    {
        AlignedToCurve = 0,
        AlignedToTangent = 1
    }

    public class IgesTemplateLineFontDefinition : IgesLineFontDefinitionBase
    {
        public IgesTemplateLineFontOrientation Orientation { get; set; }

        public IgesSubfigureDefinition Template { get; set; }

        public double CommonArcLength { get; set; }
        public double ScaleFactor { get; set; }

        public IgesTemplateLineFontDefinition()
            : this(new IgesSubfigureDefinition(), 0.0, 0.0)
        {
        }

        public IgesTemplateLineFontDefinition(IgesSubfigureDefinition template, double commonArcLength, double scaleFactor)
            : base()
        {
            this.FormNumber = 1;
            Template = template;
            CommonArcLength = commonArcLength;
            ScaleFactor = scaleFactor;
        }

        protected override int ReadParameters(List<string> parameters)
        {
            this.Orientation = (IgesTemplateLineFontOrientation)Integer(parameters, 0);
            SubEntityIndices.Add(Integer(parameters, 1));
            this.CommonArcLength = Double(parameters, 2);
            this.ScaleFactor = Double(parameters, 3);
            return 4;
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            Template = SubEntities[0] as IgesSubfigureDefinition;
        }

        internal override void OnBeforeWrite()
        {
            SubEntities.Add(Template);
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add((int)Orientation);
            parameters.Add(SubEntityIndices[0]);
            parameters.Add(CommonArcLength);
            parameters.Add(ScaleFactor);
        }
    }
}
