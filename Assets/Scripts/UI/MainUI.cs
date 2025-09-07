using System;
using System.Collections;
using System.Collections.Generic;
using EventDispatcher;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    [SerializeField] private GameObject container;

    public void SetActive(bool isActive)
    {
        container.SetActive(isActive);
    }

    public void SetDifficultModeOnClick(int dif)
    {
        if (dif >= Utils.GetEnumLength<Difficult>())
        {
            dif = 0;
        }

        LevelManager.Instance.SetDifficult((Difficult) dif);

        // Start game immediately
        this.PostEvent(EventID.StartGame);
    }

    public void Reset()
    {
        
    }
}
