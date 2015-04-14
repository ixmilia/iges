// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesDefinitionLevelsProperty : IgesProperty
    {
        public HashSet<int> DefinedLevels { get; private set; }

        public IgesDefinitionLevelsProperty()
            : base()
        {
            FormNumber = 1;
            DefinedLevels = new HashSet<int>();
        }

        protected override int ReadParameters(List<string> parameters)
        {
            var nextIndex = base.ReadParameters(parameters);
            for (int i = 0; i < PropertyCount; i++)
            {
                DefinedLevels.Add(Integer(parameters, nextIndex + i));
            }

            return nextIndex + PropertyCount;
        }

        protected override void WriteParameters(List<object> parameters)
        {
            PropertyCount = DefinedLevels.Count;
            base.WriteParameters(parameters);
            foreach (var level in DefinedLevels)
            {
                parameters.Add(level);
            }
        }
    }
}
