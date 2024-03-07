using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

public class GamePlayManager : MonoBehaviour
{
    public PlayerIdentity PI;
    public enum gameState { Intro, Gameplay, End };

    public void Score()
    {
        // Kills
    }

    public void voidCheck()
    {
        // When killed (below x Y level)
        // Respawn(player)
    }

    public void Respawn()
    {
        // Add to kills
        // Respawn
        // Update scores
    }

    public void Progession()
    {
        // Timer
        // When kills == 10 || timer == 0, end the game go to End gameState
    }
}
