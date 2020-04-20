using System;
using System.Collections;
using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public partial class IgesEntity
    {
        public class AssociatedEntityCollection : IList<IgesEntity>
        {
            private List<IgesEntity> _entities = new List<IgesEntity>();

            public IgesEntity this[int index]
            {
                get { return _entities[index]; }
                set
                {
                    VerifyEntity(value);
                    _entities[index] = value;
                }
            }

            public int Count { get { return _entities.Count; } }

            public bool IsReadOnly { get { return false; } }

            public void Add(IgesEntity item)
            {
                VerifyEntity(item);
                _entities.Add(item);
            }

            public void Clear()
            {
                _entities.Clear();
            }

            public bool Contains(IgesEntity item)
            {
                return _entities.Contains(item);
            }

            public void CopyTo(IgesEntity[] array, int arrayIndex)
            {
                _entities.CopyTo(array, arrayIndex);
            }

            public IEnumerator<IgesEntity> GetEnumerator()
            {
                return _entities.GetEnumerator();
            }

            public int IndexOf(IgesEntity item)
            {
                return _entities.IndexOf(item);
            }

            public void Insert(int index, IgesEntity item)
            {
                VerifyEntity(item);
                _entities.Insert(index, item);
            }

            public bool Remove(IgesEntity item)
            {
                return _entities.Remove(item);
            }

            public void RemoveAt(int index)
            {
                _entities.RemoveAt(index);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)_entities).GetEnumerator();
            }

            private static void VerifyEntity(IgesEntity entity)
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                switch (entity.EntityType)
                {
                    case IgesEntityType.AssociativityInstance:
                    case IgesEntityType.GeneralNote:
                    case IgesEntityType.TextDisplayTemplate:
                        break;
                    default:
                        throw new IgesException("Only associativity instance, general note, or text display templates can be used.");
                }
            }
        }
    }
}
