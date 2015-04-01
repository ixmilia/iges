// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace IxMilia.Iges.Entities
{
    public partial class IgesSubfigureDefinition : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.SubfigureDefinition; } }

        // properties
        public int Depth { get; set; }
        public string Name { get; set; }
        private int EntityCount { get; set; }

        // custom properties
        public List<IgesEntity> Entities
        {
            get
            {
                return SubEntities;
            }
        }

        public IgesSubfigureDefinition()
            : base()
        {
            this.Depth = 0;
            this.Name = null;
            this.EntityCount = 0;
        }

        protected override void ReadParameters(List<string> parameters)
        {
            int index = 0;
            this.Depth = Integer(parameters[index++]);
            this.Name = String(parameters[index++]);
            this.EntityCount = Integer(parameters[index++]);
            for (int i = 0; i < EntityCount; i++)
            {
                this.SubEntityIndices.Add(Integer(parameters[index++]));
            }

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
