using System;
using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesNodalResult
    {
        public IgesNode Node { get; set; }
        public List<double> Values { get; } = new List<double>();
    }

    public class IgesNodalResults : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.NodalResults; } }

        public IgesResultType ResultsType
        {
            get { return (IgesResultType)FormNumber; }
            set { FormNumber = (int)value; }
        }

        public uint AnalysisCaseNumber
        {
            get { return EntitySubscript; }
            set { EntitySubscript = value; }
        }

        public IgesGeneralNote GeneralNote { get; set; }
        public int AnalysisSubcase { get; set; }
        public DateTime AnalysisTime { get; set; }
        public List<IgesNodalResult> Results { get; } = new List<IgesNodalResult>();

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            Results.Clear();

            int index = 0;
            binder.BindEntity(Integer(parameters, index++), e => GeneralNote = e as IgesGeneralNote);
            AnalysisSubcase = Integer(parameters, index++);
            AnalysisTime = DateTime(parameters, index++);
            var valueCount = Integer(parameters, index++);
            var nodeCount = Integer(parameters, index++);
            for (int i = 0; i < nodeCount; i++)
            {
                var result = new IgesNodalResult();
                int nodeNumber = Integer(parameters, index++); // not used
                binder.BindEntity(Integer(parameters, index++), e => result.Node = e as IgesNode);
                for (int j = 0; j < valueCount; j++)
                {
                    result.Values.Add(Double(parameters, index++));
                }

                Results.Add(result);
            }

            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return GeneralNote;
            foreach (var result in Results)
            {
                yield return result.Node;
            }
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(binder.GetEntityId(GeneralNote));
            parameters.Add(AnalysisSubcase);
            parameters.Add(IgesFileWriter.ParameterToString(AnalysisTime));
            parameters.Add(ExpectedValueCount);
            parameters.Add(Results.Count);
            for (int i = 0; i < Results.Count; i++)
            {
                parameters.Add(i); // node number
                parameters.Add(binder.GetEntityId(Results[i].Node)); // node pointer
                foreach (var value in Results[i].Values)
                {
                    parameters.Add(value);
                }
            }
        }

        private int ExpectedValueCount
        {
            get
            {
                switch (ResultsType)
                {
                    case IgesResultType.CoefficientOfPressure:
                    case IgesResultType.KineticEnergy:
                    case IgesResultType.KineticEnergyDensity:
                    case IgesResultType.Pressure:
                    case IgesResultType.StrainEnergy:
                    case IgesResultType.StrainEnergyDensity:
                    case IgesResultType.Temperature:
                        return 1;
                    case IgesResultType.Acceleration:
                    case IgesResultType.ElementalForce:
                    case IgesResultType.Flux:
                    case IgesResultType.HydrostaticPressure:
                    case IgesResultType.ReactionForce:
                    case IgesResultType.Symmetric2DimentionalElasticStressTensor:
                    case IgesResultType.Symmetric2DimentionalTotalStressTensor:
                    case IgesResultType.Symmetric2DimentionalElasticStrainTensor:
                    case IgesResultType.Symmetric2DimentionalPlasticStrainTensor:
                    case IgesResultType.Symmetric2DimentionalTotalStrainTensor:
                    case IgesResultType.Symmetric2DimentionalThermalStrain:
                    case IgesResultType.TotalDisplacement:
                    case IgesResultType.Velocity:
                    case IgesResultType.VelocityGradient:
                        return 3;
                    case IgesResultType.Symmetric3DimentionalElasticStressTensor:
                    case IgesResultType.Symmetric3DimentionalTotalStressTensor:
                    case IgesResultType.Symmetric3DimentionalElasticStrainTensor:
                    case IgesResultType.Symmetric3DimentionalPlasticStrainTensor:
                    case IgesResultType.Symmetric3DimentionalTotalStrainTensor:
                    case IgesResultType.Symmetric3DimentionalThermalStrain:
                    case IgesResultType.TotalDisplacementAndRotation:
                        return 6;
                    case IgesResultType.GeneralElasticStressTensor:
                    case IgesResultType.GeneralTotalStressTensor:
                    case IgesResultType.GeneralElasticStrainTensor:
                    case IgesResultType.GeneralPlasticStrainTensor:
                    case IgesResultType.GeneralTotalStrainTensor:
                    case IgesResultType.GeneralThermalStrain:
                        return 9;
                    default:
                        return 0;
                }
            }
        }
    }
}
