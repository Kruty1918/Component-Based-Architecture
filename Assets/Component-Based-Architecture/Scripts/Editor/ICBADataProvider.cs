using System.Collections.Generic;

namespace SGS29.Editor
{
    public interface ICBADataProvider<T> where T : List<ControllerNode>
    {
        T Load();
        void Save(T data);
    }
}