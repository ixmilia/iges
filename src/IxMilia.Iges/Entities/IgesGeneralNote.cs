// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace IxMilia.Iges.Entities
{
    public class IgesTextString
    {
        public double BoxWidth { get; set; }
        public double BoxHeight { get; set; }
        public int FontCode { get; set; }
        public IgesTextFontDefinition TextFontDefinition { get; set; }
        public double SlantAngle { get; set; }
        public double RotationAngle { get; set; }
        public IgesTextMirroringAxis MirroringAxis { get; set; }
        public IgesTextRotationType RotationType { get; set; }
        public IgesPoint Location { get; set; }
        public string Value { get; set; }

        public IgesTextString()
        {
            Location = IgesPoint.Origin;
        }
    }

    public enum IgesGeneralNoteType
    {
        Simple = 0,
        DualStack = 1,
        ImbeddedFontChange = 2,
        Superscript = 3,
        Subscript = 4,
        SuperscriptSubscript = 5,
        MultipleStackLeftJustified = 6,
        MultipleStackCenterJustified = 7,
        MultipleStackRightJustified = 8,
        SimpleFraction = 100,
        DualStackFraction = 101,
        ImbeddedFontChangeDoubleFraction = 102,
        SuperscriptSubscriptFraction = 105
    }

    public class IgesGeneralNote : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.GeneralNote; } }

        public List<IgesTextString> Strings { get; private set; }

        public IgesGeneralNoteType NoteType
        {
            get { return (IgesGeneralNoteType)FormNumber; }
            set { FormNumber = (int)value; }
        }

        public IgesGeneralNote()
            : base()
        {
            EntityUseFlag = IgesEntityUseFlag.Annotation;
            Strings = new List<IgesTextString>();
        }

        protected override int ReadParameters(List<string> parameters)
        {
            var index = 0;
            var stringCount = Integer(parameters, index++);
            for (int i = 0; i < stringCount; i++)
            {
                var str = new IgesTextString();
                var charCount = Integer(parameters, index++);
                str.BoxWidth = Double(parameters, index++);
                str.BoxHeight = Double(parameters, index++);

                var fontCode = IntegerOrDefault(parameters, index++, 1);
                if (fontCode < 0)
                {
                    SubEntityIndices.Add(-fontCode);
                    str.FontCode = -1;
                }
                else
                {
                    SubEntityIndices.Add(0);
                    str.FontCode = fontCode;
                }

                str.SlantAngle = Double(parameters, index++);
                str.RotationAngle = Double(parameters, index++);
                str.MirroringAxis = (IgesTextMirroringAxis)Integer(parameters, index++);
                str.RotationType = (IgesTextRotationType)Integer(parameters, index++);
                str.Location.X = Double(parameters, index++);
                str.Location.Y = Double(parameters, index++);
                str.Location.Z = Double(parameters, index++);
                str.Value = String(parameters, index++);
                Debug.Assert(str.Value.Length == charCount);
                Strings.Add(str);
            }

            return index;
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(Strings.Count);
            for (int i = 0; i < Strings.Count; i++)
            {
                var str = Strings[i];
                parameters.Add(str.Value?.Length ?? 0);
                parameters.Add(str.BoxWidth);
                parameters.Add(str.BoxHeight);

                if (str.TextFontDefinition != null)
                {
                    parameters.Add(-SubEntityIndices[i]);
                }
                else
                {
                    parameters.Add(str.FontCode);
                }

                parameters.Add(str.SlantAngle);
                parameters.Add(str.RotationAngle);
                parameters.Add((int)str.MirroringAxis);
                parameters.Add((int)str.RotationType);
                parameters.Add(str.Location.X);
                parameters.Add(str.Location.Y);
                parameters.Add(str.Location.Z);
                parameters.Add(str.Value);
            }
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            Debug.Assert(EntityUseFlag == IgesEntityUseFlag.Annotation);
            Debug.Assert(
                (FormNumber >= 0 && FormNumber <= 8) ||
                (FormNumber >= 100 && FormNumber <= 102) ||
                FormNumber == 105,
                "form number must be [0,8], [100,102], [105]");
            for (int i = 0; i < Strings.Count; i++)
            {
                if (Strings[i].FontCode == -1)
                {
                    Strings[i].TextFontDefinition = SubEntities[i] as IgesTextFontDefinition;
                }
            }
        }

        internal override void OnBeforeWrite()
        {
            foreach (var str in Strings)
            {
                SubEntities.Add(str.TextFontDefinition);
            }
        }
    }
}
