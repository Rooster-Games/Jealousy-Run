using System;
using System.Collections;
using Coffee.UIEffects;
using DG.Tweening;
using RG.Core;
using RoosterHub;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour, ITransition
{
    public Image logo;
    public UITransitionEffect transitionEffect;

    private void Awake()
    {
        logo.sprite = GameSettings.Instance.transitionLogo;
    }

    public void OpenAction()
    {
        //Açılma
        
        LogoMovement(false);
        Crack(false);
        Fade(false);
    }

    public void CloseAction()
    {
        // Kapanma
        logo.enabled = false;
        transitionEffect.gameObject.SetActive(false);

        Fade(true);
    }

    public void Fade(bool status)
    {
        Image bg = GetComponent<Image>();
        if (status)
        {
            RoosterHub.Central.OnTransectionStart?.Invoke();
            
            bg.DOFade(0, 0f);
            bg.DOFade(1, .4f).OnComplete(() => LogoMovement(status));
        }
        else
        {
            bg.DOFade(0, 1f).SetDelay(.5f);//.OnComplete((() => {gameObject.SetActive(false);}));
        }
    }

    public void LogoMovement(bool status)
    {
        if (status)
        {
            logo.enabled = true;
            logo.transform.DOScale(1, .6f).From(4f).SetEase(Ease.InQuart).OnComplete(
                () => Crack(status)
            );
        }
        else
        {
            logo.transform.DOScale(5f, 1f).SetEase(Ease.InQuart).OnComplete(() =>
            {
                Central.OnTransectionComplete?.Invoke();
                gameObject.SetActive(false);
            });
        }
    }

    public void Crack(bool status)
    {
       // yield return new WaitForSeconds(0.0f);

        if (status)
        {
            transitionEffect.gameObject.SetActive(status);
            DOTween.To(()=> transitionEffect.effectFactor, x=> transitionEffect.effectFactor = x, 1f, .4f);
        }
        else
        {
            DOTween.To(()=> transitionEffect.effectFactor, x=> transitionEffect.effectFactor = x, 0f, .4f);
        }
        
    }
}