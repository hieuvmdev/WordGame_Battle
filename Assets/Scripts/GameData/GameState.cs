using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameState
{
    public int playerScore;
    public int enemyScore;
    public int curRound;

    public TurnBase curTurn;
    public TurnBase curFirstTurn;

    public StringBuilder curWord;

    public GameState()
    {
        curRound = 1;
        playerScore = 0;
        enemyScore = 0;
        curTurn = Random.Range(0, 2) == 0 ? TurnBase.PLAYER : TurnBase.ENEMY; // Roll start
        curFirstTurn = curTurn;
       
    }

    public void Reset()
    {
        curWord = new StringBuilder();
    }
}