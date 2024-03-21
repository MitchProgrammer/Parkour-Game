using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoardManager : MonoBehaviour
{
    public List<PlayerScoreCard> scoreCards = new List<PlayerScoreCard>(10);

    private void OnEnable()
    {
        GameMaster.instance.tempPlayers = GameMaster.instance.SortTempList(GameMaster.instance.tempPlayers);

        for (int i = 0; i < scoreCards.Count; i++)
        {
            scoreCards[i].playerName.text = GameMaster.instance.tempPlayers[i].playerName;
            scoreCards[i].kills.text = GameMaster.instance.tempPlayers[i].kills.ToString();
            scoreCards[i].deaths.text = GameMaster.instance.tempPlayers[i].deaths.ToString();
            scoreCards[i].KDR.text = GameMaster.instance.tempPlayers[i].kdr.ToString();
        }
    }
}