using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class KeyController : MonoBehaviour, IHitable
{
    public char KeyChar
    {
        get { return _keyChar; }
    }

    public KeyState State
    {
        get { return _state; }
        private set
        {
            _state = value;
            switch (_state)
            {
                case KeyState.PRESS:
                    OnPress();
                    break;
            }
        }
    }

    [SerializeField] private Image keyImage;

    private KeyState _state;
    private char _keyChar;

    private Vector3 _defaultScale;
    private Tween _scaleTween;

    private void Awake()
    {
        _defaultScale = transform.localScale;
    }

    public void SetKeyData(Sprite keySprite, char c)
    {
        this._keyChar = c;
        keyImage.sprite = keySprite;
    }

    public char GetHit()
    {
        if (State != KeyState.IDLE)
        {
            return ' ';
        }

        State = KeyState.PRESS;

        return KeyChar;
    }

    public void Reset()
    {
        _state = KeyState.IDLE;

        if (_scaleTween != null)
        {
            _scaleTween.Kill();
        }

        keyImage.color = Color.white;
        transform.localScale = _defaultScale;
    }

    private void OnPress()
    {
        if (_scaleTween != null)
        {
            _scaleTween.Kill();
        }

        transform.localScale = _defaultScale * GameConstants.SCALE_MULTIPLY_PRESS_KEY;
        keyImage.color = GameConstants.PRESS_CORLOR;
        _scaleTween = transform.DOScale(_defaultScale, 0.5f).SetEase(DG.Tweening.Ease.OutCirc).OnComplete(() =>
        {
            if (_state == KeyState.PRESS)
            {
                _state = KeyState.IDLE;
            }
        }).Play();

        DOVirtual.DelayedCall(0.25f, () =>
        {
            if (_state == KeyState.PRESS)
            {
                keyImage.color = Color.white;
            }
        });
    }
}
