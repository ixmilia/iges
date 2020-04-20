using System.Collections.Generic;
using System.Linq;

namespace IxMilia.Iges.Entities
{
    public enum IgesTrimCurvePreference
    {
        Unspecified = 0,
        ModelSpace = 1,
        ParameterSpace = 2,
        EqualPreference = 3
    }

    public class IgesBoundaryItem
    {
        public IgesEntity Entity { get; set; }
        public bool IsReversed { get; set; }
        public List<IgesEntity> AssociatedParameterCurves { get; private set; }

        internal int AssociatedParameterCurvesCount = 0;

        internal IgesBoundaryItem()
        {
            AssociatedParameterCurves = new List<IgesEntity>();
        }

        public IgesBoundaryItem(IgesEntity entity, bool isReversed, IEnumerable<IgesEntity> associatedParameterCurves)
        {
            Entity = entity;
            IsReversed = isReversed;
            AssociatedParameterCurves = new List<IgesEntity>(associatedParameterCurves);
        }
    }

    public class IgesBoundary : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.Boundary; } }

        public bool IsBounaryParametric { get; set; }
        public IgesTrimCurvePreference TrimCurvePreference { get; set; }
        public IgesEntity Entity { get; set; }
        public List<IgesBoundaryItem> BoundaryItems { get; private set; }

        private int _curveCount = 0;

        public IgesBoundary()
            : base()
        {
            IsBounaryParametric = false;
            TrimCurvePreference = IgesTrimCurvePreference.Unspecified;
            Entity = null;
            BoundaryItems = new List<IgesBoundaryItem>();
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var index = 0;
            this.IsBounaryParametric = Boolean(parameters, index++);
            this.TrimCurvePreference = (IgesTrimCurvePreference)Integer(parameters, index++);
            binder.BindEntity(Integer(parameters, index++), e => Entity = e);
            _curveCount = Integer(parameters, index++);
            for (int i = 0; i < _curveCount; i++)
            {
                var boundaryItem = new IgesBoundaryItem();
                binder.BindEntity(Integer(parameters, index++), e => boundaryItem.Entity = e);
                boundaryItem.IsReversed = Integer(parameters, index++) == 2;
                var associatedParameterCurvesCount = Integer(parameters, index++);
                for (int j = 0; j < associatedParameterCurvesCount; j++)
                {
                    binder.BindEntity(Integer(parameters, index++), e => boundaryItem.AssociatedParameterCurves.Add(e));
                }

                BoundaryItems.Add(boundaryItem);
            }

            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return Entity;
            foreach (var boundaryItem in BoundaryItems)
            {
                yield return boundaryItem.Entity;
                foreach (var associated in boundaryItem.AssociatedParameterCurves)
                {
                    yield return associated;
                }
            }
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(IsBounaryParametric);
            parameters.Add((int)TrimCurvePreference);
            parameters.Add(binder.GetEntityId(Entity));
            parameters.Add(BoundaryItems.Count);
            foreach (var boundaryItem in BoundaryItems)
            {
                parameters.Add(binder.GetEntityId(boundaryItem.Entity));
                parameters.Add(boundaryItem.IsReversed);
                parameters.Add(boundaryItem.AssociatedParameterCurves.Count);
                parameters.AddRange(boundaryItem.AssociatedParameterCurves.Select(binder.GetEntityId).Cast<object>());
            }
        }
    }
}
