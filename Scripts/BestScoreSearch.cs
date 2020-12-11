using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BestScoreSearch : MonoBehaviour
{
    public int personalBest;
    public int currentScore;
    public TextMeshProUGUI currentText;
    public TextMeshProUGUI personalBestText;
    // Start is called before the first frame update
    void Start()
    {
        currentScore = PlayerPrefs.GetInt("GetScore");
        personalBest = PlayerPrefs.GetInt("Personal_Best");

        if (currentScore > personalBest)
        {
            PlayerPrefs.SetInt("Personal_Best", currentScore);
            personalBest = currentScore;
        }

        string currentscoretemp = currentScore.ToString();
        int add0 = 8 - currentscoretemp.Length;
        for (int i = 0; i < add0; i++)
        {
            currentscoretemp = "0" + currentscoretemp;
        }

        string personalscoretemp = personalBest.ToString();
        int add00 = 8 - personalscoretemp.Length;
        for (int j = 0; j < add00; j++)
        {
            personalscoretemp = "0" + personalscoretemp;
        }

        currentText.text = "SCORE\n" + currentscoretemp;
        personalBestText.text = "PERSONAL BEST\n" + personalscoretemp.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
