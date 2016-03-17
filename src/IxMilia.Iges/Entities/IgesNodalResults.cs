// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace IxMilia.Iges.Entities
{
    public enum IgesNodalResultType
    {
        Unknown = 0,
        Temperature = 1,
        Pressure = 2,
        TotalDisplacement = 3,
        TotalDisplacementAndRotation = 4,
        Velocity = 5,
        VelocityGradient = 6,
        Acceleration = 7,
        Flux = 8,
        ElementalForce = 9,
        StrainEnergy = 10,
        StrainEnergyDensity = 11,
        ReactionForce = 12,
        KineticEnergy = 13,
        KineticEnergyDensity = 14,
        HydrostaticPressure = 15,
        CoefficientOfPressure = 16,
        Symmetric2DimentionalElasticStressTensor = 17,
        Symmetric2DimentionalTotalStressTensor = 18,
        Symmetric2DimentionalElasticStrainTensor = 19,
        Symmetric2DimentionalPlasticStrainTensor = 20,
        Symmetric2DimentionalTotalStrainTensor = 21,
        Symmetric2DimentionalThermalStrain = 22,
        Symmetric3DimentionalElasticStressTensor = 23,
        Symmetric3DimentionalTotalStressTensor = 24,
        Symmetric3DimentionalElasticStrainTensor = 25,
        Symmetric3DimentionalPlasticStrainTensor = 26,
        Symmetric3DimentionalTotalStrainTensor = 27,
        Symmetric3DimentionalThermalStrain = 28,
        GeneralElasticStressTensor = 29,
        GeneralTotalStressTensor = 30,
        GeneralElasticStrainTensor = 31,
        GeneralPlasticStrainTensor = 32,
        GeneralTotalStrainTensor = 33,
        GeneralThermalStrain = 34
    }

    public class IgesNodalResult
    {
        public IgesNode Node { get; set; }
        public List<double> Values { get; } = new List<double>();
    }

    public class IgesNodalResults : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.NodalResults; } }

        public IgesNodalResultType NodalResultsType
        {
            get { return (IgesNodalResultType)FormNumber; }
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

        protected override int ReadParameters(List<string> parameters)
        {
            Results.Clear();

            int index = 0;
            SubEntityIndices.Add(Integer(parameters, index++));
            AnalysisSubcase = Integer(parameters, index++);
            AnalysisTime = DateTime(parameters, index++);
            var valueCount = Integer(parameters, index++);
            var nodeCount = Integer(parameters, index++);
            for (int i = 0; i < nodeCount; i++)
            {
                var result = new IgesNodalResult();
                int nodeNumber = Integer(parameters, index++); // not used
                SubEntityIndices.Add(Integer(parameters, index++));
                for (int j = 0; j < valueCount; j++)
                {
                    result.Values.Add(Double(parameters, index++));
                }

                Results.Add(result);
            }

            return index;
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            GeneralNote = SubEntities[0] as IgesGeneralNote;
            for (int i = 0; i < Results.Count; i++)
            {
                Results[i].Node = SubEntities[i + 1] as IgesNode;
            }
        }

        internal override void OnBeforeWrite()
        {
            SubEntities.Add(GeneralNote);
            SubEntities.AddRange(Results.Select(r => r.Node));
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(SubEntityIndices[0]);
            parameters.Add(AnalysisSubcase);
            parameters.Add(IgesFileWriter.ParameterToString(AnalysisTime));
            parameters.Add(ExpectedValueCount);
            parameters.Add(Results.Count);
            for (int i = 0; i < Results.Count; i++)
            {
                parameters.Add(i); // node number
                parameters.Add(SubEntityIndices[i + 1]); // node pointer
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
                switch (NodalResultsType)
                {
                    case IgesNodalResultType.CoefficientOfPressure:
                    case IgesNodalResultType.KineticEnergy:
                    case IgesNodalResultType.KineticEnergyDensity:
                    case IgesNodalResultType.Pressure:
                    case IgesNodalResultType.StrainEnergy:
                    case IgesNodalResultType.StrainEnergyDensity:
                    case IgesNodalResultType.Temperature:
                        return 1;
                    case IgesNodalResultType.Acceleration:
                    case IgesNodalResultType.ElementalForce:
                    case IgesNodalResultType.Flux:
                    case IgesNodalResultType.HydrostaticPressure:
                    case IgesNodalResultType.ReactionForce:
                    case IgesNodalResultType.Symmetric2DimentionalElasticStressTensor:
                    case IgesNodalResultType.Symmetric2DimentionalTotalStressTensor:
                    case IgesNodalResultType.Symmetric2DimentionalElasticStrainTensor:
                    case IgesNodalResultType.Symmetric2DimentionalPlasticStrainTensor:
                    case IgesNodalResultType.Symmetric2DimentionalTotalStrainTensor:
                    case IgesNodalResultType.Symmetric2DimentionalThermalStrain:
                    case IgesNodalResultType.TotalDisplacement:
                    case IgesNodalResultType.Velocity:
                    case IgesNodalResultType.VelocityGradient:
                        return 3;
                    case IgesNodalResultType.Symmetric3DimentionalElasticStressTensor:
                    case IgesNodalResultType.Symmetric3DimentionalTotalStressTensor:
                    case IgesNodalResultType.Symmetric3DimentionalElasticStrainTensor:
                    case IgesNodalResultType.Symmetric3DimentionalPlasticStrainTensor:
                    case IgesNodalResultType.Symmetric3DimentionalTotalStrainTensor:
                    case IgesNodalResultType.Symmetric3DimentionalThermalStrain:
                    case IgesNodalResultType.TotalDisplacementAndRotation:
                        return 6;
                    case IgesNodalResultType.GeneralElasticStressTensor:
                    case IgesNodalResultType.GeneralTotalStressTensor:
                    case IgesNodalResultType.GeneralElasticStrainTensor:
                    case IgesNodalResultType.GeneralPlasticStrainTensor:
                    case IgesNodalResultType.GeneralTotalStrainTensor:
                    case IgesNodalResultType.GeneralThermalStrain:
                        return 9;
                    default:
                        return 0;
                }
            }
        }
    }
}
