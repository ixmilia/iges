// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public enum IgesTrimCurvePreference
    {
        Unspecified = 0,
        ModelSpace = 1,
        ParameterSpace = 2,
        EqualPreference = 3
    }

    public class IgesBounaryItem
    {
        public IgesEntity Entity { get; set; }
        public bool IsReversed { get; set; }
        public List<IgesEntity> AssociatedParameterCurves { get; private set; }

        internal int AssociatedParameterCurvesCount = 0;

        public IgesBounaryItem(IgesEntity entity, bool isReversed, IEnumerable<IgesEntity> associatedParameterCurves)
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
        public List<IgesBounaryItem> BoundaryItems { get; private set; }

        private int _curveCount = 0;

        public IgesBoundary()
            : base()
        {
            IsBounaryParametric = false;
            TrimCurvePreference = IgesTrimCurvePreference.Unspecified;
            Entity = null;
            BoundaryItems = new List<IgesBounaryItem>();
        }

        protected override int ReadParameters(List<string> parameters)
        {
            var index = 0;
            this.IsBounaryParametric = Boolean(parameters, index++);
            this.TrimCurvePreference = (IgesTrimCurvePreference)Integer(parameters, index++);
            SubEntityIndices.Add(Integer(parameters, index++));
            _curveCount = Integer(parameters, index++);
            for (int i = 0; i < _curveCount; i++)
            {
                SubEntityIndices.Add(Integer(parameters, index++));
                var isReversed = Integer(parameters, index++) == 2;
                var associatedParameterCurvesCount = Integer(parameters, index++);
                for (int j = 0; j < associatedParameterCurvesCount; j++)
                {
                    SubEntityIndices.Add(Integer(parameters, index++));
                }

                BoundaryItems.Add(new IgesBounaryItem(null, isReversed, new IgesEntity[0]) { AssociatedParameterCurvesCount = associatedParameterCurvesCount });
            }

            return index;
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            int index = 0;
            Entity = SubEntities[index++];
            for (int i = 0; i < _curveCount; i++)
            {
                BoundaryItems[i].Entity = SubEntities[index++];
                for (int j = 0; j < BoundaryItems[i].AssociatedParameterCurvesCount; j++)
                {
                    BoundaryItems[i].AssociatedParameterCurves.Add(SubEntities[index++]);
                }
            }
        }

        internal override void OnBeforeWrite()
        {
            SubEntities.Add(Entity);
            foreach (var boundaryItem in BoundaryItems)
            {
                SubEntities.Add(boundaryItem.Entity);
                foreach (var parameterCurve in boundaryItem.AssociatedParameterCurves)
                {
                    SubEntities.Add(parameterCurve);
                }
            }
        }

        protected override void WriteParameters(List<object> parameters)
        {
            int index = 0;
            parameters.Add(IsBounaryParametric);
            parameters.Add((int)TrimCurvePreference);
            parameters.Add(SubEntityIndices[index++]);
            parameters.Add(BoundaryItems.Count);
            foreach (var boundaryItem in BoundaryItems)
            {
                parameters.Add(SubEntityIndices[index++]);
                parameters.Add(boundaryItem.IsReversed);
                parameters.Add(boundaryItem.AssociatedParameterCurves.Count);
                foreach (var parameterCurve in boundaryItem.AssociatedParameterCurves)
                {
                    parameters.Add(SubEntities[index++]);
                }
            }
        }
    }
}
