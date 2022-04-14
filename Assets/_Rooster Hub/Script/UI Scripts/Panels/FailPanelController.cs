using UnityEngine;

public class FailPanelController : MonoBehaviour
{
    private IExtension passType=>GetComponent<IExtension>();
    void OnEnable()
    {
        passType.RunExtension();
    }
}
