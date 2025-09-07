using DG.Tweening;
using EventDispatcher;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private GameObject container;

    [Header("Game Play UI")]
    [SerializeField] private TextMeshProUGUI gameScoreTxt;
    [SerializeField] private Image playerIcon;
    [SerializeField] private Image enemyIcon;


    [Header("Setting UI")]
    [SerializeField] private Image soundButtonIcon;
    [SerializeField] private Image vibratoButtonIcon;

    [Header("End Round UI")]
    [SerializeField] private Transform endRoundContainer;
    [SerializeField] private GameObject blackBackground;
    [SerializeField] private TextMeshProUGUI titleEndRoundTxt;
    [SerializeField] private Color32[] titleColors;

    [Header("End Round UI")]
    [SerializeField] private GameObject endGameContainer;
    [SerializeField] private TextMeshProUGUI titleEndGameTxt;
    [SerializeField] private TextMeshProUGUI playerScoreEndGameTxt;
    [SerializeField] private TextMeshProUGUI enemyScoreEndGameTxt;

    [Header("Menu UI")]
    [SerializeField] private GameObject menuPanelContainer;

    [Header("Sprites")]
    [SerializeField] private Sprite[] soundIcons;
    [SerializeField] private Sprite[] vibratoIcons;

    public void SetActive(bool isActive)
    {
        container.SetActive(isActive);
    }

    public void HomeButtonOnClick()
    {
        this.PostEvent(EventID.BackHome);
    }

    public void ResetButtonOnClick()
    {
        this.PostEvent(EventID.ResetImmediately);
    }

    public bool IsMenuPanelActive()
    {
        return menuPanelContainer.activeInHierarchy;
    }

    public void SwitchSoundStateOnClick()
    {
        SoundManager.Instance.IsActive = !SoundManager.Instance.IsActive;
        soundButtonIcon.sprite = SoundManager.Instance.IsActive ? soundIcons[0] : soundIcons[1];
    }

    public void SwitchVibratoStateOnClick()
    {
        GameManager.Instance.IsVibrato = !GameManager.Instance.IsVibrato;
        vibratoButtonIcon.sprite = GameManager.Instance.IsVibrato ? vibratoIcons[0] : vibratoIcons[1];
    }

    public void MenuButtonOnClick()
    {
        if (LevelManager.Instance.CurrentState != LevelState.Play)
        {
            return;
        }

        menuPanelContainer.SetActive(true);
        blackBackground.SetActive(true);

        SoundManager.Instance.PlayButtonClickSFX();
    }

    public void SetGameScore(int playerScore, int enemyScore)
    {
        gameScoreTxt.text = playerScore + " - " + enemyScore;
    }

    public void ShowEndRound(GameResult result)
    {
        SoundManager.Instance.PlayAtPoint(SoundManager.Instance.endRoundSFX);
        blackBackground.SetActive(true);
        endRoundContainer.gameObject.SetActive(true);

        Vector3 defaultScale = endRoundContainer.localScale;
        endRoundContainer.localScale = Vector3.zero;

        switch (result)
        {
            case GameResult.WIN:
                titleEndRoundTxt.text = GameConstants.GAME_WIN_TITLE;
                titleEndRoundTxt.color = titleColors[0];
                break;
            case GameResult.LOSE:
                titleEndRoundTxt.text = GameConstants.GAME_LOSE_TITLE;
                titleEndRoundTxt.color = titleColors[1];
                break;
            case GameResult.DRAW:
                titleEndRoundTxt.text = GameConstants.GAME_DRAW_TITLE;
                titleEndRoundTxt.color = titleColors[2];
                break;
        }


        endRoundContainer.DOScale(defaultScale, 0.3f).Play();
    }

    public void HideEndRound()
    {
        blackBackground.SetActive(false);
        endRoundContainer.gameObject.SetActive(false);
    }

    public void ShowEndGame(int playerScore, int enemyScore, GameResult result)
    {
        switch (result)
        {
            case GameResult.WIN:
                SoundManager.Instance.PlayAtPoint(SoundManager.Instance.gameWinSFX);

                titleEndGameTxt.text = GameConstants.GAME_WIN_TITLE;
                titleEndGameTxt.color = titleColors[0];
                break;
            case GameResult.LOSE:
                SoundManager.Instance.PlayAtPoint(SoundManager.Instance.gameLoseSFX);

                titleEndGameTxt.text = GameConstants.GAME_LOSE_TITLE;
                titleEndGameTxt.color = titleColors[1];
                break;
            case GameResult.DRAW:
                SoundManager.Instance.PlayAtPoint(SoundManager.Instance.gameLoseSFX);

                titleEndGameTxt.text = GameConstants.GAME_DRAW_TITLE;
                titleEndGameTxt.color = titleColors[2];
                break;
        }

        playerScoreEndGameTxt.text = playerScore.ToString();
        enemyScoreEndGameTxt.text = enemyScore.ToString();

        endGameContainer.SetActive(true);
        blackBackground.SetActive(true);

    }

    public void HideEndGame()
    {
        blackBackground.SetActive(false);
        endGameContainer.SetActive(false);
    }

    public void Reset()
    {
        SetGameScore(0, 0);
        HideEndRound();
        HideEndGame();
        menuPanelContainer.SetActive(false);

        blackBackground.SetActive(false);
    }
}
