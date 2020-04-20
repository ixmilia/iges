namespace IxMilia.Iges.Entities
{
    public abstract class IgesLineFontDefinitionBase : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.LineFontDefinition; } }

        public IgesLineFontDefinitionBase()
            : base()
        {
            this.SubordinateEntitySwitchType = IgesSubordinateEntitySwitchType.Independent;
            this.EntityUseFlag = IgesEntityUseFlag.Definition;
        }
    }
}
