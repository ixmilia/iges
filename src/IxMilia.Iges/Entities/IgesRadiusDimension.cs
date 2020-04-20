using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesRadiusDimension : IgesDimensionBase
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.RadiusDimension; } }

        public IgesPoint ArcCenter { get; set; }

        public bool HasTwoLeaders { get { return SecondLeader != null; } }

        public IgesRadiusDimension()
        {
            ArcCenter = IgesPoint.Origin;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var index = 0;
            binder.BindEntity(Integer(parameters, index++), generalNote => GeneralNote = generalNote as IgesGeneralNote);
            binder.BindEntity(Integer(parameters, index++), leader => FirstLeader = leader as IgesLeader);
            ArcCenter = Point2(parameters, ref index);
            if (FormNumber == 1)
            {
                binder.BindEntity(Integer(parameters, index++), leader => SecondLeader = leader as IgesLeader);
            }

            return index;
        }

        internal override void OnBeforeWrite()
        {
            FormNumber = HasTwoLeaders ? 1 : 0;
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            parameters.Add(binder.GetEntityId(GeneralNote));
            parameters.Add(binder.GetEntityId(FirstLeader));
            parameters.Add(ArcCenter.X);
            parameters.Add(ArcCenter.Y);
            if (HasTwoLeaders)
            {
                parameters.Add(binder.GetEntityId(SecondLeader));
            }
        }
    }
}
