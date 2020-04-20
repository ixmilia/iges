using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesManifestSolidBRepVoid
    {
        public IgesEntity Shell { get; set; }
        public bool IsOriented { get; set; }

        public IgesManifestSolidBRepVoid(IgesEntity shell, bool isOriented)
        {
            Shell = shell;
            IsOriented = isOriented;
        }
    }

    public class IgesManifestSolidBRepObject : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.ManifestSolidBRepObject; } }

        public IgesEntity Shell { get; set; }
        public bool IsOriented { get; set; }
        public List<IgesManifestSolidBRepVoid> Voids { get; } = new List<IgesManifestSolidBRepVoid>();

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var index = 0;
            binder.BindEntity(Integer(parameters, index++), shell => Shell = shell);
            IsOriented = Boolean(parameters, index++);
            var voidCount = Integer(parameters, index++);
            for (int i = 0; i < voidCount; i++)
            {
                var pointer = Integer(parameters, index++);
                var orientation = Boolean(parameters, index++);
                var vd = new IgesManifestSolidBRepVoid(null, orientation);
                Voids.Add(vd);
                binder.BindEntity(pointer, v => vd.Shell = v);
            }

            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return Shell;
            foreach (var v in Voids)
            {
                yield return v.Shell;
            }
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(binder.GetEntityId(Shell));
            parameters.Add(IsOriented);
            parameters.Add(Voids.Count);
            foreach (var v in Voids)
            {
                parameters.Add(binder.GetEntityId(v.Shell));
                parameters.Add(v.IsOriented);
            }
        }
    }
}
