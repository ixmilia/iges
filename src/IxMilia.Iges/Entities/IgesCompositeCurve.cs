// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace IxMilia.Iges.Entities
{
    public partial class IgesCompositeCurve : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.CompositeCurve; } }

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
        }

        protected override int ReadParameters(List<string> parameters)
        {
            var entityCount = Integer(parameters, 0);
            for (int i = 0; i < entityCount; i++)
            {
                this.SubEntityIndices.Add(Integer(parameters, i + 1));
            }

            return entityCount + 1;
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(this.Entities.Count);
            parameters.AddRange(this.SubEntityIndices.Cast<object>());
        }
    }
}
