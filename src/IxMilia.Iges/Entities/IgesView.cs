using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesView: IgesViewBase
    {
        public IgesView()
            : this(0, 0.0, null, null, null, null, null, null)
        {
        }

        public IgesView(int viewNumber, double scaleFactor, IgesPlane left, IgesPlane top, IgesPlane right, IgesPlane bottom, IgesPlane back, IgesPlane front)
            : base(viewNumber, scaleFactor)
        {
            this.FormNumber = 0;
            ViewVolumeLeft = left;
            ViewVolumeTop = top;
            ViewVolumeRight = right;
            ViewVolumeBottom = bottom;
            ViewVolumeBack = back;
            ViewVolumeFront = front;
        }

        public IgesPlane ViewVolumeLeft { get; set; }

        public IgesPlane ViewVolumeTop { get; set; }

        public IgesPlane ViewVolumeRight { get; set; }

        public IgesPlane ViewVolumeBottom { get; set; }

        public IgesPlane ViewVolumeBack { get; set; }

        public IgesPlane ViewVolumeFront { get; set; }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var nextIndex = base.ReadParameters(parameters, binder);
            binder.BindEntity(Integer(parameters, nextIndex), e => ViewVolumeLeft = e as IgesPlane);
            binder.BindEntity(Integer(parameters, nextIndex + 1), e => ViewVolumeTop = e as IgesPlane);
            binder.BindEntity(Integer(parameters, nextIndex + 2), e => ViewVolumeRight = e as IgesPlane);
            binder.BindEntity(Integer(parameters, nextIndex + 3), e => ViewVolumeBottom = e as IgesPlane);
            binder.BindEntity(Integer(parameters, nextIndex + 4), e => ViewVolumeBack = e as IgesPlane);
            binder.BindEntity(Integer(parameters, nextIndex + 5), e => ViewVolumeFront = e as IgesPlane);
            return nextIndex + 6;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return ViewVolumeLeft;
            yield return ViewVolumeTop;
            yield return ViewVolumeRight;
            yield return ViewVolumeBottom;
            yield return ViewVolumeBack;
            yield return ViewVolumeFront;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            base.WriteParameters(parameters, binder);
            parameters.Add(binder.GetEntityId(ViewVolumeLeft));
            parameters.Add(binder.GetEntityId(ViewVolumeTop));
            parameters.Add(binder.GetEntityId(ViewVolumeRight));
            parameters.Add(binder.GetEntityId(ViewVolumeBottom));
            parameters.Add(binder.GetEntityId(ViewVolumeBack));
            parameters.Add(binder.GetEntityId(ViewVolumeFront));
        }
    }
}
