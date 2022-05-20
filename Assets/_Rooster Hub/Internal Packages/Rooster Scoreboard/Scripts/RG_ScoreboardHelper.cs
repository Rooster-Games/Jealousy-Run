using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using RG.Core;
using RoosterHub;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace RG.Helpers
{
    public class RG_ScoreboardHelper : MonoBehaviour
    {
        public static RG_ScoreboardHelper instance;
        public GameObject playerItemPrefab;

        Transform playerItemCurrentLocation;
        GameObject tempPlayerItem;
        GameObject oldPlayerItem;

        public TextMeshProUGUI leagueNameText;

        public Transform content;

        public List<RG_ScoreboardItem> scoreboardItems = new List<RG_ScoreboardItem>();
        public List<ScoreboarItemContainer> scoreboardItemContainer = new List<ScoreboarItemContainer>();

        public List<LeagueContainer> leagues = new List<LeagueContainer>();

        private void OnDisable()
        {
            CheckLeader();
        }


        public int myScore;
        int myRank;
        ScoreboarItemContainer playerContainer;
        void Awake()
        {
            instance = this;
          
            CreateLeague(leagues[0]);
        }

        private void OnEnable()
        {
            myScore = GamePrefs.GameScore;
            Placement();
            RunScrollAnimation();
        }

        private void CreateLeague(LeagueContainer leagueContainer)
        {
            leagueNameText.text = leagueContainer.leagueName;

            playerContainer = new ScoreboarItemContainer()
            {
                scoreText = myScore,
                playerNameText = "Me",
                rankText = myRank,
                flag = null
            };
            scoreboardItemContainer.Add(playerContainer);

            for (int i = 0; i < leagueContainer.size; i++)
            {
                RoosterPlayer rp = Player.GetRandomPlayer();
                ScoreboarItemContainer itemContainer = new ScoreboarItemContainer()
                {
                    scoreText = GetRandomScore(i, leagueContainer),
                    playerNameText = rp.playerName,
                    rankText = i + 1,
                    flag = rp.flag
                };
                scoreboardItemContainer.Add(itemContainer);
            }

            for (int i = 0; i < scoreboardItems.Count; i++)
            {
                scoreboardItems[i].ChangeVisibilty(true);
            }

            Placement();
            GetNearList();

        }

        int GetRandomScore(int i, LeagueContainer leagueContainer)
        {
            var min = leagues[0].scoreRange.x;
            var max = leagues[0].scoreRange.y;

            if (i <= leagueContainer.size / 3)
            {
                return Mathf.FloorToInt(Random.Range(min, max / 3));
            }
            else if (i > leagueContainer.size / 3 && i <= leagueContainer.size / 1.5f)
            {
                return Mathf.FloorToInt(Random.Range(max / 3, max / 1.5f));
            }
            else if (i > leagueContainer.size / 1.5f)
            {
                return Mathf.FloorToInt(Random.Range(max / 1.5f, max));
            }

            return Mathf.FloorToInt(Random.Range(min, max));

        }

        void Placement()
        {
            playerContainer.rankText = myRank;
            playerContainer.scoreText = myScore;
            playerContainer.flag = Player.GetRandomFlag(); // language flag

            var tempList = new List<ScoreboarItemContainer>(scoreboardItemContainer);

            var itemList = tempList.OrderByDescending(x => x.scoreText).ToList();

            for (var i = 0; i < itemList.Count(); i++)
            {
                if (itemList[i] == playerContainer)
                {
                    myRank = i + 1;
                }
                itemList[i].rankText = i + 1;
            }

            playerItemPrefab.GetComponent<RG_ScoreboardItem>().FillTheScoreboardItem(playerContainer);

            scoreboardItemContainer.Clear();
            scoreboardItemContainer.AddRange(itemList.ToList());

        }

        void CheckLeader()
        {
            if (myRank == 1)
            {
                IncreaseLeague();
            }
        }


        void GetNearList()
        {
            int index = 1;
            if (myRank < 7)
            {
                for (int i = 0; i < scoreboardItems.Count; i++)
                {
                    scoreboardItems[i].FillTheScoreboardItem(scoreboardItemContainer[i]);

                    if (scoreboardItemContainer[i].playerNameText == playerContainer.playerNameText)
                    {
                        tempPlayerItem = scoreboardItems[i].gameObject;
                        playerItemCurrentLocation = scoreboardItems[i].transform;
                    }
                }
            }
            else
            {
                for (int i = scoreboardItems.Count - 1; i >= 0; i--)
                {
                    scoreboardItems[i].FillTheScoreboardItem(scoreboardItemContainer[myRank - index]);

                    if (scoreboardItemContainer[myRank - index].playerNameText == playerContainer.playerNameText)
                    {
                        tempPlayerItem = scoreboardItems[i].gameObject;
                        playerItemCurrentLocation = scoreboardItems[i].transform;
                    }

                    index++;
                }
            }
        }

        void IncreaseLeague()
        {
            /*scoreboardItemContainer.Clear();
            GamePrefs.GameScore = 0;
            GamePrefs.ScoreboardIndex++;*/
            CreateLeague(leagues[0]);
        }

        void RunScrollAnimation()
        {
            oldPlayerItem = tempPlayerItem;
            tempPlayerItem.GetComponent<RG_ScoreboardItem>().ChangeVisibilty(false);
            oldPlayerItem.GetComponent<RG_ScoreboardItem>().ChangeVisibilty(false);

            playerItemPrefab.transform.position = playerItemCurrentLocation.position;


            playerItemPrefab.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.Linear);
            playerItemPrefab.transform.DOScale(1.24f, 0.8f).SetEase(Ease.OutQuad).OnComplete(RollScoreBoard);
        }
        void RollScoreBoard()
        {
            Transform x = content.GetChild(0);
            x.GetComponent<VerticalLayoutGroup>().enabled = false;
            GetNearList();

            oldPlayerItem.GetComponent<RG_ScoreboardItem>().ChangeVisibilty(true);
            tempPlayerItem.GetComponent<RG_ScoreboardItem>().ChangeVisibilty(false);

            x.DOLocalMoveY(200f, .03f).SetEase(Ease.Linear).SetLoops(50, LoopType.Yoyo).OnComplete(() =>
            {
                x.GetComponent<VerticalLayoutGroup>().enabled = true;

                playerItemPrefab.transform.DOMove(playerItemCurrentLocation.position, 1f).OnComplete(() =>
                {
                    playerItemPrefab.transform.DOScale(1f, 0.4f).SetEase(Ease.InQuad);


                });
            });
        }
    }


    [Serializable]
    public class LeagueContainer
    {
        public string leagueName;
        [MinMaxSlider(10, 2000)]
        public Vector2 scoreRange;
        public int size;
    }

    [Serializable]
    public class ScoreboarItemContainer
    {
        [Header("Rank")]
        public Image leaderImage;
        public int rankText;

        [Header("Player Name")]
        public Sprite flag;
        public string playerNameText;

        [Header("Score")]
        public int scoreText;
    }
}
