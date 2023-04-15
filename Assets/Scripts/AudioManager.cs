using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMonoBehaviour<AudioManager> {

    [SerializeField] AudioSource theme;
    [SerializeField] AudioSource tap;
    [SerializeField] AudioSource move;
    [SerializeField] AudioSource hit;
    [SerializeField] AudioSource avalanche;
    [SerializeField] AudioSource yeti;
    [SerializeField] AudioSource gameOver;
    [SerializeField] AudioSource pop;
    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    public void PlayTheme() {
        if(Controller.Instance.Music) theme.Play();
    }
    public void StopTheme() {
        theme.Stop();
    }
    public void PlayTap() {
        if(Controller.Instance.SFX)tap.Play();
    }
    public void PlayMove() {
        if (Controller.Instance.SFX) move.Play();
    }
    public void PlayHit() {
        if (Controller.Instance.SFX) hit.Play();
    }
    public void PlayAvalanche() {
        if (Controller.Instance.SFX) avalanche.Play();
    }
    public void PlayYeti() {
        if (Controller.Instance.SFX) hit.Play();
        if (Controller.Instance.SFX) yeti.Play();
    }
    public void PlayGameOver() {
        if (Controller.Instance.SFX) gameOver.Play();
    }
    public void PlayPop() {
        if (Controller.Instance.SFX) pop.Play();
    }
}
