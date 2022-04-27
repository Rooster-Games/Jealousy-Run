using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteShow : MonoBehaviour
{
    public static Action OnLevelCompleteAnimation;

    [SerializeField] private TextMeshProUGUI _midText;
    [SerializeField] private TextMeshProUGUI _rightOneText;
    [SerializeField] private TextMeshProUGUI _rightTwoText;
    [SerializeField] private TextMeshProUGUI _rightThreeText;
    [SerializeField] private TextMeshProUGUI _leftOneText;
    [SerializeField] private TextMeshProUGUI _leftTwoText;
    [SerializeField] private TextMeshProUGUI _leftThreeText;

    private void OnEnable()
    {
        OnLevelCompleteAnimation += LevelCompleteAnimation;
        _rightOneText.text = (RoosterHub.Central.GetLevelNo()).ToString();
        _rightTwoText.text = (RoosterHub.Central.GetLevelNo() + 1).ToString();
        _rightThreeText.text = (RoosterHub.Central.GetLevelNo() + 2).ToString();

        _leftOneText.text = (RoosterHub.Central.GetLevelNo() - 2).ToString();
        _leftTwoText.text = (RoosterHub.Central.GetLevelNo() - 3).ToString();
        _leftThreeText.text = (RoosterHub.Central.GetLevelNo() - 4).ToString();
    }

    private void OnDisable()
    {
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
        MovePositions(_leftTwoText.transform.parent, _leftThreeText.transform.parent.position);
        MovePositions(_leftOneText.transform.parent, _leftTwoText.transform.parent.position);
        MovePositions(_midText.transform.parent, _leftOneText.transform.parent.position);

        MovePositions(_rightOneText.transform.parent, _midText.transform.parent.position);
        MovePositions(_rightTwoText.transform.parent, _rightOneText.transform.parent.position);
        MovePositions(_rightThreeText.transform.parent, _rightTwoText.transform.parent.position);


        SetScales(_leftTwoText.transform.parent, Vector3.one * 0.6f);
        SetScales(_leftOneText.transform.parent, Vector3.one * 0.75f);
        SetScales(_midText.transform.parent, Vector3.one * 0.82f);

        SetScales(_rightOneText.transform.parent, Vector3.one * 1f);
        SetScales(_rightTwoText.transform.parent, Vector3.one * 0.82f);
        SetScales(_rightThreeText.transform.parent, Vector3.one * 0.75f);

        SetColor(_midText, new Color32(226, 185, 161,255));
        SetColor(_rightOneText, new Color32(149, 75, 32,255));
        
        SetFade(_rightThreeText, 0.2980392f);
        SetFade(_rightTwoText, 0.5686275f);
        SetFade(_rightOneText, 1);

        SetFade(_midText, 0.5686275f);
        SetFade(_leftOneText, 0.2980392f);
        SetFade(_leftTwoText, 0f);
        
        

        SetFade(_rightThreeText.transform.parent.GetComponent<Image>(), 0.04313726f);
        SetFade(_rightTwoText.transform.parent.GetComponent<Image>(), 0.1294118f);
        SetFade(_rightOneText.transform.parent.GetComponent<Image>(), 1);

        SetFade(_midText.transform.parent.GetComponent<Image>(), 0.1294118f);
        SetFade(_leftOneText.transform.parent.GetComponent<Image>(), 0.04313726f);
        SetFade(_leftTwoText.transform.parent.GetComponent<Image>(), 0f);

     
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