namespace IxMilia.Iges.Entities
{
    public class IgesBooleanTreeEntity : IIgesBooleanTreeItem
    {
        public bool IsEntity { get { return true; } }

        public IgesEntity Entity { get; set; }

        public IgesBooleanTreeEntity(IgesEntity entity)
        {
            Entity = entity;
        }
    }
}
