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
            this.Depth = Integer(parameters, 0);
            this.Name = String(parameters, 1);
            this.EntityCount = Integer(parameters, 2);
            for (int i = 0; i < EntityCount; i++)
            {
                this.SubEntityIndices.Add(Integer(parameters, i + 3));
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
