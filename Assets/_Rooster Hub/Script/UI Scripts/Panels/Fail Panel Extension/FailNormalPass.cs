using RG.Handlers;
using RG.Loader;
using UnityEngine;
using UnityEngine.UI;

public class FailNormalPass : MonoBehaviour,IExtension
{
    public Button RestartButton;

    private void OnEnable()
    {
        RestartButton.onClick.AddListener(RestartGame);
    }
    private void OnDisable()
    {
        RestartButton.onClick.RemoveListener(RestartGame);
    }
    private void RestartGame()
    {
        RoosterHaptic.Selection();
        RoosterSound.PlayButtonSound();
        
        UI_Loop.OnChangeUI?.Invoke(true);
        RoosterEventHandler.OnClickedRestartLevelButton?.Invoke();
    }
    public void RunExtension()
    {
        RestartButton.gameObject.SetActive(true);
    }
}
