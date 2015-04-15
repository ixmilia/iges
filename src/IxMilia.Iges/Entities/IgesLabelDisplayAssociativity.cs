// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace IxMilia.Iges.Entities
{
    public class IgesLabelDisplayAssociativity : IgesAssociativity
    {
        public class IgesLabelPlacement
        {
            public IgesViewBase View { get; internal set; }
            public IgesPoint Location { get; internal set; }
            public IgesLeader Leader { get; internal set; }
            public int Level { get; internal set; }
            public IgesEntity Entity { get; internal set; }

            public IgesLabelPlacement(IgesViewBase view, IgesPoint location, IgesLeader leader, int level, IgesEntity entity)
            {
                View = view;
                Location = location;
                Leader = leader;
                Level = level;
                Entity = entity;
            }
        }

        private List<IgesPoint> _labelLocations;
        private List<int> _labelLevels;

        public IgesLabelDisplayAssociativity()
            : base()
        {
            FormNumber = 5;
            _labelLocations = new List<IgesPoint>();
            _labelLevels = new List<int>();
        }

        public IgesLabelPlacement this[int index]
        {
            get
            {
                return new IgesLabelPlacement(
                    (IgesViewBase)SubEntities[index * 3],
                    _labelLocations[index],
                    (IgesLeader)SubEntities[index * 3 + 1],
                    _labelLevels[index],
                    SubEntities[index * 3 + 2]);
            }
            set
            {
                AssertCollections();
                SubEntities[index * 3] = value.View;
                _labelLocations[index] = value.Location;
                SubEntities[index * 3 + 1] = value.Leader;
                _labelLevels[index] = value.Level;
                SubEntities[index * 3 + 2] = value.Entity;
            }
        }

        public void Add(IgesLabelPlacement labelPlacement)
        {
            AssertCollections();
            SubEntities.Add(labelPlacement.View);
            _labelLocations.Add(labelPlacement.Location);
            SubEntities.Add(labelPlacement.Leader);
            _labelLevels.Add(labelPlacement.Level);
            SubEntities.Add(labelPlacement.Entity);
            AssertCollections();
        }

        public void Clear()
        {
            SubEntities.Clear();
            _labelLocations.Clear();
            _labelLevels.Clear();
            AssertCollections();
        }

        public int Count { get { return _labelLocations.Count; } }

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
