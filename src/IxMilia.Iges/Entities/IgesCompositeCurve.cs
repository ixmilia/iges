// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace IxMilia.Iges.Entities
{
    public partial class IgesCompositeCurve : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.CompositeCurve; } }

        // properties
        private int EntityCount { get; set; }

        // custom properties
        public List<IgesEntity> Entities
        {
            get
            {
                return SubEntities;
            }
        }

        public IgesCompositeCurve()
            : base()
        {
            this.EntityCount = 0;
        }

        protected override void ReadParameters(List<string> parameters)
        {
            this.EntityCount = Integer(parameters, 0);
            for (int i = 0; i < EntityCount; i++)
            {
                this.SubEntityIndices.Add(Integer(parameters, i + 1));
            }
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(this.Entities.Count);
            parameters.AddRange(this.SubEntityIndices.Cast<object>());
        }
    }
}
