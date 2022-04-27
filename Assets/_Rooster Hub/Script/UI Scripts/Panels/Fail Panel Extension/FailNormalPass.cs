using RG.Handlers;
using RG.Loader;
using RoosterHub;
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
        Haptic.Selection();
        Sound.PlayButtonSound();

        if (GetComponentInParent<UI_Loop>().useTransition)
        {
            RoosterEventHandler.OnShowTransition?.Invoke(true);
            Invoke(nameof(TransitionTrigger),3f);
        }
        else
        {
            UI_Loop.OnChangeUI?.Invoke(true);
            RoosterEventHandler.OnClickedNextLevelButton?.Invoke();    
        }
    }

    void TransitionTrigger()
    {
        UI_Loop.OnChangeUI?.Invoke(true);
        RoosterEventHandler.OnClickedRestartLevelButton?.Invoke();
    }
    public void RunExtension()
    {
        RestartButton.gameObject.SetActive(true);
    }
}
