using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Animator anim;
    public GameObject fade;
    public bool volumnDown = false;

    public MainMenuSoundController sound;
    public AudioSource buttonSound;

    public AudioClip sound_over;
    public AudioClip sound_click;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(setFadeOut());
    }

    // Update is called once per frame
    void Update()
    {
        if (volumnDown && sound.song.volume > 0) sound.song.volume -= 0.001f;
    }

    public void nextScene()
    {
        buttonSound.clip = sound_click;
        buttonSound.time = 0.1f;
        buttonSound.Play();
        volumnDown = true;
        fade.SetActive(true);
        fade.GetComponent<Animator>().SetBool("Fade", true);
        StartCoroutine(swapScene());
    }

    public void menuScene()
    {
        buttonSound.clip = sound_click;
        buttonSound.time = 0.1f;
        buttonSound.Play();
        volumnDown = true;
        fade.SetActive(true);
        fade.GetComponent<Animator>().SetBool("Fade", true);
        StartCoroutine(MainMenuSwapScene());
    }

    public void OnMouseOver()
    {
        buttonSound.clip = sound_over;
        //buttonSound.time = 0.1f;
        buttonSound.Play();
        //Debug.Log("Active");
        anim.SetBool("onOver", true);
    }

    public void OnMouseExit()
    {
        anim.SetBool("onOver", false);
    }

    public IEnumerator swapScene()
    {
        yield return new WaitForSeconds(1.75f);
        SceneManager.LoadScene("BattleStage");
    }

    public IEnumerator MainMenuSwapScene()
    {
        yield return new WaitForSeconds(1.75f);
        SceneManager.LoadScene("Menu");
    }

    public IEnumerator setFadeOut()
    {
        yield return new WaitForSeconds(1.75f);
        PlayerPrefs.SetInt("IntroLength", (int)sound.song.time);
        fade.SetActive(false);
    }
}
