using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSoundController : MonoBehaviour
{

    public AudioSource song;
    public TextMeshProUGUI maxLengthTime;
    public TextMeshProUGUI currentTimeText;
    public float songLength;
    public float timebarUnit;
    public Image timeBar;
    public Image timeBarRemain;


    // Start is called before the first frame update
    void Start()
    {
        songLength = song.clip.length;
        //timeBar.transform.position = new Vector3(timeBar.transform.position.x, 40);
        setMaxTurnText();
        setTimeUnit();
        //InvokeRepeating("setCurrentTurnText",0f,1f);
    }

    // Update is called once per frame
    void Update()
    {
        string minute = ((int)(song.time / 60)).ToString();
        string second = ((int)(song.time % 60)).ToString();

        timeBarIncrease();

        if (second.Length == 1) second = "0" + second;

        currentTimeText.text = minute + ":" + second;
    }

    public void setMaxTurnText()
    {
        string minute = ((int)(songLength / 60)).ToString();
        string second = ((int)(songLength % 60)).ToString();



        if (second.Length == 1) second = "0" + second;

        maxLengthTime.text = minute + ":" + second;
    }

    public void setTimeUnit()
    {
        timebarUnit = 300f / (float)songLength;
        Debug.Log(timebarUnit);
    }

    public void timeBarIncrease()
    {
        timeBar.transform.position += new Vector3(timebarUnit, 0);
        timeBarRemain.fillAmount = song.time/songLength;
    }

    public void setCurrentTurnText()
    {
        string minute = ((int)(song.time / 60)).ToString();
        string second = ((int)(song.time % 60)).ToString();

        timeBarIncrease();

        if (second.Length == 1) second = "0" + second;

        currentTimeText.text = minute + ":" + second;
    }
}
