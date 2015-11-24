// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace IxMilia.Iges.Entities
{
    public class IgesBoundedSurface : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.BoundedSurface; } }

        public bool AreBoundaryEntitiesOnlyInModelSpace { get; set; }
        public IgesEntity Surface { get; set; }
        public List<IgesEntity> BoundaryEntities { get; private set; }

        public IgesBoundedSurface()
            : base()
        {
            BoundaryEntities = new List<IgesEntity>();
        }

        protected override int ReadParameters(List<string> parameters)
        {
            int index = 0;
            AreBoundaryEntitiesOnlyInModelSpace = !Boolean(parameters, index++);
            SubEntityIndices.Add(Integer(parameters, index++)); // Surface
            var boundaryItemCount = Integer(parameters, index++);
            for (int i = 0; i < boundaryItemCount; i++)
            {
                SubEntityIndices.Add(Integer(parameters, index++));
            }

            return index;
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            Surface = SubEntities[0];
            BoundaryEntities.AddRange(SubEntities.Skip(1));
        }

        internal override void OnBeforeWrite()
        {
            SubEntities.Add(Surface);
            SubEntities.AddRange(BoundaryEntities);
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(!AreBoundaryEntitiesOnlyInModelSpace);
            parameters.Add(SubEntityIndices[0]);
            parameters.Add(BoundaryEntities.Count);
            parameters.AddRange(SubEntityIndices.Skip(1).Cast<object>());
        }
    }
}
