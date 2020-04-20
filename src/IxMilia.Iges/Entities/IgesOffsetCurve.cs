using System.Collections.Generic;

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

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            binder.BindEntity(Integer(parameters, 0), e => CurveToOffset = e);
            DistanceType = (IgesOffsetDistanceType)Integer(parameters, 1);
            binder.BindEntity(Integer(parameters, 2), e => EntityOffsetCurveFunction = e);
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

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return CurveToOffset;
            yield return EntityOffsetCurveFunction;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(binder.GetEntityId(CurveToOffset));
            parameters.Add((int)DistanceType);
            parameters.Add(binder.GetEntityId(EntityOffsetCurveFunction));
            parameters.Add(ParameterIndexOfFunctionEntityCurve);
            parameters.Add((int)TaperedOffsetType);
            parameters.Add(FirstOffsetDistance);
            parameters.Add(FirstOffsetDistanceValue);
            parameters.Add(SecondOffsetDistance);
            parameters.Add(SecondOffsetDistanceValue);
            parameters.Add(EntityNormal.X);
            parameters.Add(EntityNormal.Y);
            parameters.Add(EntityNormal.Z);
            parameters.Add(StartingParameterValue);
            parameters.Add(EndingParameterValue);
        }
    }
}
