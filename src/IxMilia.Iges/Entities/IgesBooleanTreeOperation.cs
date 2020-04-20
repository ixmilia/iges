namespace IxMilia.Iges.Entities
{
    public class IgesBooleanTreeOperation : IIgesBooleanTreeItem
    {
        public bool IsEntity { get { return false; } }

        public IgesBooleanTreeOperationKind OperationKind { get; set; }

        public IIgesBooleanTreeItem LeftChild { get; set; }

        public IIgesBooleanTreeItem RightChild { get; set; }

        public IgesBooleanTreeOperation(IgesBooleanTreeOperationKind operationKind)
            : this(operationKind, null, null)
        {
        }

        public IgesBooleanTreeOperation(IgesBooleanTreeOperationKind operationKind, IIgesBooleanTreeItem leftChild, IIgesBooleanTreeItem rightChild)
        {
            OperationKind = operationKind;
            LeftChild = leftChild;
            RightChild = rightChild;
        }
    }
}
