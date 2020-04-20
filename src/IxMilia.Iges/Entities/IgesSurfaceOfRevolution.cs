using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesSurfaceOfRevolution : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.SurfaceOfRevolution; } }

        public IgesLine AxisOfRevolution { get; set; }
        public IgesEntity Generatrix { get; set; }
        public double StartAngle { get; set; }
        public double EndAngle { get; set; }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            binder.BindEntity(Integer(parameters, 0), e => AxisOfRevolution = e as IgesLine);
            binder.BindEntity(Integer(parameters, 1), e => Generatrix = e);
            StartAngle = Double(parameters, 2);
            EndAngle = Double(parameters, 3);
            return 4;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return AxisOfRevolution;
            yield return Generatrix;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(binder.GetEntityId(AxisOfRevolution));
            parameters.Add(binder.GetEntityId(Generatrix));
            parameters.Add(StartAngle);
            parameters.Add(EndAngle);
        }
    }
}
