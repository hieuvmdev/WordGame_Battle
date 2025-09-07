using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EventDispatcher;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : Singleton<GameManager>
{
    public string CurrentState
    {
        get { return _stateMachine.CurrentState.name; }
    }

    public bool IsVibrato
    {
        get { return _isVibrato; }
        set { _isVibrato = value; }
    }

    [HideInInspector] public StateMachine _stateMachine;

    private HUDManager _hudManager;
    private EffectManager _effectManager;
    private LevelManager _levelManager;

    private bool _isVibrato = true;

    #region Sciprt Life Cycle
    private void Awake()
    {
        Initialized();
    }

    private void Start()
    {
        SoundManager.Instance.PlayBackgroundMusic();

        _stateMachine.SetCurrentState(GameConstants.MENU);
        OnMenuIn();
    }

    private void Update()
    {
        _stateMachine.Update();

    }
    #endregion

    #region Private Function
    private void Initialized()
    {
        DOTween.Init();
        _hudManager = HUDManager.Instance;
        _effectManager = EffectManager.Instance;
        _levelManager = LevelManager.Instance;

        _stateMachine = new StateMachine();

        _stateMachine.CreateState(GameConstants.MENU, OnMenu);
        _stateMachine.CreateState(GameConstants.INIT, OnInit);
        _stateMachine.CreateState(GameConstants.PLAYING, OnPlaying);
        _stateMachine.CreateState(GameConstants.ENDING, OnEnding);

        _stateMachine.CreateTransition(GameConstants.MENU, GameConstants.INIT, GameConstants.MENU_TO_INIT, OnInitIn);
        _stateMachine.CreateTransition(GameConstants.INIT, GameConstants.PLAYING, GameConstants.INIT_TO_PLAYING, OnPlayingIn);
        _stateMachine.CreateTransition(GameConstants.PLAYING, GameConstants.ENDING, GameConstants.PLAYING_TO_ENDING, OnEndingIn);
        _stateMachine.CreateTransition(GameConstants.PLAYING, GameConstants.INIT, GameConstants.PLAYING_TO_INIT, OnInitIn);
        _stateMachine.CreateTransition(GameConstants.ENDING, GameConstants.INIT, GameConstants.ENDING_TO_INIT, OnInitIn);
        _stateMachine.CreateTransition(GameConstants.ENDING, GameConstants.PLAYING, GameConstants.ENDING_TO_PLAYING, OnPlayingIn);
        _stateMachine.CreateTransition(GameConstants.ENDING, GameConstants.MENU, GameConstants.ENDING_TO_MENU, OnMenuIn);
        _stateMachine.CreateTransition(GameConstants.PLAYING, GameConstants.MENU, GameConstants.PLAYING_TO_MENU, OnMenuIn);

        this.RegisterListener(EventID.StartGame, o => StartGame());

        this.RegisterListener(EventID.BackHome, (o) =>
        {
            DebugUtils.Log("Post Event: BackHome");
            if (CurrentState == GameConstants.PLAYING)
            {
                _stateMachine.ProcessTriggerEvent(GameConstants.PLAYING_TO_MENU);
            }
            else
            {
                _stateMachine.ProcessTriggerEvent(GameConstants.ENDING_TO_MENU);
            }

        });
        this.RegisterListener(EventID.ResetImmediately, (o) =>
        {
            if (CurrentState == GameConstants.PLAYING)
            {
                _stateMachine.ProcessTriggerEvent(GameConstants.PLAYING_TO_INIT);
            }
            else
            {
                _stateMachine.ProcessTriggerEvent(GameConstants.ENDING_TO_INIT);
            }
        });

        this.RegisterListener(EventID.EndGame, (o) =>
        {
            _stateMachine.ProcessTriggerEvent(GameConstants.PLAYING_TO_ENDING);
        });
    }

    private void StartGame()
    {
        _hudManager.mainUI.SetActive(false);
        _hudManager.gameplayUI.SetActive(true);

        _stateMachine.ProcessTriggerEvent(GameConstants.MENU_TO_INIT);
    }
    #endregion

    #region State Machine Life Cycle

    private void OnMenuIn()
    {
        _hudManager.mainUI.SetActive(true);
        _hudManager.gameplayUI.SetActive(false);
    }

    private void OnMenu()
    {

    }

    private void OnInitIn()
    {
        DebugUtils.LogColor("InitIn", "blue");

        _levelManager.Reset();
        _hudManager.Reset();


        // Waiting for Reset Game
        Run.After(0.1f, () =>
        {
            _stateMachine.ProcessTriggerEvent(GameConstants.INIT_TO_PLAYING);
        });
    }

    private void OnInit()
    {

    }


    private void OnPlayingIn()
    {
        DebugUtils.LogColor("Playing", "blue");
        _levelManager.StartLevel();
    }

    private void OnPlaying()
    {

    }

    private void OnEndingIn()
    {
        DebugUtils.LogColor("Ending", "blue");
    }

    private void OnEnding()
    {

    }

    private void CheckHotKeys()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            PlayerPrefs.DeleteAll();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
    }

    #endregion

}
