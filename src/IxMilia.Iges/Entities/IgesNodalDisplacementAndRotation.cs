// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace IxMilia.Iges.Entities
{
    public class IgesNodalAnalysis
    {
        public int Identifier { get; set; }
        public IgesGeneralNote GeneralNote { get; set; }
        public IgesFiniteElement FiniteElement { get; set; }
        public List<IgesNodalAnalysisCase> AnalysisCases { get; private set; }

        public IgesNodalAnalysis(int id, IgesGeneralNote generalNote, IgesFiniteElement finiteElement, IEnumerable<IgesNodalAnalysisCase> analysisCases)
        {
            Identifier = id;
            GeneralNote = generalNote;
            FiniteElement = finiteElement;
            AnalysisCases = analysisCases.ToList();
        }
    }

    public class IgesNodalAnalysisCase
    {
        public IgesVector Offset { get; set; }
        public double XRotation { get; set; }
        public double YRotation { get; set; }
        public double ZRotation { get; set; }

        public IgesNodalAnalysisCase(IgesVector offset, double xRotation, double yRotation, double zRotation)
        {
            Offset = offset;
            XRotation = xRotation;
            YRotation = yRotation;
            ZRotation = zRotation;
        }
    }

    public class IgesNodalDisplacementAndRotation : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.NodalDisplacementAndRotation; } }

        public List<IgesNodalAnalysis> NodeAnalyses { get; private set; }

        private int _analysisCount;
        private int _nodeCount;
        private List<int> _entityIds;

        public IgesNodalDisplacementAndRotation()
        {
            NodeAnalyses = new List<IgesNodalAnalysis>();
        }

        protected override int ReadParameters(List<string> parameters)
        {
            _entityIds = new List<int>();
            var index = 0;
            _analysisCount = Integer(parameters, index++);
            for (int i = 0; i < _analysisCount; i++)
            {
                SubEntityIndices.Add(Integer(parameters, index++));
            }

            _nodeCount = Integer(parameters, index++);
            for (int i = 0; i < _nodeCount; i++)
            {
                var analysisCases = new List<IgesNodalAnalysisCase>();
                int id = Integer(parameters, index++);
                var entityId = Integer(parameters, index++);
                _entityIds.Add(entityId);
                SubEntityIndices.Add(entityId);
                for (int j = 0; j < _analysisCount; j++)
                {
                    var x = Double(parameters, index++);
                    var y = Double(parameters, index++);
                    var z = Double(parameters, index++);
                    var rx = Double(parameters, index++);
                    var ry = Double(parameters, index++);
                    var rz = Double(parameters, index++);
                    analysisCases.Add(new IgesNodalAnalysisCase(new IgesVector(x, y, z), rx, ry, rz));
                }

                NodeAnalyses.Add(new IgesNodalAnalysis(id, null, null, analysisCases));
            }

            return index;
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            for (int i = 0; i < _analysisCount; i++)
            {
                NodeAnalyses[i].GeneralNote = SubEntities[i] as IgesGeneralNote;
            }

            for (int i = _analysisCount; i < _nodeCount; i++)
            {
                NodeAnalyses[i].FiniteElement = SubEntities[i] as IgesFiniteElement;
            }
        }

        internal override void UnMarkEntitiesForTrimming(HashSet<int> entitiesToTrim)
        {
            foreach (var entityId in _entityIds)
            {
                entitiesToTrim.Remove(entityId);
            }

            _entityIds.Clear();
        }

        internal override void OnBeforeWrite()
        {
            foreach (var analysis in NodeAnalyses)
            {
                SubEntities.Add(analysis.GeneralNote);
            }

            foreach (var analysis in NodeAnalyses)
            {
                SubEntities.Add(analysis.FiniteElement);
            }
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(NodeAnalyses.Count);
            parameters.AddRange(SubEntityIndices.Take(NodeAnalyses.Count).Cast<object>());

            var count = NodeAnalyses.FirstOrDefault()?.AnalysisCases.Count ?? 0;
            for (var i = 0; i < NodeAnalyses.Count; i++)
            {
                var analysis = NodeAnalyses[i];
                parameters.Add(NodeAnalyses.Count);
                parameters.Add(analysis.Identifier);
                parameters.Add(SubEntityIndices[NodeAnalyses.Count + i]);
                foreach (var analysisCase in analysis.AnalysisCases)
                {
                    parameters.Add(analysisCase.Offset.X);
                    parameters.Add(analysisCase.Offset.Y);
                    parameters.Add(analysisCase.Offset.Z);
                    parameters.Add(analysisCase.XRotation);
                    parameters.Add(analysisCase.YRotation);
                    parameters.Add(analysisCase.ZRotation);
                }
            }
        }
    }
}
