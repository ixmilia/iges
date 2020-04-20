using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesGridPoint
    {
        public int X { get; set; }
        public int Y { get; set; }

        public IgesGridPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class IgesTextFontDefinitionCharacterMovement
    {
        public bool IsUp { get; set; }
        public IgesGridPoint Location { get; set; }

        public IgesTextFontDefinitionCharacterMovement()
        {
            Location = new IgesGridPoint(0, 0);
        }
    }

    public class IgesTextFontDefinitionCharacter
    {
        public int ASCIICode { get; set; }
        public IgesGridPoint CharacterOrigin { get; set; }
        public List<IgesTextFontDefinitionCharacterMovement> CharacterMovements { get; private set; }

        public IgesTextFontDefinitionCharacter()
        {
            CharacterOrigin = new IgesGridPoint(0, 0);
            CharacterMovements = new List<IgesTextFontDefinitionCharacterMovement>();
        }
    }

    public class IgesTextFontDefinition : IgesEntity
    {
        public override IgesEntityType EntityType {  get { return IgesEntityType.TextFontDefinition; } }

        // properties
        public int FontCode { get; set; }
        public string Name { get; set; }
        public int Scale { get; set; }
        public List<IgesTextFontDefinitionCharacter> Characters { get; private set; }
        public int SupercedesCode { get; set; }
        public IgesTextFontDefinition SupercedesFont { get; set; }

        public IgesTextFontDefinition()
            : base()
        {
            SubordinateEntitySwitchType = IgesSubordinateEntitySwitchType.Independent;
            EntityUseFlag = IgesEntityUseFlag.Definition;
            FormNumber = 0;
            Characters = new List<IgesTextFontDefinitionCharacter>();
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            this.FontCode = Integer(parameters, 0);
            this.Name = String(parameters, 1);
            var supercedes = Integer(parameters, 2);
            if (supercedes < 0)
            {
                binder.BindEntity(-supercedes, e => SupercedesFont = e as IgesTextFontDefinition);
            }
            else
            {
                SupercedesCode = supercedes;
            }

            this.Scale = Integer(parameters, 3);
            var count = Integer(parameters, 4);
            var index = 5;
            for (int i = 0; i < count; i++)
            {
                var character = new IgesTextFontDefinitionCharacter();
                character.ASCIICode = Integer(parameters, index++);
                character.CharacterOrigin.X = Integer(parameters, index++);
                character.CharacterOrigin.Y = Integer(parameters, index++);
                var penMovements = Integer(parameters, index++);
                for (int j = 0; j < penMovements; j++)
                {
                    var movement = new IgesTextFontDefinitionCharacterMovement();
                    movement.IsUp = Boolean(parameters, index++);
                    movement.Location.X = Integer(parameters, index++);
                    movement.Location.Y = Integer(parameters, index++);
                    character.CharacterMovements.Add(movement);
                }

                Characters.Add(character);
            }

            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return SupercedesFont;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(FontCode);
            parameters.Add(Name);
            if (SupercedesFont != null)
            {
                parameters.Add(-binder.GetEntityId(SupercedesFont));
            }
            else if (SupercedesCode == 0)
            {
                parameters.Add(null);
            }
            else
            {
                parameters.Add(SupercedesCode);
            }

            parameters.Add(Scale);
            parameters.Add(Characters.Count);
            foreach (var character in Characters)
            {
                parameters.Add(character.ASCIICode);
                parameters.Add(character.CharacterOrigin.X);
                parameters.Add(character.CharacterOrigin.Y);
                parameters.Add(character.CharacterMovements.Count);
                foreach (var movement in character.CharacterMovements)
                {
                    parameters.Add(movement.IsUp);
                    parameters.Add(movement.Location.X);
                    parameters.Add(movement.Location.Y);
                }
            }
        }
    }
}
