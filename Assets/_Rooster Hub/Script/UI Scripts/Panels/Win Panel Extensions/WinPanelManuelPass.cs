using RG.Handlers;
using RG.Loader;
using UnityEngine;
using UnityEngine.UI;

public class WinPanelManuelPass : MonoBehaviour, IExtension
{
    public Button nextButton;
    public Button rwNextButton;

    private void OnEnable()
    {
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
        RoosterHaptic.Selection();
        RoosterSound.PlayButtonSound();
    }

    private void OnClickNextLevelButton()
    {
        RoosterHaptic.Selection();
        RoosterSound.PlayButtonSound();
        
        RoosterEventHandler.OnUpdateCoinText?.Invoke();
        UI_Loop.OnChangeUI?.Invoke(true);
        
        RoosterEventHandler.OnClickedNextLevelButton?.Invoke();
    }


    public void RunExtension()
    {
        nextButton.gameObject.SetActive(true);
    }
}