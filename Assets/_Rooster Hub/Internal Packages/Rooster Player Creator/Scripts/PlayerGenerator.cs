using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerGenerator : MonoBehaviour
{
    public static PlayerGenerator Instance;
    public TextAsset nicknameAsset;
    public List<string> nicknames = new List<string>();
    public List<Sprite> flags = new List<Sprite>();
    private void Awake()
    {
        Instance = this;
        var nicks= nicknameAsset.text.Split('\n');
        nicknames.AddRange(nicks);
    }

    public RoosterPlayer GetPlayer()
    {
        RoosterPlayer player = new RoosterPlayer
        {
            playerName =GetPlayerName(),
            flag = null
        };
        return player;
    }

    public string GetPlayerName()
    {
        return nicknames[Random.Range(0, nicknames.Count)];
    }

    public Sprite GetPlayerFlag()
    {
        return flags[Random.Range(0, flags.Count)];
    }
}

[Serializable]
public class RoosterPlayer
{
    public string playerName;
    public Sprite flag;
}