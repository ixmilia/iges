using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public enum IgesRuledSurfaceDirection
    {
        FirstToFirst_LastToLast = 0,
        FirstToLast_LastToFirst = 1
    }

    public class IgesRuledSurface : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.RuledSurface; } }

        public IgesEntity FirstCurve { get; set; }
        public IgesEntity SecondCurve { get; set; }
        public IgesRuledSurfaceDirection Direction { get; set; }
        public bool IsDevelopable { get; set; }
        public bool CurvesProvideParameterization
        {
            get { return FormNumber == 0; }
            set { FormNumber = value ? 0 : 1; }
        }

        public IgesRuledSurface()
            : base()
        {
        }

        public IgesRuledSurface(IgesEntity firstCurve, IgesEntity secondCurve)
            : this()
        {
            FirstCurve = firstCurve;
            SecondCurve = secondCurve;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            binder.BindEntity(Integer(parameters, 0), e => FirstCurve = e);
            binder.BindEntity(Integer(parameters, 1), e => SecondCurve = e);
            Direction = (IgesRuledSurfaceDirection)Integer(parameters, 2);
            IsDevelopable = Boolean(parameters, 3);
            return 4;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return FirstCurve;
            yield return SecondCurve;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(binder.GetEntityId(FirstCurve));
            parameters.Add(binder.GetEntityId(SecondCurve));
            parameters.Add((int)Direction);
            parameters.Add(IsDevelopable ? 1 : 0);
        }
    }
}
