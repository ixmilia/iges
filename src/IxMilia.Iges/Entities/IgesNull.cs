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

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            return 0;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
        }
    }
}
