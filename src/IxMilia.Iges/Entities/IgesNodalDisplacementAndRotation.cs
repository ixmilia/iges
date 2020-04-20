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

        internal IgesNodalAnalysis()
        {
        }

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

        public IgesNodalDisplacementAndRotation()
        {
            NodeAnalyses = new List<IgesNodalAnalysis>();
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var index = 0;
            var analysisCount = Integer(parameters, index++);
            NodeAnalyses = Enumerable.Range(0, analysisCount).Select(_ => new IgesNodalAnalysis()).ToList();
            for (int i = 0; i < analysisCount; i++)
            {
                var idx = i;
                binder.BindEntity(Integer(parameters, index++), e => NodeAnalyses[idx].GeneralNote = e as IgesGeneralNote);
            }

            var nodeCount = Integer(parameters, index++);
            for (int i = 0; i < nodeCount; i++)
            {
                var idx = i;
                var analysisCases = new List<IgesNodalAnalysisCase>();
                NodeAnalyses[idx].Identifier = Integer(parameters, index++);
                binder.BindEntity(Integer(parameters, index++), e => NodeAnalyses[idx].FiniteElement = e as IgesFiniteElement);
                for (int j = 0; j < analysisCount; j++)
                {
                    var x = Double(parameters, index++);
                    var y = Double(parameters, index++);
                    var z = Double(parameters, index++);
                    var rx = Double(parameters, index++);
                    var ry = Double(parameters, index++);
                    var rz = Double(parameters, index++);
                    analysisCases.Add(new IgesNodalAnalysisCase(new IgesVector(x, y, z), rx, ry, rz));
                }
            }

            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            foreach (var analysis in NodeAnalyses)
            {
                yield return analysis.GeneralNote;
                yield return analysis.FiniteElement;
            }
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(NodeAnalyses.Count);
            parameters.AddRange(NodeAnalyses.Select(na => binder.GetEntityId(na.GeneralNote)).Cast<object>());

            var count = NodeAnalyses.FirstOrDefault()?.AnalysisCases.Count ?? 0;
            foreach (var analysis in NodeAnalyses)
            {
                parameters.Add(NodeAnalyses.Count);
                parameters.Add(analysis.Identifier);
                parameters.Add(binder.GetEntityId(analysis.FiniteElement));
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
