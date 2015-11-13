// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace IxMilia.Iges.Entities
{
    public partial class IgesSubfigureDefinition : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.SubfigureDefinition; } }

        // properties
        public int Depth { get; set; }
        public string Name { get; set; }

        // custom properties
        public List<IgesEntity> Entities { get; private set; }

        public IgesSubfigureDefinition()
            : base()
        {
            this.Depth = 0;
            this.Name = null;
            this.EntityUseFlag = IgesEntityUseFlag.Definition;
            Entities = new List<IgesEntity>();
        }

        protected override int ReadParameters(List<string> parameters)
        {
            this.Depth = Integer(parameters, 0);
            this.Name = String(parameters, 1);
            var entityCount = Integer(parameters, 2);
            for (int i = 0; i < entityCount; i++)
            {
                this.SubEntityIndices.Add(Integer(parameters, i + 3));
            }

            return entityCount + 3;
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            Debug.Assert(EntityUseFlag == IgesEntityUseFlag.Definition);
            Entities.Clear();
            Entities.AddRange(SubEntities);
        }

        internal override void OnBeforeWrite()
        {
            SubEntities.AddRange(Entities);
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(this.Depth);
            parameters.Add(this.Name);
            parameters.Add(this.Entities.Count);
            parameters.AddRange(this.SubEntityIndices.Cast<object>());
        }
    }
}
