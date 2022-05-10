
using UnityEngine;

namespace GameCores
{
    public interface ICompRoot
    {
        void RegisterToContainer();
    }

    public interface ICompRoot<T>
    {
        void RegisterToContainer(T registerParameters);
    }

    public abstract class BaseCompRootGO : MonoBehaviour, ICompRoot
    {
        public abstract void RegisterToContainer();
    }
}