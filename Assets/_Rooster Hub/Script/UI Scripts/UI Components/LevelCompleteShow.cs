using System;
using System.Collections.Generic;
using DG.Tweening;
using RoosterHub;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LevelCompleteShow : MonoBehaviour
{
    public static Action OnLevelCompleteAnimation;

    public List<TextMeshProUGUI> textsList = new List<TextMeshProUGUI>();

    private List<Vector3> posList = new List<Vector3>();
    private List<Vector3> scaleList = new List<Vector3>();
    private List<float> fadeLists = new List<float>();
    private List<float> textFadeLists = new List<float>();
    private List<Color> textColorLists = new List<Color>();

    private void OnEnable()
    {
       
        OnLevelCompleteAnimation += LevelCompleteAnimation;
        textsList[4].text = (Central.GetLevelNo()).ToString();
        textsList[5].text = (Central.GetLevelNo() + 1).ToString();
        textsList[6].text = (Central.GetLevelNo() + 2).ToString();

        textsList[2].text = (Central.GetLevelNo() - 2).ToString();
        textsList[1].text = (Central.GetLevelNo() - 3).ToString();
        textsList[0].text = (Central.GetLevelNo() - 4).ToString();


        if (Central.GetLevelNo() - 2 < 1)
        {
            textsList[2].text = " ";
        }

        if (Central.GetLevelNo() - 3 < 1)
        {
            textsList[1].text = " ";
        }
    }

    void CollectDefault()
    {
        foreach (var t in textsList)
        {
            posList.Add(t.transform.parent.position);
            scaleList.Add(t.transform.parent.localScale);
            fadeLists.Add(t.transform.parent.GetComponent<Image>().color.a);
            textFadeLists.Add(t.color.a);
            textColorLists.Add(t.color);
        }
    }

    void SetDefault()
    {
        for (int i = 0; i < textsList.Count; i++)
        {
            textsList[i].transform.parent.position = posList[i];
            textsList[i].transform.parent.localScale = scaleList[i];
            textsList[i].transform.parent.GetComponent<Image>().DOFade(fadeLists[i], 0.1f).SetEase(Ease.Linear);
            textsList[i].DOFade(textFadeLists[i], 0.1f).SetEase(Ease.Linear);
            textsList[i].color = textColorLists[i];
        }
    }


    private void OnDisable()
    {
        SetDefault();
        ClearLists();
        OnLevelCompleteAnimation -= LevelCompleteAnimation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            OnLevelCompleteAnimation?.Invoke();
        }
    }

    private void LevelCompleteAnimation()
    {
        CollectDefault();

        for (var i = 1; i < textsList.Count; i++)
        {
            MovePositions(textsList[i].transform.parent, posList[i - 1]);
            SetScales(textsList[i].transform.parent, scaleList[i - 1]);
            SetFade(textsList[i].transform.parent.GetComponent<Image>(), fadeLists[i - 1]);
            SetFade(textsList[i], textFadeLists[i - 1]);
            SetColor(textsList[i], textColorLists[i - 1]);
        }

        
    }

    void ClearLists()
    {
        for (int i = 0; i < textsList.Count; i++)
        {
            posList.Clear();
            scaleList.Clear();
            fadeLists.Clear();
            textFadeLists.Clear();
            textColorLists.Clear();
        }
    }
    private void MovePositions(Transform trns, Vector3 destination)
    {
        trns.DOMove(destination, 1f);
    }

    private void SetScales(Transform trns, Vector3 destination)
    {
        trns.DOScale(destination, 1f);
    }

    private void SetFade(Image img, float val)
    {
        img.DOFade(val, 1f);
    }


    private void SetFade(TextMeshProUGUI img, float val)
    {
        img.DOFade(val, 1f);
    }

    private void SetColor(TextMeshProUGUI baseColor, Color targetColor)
    {
        baseColor.DOColor(targetColor, 1f);
    }
}