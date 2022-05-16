using UnityEngine;

public class WinPanelControl : MonoBehaviour
{
    private IExtension[] extensions=>GetComponents<IExtension>();
    void OnEnable()
    {
        foreach (var exts in extensions)
        {
            exts.RunExtension();
        }
    }
}