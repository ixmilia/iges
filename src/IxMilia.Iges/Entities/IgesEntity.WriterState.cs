using System.Collections.Generic;
using System.Linq;

namespace IxMilia.Iges.Entities
{
    public abstract partial class IgesEntity
    {
        internal class WriterState
        {
            /// <summary>
            /// Map from a given entity to its directory pointer
            /// </summary>
            public Dictionary<IgesEntity, int> EntityMap { get; private set; }

            public List<string> DirectoryLines { get; private set; }

            public List<string> ParameterLines { get; private set; }

            public char FieldDelimiter { get; private set; }

            public char RecordDelimiter { get; private set; }

            public IgesWriterBinder WriterBinder { get; private set; }

            private Dictionary<HashSet<int>, int> _levelsPointers;

            public WriterState(Dictionary<IgesEntity, int> entityMap, List<string> directoryLines, List<string> parameterLines, char fieldDelimiter, char recordDelimiter)
            {
                EntityMap = entityMap;
                DirectoryLines = directoryLines;
                ParameterLines = parameterLines;
                FieldDelimiter = fieldDelimiter;
                RecordDelimiter = recordDelimiter;
                WriterBinder = new IgesWriterBinder(entityMap);
                _levelsPointers = new Dictionary<HashSet<int>, int>(new HashSetComparer());
            }

            public int GetOrWriteEntityIndex(IgesEntity entity)
            {
                if (entity == null)
                {
                    return 0;
                }
                else if (!EntityMap.ContainsKey(entity))
                {
                    var index = entity.AddDirectoryAndParameterLines(this);
                    EntityMap[entity] = index;
                    return index;
                }
                else
                {
                    return EntityMap[entity];
                }
            }

            public void ReportReferencedEntities(IEnumerable<IgesEntity> entities)
            {
                foreach (var entity in entities)
                {
                    GetOrWriteEntityIndex(entity);
                }
            }

            public int GetLevelsPointer(HashSet<int> levels)
            {
                if (!_levelsPointers.ContainsKey(levels))
                {
                    var custom = new IgesDefinitionLevelsProperty();
                    foreach (var level in levels)
                    {
                        custom.DefinedLevels.Add(level);
                    }

                    var index = custom.AddDirectoryAndParameterLines(this);
                    _levelsPointers[levels] = index;
                    return index;
                }
                else
                {
                    return _levelsPointers[levels];
                }
            }

            private class HashSetComparer : IEqualityComparer<HashSet<int>>
            {
                public bool Equals(HashSet<int> x, HashSet<int> y)
                {
                    if (x == null || y == null)
                    {
                        return false;
                    }

                    if (x.Count != y.Count)
                    {
                        return false;
                    }

                    return x.All(item => y.Contains(item));
                }

                public int GetHashCode(HashSet<int> obj)
                {
                    var hash = obj.Count.GetHashCode();
                    foreach (var item in obj)
                    {
                        hash = (hash << 3) ^ item.GetHashCode();
                    }

                    return hash;
                }
            }
        }
    }
}
