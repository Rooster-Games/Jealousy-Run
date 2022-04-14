
using UnityEngine;

namespace RG.Loader
{
    public class LoadMainScene : MonoBehaviour, ILoader
    {
        public void Load()
        {
            ILooper levelLoader = GetComponent<ILooper>();
            levelLoader.LoadMainMenu();
        }
    }
}