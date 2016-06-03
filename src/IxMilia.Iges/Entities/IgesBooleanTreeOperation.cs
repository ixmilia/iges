// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

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
