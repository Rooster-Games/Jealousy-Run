using RG.Handlers;
using RG.Loader;
using UnityEngine;

public class WinPanelAutoPass : MonoBehaviour,IExtension
{
    public float nextLevelPassTime;

    public void RunExtension()
    {
        Invoke(nameof(AutoPass),nextLevelPassTime);
    }

    void AutoPass()
    {
        UI_Loop.OnChangeUI?.Invoke(true);
        RoosterEventHandler.OnClickedNextLevelButton?.Invoke();
    }
}
