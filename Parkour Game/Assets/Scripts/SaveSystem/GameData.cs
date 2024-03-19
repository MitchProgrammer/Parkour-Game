using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // you can only save simple data, ints strings floats and bools and arrays of each of them
    public string[] playerNames = new string[10];

    public int[] kills = new int[10];
    public int[] deaths = new int[10];

    // Game settings
    public float maxRoundTime = 120f;
    public int maxKills = 10;

    public string[] lastPlayerNames = new string[2];
}
