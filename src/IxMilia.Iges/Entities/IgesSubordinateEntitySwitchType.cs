namespace IxMilia.Iges.Entities
{
    public enum IgesSubordinateEntitySwitchType
    {
        Independent = 0,
        PhysicallyDependent = 1,
        LogicallyDependent = 2,
        PhysicallyAndLogicallyDependent = PhysicallyDependent | LogicallyDependent
    }
}
