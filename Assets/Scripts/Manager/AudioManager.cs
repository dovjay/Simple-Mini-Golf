using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("SFX")]
    public AudioClip pickUpSwoosh;
    public AudioClip hitSwoosh;

    [Header("Music")]
    public AudioClip mainMusic;

    [Header("UI")]
    public AudioClip UIClick;
    public AudioClip UISwitch;

    private AudioSource sfxSource;
    private AudioSource musicSource;

    private void Awake() {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        sfxSource = audioSources[0];
        musicSource = audioSources[1];
    }

    private void Start() {
        PlayMusic(mainMusic);
    }

    public void PlaySFX(AudioClip clip) {
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    public void PlayMusic(AudioClip clip) {
        musicSource.clip = clip;
        musicSource.Play();
    }
}
