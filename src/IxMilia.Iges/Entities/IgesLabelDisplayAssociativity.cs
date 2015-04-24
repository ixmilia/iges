// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace IxMilia.Iges.Entities
{
    public class IgesLabelPlacement
    {
        public IgesViewBase View { get; set; }
        public IgesPoint Location { get; set; }
        public IgesLeader Leader { get; set; }
        public int Level { get; set; }
        public IgesEntity Label { get; set; }

        public IgesLabelPlacement(IgesViewBase view, IgesPoint location, IgesLeader leader, int level, IgesEntity label)
        {
            View = view;
            Location = location;
            Leader = leader;
            Level = level;
            Label = label;
        }
    }

    public class IgesLabelDisplayAssociativity : IgesAssociativity
    {
        private List<IgesPoint> _labelLocations;
        private List<int> _labelLevels;

        public List<IgesLabelPlacement> LabelPlacements { get; private set; }
        public IgesEntity AssociatedEntity { get; internal set; }

        public IgesLabelDisplayAssociativity()
            : base()
        {
            FormNumber = 5;
            _labelLocations = new List<IgesPoint>();
            _labelLevels = new List<int>();
            LabelPlacements = new List<IgesLabelPlacement>();
        }

        protected override int ReadParameters(List<string> parameters)
        {
            int index = 0;
            var labelPlacementCount = Integer(parameters, index++);
            for (int i = 0; i < labelPlacementCount; i++)
            {
                SubEntityIndices.Add(Integer(parameters, index++)); // pointer to view
                var x = Double(parameters, index++);
                var y = Double(parameters, index++);
                var z = Double(parameters, index++);
                _labelLocations.Add(new IgesPoint(x, y, z));
                SubEntityIndices.Add(Integer(parameters, index++)); // pointer to leader
                _labelLevels.Add(Integer(parameters, index++));
                SubEntityIndices.Add(Integer(parameters, index++)); // pointer to entity
            }

            return index;
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            base.OnAfterRead(directoryData);
            AssertCollections();
            LabelPlacements.Clear();
            for (int i = 0; i < _labelLocations.Count; i++)
            {
                LabelPlacements.Add(
                    new IgesLabelPlacement(
                        SubEntities[i * 3] as IgesViewBase,
                        _labelLocations[i],
                        SubEntities[i * 3 + 1] as IgesLeader,
                        _labelLevels[i],
                        SubEntities[i * 3 + 2]));
            }

            _labelLevels.Clear();
            _labelLocations.Clear();
            SubEntities.Clear();
        }

        protected override void WriteParameters(List<object> parameters)
        {
            AssertCollections();
            parameters.Add(_labelLocations.Count);
            for (int i = 0; i < _labelLocations.Count; i++)
            {
                parameters.Add(SubEntityIndices[i * 3]); // pointer to view
                parameters.Add(_labelLocations[i].X);
                parameters.Add(_labelLocations[i].Y);
                parameters.Add(_labelLocations[i].Z);
                parameters.Add(SubEntityIndices[i * 3 + 1]); // pointer to leader
                parameters.Add(_labelLevels[i]);
                parameters.Add(SubEntityIndices[i * 3 + 2]); // pointer to entity
            }
        }

        internal override void OnBeforeWrite()
        {
            base.OnBeforeWrite();
            SubEntities.Clear();
            _labelLocations.Clear();
            _labelLevels.Clear();
            for (int i = 0; i < LabelPlacements.Count; i++)
            {
                SubEntities.Add(LabelPlacements[i].View);
                _labelLocations.Add(LabelPlacements[i].Location);
                SubEntities.Add(LabelPlacements[i].Leader);
                _labelLevels.Add(LabelPlacements[i].Level);
                SubEntities.Add(LabelPlacements[i].Label);
            }
        }

        internal override void UnMarkEntitiesForTrimming(HashSet<int> entitiesToTrim)
        {
            base.UnMarkEntitiesForTrimming(entitiesToTrim);
            for (int i = 0; i < _labelLocations.Count; i++)
            {
                // don't trim the actual entity
                entitiesToTrim.Remove(SubEntityIndices[i * 3 + 2]);
            }
        }

        private void AssertCollections()
        {
            Debug.Assert(_labelLocations.Count == _labelLevels.Count);
            Debug.Assert(_labelLocations.Count * 3 == SubEntities.Count);
        }
    }
}
