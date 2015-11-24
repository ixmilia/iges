// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace IxMilia.Iges.Entities
{
    public class IgesTrimmedParametricSurface : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.TrimmedParametricSurface; } }

        public IgesEntity Surface { get; set; }
        public bool IsOuterBoundaryD { get; set; }
        public List<IgesEntity> BoundaryEntities { get; private set; }
        public IgesEntity OuterBoundary { get; set; }

        protected override int ReadParameters(List<string> parameters)
        {
            int index = 0;
            SubEntityIndices.Add(Integer(parameters, index++)); // Surface
            IsOuterBoundaryD = !Boolean(parameters, index++);
            var boundaryEntityCount = Integer(parameters, index++);
            SubEntityIndices.Add(Integer(parameters, index++)); // OuterBoundary
            for (int i = 0; i < boundaryEntityCount; i++)
            {
                SubEntityIndices.Add(Integer(parameters, index++));
            }

            return index;
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            Surface = SubEntities[0];
            OuterBoundary = SubEntities[1];
            BoundaryEntities.AddRange(SubEntities.Skip(2));
        }

        internal override void OnBeforeWrite()
        {
            SubEntities.Add(Surface);
            SubEntities.Add(OuterBoundary);
            SubEntities.AddRange(BoundaryEntities);
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(SubEntityIndices[0]); // Surface
            parameters.Add(!IsOuterBoundaryD);
            parameters.Add(BoundaryEntities.Count);
            parameters.Add(SubEntityIndices[1]); // OuterBoundary
            parameters.AddRange(SubEntityIndices.Skip(2).Cast<object>());
        }
    }
}
