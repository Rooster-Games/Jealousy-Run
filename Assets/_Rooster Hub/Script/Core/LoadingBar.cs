using System;
using DG.Tweening;
using RG.Handlers;
using RG.Loader;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    public Image loadingBar;
    public GameObject loadingText;
    private void Awake()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        RoosterEventHandler.OnLoading += OnBarValueChange;
    }
    private void OnDisable()
    {
        RoosterEventHandler.OnLoading -= OnBarValueChange;
    }

    private void OnBarValueChange()
    {
        loadingBar.DOFillAmount(1, 2.5f).SetUpdate(true).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            //gameObject.SetActive(false);
            loadingText.gameObject.SetActive(false);
            loadingBar.transform.parent.gameObject.SetActive(false);
            
            if (GetComponentInParent<UI_Loop>().useTransition)
            {
                transform.GetChild(0).GetComponent<Transition>().LogoMovement(true);
                Invoke(nameof(CloseTransition),1.4f);
            }
        });
    }

    void CloseTransition()
    {
        transform.GetChild(0).GetComponent<Transition>().LogoMovement(false);
        transform.GetChild(0).GetComponent<Transition>().Crack(false);
        transform.GetChild(0).GetComponent<Transition>().Fade(false);
        Invoke(nameof(CloseLoading),1.4f);
    }

    void CloseLoading()
    {
        gameObject.SetActive(false);
            
    }
}