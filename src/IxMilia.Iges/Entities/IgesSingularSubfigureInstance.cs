using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public partial class IgesSingularSubfigureInstance : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.SingularSubfigureInstance; } }

        public IgesSubfigureDefinition SubfigureDefinition { get; set; }

        public IgesVector Translation { get; set; }

        public double Scale { get; set; }

        internal IgesSingularSubfigureInstance()
        {
        }

        public IgesSingularSubfigureInstance(IgesSubfigureDefinition subfigureDefinition, IgesVector translation, double scale)
        {
            SubfigureDefinition = subfigureDefinition;
            Translation = translation;
            Scale = scale;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var index = 0;
            binder.BindEntity(Integer(parameters, index++), subfigureDefinition => SubfigureDefinition = subfigureDefinition as IgesSubfigureDefinition);
            Translation = new IgesVector(Double(parameters, index++), Double(parameters, index++), Double(parameters, index++));
            Scale = Double(parameters, index++);
            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return SubfigureDefinition;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(binder.GetEntityId(SubfigureDefinition));
            parameters.Add(Translation.X);
            parameters.Add(Translation.Y);
            parameters.Add(Translation.Z);
            parameters.Add(Scale);
        }
    }
}
