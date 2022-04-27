using RoosterHub;
using TMPro;
using UnityEngine;

public class LevelNoWriter : MonoBehaviour
{
    private TextMeshProUGUI _levelText => GetComponent<TextMeshProUGUI>();
    public bool negativeOne;
    public bool withoutLevelText;
    private int localLevelNo;

    private void OnEnable()
    {
        localLevelNo = Central.GetLevelNo();
        if (negativeOne)
        {
            localLevelNo = Central.GetLevelNo() - 1;
        }
        else
        {
            localLevelNo = Central.GetLevelNo();
        }

        if (withoutLevelText)
        {
            _levelText.text = localLevelNo.ToString();
        }
        else
        {
            _levelText.text = "Level " + localLevelNo;
        }
    }
}