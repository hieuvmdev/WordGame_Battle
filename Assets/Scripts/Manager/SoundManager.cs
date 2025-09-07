using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : Singleton<SoundManager>
{
    public bool IsActive
    {
        get { return _isActive; }
        set
        {
            _isActive = value;
            if (_isActive)
            {
                PlayBackgroundMusic();
                
            }
            else
            {
                StopBackgroundMusic();
            }
        }
    }

    [SerializeField] private AudioSource _audioSourceBackground;
    [SerializeField] private AudioSource audioSourcePrefab;
    [SerializeField] private Transform audioSourceContainer;

    [Header("Configuration")]
    [SerializeField] private float maxBackgroundVolume = 0.3f;

    [Header("Background")]
    public AudioClip backgroundMusic;

    [Header("UI")]
    public AudioClip buttonClickSFX;

    [Header("Gameplay")]
    public AudioClip moveSuccessSFX;
    public AudioClip moveFailedSFX;
    public AudioClip endRoundSFX;
    public AudioClip gameWinSFX;
    public AudioClip gameLoseSFX;

    private readonly Queue<AudioSource> audioSources = new Queue<AudioSource>();
    private readonly LinkedList<AudioSource> inuse = new LinkedList<AudioSource>();
    private readonly Queue<LinkedListNode<AudioSource>> nodePool = new Queue<LinkedListNode<AudioSource>>();

    private bool _isActive = true;
    private int _lastCheckFrame = -1;

    private void Start()
    {
        AudioSettings.OnAudioConfigurationChanged += (value) =>
        {
            
        };
    }

    public void PlayBackgroundMusic()
    {
        _audioSourceBackground.volume = maxBackgroundVolume;
        _audioSourceBackground.clip = backgroundMusic;
        _audioSourceBackground.loop = true;
        _audioSourceBackground.Play();
    }

    public void StopBackgroundMusic()
    {
        _audioSourceBackground.Stop();
    }

    public void PlayButtonClickSFX()
    {
        PlayAtPoint(buttonClickSFX);
    }

    public void PlayAtPoint(AudioClip clip, float Delay = 0)
    {

        if (!IsActive)
        {
            return;
        }

        AudioSource source;

        if (_lastCheckFrame != Time.frameCount)
        {
            _lastCheckFrame = Time.frameCount;
            CheckInUse();
        }

        if (audioSources.Count == 0)
            source = GameObject.Instantiate(audioSourcePrefab, audioSourceContainer);
        else
            source = audioSources.Dequeue();

        if (nodePool.Count == 0)
            inuse.AddLast(source);
        else
        {
            var node = nodePool.Dequeue();
            node.Value = source;
            inuse.AddLast(node);
        }

        source.transform.position = Vector3.zero;
        source.clip = clip;
        source.volume = 1.0f;
        source.PlayDelayed(Delay);
    }

    private void CheckInUse()
    {
        var node = inuse.First;
        while (node != null)
        {
            var current = node;
            node = node.Next;

            if (!current.Value.isPlaying)
            {
                audioSources.Enqueue(current.Value);
                inuse.Remove(current);
                nodePool.Enqueue(current);
            }
        }
    }

    
}
