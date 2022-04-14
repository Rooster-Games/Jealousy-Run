using UnityEngine;

public class WinPanelControl : MonoBehaviour
{
    private IExtension passType=>GetComponent<IExtension>();
    void OnEnable()
    {
        passType.RunExtension();
    }
}