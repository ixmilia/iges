// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

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
