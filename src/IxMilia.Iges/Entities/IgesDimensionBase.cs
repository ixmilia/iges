using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public abstract class IgesDimensionBase : IgesEntity
    {
        public IgesGeneralNote GeneralNote { get; set; }
        public IgesLeader FirstLeader { get; set; }
        public IgesLeader SecondLeader { get; set; }

        protected IgesDimensionBase()
        {
            EntityUseFlag = IgesEntityUseFlag.Annotation;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            yield return GeneralNote;
            yield return FirstLeader;
            yield return SecondLeader;
        }
    }
}
