using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MarchingBytes;
using UnityEngine;
using UnityEngine.UI;

public class KeyWordController : MonoBehaviour
{
    public char KeyChar
    {
        get { return _keyChar; }
    }

    [SerializeField] private Image keyImage;
    [SerializeField] private GameObject underScore;

    private char _keyChar;
    private Vector3 _defaultScale = Vector3.zero;
    private Tween _scaleTween;

    public void ActiveKey()
    {
        EasyObjectPool.instance.GetObjectFromPool(GameConstants.KEY_APPEAR_EFFECT, transform.position,
            Quaternion.identity);
        keyImage.enabled = true;
        transform.localScale = Vector3.zero;
        _scaleTween = transform.DOScale(_defaultScale, 0.5f).Play();
    }

    public void ActiveUnderscore()
    {
        underScore.SetActive(true);
    }

    public void DeactiveUnderScore()
    {
        underScore.SetActive(false);
    }

    public void SetKey(char _key)
    {
        _keyChar = _key;
        keyImage.sprite = KeyFactory.Instance.GetKey(_key).sprite;
    }

    public void Reset()
    {
        _keyChar = ' ';
        keyImage.enabled = false;

        if (_scaleTween != null)
        {
            _scaleTween.Kill();
        }

        if (_defaultScale == Vector3.zero)
        {
            _defaultScale = transform.localScale;
        }
        transform.localScale = _defaultScale;
        DeactiveUnderScore();
    }
}
