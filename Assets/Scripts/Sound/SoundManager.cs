using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }


    [SerializeField] private AudioSource soundEffect;
    [SerializeField] private AudioSource soundMusic;
    [SerializeField] private SoundType[] Sounds;
    bool IsMute = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        PlayBGM(global::Sounds.BGM);
    }

    public void Mute(bool status)
    {
        IsMute = status;
    }

    public void PlayBGM(Sounds soundType)
    {
        if (IsMute)
            return;
        AudioClip clip = GetSoundClip(soundType);
        soundMusic.clip = clip;

        if (clip != null)
        {

            soundMusic.Play();
        }
        else
        {
            Debug.Log("Clip not found for sound type " + soundType);
        }
    }

    public void Play(Sounds soundType)
    {
        if (IsMute)
            return;
        AudioClip clip = GetSoundClip(soundType);

        if (clip != null)
        {
            soundEffect.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("Clip not found for sound type " + soundType);
        }
    }

    private AudioClip GetSoundClip(Sounds soundType)
    {
        SoundType sound = Array.Find(Sounds, i => i.soundType == soundType);
        if (sound != null)
        {
            return sound.soundClip;
        }
        else
        {
            return null;
        }
    }
}

[Serializable]
public class SoundType
{
    public Sounds soundType;
    public AudioClip soundClip;
}
public enum Sounds
{
    ButtonClick,
    CarCrash,
    BGM,
    Park,
    LevelComplete
}