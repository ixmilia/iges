using System.Collections.Generic;
using System.Linq;

namespace IxMilia.Iges.Entities
{
    public enum IgesTextJustification
    {
        None = 0,
        Right = 1,
        Center = 2,
        Left = 3
    }

    public enum IgesCharacterDisplay
    {
        Fixed = 0,
        Variable = 1
    }

    public enum IgesFontStyle
    {
        StandardBlock = 1,
        LeRoy = 2,
        Futura = 3,
        Comp80 = 6,
        NewsGothic = 12,
        LightlineGothic = 13,
        SimplexRoman = 14,
        CenturySchoolbook = 17,
        Helvetica = 18,
        OCR_ISO_1073 = 19
    }

    public class IgesNewTextString : IgesTextString
    {
        public IgesCharacterDisplay CharacterDisplay { get; set; }
        public double CharacterWidth { get; set; }
        public double CharacterHeight { get; set; }
        public double InterCharacterSpacing { get; set; }
        public double InterLineSpacing { get; set; }
        public IgesFontStyle FontStyle { get; set; }
        public double CharacterAngle { get; set; }
        public string ControlCode { get; set; }

        public IgesNewTextString()
            : base()
        {
            FontStyle = IgesFontStyle.StandardBlock;
        }

        internal static new IgesNewTextString ReadParameters(List<string> parameters, IgesReaderBinder binder, ref int index)
        {
            var str = new IgesNewTextString();
            str.CharacterDisplay = (IgesCharacterDisplay)IgesParameterReader.Integer(parameters, index++);
            str.CharacterWidth = IgesParameterReader.Double(parameters, index++);
            str.CharacterHeight = IgesParameterReader.Double(parameters, index++);
            str.InterCharacterSpacing = IgesParameterReader.Double(parameters, index++);
            str.InterLineSpacing = IgesParameterReader.Double(parameters, index++);
            str.FontStyle = (IgesFontStyle)IgesParameterReader.Integer(parameters, index++);
            str.CharacterAngle = IgesParameterReader.Double(parameters, index++);
            str.ControlCode = IgesParameterReader.String(parameters, index++);
            str.PopulateFromParameters(parameters, binder, ref index);
            return str;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add((int)CharacterDisplay);
            parameters.Add(CharacterWidth);
            parameters.Add(CharacterHeight);
            parameters.Add(InterCharacterSpacing);
            parameters.Add(InterLineSpacing);
            parameters.Add((int)FontStyle);
            parameters.Add(CharacterAngle);
            parameters.Add(ControlCode);
            base.WriteParameters(parameters, binder);
        }
    }

    public class IgesNewGeneralNote : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.NewGeneralNote; } }

        public double TextContainmentAreaWidth { get; set; }
        public double TextContainmentAreaHeight { get; set; }
        public IgesTextJustification TextJustification { get; set; }
        public IgesPoint TextContainmentAreaLocation { get; set; }

        /// <summary>
        /// The rotation angle of the text containment area in radians.
        /// </summary>
        public double TextContainmentAreaRotation { get; set; }

        public IgesPoint FirstBaseLineLocation { get; set; }
        public double NormalInterLineSpacing { get; set; }

        public IList<IgesNewTextString> Strings { get; private set; }

        public IgesNewGeneralNote()
        {
            EntityUseFlag = IgesEntityUseFlag.Annotation;
            TextContainmentAreaLocation = IgesPoint.Origin;
            FirstBaseLineLocation = IgesPoint.Origin;
            Strings = new List<IgesNewTextString>();
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var index = 0;
            TextContainmentAreaWidth = Double(parameters, index++);
            TextContainmentAreaHeight = Double(parameters, index++);
            TextJustification = (IgesTextJustification)Integer(parameters, index++);
            TextContainmentAreaLocation = Point3(parameters, ref index);
            TextContainmentAreaRotation = Double(parameters, index++);
            FirstBaseLineLocation = Point3(parameters, ref index);
            NormalInterLineSpacing = Double(parameters, index++);
            var stringCount = Integer(parameters, index++);
            for (int i = 0; i < stringCount; i++)
            {
                Strings.Add(IgesNewTextString.ReadParameters(parameters, binder, ref index));
            }

            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            return Strings.Select(s => s?.TextFontDefinition);
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(TextContainmentAreaWidth);
            parameters.Add(TextContainmentAreaHeight);
            parameters.Add((int)TextJustification);
            parameters.Add(TextContainmentAreaLocation.X);
            parameters.Add(TextContainmentAreaLocation.Y);
            parameters.Add(TextContainmentAreaLocation.Z);
            parameters.Add(TextContainmentAreaRotation);
            parameters.Add(FirstBaseLineLocation.X);
            parameters.Add(FirstBaseLineLocation.Y);
            parameters.Add(FirstBaseLineLocation.Z);
            parameters.Add(NormalInterLineSpacing);
            parameters.Add(Strings.Count);
            foreach (var str in Strings)
            {
                str.WriteParameters(parameters, binder);
            }
        }
    }
}
