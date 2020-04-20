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

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var nextIndex = base.ReadParameters(parameters, binder);
            for (int i = 0; i < PropertyCount; i++)
            {
                DefinedLevels.Add(Integer(parameters, nextIndex + i));
            }

            return nextIndex + PropertyCount;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            PropertyCount = DefinedLevels.Count;
            base.WriteParameters(parameters, binder);
            foreach (var level in DefinedLevels)
            {
                parameters.Add(level);
            }
        }
    }
}
