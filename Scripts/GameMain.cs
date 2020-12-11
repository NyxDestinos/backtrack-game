using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMain : MonoBehaviour
{
    public static GameMain main;
    public GameObject fade;
    public Animator pathAnim;
    public Animator resultAnim;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI resultRawScore;
    public TextMeshProUGUI finalScoreSongText;
    public TextMeshProUGUI CurrentScoreText;

    public TextMeshProUGUI descriptionText;

    public TextMeshProUGUI StageText;


    [Header("Result")]

    public int stageCount;
    public int allScore = 0;
    public int allScoreThisSong = 0;
    public bool isSongEnd = false;
    public bool isPaficist = false;
    public bool waitForResult = false;

    public bool isPlayerAttack = false;
    public bool isPlayerDamage = false;

    [Header("Scoring")]
    public int paficistEndScore = 1000;
    public int genocideEndScore = 500;
    public int naturalEndScore = 300;

    public int noDamageTakenScore = 500;

    public int killScore = 0;



    [Header("EnemyMutate")]
    public int enemyHPpercentage = 100;
    public int enemyattackPercentage = 100;
    public int turnIncrease = 0;
    public int numberIncrease = 0;

    [Header("Bonus")]
    public int paficistBonus;
    public int genocideBonus;
    public int naturalBonus;


    public enum gameState { Path, Battle, Event, Relic, Result };

    public gameState currentState;
    // Start is called before the first frame update
    void Start()
    {
        main = gameObject.GetComponent<GameMain>();
        currentState = gameState.Path;
        StartCoroutine(setFadeOut());
        
    }

    // Update is called once per frame
    void Update()
    {
        if (waitForResult && currentState.Equals(gameState.Result) && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            currentState = gameState.Path;
            resultAnim.SetBool("ResultEnd", true);
            pathAnim.SetBool("ResultEnd", true);
            waitForResult = false;
        }
            
    }

    public void battleStart()
    {
        Player.main.heal(15);
        Player.main.energyUse(-15);
        StageText.text = "Stage " + (stageCount + 1).ToString();
        isPlayerDamage = false;
        isPlayerAttack = false;
        pathAnim.SetBool("SelectBattle",true);
        pathAnim.SetBool("Battling", true);
        pathAnim.SetBool("ResultEnd", false);
        Invoke("battleStarted", 0.1f);
    }

    public void battleStarted()
    {
        pathAnim.SetBool("SelectBattle", false);
    }

    public void battleEnd()
    {
        Debug.Log("Battle End Step");
        stageCount++;
        enemyRandomUpgrade();
        foreach (GameObject card in GameObject.FindGameObjectsWithTag("Card"))
        {
            card.GetComponent<PathCard>().resetCard();
        }

        foreach (GameObject grid in GameObject.FindGameObjectsWithTag("WarnGrid"))
        {
            Destroy(grid);
        }
        EnemyGrid.main.resetGrid();
        pathAnim.SetBool("Battling", false);
        resultAnim.gameObject.SetActive(true);
        resultAnim.SetBool("battleEnd", true);
        resultAnim.SetBool("ResultEnd", false);
        setResultText();
        Invoke("battleEnded", 0.5f);
        Invoke("resultWait", 1f);

    }

    public void battleEnded()
    {
        resultAnim.SetBool("battleEnd", false);
        pathAnim.SetBool("ResultEnd", false);
    }

    public void resultWait()
    {
        waitForResult = true;
    }

    public void setResultText()
    {
        
        resultText.text = "";

        if (!isSongEnd)
        {
            if (isPaficist && !isPlayerAttack)
            {
                Debug.Log("Attack ? : " + isPlayerAttack);
                resultText.text += "Paficist Victory";
                allScoreThisSong += paficistEndScore;
                resultRawScore.text = paficistEndScore.ToString();

                resultText.text += "\nPaficist Bunus (" + (numberIncrease + 2) + ")";
                allScoreThisSong += (numberIncrease + 2) * 125;
                resultRawScore.text += "\n" + ((numberIncrease + 2) * 100).ToString();
            }
            else
            {
                resultText.text += "Natural Victory";
                allScoreThisSong += naturalEndScore;
                resultRawScore.text = naturalEndScore.ToString();
            }

        }

        else
        {
            resultText.text += "Genocide Victory";
            allScoreThisSong += genocideEndScore;
            resultRawScore.text = genocideEndScore.ToString();
        }

        if (killScore != 0)
        {
            resultText.text += "\nKill Point";
            allScoreThisSong += killScore;
            resultRawScore.text += "\n" + killScore.ToString();
        }

        if (!isPlayerDamage)
        {
            resultText.text += "\nNo Damage Taken";
            allScoreThisSong += 400;
            resultRawScore.text += "\n" + 400.ToString();
        }

        finalScoreSongText.text = allScoreThisSong.ToString();
        allScore += allScoreThisSong;

        string allscoretemp = allScore.ToString();
        int add0 = 8 - allscoretemp.Length;
        for (int i = 0; i < add0; i++)
        {
            allscoretemp = "0" + allscoretemp;
        }

        allScoreThisSong = 0;
        killScore = 0;
        CurrentScoreText.text = "Score\n" + allscoretemp;



    }
    
    public IEnumerator setFadeOut()
    {
        yield return new WaitForSeconds(1.75f);
        fade.SetActive(false);
    }

    public void enemyRandomUpgrade()
    {
        if ((stageCount + 1)%3 == 0 && numberIncrease < 15)
        {
            numberIncrease++;
        }

        if ((stageCount + 1)% 2 == 0 && turnIncrease < 150)
        {
            turnIncrease += Random.Range(2,5);
        }

        else
        {
            int enemyMutate = Random.Range(0, 3);
            switch (enemyMutate)
            {
                case 0:
                    enemyHPpercentage += 10;
                    break;
                case 1:
                    enemyattackPercentage += 10;
                    break;
            }
        

        }
    }
}
