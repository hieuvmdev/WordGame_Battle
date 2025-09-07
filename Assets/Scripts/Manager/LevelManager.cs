using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EventDispatcher;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public LevelState CurrentState
    {
        get { return _state; }
    }

    [SerializeField] private Difficult difficult;
    [SerializeField] private DifficultConfiguration[] difficultsConfig;
    [SerializeField] private KeyWordController[] _keyWords;
    [SerializeField] private KeyController[] _keysBoard;


    [System.Serializable]
    private struct DifficultConfiguration
    {
        public Difficult difficult;
        public int percentAIHasSmart;
        public int percentAIRandomSmart;
    }

    private GameManager _gameManager;
    private EffectManager _effectManager;
    private HUDManager _hudManager;
    private WordDictionary _wordDictionary;

    private GameState _gameState;
    private LevelState _state;

    private bool _isAIOnCalculate;
    private int _percentAIHasSmartMove;
    private int _percentAIRandomSmart;

    #region Script Life Cycle
    // Start is called before the first frame update
    private void Awake()
    {
        Initialized();
    }

    private void Update()
    {
        
        if (_state == LevelState.Play)
        {
            switch (_gameState.curTurn)
            {
                case TurnBase.PLAYER:
                    InputHandle();
                    break;
                case TurnBase.ENEMY:
                    if (!_hudManager.IsMenuPanelActive())
                    {
                        PlayAIMove();
                    }
                    break;
            }
        }
    }
    #endregion

    #region Public Function
    public void StartLevel()
    {
        _state = LevelState.Play;

        _effectManager.PlayHightLightEffect(_gameState.curTurn);
        _keyWords[0].ActiveUnderscore();
    }

    public void SetDifficult(Difficult dif)
    {
        difficult = dif;

        for (int i = 0; i < difficultsConfig.Length; i++)
        {
            if (difficultsConfig[i].difficult == dif)
            {
                _percentAIHasSmartMove = difficultsConfig[i].percentAIHasSmart;
                _percentAIRandomSmart = difficultsConfig[i].percentAIRandomSmart;
            }
        }
    }

    public void Reset()
    {
        _gameState = new GameState();
        _state = LevelState.Init;
        _effectManager.PlayHightLightEffect(TurnBase.NONE);

        ResetRound();
    }

    #endregion

    #region Private Function
    private void ResetRound()
    {
        for (int i = 0; i < _keyWords.Length; i++)
        {
            _keyWords[i].Reset();
        }

        for (int i = 0; i < _keysBoard.Length; i++)
        {
            _keysBoard[i].Reset();
        }

        _gameState.Reset();
    }

    private void Initialized()
    {
        _hudManager = HUDManager.Instance;
        _effectManager = EffectManager.Instance;
        _gameManager = GameManager.Instance;

        _gameState = new GameState();

        this.RegisterListener(EventID.BackHome, (o) =>
        {
            _effectManager.PlayHightLightEffect(TurnBase.NONE);
            _state = LevelState.End;
        });

        if (!WordDictionary.Instance.IsInitialized)
        {
            WordDictionary.Instance.GenDictionary(GameConstants.FILE_PATH_WORD_COMMON);
        }
        _wordDictionary = WordDictionary.Instance;
        // Init Key Board
        KeyNode[] keys = KeyFactory.Instance.GetAllKeys();

        for (int i = 0; i < _keysBoard.Length; i++)
        {
            _keysBoard[i].SetKeyData(keys[i].sprite, keys[i].key);
        }
    }

    private char RandomKey()
    {
        int randomID = Random.Range(0, _keysBoard.Length);

        return _keysBoard[randomID].KeyChar;
    }
    private void PlayAIMove()
    {
        if (_isAIOnCalculate)
        {
            return;
        }

        _isAIOnCalculate = true;
        char moveKey = ' ';
        if (Random.Range(1, 101) <= _percentAIHasSmartMove)
        {
            List<char> keysRight = new List<char>();
            WordNode curNode = _wordDictionary.GetWord(_gameState.curWord.ToString());

            for (int i = 0; i < curNode.GetAmountOfChildren(); i++)
            {
                if (curNode.childrenNodes[i] != null)
                {
                    keysRight.Add(_wordDictionary.KeyNumberToChar(i));
                }
            }

            int amountKeyRandom = keysRight.Count + Mathf.RoundToInt((keysRight.Count * ((float) (100 - _percentAIRandomSmart) / 100)));

            amountKeyRandom = Mathf.Clamp(amountKeyRandom, 1, _keysBoard.Length);

            for (int i = 0; i < _keysBoard.Length; i++)
            {
                if (keysRight.Count == amountKeyRandom)
                {
                    break;
                }

                if (!keysRight.Contains(_keysBoard[i].KeyChar))
                {
                    keysRight.Add(_keysBoard[i].KeyChar);
                }
            }

            moveKey = keysRight[Random.Range(0, keysRight.Count)];

            if (moveKey == ' ')
            {
                DebugUtils.LogError("Cannot find best move");
                moveKey = RandomKey();
            }
        }
        else
        {
            moveKey = RandomKey();
        }

        DOVirtual.DelayedCall(GameConstants.TIME_AI_DELAY_MOVE, () =>
        {
            FillKey(moveKey);
            _isAIOnCalculate = false;
        }).Play();
    }

    private void InputHandle()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, transform.position.z));

            RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);
            if (hit.transform != null && hit.transform.CompareTag("Key"))
            {

                _effectManager.PlayMoveBurstEffectAt(hit.transform.position);

                char keyChar = hit.transform.GetComponent<IHitable>().GetHit();
                FillKey(keyChar);
            }
        }
    }

    private void FillKey(char keyChar)
    {
        if (keyChar != ' ')
        {
            DebugUtils.Log("Fill Key: " + keyChar);

            SoundManager.Instance.PlayAtPoint(SoundManager.Instance.moveSuccessSFX);
            AppendKeyToCurWord(keyChar);
            CheckTurn(keyChar);
        }
        else
        {
            SoundManager.Instance.PlayAtPoint(SoundManager.Instance.moveFailedSFX);
            DebugUtils.Log("Error Move: " + keyChar);
        }
    }

    private void AppendKeyToCurWord(char key)
    {
        _gameState.curWord.Append(key);
        _keyWords[_gameState.curWord.Length - 1].SetKey(key);
        _keyWords[_gameState.curWord.Length - 1].ActiveKey();
        _keyWords[_gameState.curWord.Length - 1].DeactiveUnderScore();
        _keyWords[_gameState.curWord.Length].ActiveUnderscore();
    }

    private void CheckTurn(char key)
    {
        GameResult result = CheckEndGame();

        if (result != GameResult.NONE)
        {
            EndGameHandle(result);
        }
        else
        {
            NextTurn();
        }
    }

    /// <summary>
    /// Checking amount of key of current word to check end game.
    /// If value 0, this game draw
    /// </summary>
    /// <returns> int number is amount of key that can fill</returns>
    private GameResult CheckEndGame()
    {
        GameResult result = GameResult.NONE;

        WordNode _wordNode = _wordDictionary.GetWord(_gameState.curWord.ToString());

        if (_wordNode == null)
        {
            result = _gameState.curTurn == TurnBase.PLAYER ? GameResult.LOSE : GameResult.WIN;
        }
        else
        {
            DebugUtils.Log("Amount Key: " + _wordNode.GetAmountOfChildren());

            if (_wordNode.GetAmountOfChildren() == 0)
            {
                result = GameResult.DRAW;
            }
        }
      
        return result;
    }

    private void NextTurn()
    {
        _effectManager.PlayHightLightEffect(TurnBase.NONE);

        TurnBase nextTurn = TurnBase.NONE;
        if (_gameState.curTurn == TurnBase.ENEMY)
        {
            nextTurn = TurnBase.PLAYER;
        }
        else
        {
            nextTurn = TurnBase.ENEMY;
        }

        _gameState.curTurn = TurnBase.NONE;
        DOVirtual.DelayedCall(GameConstants.TIME_DELAY_BEFORE_CHANGE_TURN, () =>
        {
            if (_state == LevelState.Play)
            {
                _gameState.curTurn = nextTurn;
                _effectManager.PlayHightLightEffect(_gameState.curTurn);
                DebugUtils.Log("Next Turn: " + _gameState.curTurn.ToString());
            }
        }).Play();
    }

    private void EndGameHandle(GameResult result)
    {
        _state = LevelState.End;

        switch (result)
        {
            case GameResult.WIN:
                _gameState.playerScore++;
                break;
            case GameResult.LOSE:
                _gameState.enemyScore++;
                break;
            default:
                break;
        }

        _hudManager.gameplayUI.SetGameScore(_gameState.playerScore, _gameState.enemyScore);

        if (_gameState.curRound == GameConstants.MAX_ROUND)
        {
            // End Game
            this.PostEvent(EventID.EndGame);

            GameResult finalResult;

            if (_gameState.playerScore > _gameState.enemyScore)
            {
                finalResult = GameResult.WIN;
            }
            else if (_gameState.playerScore < _gameState.enemyScore)
            {
                finalResult = GameResult.LOSE;
            }
            else
            {
                finalResult = GameResult.DRAW;
            }

            DOVirtual.DelayedCall(0.1f, () =>
            {
                _effectManager.PlayEndGameEffect();
            }).Play();
            _hudManager.gameplayUI.ShowEndGame(_gameState.playerScore, _gameState.enemyScore, finalResult);
        }
        else
        {
            _hudManager.gameplayUI.ShowEndRound(result);
            DOVirtual.DelayedCall(GameConstants.NEXT_ROUND_TIME, () =>
            {
                _hudManager.gameplayUI.HideEndRound();
                NextRound();
            });
        }
    }

    private void NextRound()
    {
        _gameState.curRound++;
        ResetRound();

        _gameState.curTurn = _gameState.curFirstTurn == TurnBase.PLAYER ? TurnBase.ENEMY : TurnBase.PLAYER;
        _gameState.curFirstTurn = _gameState.curTurn;

        StartLevel();

       
    }

    private void CheckHotkeys()
    {

    }


    #endregion

}
