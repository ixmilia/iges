using System;
using System.Collections.Generic;

namespace IxMilia.Iges.Entities
{
    public class IgesBooleanTree : IgesEntity
    {
        public override IgesEntityType EntityType { get { return IgesEntityType.BooleanTree; } }

        public IIgesBooleanTreeItem RootNode { get; set; }

        public IgesBooleanTree()
            : this(null)
        {
        }

        public IgesBooleanTree(IIgesBooleanTreeItem rootNode)
        {
            EntityUseFlag = IgesEntityUseFlag.Geometry;
            RootNode = rootNode;
        }

        internal override int ReadParameters(List<string> parameters, IgesReaderBinder binder)
        {
            var index = 0;
            var stack = new Stack<object>();
            var parameterCount = Integer(parameters, index++);
            for (int i = 0; i < parameterCount; i++)
            {
                var value = Integer(parameters, index++);
                if (value < 0)
                {
                    // negative index
                    stack.Push(-value);
                }
                else if (value > 0)
                {
                    // operation
                    var node = new IgesBooleanTreeOperation((IgesBooleanTreeOperationKind)value);
                    var rightItem = stack.Pop();
                    var leftItem = stack.Pop();

                    if (rightItem is int)
                    {
                        binder.BindEntity((int)rightItem, e => node.RightChild = new IgesBooleanTreeEntity(e));
                    }
                    else if (rightItem is IIgesBooleanTreeItem)
                    {
                        node.RightChild = (IIgesBooleanTreeItem)rightItem;
                    }
                    else
                    {
                        throw new InvalidOperationException("Unexpected item on stack: " + rightItem.GetType().Name);
                    }

                    if (leftItem is int)
                    {
                        binder.BindEntity((int)leftItem, e => node.LeftChild = new IgesBooleanTreeEntity(e));
                    }
                    else if (leftItem is IIgesBooleanTreeItem)
                    {
                        node.LeftChild = (IIgesBooleanTreeItem)leftItem;
                    }
                    else
                    {
                        throw new InvalidOperationException("Unexpected item on stack: " + leftItem.GetType().Name);
                    }

                    stack.Push(node);
                }
            }

            RootNode = stack.Count == 0
                ? null
                : (IIgesBooleanTreeItem)stack.Pop();

            return index;
        }

        internal override IEnumerable<IgesEntity> GetReferencedEntities()
        {
            return RootNode == null
                ? new IgesEntity[0]
                : GetReferencedEntities(RootNode);
        }

        private IEnumerable<IgesEntity> GetReferencedEntities(IIgesBooleanTreeItem node)
        {
            if (node.IsEntity)
            {
                yield return ((IgesBooleanTreeEntity)node).Entity;
            }
            else
            {
                var operation = (IgesBooleanTreeOperation)node;
                foreach (var entity in GetReferencedEntities(operation.LeftChild))
                {
                    yield return entity;
                }

                foreach (var entity in GetReferencedEntities(operation.RightChild))
                {
                    yield return entity;
                }
            }
        }

        internal override void WriteParameters(List<object> parameters, IgesWriterBinder binder)
        {
            if (RootNode == null)
            {
                parameters.Add(0);
            }
            else
            {
                parameters.Add(GetItemCount(RootNode));
                WriteParameters(parameters, binder, RootNode);
            }
        }

        private int GetItemCount(IIgesBooleanTreeItem node)
        {
            if (node.IsEntity)
            {
                return 1;
            }
            else
            {
                var operation = (IgesBooleanTreeOperation)node;
                return GetItemCount(operation.LeftChild)
                    + GetItemCount(operation.RightChild)
                    + 1;
            }
        }

        private void WriteParameters(List<object> parameters, IgesWriterBinder binder, IIgesBooleanTreeItem node)
        {
            if (node.IsEntity)
            {
                parameters.Add(-binder.GetEntityId(((IgesBooleanTreeEntity)node).Entity));
            }
            else
            {
                var operation = (IgesBooleanTreeOperation)node;
                WriteParameters(parameters, binder, operation.LeftChild);
                WriteParameters(parameters, binder, operation.RightChild);
                parameters.Add((int)operation.OperationKind);
            }
        }
    }
}
