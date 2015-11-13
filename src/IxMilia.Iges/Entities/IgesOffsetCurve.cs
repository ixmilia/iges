// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace IxMilia.Iges.Entities
{
    public enum IgesOffsetDistanceType
    {
        SingleUniformOffset = 1,
        VaryingLinearly = 2,
        FunctionSpecified = 3
    }

    public enum IgesTaperedOffsetType
    {
        None = 0,
        FunctionOfArcLength = 1,
        FunctionOfParameter = 2
    }

    public class IgesOffsetCurve : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.OffsetCurve; } }

        public IgesEntity CurveToOffset { get; set; }
        public IgesOffsetDistanceType DistanceType { get; set; }
        public IgesEntity EntityOffsetCurveFunction { get; set; }
        public int ParameterIndexOfFunctionEntityCurve { get; set; }
        public IgesTaperedOffsetType TaperedOffsetType { get; set; }
        public double FirstOffsetDistance { get; set; }
        public double FirstOffsetDistanceValue { get; set; }
        public double SecondOffsetDistance { get; set; }
        public double SecondOffsetDistanceValue { get; set; }
        public IgesVector EntityNormal { get; set; }
        public double StartingParameterValue { get; set; }
        public double EndingParameterValue { get; set; }

        public IgesOffsetCurve()
            : base()
        {
            EntityNormal = IgesVector.ZAxis;
        }

        protected override int ReadParameters(List<string> parameters)
        {
            SubEntityIndices.Add(Integer(parameters, 0));
            DistanceType = (IgesOffsetDistanceType)Integer(parameters, 1);
            SubEntityIndices.Add(Integer(parameters, 2));
            ParameterIndexOfFunctionEntityCurve = Integer(parameters, 3);
            TaperedOffsetType = (IgesTaperedOffsetType)Integer(parameters, 4);
            FirstOffsetDistance = Double(parameters, 5);
            FirstOffsetDistanceValue = Double(parameters, 6);
            SecondOffsetDistance = Double(parameters, 7);
            SecondOffsetDistanceValue = Double(parameters, 8);
            EntityNormal = new IgesVector(Double(parameters, 9), Double(parameters, 10), Double(parameters, 11));
            StartingParameterValue = Double(parameters, 12);
            EndingParameterValue = Double(parameters, 13);
            return 14;
        }

        protected override void WriteParameters(List<object> parameters)
        {
            parameters.Add(SubEntityIndices[0]);
            parameters.Add((int)DistanceType);
            parameters.Add(SubEntityIndices[1]);
            parameters.Add(ParameterIndexOfFunctionEntityCurve);
            parameters.Add((int)TaperedOffsetType);
            parameters.Add(FirstOffsetDistance);
            parameters.Add(FirstOffsetDistanceValue);
            parameters.Add(SecondOffsetDistance);
            parameters.Add(SecondOffsetDistanceValue);
            parameters.Add(EntityNormal?.X ?? 0.0);
            parameters.Add(EntityNormal?.Y ?? 0.0);
            parameters.Add(EntityNormal?.Z ?? 0.0);
            parameters.Add(StartingParameterValue);
            parameters.Add(EndingParameterValue);
        }

        internal override void OnBeforeWrite()
        {
            SubEntities.Add(CurveToOffset);
            SubEntities.Add(EntityOffsetCurveFunction);
        }

        internal override void OnAfterRead(IgesDirectoryData directoryData)
        {
            Debug.Assert(FormNumber == 0);
            CurveToOffset = SubEntities[0];
            EntityOffsetCurveFunction = SubEntities[1];

            Debug.Assert(DistanceType == IgesOffsetDistanceType.FunctionSpecified ^ EntityOffsetCurveFunction == null);
            Debug.Assert(DistanceType == IgesOffsetDistanceType.FunctionSpecified ^ ParameterIndexOfFunctionEntityCurve == 0);
            Debug.Assert((DistanceType == IgesOffsetDistanceType.VaryingLinearly || DistanceType == IgesOffsetDistanceType.FunctionSpecified) ^ TaperedOffsetType == IgesTaperedOffsetType.None);
        }
    }
}
