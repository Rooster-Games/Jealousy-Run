using RG.Handlers;
using RG.Loader;
using RoosterHub;
using UnityEngine;
using UnityEngine.UI;

public class WinPanelManuelPass : MonoBehaviour, IExtension
{
    public Button nextButton;
    public Button rwNextButton;

    private void OnEnable()
    {
        nextButton.interactable = true;
        rwNextButton.interactable = true;
        nextButton.onClick.AddListener(OnClickNextLevelButton);
        rwNextButton.onClick.AddListener(OnClickRewardedButton);
    }

    private void OnDisable()
    {
        nextButton.onClick.RemoveListener(OnClickNextLevelButton);
        rwNextButton.onClick.AddListener(OnClickRewardedButton);
    }

    private void OnClickRewardedButton()
    {
        Haptic.Selection();
        Sound.PlayButtonSound();
        
        rwNextButton.interactable = false;
        nextButton.interactable = false;
    }

    private void OnClickNextLevelButton()
    {
        Haptic.Selection();
        Sound.PlayButtonSound();
        
        RoosterEventHandler.OnUpdateCoinText?.Invoke();

        if (LevelCompleteShow.OnLevelCompleteAnimation!=null)
        {
            LevelCompleteShow.OnLevelCompleteAnimation?.Invoke();
            Invoke(nameof(OnClickAfterButtonDelay),1.3f);
        }
        else
        {
            OnClickAfterButtonDelay();
        }

        nextButton.interactable = false;
        rwNextButton.interactable = false;
    }

    private void OnClickAfterButtonDelay()
    {
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

    private void TransitionTrigger()
    {
        UI_Loop.OnChangeUI?.Invoke(true);
        RoosterEventHandler.OnClickedNextLevelButton?.Invoke();
    }

    public void RunExtension()
    {
        nextButton.gameObject.SetActive(true);
    }
}