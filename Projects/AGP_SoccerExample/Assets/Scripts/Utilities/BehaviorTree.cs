using System;

namespace BehaviorTree
{
    public abstract class Node<T>
    {
        public abstract bool Update(T context);
    }

    public class Tree<T> : Node<T>
    {
        private readonly Node<T> _root;

        public Tree(Node<T> root) {
            _root = root;
        }

        public override bool Update(T context)
        {
            return _root.Update(context);
        }
    }

    public class Do<T> : Node<T>
    {
        public delegate bool NodeAction(T context);

        private readonly NodeAction _action;

        public Do(NodeAction action)
        {
            _action = action;
        }

        public override bool Update(T context)
        {
            return _action(context);
        }
    }

    public class Condition<T> : Node<T>
    {
        private readonly Predicate<T> _condition;

        public Condition(Predicate<T> condition)
        {
            _condition = condition;
        }

        public override bool Update(T context)
        {
            return _condition(context);
        }
    }

    public abstract class CompositeNode<T> : Node<T>
    {
        protected Node<T>[] Children { get; private set; }

        protected CompositeNode(params Node<T>[] children)
        {
            Children = children;
        }
    }
    
    public class Selector<T> : CompositeNode<T>
    {
        public Selector(params Node<T>[] children) : base(children) {}

        public override bool Update(T context)
        {
            foreach (var child in Children)
            {
                if (child.Update(context)) return true;
            }
            return false;
        }
    }

    public class Sequence<T> : CompositeNode<T>
    {
        public Sequence(params Node<T>[] children) : base(children) {}

        public override bool Update(T context)
        {
            foreach (var child in Children)
            {
                if (!child.Update(context)) return false;
            }
            return true;
        }
    }

    public abstract class Decorator<T> : Node<T>
    {
        protected Node<T> Child { get; private set; }

        protected Decorator(Node<T> child)
        {
            Child = child;
        }
    }

    public class Not<T> : Decorator<T>
    {
        public Not(Node<T> child) : base(child) {}

        public override bool Update(T context)
        {
            return !Child.Update(context);
        }
    }
}