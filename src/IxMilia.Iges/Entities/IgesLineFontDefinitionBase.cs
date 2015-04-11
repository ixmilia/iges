// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Diagnostics;

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

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            base.OnAfterRead(directoryData);
            Debug.Assert(SubordinateEntitySwitchType == IgesSubordinateEntitySwitchType.Independent);
            Debug.Assert(EntityUseFlag == IgesEntityUseFlag.Definition);
        }
    }
}
