using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstants
{
    public const string FILE_PATH_WORD_COMMON = "10000_common_words";

    public const string MENU = "Menu";
    public const string INIT = "Init";
    public const string PLAYING = "Playing";
    public const string ENDING = "Ending";

    public const string MENU_TO_INIT = "MenuToInit";
    public const string INIT_TO_PLAYING = "InitToPlaying";
    public const string PLAYING_TO_ENDING = "PlayingToEnding";
    public const string PLAYING_TO_INIT = "PlayingToInit";
    public const string ENDING_TO_INIT = "EndingToInit";
    public const string ENDING_TO_PLAYING = "EndingToPlaying";
    public const string PLAYING_TO_MENU = "PlayingToMenu";
    public const string ENDING_TO_MENU = "EndingToMenu";

    public const int AMOUNT_CHAR = 26;
    public const float NEXT_ROUND_TIME = 3.0f;

    public static int MAX_ROUND = 5;
    public static float TIME_AI_DELAY_MOVE = 0f;
    public static float TIME_DELAY_BEFORE_CHANGE_TURN = 1.0f;

    public static float SCALE_MULTIPLY_PRESS_KEY = 2.0f;
    public static Color32 PRESS_CORLOR = Color.red;

    public static string GAME_WIN_TITLE = "YOU WIN!";
    public static string GAME_LOSE_TITLE = "YOU LOSE!";
    public static string GAME_DRAW_TITLE = "DRAW!";

    // Pool Effect Name

    public static string MOVE_BURST_EFFECT = "MoveBurstEffect";
    public static string KEY_APPEAR_EFFECT = "KeyAppearEffect";
}
