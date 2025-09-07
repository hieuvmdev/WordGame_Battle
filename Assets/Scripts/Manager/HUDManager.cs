using System.Collections;
using System.Collections.Generic;
using EventDispatcher;
using UnityEngine;

public class HUDManager : Singleton<HUDManager>
{
    public MainUI mainUI;
    public GameplayUI gameplayUI;

    private GameManager _gameManager;
    private EffectManager _effectManager;
    private LevelManager _levelManager;

    #region Script Life Cycle
    // Start is called before the first frame update
    private void Awake()
    {
        Initialized();
    }

    #endregion

    #region Public Function

    public bool IsMenuPanelActive()
    {
        return gameplayUI.IsMenuPanelActive();
    }

    public void Reset()
    {
        mainUI.Reset();
        gameplayUI.Reset();

        DebugUtils.LogColor("Reset HUD", "blue");
    }
    #endregion

    #region Private Function
    private void Initialized()
    {
        _effectManager = EffectManager.Instance;
        _levelManager = LevelManager.Instance;

        this.RegisterListener(EventID.StartGame, (o) =>
        {
            mainUI.SetActive(false);
            gameplayUI.SetActive(true);
        });
    }
    #endregion
}
