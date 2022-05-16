using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace RG.Helpers
{
    public class RG_ScoreboardItem : MonoBehaviour
    {
        [Header("Rank")]
        public Image leaderImage;
        public TextMeshProUGUI rankText;

        [Header("Player Name")]
        public Image flag;
        public TextMeshProUGUI playerNameText;

        [Header("Score")]
        public TextMeshProUGUI scoreText;

        public void FillTheScoreboardItem(ScoreboarItemContainer container)
        {

            if (container.rankText == 1)
            {
                rankText.gameObject.SetActive(false);
                leaderImage.gameObject.SetActive(true);
            }
            else
            {
                rankText.gameObject.SetActive(true);
                leaderImage.gameObject.SetActive(false);
            }

            playerNameText.text = container.playerNameText;
            scoreText.text = container.scoreText.ToString();
            rankText.text = container.rankText.ToString();
            flag.sprite = container.flag;

        }

        public void ChangeVisibilty(bool status)
        {
            flag.gameObject.SetActive(status);
            rankText.gameObject.SetActive(status);
            playerNameText.gameObject.SetActive(status);
            scoreText.gameObject.SetActive(status);
            GetComponent<Image>().enabled = status;
        }
    }
}
