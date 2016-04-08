// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
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
                    binder.BindEntity(-fontCode, e => str.TextFontDefinition = e as IgesTextFontDefinition);
                    str.FontCode = -1;
                }
                else
                {
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

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            return Strings.Select(s => s.TextFontDefinition);
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
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
                    parameters.Add(-binder.GetEntityId(str.TextFontDefinition));
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
    }
}
