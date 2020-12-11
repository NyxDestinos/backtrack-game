using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PathCard : MonoBehaviour
{

    /*public static PathCard main;*/
    public Animator anim;

    public TextMeshProUGUI cardText;
    public GameObject archer;
    public GameObject swordman;

    public bool isSelect;

    public AudioSource sound;

    public AudioClip sound_over;
    public AudioClip sound_click;

    // Start is called before the first frame update
    void Start()
    {
        //Invoke("setCardData", 0.01f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseOver()
    {
        sound.clip = sound_over;
        sound.Play();
        anim.SetBool("onOver", true);
    }

    public void OnMouseExit()
    {

        anim.SetBool("onOver", false);
    }

    /*public void setCardData()
    {
        cardType =  deck.draw();

        switch (cardType)
        {
            case 0:
                cardText.text = "Enemy";
                break;
            case 1:
                cardText.text = "Event";
                break;
            case 2:
                cardText.text = "Chest";
                break;
        }
    }*/

    public void pressCard()
    {
        if(/*cardType == 0 && */!isSelect)
        {
            sound.clip = sound_click;
            sound.Play();
            isSelect = true;
            ArrayList grid = new ArrayList();
            for(int i = 0; i < 2 + GameMain.main.numberIncrease; i++)
            {
                int[] temp = new int[] { Random.Range(0,5), Random.Range(0,5)};
                bool isDup = false;
                for(int j = 0; j < grid.Count; j++)
                {
                    int[] test = (int[])grid[j];
                    if (test[0] == temp[0] && test[1] == temp[1])
                    {
                        isDup = true;
                        i--;
                        break;
                    }
                }

                if(!isDup) grid.Add(temp);


                /*if (!grid.Contains(temp)) grid.Add(temp);
                else {
                    i--;
                    continue; 
                }*/
                GameObject enemySpawn = null;
                switch (Random.Range(0, 2))
                {
                    case 0:
                        enemySpawn = archer;
                        break;
                    case 1:
                        enemySpawn = swordman;
                        break;
                }
                GameObject spawnedEnemy = Instantiate(enemySpawn, new Vector3(8.05f, -1.75f, 0), archer.transform.rotation);
                spawnedEnemy.GetComponent<Enemy>().enemyX = temp[0] + 1;
                spawnedEnemy.GetComponent<Enemy>().enemyY = temp[1] + 1;
                /*enemySpawn.transform.position += new Vector3(3.25f * (temp[0] - 3), 1.75f * (temp[1] - 3), temp[1]);*/
            }
            Debug.Log("Spawn Complete");
            GameMain.main.currentState = GameMain.gameState.Battle;
            GameMain.main.battleStart();
            TurnSystem.turnSystemMain.battleStart();

        }
        /*else if (cardType == 1)
        {

        }
        else if (cardType == 2)
        {

        }*/
    }

    public void resetCard()
    {
        isSelect = false;
    }
}
