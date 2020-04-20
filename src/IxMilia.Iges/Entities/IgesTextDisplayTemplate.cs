using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesTextDisplayTemplate : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.TextDisplayTemplate; } }

        public double CharacterBoxWidth { get; set; }
        public double CharacterBoxHeight { get; set; }
        public int FontCode { get; set; }
        public IgesTextFontDefinition TextFontDefinition { get; set; }
        public double SlantAngle { get; set; }
        public double RotationAngle { get; set; }
        public IgesTextMirroringAxis MirroringAxis { get; set; }
        public IgesTextRotationType RotationType { get; set; }
        public IgesVector LocationOrOffset { get; set; }

        public bool IsAbsoluteDisplayTemplate
        {
            get { return FormNumber == 0; }
            set { FormNumber = value ? 0 : 1; }
        }

        public bool IsIncrementalDisplayTemplate
        {
            get { return !IsAbsoluteDisplayTemplate; }
            set { IsAbsoluteDisplayTemplate = !value; }
        }

        public IgesTextDisplayTemplate()
            : base()
        {
            SubordinateEntitySwitchType = IgesSubordinateEntitySwitchType.Independent;
            EntityUseFlag = IgesEntityUseFlag.Definition;
            Hierarchy = IgesHierarchy.GlobalTopDown;
            FontCode = 1;
            LocationOrOffset = IgesVector.Zero;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            int index = 0;
            this.CharacterBoxWidth = Double(parameters, index++);
            this.CharacterBoxHeight = Double(parameters, index++);
            var fontCode = Integer(parameters, index++);
            if (fontCode < 0)
            {
                binder.BindEntity(-fontCode, e => TextFontDefinition = e as IgesTextFontDefinition);
                this.FontCode = 0;
            }
            else
            {
                this.FontCode = fontCode;
            }

            this.SlantAngle = Double(parameters, index++);
            this.RotationAngle = Double(parameters, index++);
            this.MirroringAxis = (IgesTextMirroringAxis)Integer(parameters, index++);
            this.RotationType = (IgesTextRotationType)Integer(parameters, index++);
            this.LocationOrOffset = Vector(parameters, ref index);
            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return TextFontDefinition;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(CharacterBoxWidth);
            parameters.Add(CharacterBoxHeight);
            if (TextFontDefinition == null)
            {
                parameters.Add(FontCode);
            }
            else
            {
                parameters.Add(-binder.GetEntityId(TextFontDefinition));
            }

            parameters.Add(SlantAngle);
            parameters.Add(RotationAngle);
            parameters.Add((int)MirroringAxis);
            parameters.Add((int)RotationType);
            parameters.Add(LocationOrOffset.X);
            parameters.Add(LocationOrOffset.Y);
            parameters.Add(LocationOrOffset.Z);
        }
    }
}
