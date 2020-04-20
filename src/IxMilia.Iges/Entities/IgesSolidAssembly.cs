using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesSolidAndTransformationMatrix
    {
        public IgesEntity Solid { get; set; }
        public IgesTransformationMatrix TransformationMatrix { get; set; }

        internal IgesSolidAndTransformationMatrix()
            : this(null, null)
        {
        }

        public IgesSolidAndTransformationMatrix(IgesEntity solid, IgesTransformationMatrix matrix)
        {
            Solid = solid;
            TransformationMatrix = matrix;
        }
    }

    public class IgesSolidAssembly : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.SolidAssembly; } }

        public List<IgesSolidAndTransformationMatrix> Solids { get; }

        public IgesSolidAssembly()
        {
            EntityUseFlag = IgesEntityUseFlag.Definition;
            Solids = new List<IgesSolidAndTransformationMatrix>();
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var index = 0;
            var itemCount = Integer(parameters, index++);
            for (int i = 0; i < itemCount; i++)
            {
                var pointer = Integer(parameters, index++);
                var item = new IgesSolidAndTransformationMatrix();
                Solids.Add(item);
                binder.BindEntity(pointer, solid => item.Solid = solid);
            }

            for (int i = 0; i < itemCount; i++)
            {
                var pointer = Integer(parameters, index++);
                var item = Solids[i];
                binder.BindEntity(pointer, matrix => item.TransformationMatrix = matrix as IgesTransformationMatrix ?? IgesTransformationMatrix.Identity);
            }

            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            foreach (var solid in Solids)
            {
                yield return solid.Solid;
                yield return solid.TransformationMatrix;
            }
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(Solids.Count);
            foreach (var solid in Solids)
            {
                parameters.Add(binder.GetEntityId(solid.Solid));
            }

            foreach (var solid in Solids)
            {
                parameters.Add(binder.GetEntityId(solid.TransformationMatrix));
            }
        }
    }
}
