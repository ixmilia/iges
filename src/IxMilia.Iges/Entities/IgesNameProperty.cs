// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesNameProperty : IgesProperty
    {
        public string Name { get; set; }

        internal IgesNameProperty()
            : base()
        {
            FormNumber = 15;
        }

        public IgesNameProperty(string name)
            : this()
        {
            Name = name;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var nextIndex = base.ReadParameters(parameters, binder);
            Name = String(parameters, nextIndex);
            return nextIndex + PropertyCount;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            PropertyCount = 1;
            base.WriteParameters(parameters, binder);
            parameters.Add(Name);
        }
    }
}
