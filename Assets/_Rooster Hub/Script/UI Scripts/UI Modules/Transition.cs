using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RG.Loader;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public GameObject panelOne;
    public GameObject panelTwo;

    private void OnEnable()
    {
        panelOne.GetComponent<DOTweenAnimation>().DOPlay();
        panelTwo.GetComponent<DOTweenAnimation>().DOPlay();
    }
}
