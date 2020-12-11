using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BestScoreMain : MonoBehaviour
{
    public int personalBest;
    public TextMeshProUGUI personalBestText;
    // Start is called before the first frame update
    void Start()
    {
        personalBest = PlayerPrefs.GetInt("Personal_Best");

        string personalscoretemp = personalBest.ToString();
        int add00 = 8 - personalscoretemp.Length;
        for (int j = 0; j < add00; j++)
        {
            personalscoretemp = "0" + personalscoretemp;
        }

        personalBestText.text = "PERSONAL BEST\n" + personalscoretemp.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            PlayerPrefs.SetInt("Personal_Best", 0);
            personalBest = PlayerPrefs.GetInt("Personal_Best");

            string personalscoretemp = personalBest.ToString();
            int add00 = 8 - personalscoretemp.Length;
            for (int j = 0; j < add00; j++)
            {
                personalscoretemp = "0" + personalscoretemp;
            }

            personalBestText.text = "PERSONAL BEST\n" + personalscoretemp.ToString();
        }
    }
}
