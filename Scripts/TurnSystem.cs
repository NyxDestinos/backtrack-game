using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TurnSystem : MonoBehaviour
{
    public GameObject player;
    public static TurnSystem turnSystemMain;
    public TextMeshProUGUI maxTurnText;
    public TextMeshProUGUI currentTurnText;

    public int currentTurn;
    public int maxTurn;
    public int maxCurrentTurn;
    public int enemyCount;

    public bool isPlayerMove;
    public bool isRewind;
    public ArrayList turnOrder = new ArrayList();
    public Image timeBar;
    public float timeBarUnit;
    public PostProcessVolume postProcess;

    public AudioSource sound;

    public Animator rewindAnim;
    public GameObject fade;

    // Start is called before the first frame update
    void Start()
    {
        turnSystemMain = gameObject.GetComponent<TurnSystem>();

    }



    public void battleStart()
    {
        currentTurn = 0;
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        maxTurn = maxTurn + GameMain.main.turnIncrease;
        maxCurrentTurn = 0;
        turnOrder = new ArrayList();
        isPlayerMove = false;
        Invoke("setOrder", 0.1f);
        setTurnCount();
        setMaxTurnText();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.main.health <= 0)
        {
            GameOver();
        }
        if (!TurnSystem.turnSystemMain.isPlayerMove && Input.GetKeyDown("r"))
        {
            isRewind = !isRewind;
            rewindAnim.SetBool("isRewind", isRewind);
            if (isRewind) GameMain.main.descriptionText.text = "Press A to rewind for a step \nPress D to forward for a step\nPress R Again to enter to current time";
            else GameMain.main.descriptionText.text = "";
        }

        if (isRewind)
        {
            if (Input.GetKeyDown("a") && Player.main.energy > Player.main.energyPerRewind)
            {
                rewindCommand();

            }

            if (Input.GetKeyDown("d"))
            {
                timeForwardCommand();
            }
        }

        if (isRewind)
        {
            if (sound.pitch > -0.4f) sound.pitch -= Time.deltaTime * 3f;
        }
        else
        {
            if (sound.pitch < 1f) sound.pitch += Time.deltaTime * 3f;
            else sound.pitch = 1;
        }

    }

    

    public void rewindCommand()
    {
        if (currentTurn > 0)
        {
            turnRewind(1);
            Player.main.energyUse(Player.main.energyPerRewind);
        }
            
        else Debug.LogError("You are at the begining of stage");
    }

    public void timeForwardCommand()
    {
        if (currentTurn < maxCurrentTurn)
        {
            Player.main.energyUse(-Player.main.energyPerRewind);
            turnForward(1);
        } 
        else Debug.LogError("You are at the lasted turn");
    }
    public void nextTurn()
    {
        isPlayerMove = true;
        Invoke("enemyMove",0.1f);
    }

    public void win()
    {
        Debug.Log("You Win");
        GameMain.main.currentState = GameMain.gameState.Result;
        GameMain.main.isPaficist = false;
        GameMain.main.isSongEnd = true;
        GameMain.main.battleEnd();


    }

    public void paficistwin()
    {
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log(enemyList.Length + " : " + enemyCount);
        if (enemyList.Length != enemyCount)
        {
            Debug.Log("Real Natural");
            GameMain.main.isPaficist = false;
            GameMain.main.isSongEnd = false;
        }
        else
        {
            Debug.Log("Real Paficist");
            GameMain.main.isPaficist = true;
            GameMain.main.isSongEnd = false;

        }

        foreach (GameObject enemy in enemyList)
        {
            enemy.GetComponent<Enemy>().GetComponent<Enemy>().isPaficist = true;
            enemy.GetComponent<Enemy>().GetComponent<Enemy>().getDamage(enemy.GetComponent<Enemy>().health);
            enemy.GetComponent<Enemy>().GetComponent<Enemy>().isDeath();
        }

        Debug.Log("Paficist Win");
        GameMain.main.currentState = GameMain.gameState.Result;
        GameMain.main.battleEnd();
    }

    public void enemyMove()
    {
        Debug.Log("In Order : " + turnOrder.Count);
        GameObject enemy = (GameObject)turnOrder[0];
        enemy.GetComponent<Enemy>().getAction();
    }

    public void endTurn()
    {

        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemyList)
        {
            enemy.GetComponent<Enemy>().isDeath();
        }
        StartCoroutine(endTurn2());
    }
     IEnumerator endTurn2() 
    {
        yield return new WaitForSeconds(0.01f);
        if (currentTurn == maxTurn)
        {
            paficistwin();
            
        }
        else if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            win();
        }
        else
        {
            currentTurn++;
            maxCurrentTurn = currentTurn;
            timeBarIncrease();
            isPlayerMove = false;
            setOrder();
            setCurrentTurnText();
            GridDestroy();
        }

    }

    public void setOrder()
    {
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
       int id = 1;
        foreach (GameObject enemy in enemyList)
        {
            turnOrder.Add(enemy);
            enemy.GetComponent<Enemy>().setID(id++);
        }
    }

    public void turnRewind(int amount)
    {
        for (int i =0; i < amount; i++)
        {
            currentTurn--;
            setCurrentTurnText();
            timeBarDecrease();
            GridDestroy();

            GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemyList)
            {
                enemy.GetComponent<Enemy>().Rewind();
            }
        }
        
    }

    public void turnForward(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            currentTurn++;
            setCurrentTurnText();
            timeBarDecrease();
            GridDestroy();

            GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemyList)
            {
                enemy.GetComponent<Enemy>().Rewind();
            }
        }

    }

    public void GridDestroy()
    {
        GameObject[] warnGrid = GameObject.FindGameObjectsWithTag("WarnGrid");

        foreach(GameObject grid in warnGrid)
        {
            grid.GetComponent<WarningGrid>().checkDestroy();
        }
    }

    public void timeBarIncrease()
    {
        //timeBar.rectTransform.position += new Vector3(2.5f,0);
        timeBar.fillAmount = (float)currentTurn / maxTurn;
    }

    public void timeBarDecrease()
    {
        //timeBar.transform.position -= new Vector3(timeBarUnit, 0);
        timeBar.fillAmount = (float)currentTurn / maxTurn;
    }

    public void setTurnCount()
    {
        timeBarUnit = 300f / (float)maxTurn;
        Debug.Log(timeBarUnit);
    }

    public void setMaxTurnText()
    {
        string minute = (maxTurn / 60).ToString();
        string second = (maxTurn % 60).ToString();

        if (second.Length == 1) second = "0" + second;
        
        maxTurnText.text = minute + ":" + second;
    }

    public void setCurrentTurnText()
    {
        string minute = (currentTurn / 60).ToString();
        string second = (currentTurn % 60).ToString();

        if (second.Length == 1) second = "0" + second;

        currentTurnText.text = minute + ":" + second;
    }

    public void GameOver()
    {
        fade.SetActive(true);
        fade.GetComponent<Animator>().SetBool("Fade", true);
        StartCoroutine(setFadeOut());
    }

    public IEnumerator setFadeOut()
    {
        yield return new WaitForSeconds(1.75f);
        PlayerPrefs.SetInt("GetScore", GameMain.main.allScore);
        //fade.SetActive(false);
        SceneManager.LoadScene("GameOver");
    }
}
