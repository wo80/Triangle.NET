using System;

namespace MeshExplorer
{
    public class GenericEventArgs<T> : EventArgs
    {
        T argument;

        public T Argument
        {
            get { return argument; }
        }

        public GenericEventArgs(T arg)
        {
            argument = arg;
        }
    }
}
