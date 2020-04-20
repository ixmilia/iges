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
