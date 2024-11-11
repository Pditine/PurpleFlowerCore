using System.Collections.Generic;

namespace PurpleFlowerCore
{
    public class Tree<T>
    {
        public TreeNode<T> Root;
        
        public Tree(string rootName)
        {
            Root = new TreeNode<T>(new[] {rootName});
        }
    }
    
    public class TreeNode<T>
    {
        public string name;
        public string[] path;
        public T Data;
        public TreeNode<T> Parent;
        public List<TreeNode<T>> Children;
        
        public TreeNode<T> CreateNodeBuPath(string[] path, T data)
        {
            var child = new TreeNode<T>(path);
            AddChild(child);
            path = path[1..];
            if (path.Length == 0)
            {
                child.Data = data;
                return child;
            }
            return child.CreateNodeBuPath(path, data);
        }
        
        public TreeNode(string[] path, T data = default)
        {
            name = path[0];
            Data = data;
            this.path = path.Clone() as string[];
            Children = new List<TreeNode<T>>();
        }
        
        public void AddChild(TreeNode<T> child)
        {
            Children.Add(child);
            child.Parent = this;
        }
        
        public void RemoveChild(TreeNode<T> child)
        {
            Children.Remove(child);
            child.Parent = null;
        }
        
        public TreeNode<T> LeftChild()
        {
            return Children[0];
        }
        
        public TreeNode<T> RightChild()
        {
            return Children[1];
        }
        
        public TreeNode<T> FindChild(string name)
        {
            return Children.Find(child => child.name == name);
        }
        
        public bool IsLeaf()
        {
            return Children.Count == 0;
        }
    }
}