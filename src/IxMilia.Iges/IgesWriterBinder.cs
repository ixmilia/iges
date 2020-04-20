using System;
using System.Collections.Generic;
using IxMilia.Iges.Entities;

namespace IxMilia.Iges
{
    internal class IgesWriterBinder
    {
        private Dictionary<IgesEntity, int> _entityMap;

        public IgesWriterBinder(Dictionary<IgesEntity, int> entityMap)
        {
            _entityMap = entityMap;
        }

        public int GetEntityId(IgesEntity entity)
        {
            if (entity == null)
            {
                return 0;
            }
            else if (!_entityMap.ContainsKey(entity))
            {
                throw new InvalidOperationException($"Entity not found.  Did you forget to override {nameof(IgesEntity)}.{nameof(IgesEntity.GetReferencedEntities)}()?");
            }

            return _entityMap[entity];
        }
    }
}
