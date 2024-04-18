using System;
using System.Threading.Tasks;

namespace Loading
{
    public interface ILoadingOperation
    {
        //string GetName { get; }
        string Description { get; }

        Task Load(Action<float> onProgress);
    }
}