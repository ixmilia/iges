using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesNode : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.Node; } }

        public IgesPoint Offset { get; set; }

        public IgesTransformationMatrix DisplacementCoordinateSystem { get; set; }

        public uint NodeNumber
        {
            get { return EntitySubscript; }
            set { EntitySubscript = value; }
        }

        public IgesNode()
            : this(IgesPoint.Origin)
        {
        }

        public IgesNode(IgesPoint offset)
            : base()
        {
            EntityUseFlag = IgesEntityUseFlag.LogicalOrPositional;
            FormNumber = 0;
            Offset = offset;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            int index = 0;
            this.Offset = Point3(parameters, ref index);
            binder.BindEntity(Integer(parameters, index++), e => DisplacementCoordinateSystem = e as IgesTransformationMatrix);
            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return DisplacementCoordinateSystem;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(this.Offset.X);
            parameters.Add(this.Offset.Y);
            parameters.Add(this.Offset.Z);
            parameters.Add(binder.GetEntityId(DisplacementCoordinateSystem));
        }
    }
}
