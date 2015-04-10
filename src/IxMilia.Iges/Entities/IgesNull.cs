// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public partial class IgesNull : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.Null; } }

        public IgesNull()
            : base()
        {
        }

        protected override int ReadParameters(List<string> parameters)
        {
            return 0;
        }

        protected override void WriteParameters(List<object> parameters)
        {
        }
    }
}
