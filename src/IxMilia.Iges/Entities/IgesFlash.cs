using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public enum IgesClosedAreaType
    {
        ReferencedEntity = 0,
        Circular = 1,
        Rectangular = 2,
        Donut = 3,
        Canoe = 4
    }

    public class IgesFlash : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.Flash; } }

        private IgesEntity _referenceEntity;

        public double XOffset { get; set; }
        public double YOffset { get; set; }
        public double SizeParameter1 { get; set; }
        public double SizeParameter2 { get; set; }
        public double RotationAngle { get; set; }

        public IgesEntity ReferenceEntity
        {
            get { return _referenceEntity; }
            set
            {
                _referenceEntity = value;
                AreaType = IgesClosedAreaType.ReferencedEntity;
            }
        }

        public IgesClosedAreaType AreaType
        {
            get { return (IgesClosedAreaType)FormNumber; }
            set
            {
                FormNumber = (int)value;
                if (AreaType != IgesClosedAreaType.ReferencedEntity)
                {
                    _referenceEntity = null;
                }
            }
        }

        public IgesFlash()
            : base()
        {
            Hierarchy = IgesHierarchy.GlobalTopDown;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            XOffset = Double(parameters, 0);
            YOffset = Double(parameters, 1);
            SizeParameter1 = Double(parameters, 2);
            SizeParameter2 = Double(parameters, 3);
            RotationAngle = Double(parameters, 4);
            binder.BindEntity(Integer(parameters, 5), e =>
            {
                if (e != null)
                {
                    ReferenceEntity = e;
                }
            });
            return 6;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return ReferenceEntity;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(XOffset);
            parameters.Add(YOffset);
            parameters.Add(SizeParameter1);
            parameters.Add(SizeParameter2);
            parameters.Add(RotationAngle);
            parameters.Add(binder.GetEntityId(ReferenceEntity));
        }
    }
}
