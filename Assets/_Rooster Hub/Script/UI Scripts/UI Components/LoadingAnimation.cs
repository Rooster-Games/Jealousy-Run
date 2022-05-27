using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{
    public TextMeshProUGUI loadingDots;

    private void OnEnable()
    {
        loadingDots.DOText("...", .3f).SetLoops(-1);

    }
}
