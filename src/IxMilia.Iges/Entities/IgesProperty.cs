// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public abstract class IgesProperty : IgesEntity
    {
        protected int PropertyCount { get; set; }

        public override IgesEntityType EntityType { get { return IgesEntityType.Property; } }

        protected override int ReadParameters(List<string> parameters)
        {
            PropertyCount = Integer(parameters, 0);
            return 1;
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(PropertyCount);
        }
    }
}
