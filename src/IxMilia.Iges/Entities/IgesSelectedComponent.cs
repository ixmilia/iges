using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesSelectedComponent : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.SelectedComponent; } }

        public IgesBooleanTree BooleanTree { get; set; }
        public IgesPoint SelectionPoint { get; set; }

        public IgesSelectedComponent()
            : this(null, IgesPoint.Origin)
        {
        }

        public IgesSelectedComponent(IgesBooleanTree booleanTree, IgesPoint selectionPoint)
        {
            EntityUseFlag = IgesEntityUseFlag.Other;
            BooleanTree = booleanTree;
            SelectionPoint = selectionPoint;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            int index = 0;
            binder.BindEntity(Integer(parameters, index++), e => BooleanTree = e as IgesBooleanTree);
            SelectionPoint = Point3(parameters, ref index);
            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return BooleanTree;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(binder.GetEntityId(BooleanTree));
            parameters.Add(SelectionPoint.X);
            parameters.Add(SelectionPoint.Y);
            parameters.Add(SelectionPoint.Z);
        }
    }
}
