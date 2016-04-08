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

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            int index = 0;
            AreBoundaryEntitiesOnlyInModelSpace = !Boolean(parameters, index++);
            binder.BindEntity(Integer(parameters, index++), e => Surface = e);
            var boundaryItemCount = Integer(parameters, index++);
            for (int i = 0; i < boundaryItemCount; i++)
            {
                binder.BindEntity(Integer(parameters, index++), e => BoundaryEntities.Add(e));
            }

            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return Surface;
            foreach (var boundary in BoundaryEntities)
            {
                yield return boundary;
            }
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            
            parameters.Add(!AreBoundaryEntitiesOnlyInModelSpace);
            parameters.Add(binder.GetEntityId(Surface));
            parameters.Add(BoundaryEntities.Count);
            parameters.AddRange(BoundaryEntities.Select(binder.GetEntityId).Cast<object>());
        }
    }
}
