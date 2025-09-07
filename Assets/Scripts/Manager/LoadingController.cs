using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{

    [SerializeField] private AudioSource loadingAS;
    [SerializeField] private AudioClip titleAppear;
    [SerializeField] private ParticleSystem loadingAppearEffect;

    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private float loadingTime = 3.0f;

    // Start is called before the first frame update
    private void Start()
    {
        Application.targetFrameRate = 60;

        WordDictionary.Instance.GenDictionary(GameConstants.FILE_PATH_WORD_COMMON);
        LoadingCutScene(() =>
        {
            DOVirtual.DelayedCall(loadingTime, () =>
            {
                SceneManager.LoadScene(1);
            }).Play();
        });
    }

    private void LoadingCutScene(Action loadingGame)
    {
        titleTxt.enabled = true;
        titleTxt.DOFade(0, 0).Play();
        Vector3 defaultScale = titleTxt.transform.localScale;
        titleTxt.transform.localScale = Vector3.zero;

        titleTxt.DOFade(1, 0.2f).OnComplete(() =>
        {
            loadingAppearEffect.Play(true);
            titleTxt.transform.DOScale(defaultScale, 0.3f).SetEase(DG.Tweening.Ease.OutCirc).OnComplete(() =>
            {
                loadingAS.volume = 0.5f;
                loadingAS.PlayOneShot(titleAppear);
                loadingGame.Invoke();
            }).Play();
        }).Play();

    }
}
