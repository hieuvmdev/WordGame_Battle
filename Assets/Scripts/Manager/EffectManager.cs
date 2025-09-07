using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MarchingBytes;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [SerializeField] private ParticleSystem playerHightLightEffect;
    [SerializeField] private ParticleSystem enemyHightLightEffect;
    [SerializeField] private ParticleSystem endGameEffect;

    public void PlayHightLightEffect(TurnBase turn)
    {
        playerHightLightEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        enemyHightLightEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        switch (turn)
        {
            case TurnBase.ENEMY:
                enemyHightLightEffect.Play(true);
                break;
            case TurnBase.PLAYER:
                playerHightLightEffect.Play(true);
                break;
        }
    }

    public void PlayEndGameEffect()
    {
        endGameEffect.Play();
    }

    public void PlayMoveBurstEffectAt(Vector3 position)
    {
        EasyObjectPool.instance.GetObjectFromPool(GameConstants.MOVE_BURST_EFFECT, position, Quaternion.identity).SetActive(true);
    }
}
