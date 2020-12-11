using JetBrains.Annotations;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Stat")]
    public int enemyID;
    public int health = 30;
    public int attack = 15;

    public int killPoint;

    [Header("Position")]
    public int enemyX = 3;
    public int enemyY = 3;
    public int[] enemyPosition = new int[2];
    public Vector3 startPosition;
    public ArrayList moveTrack = new ArrayList();

    [Header("Counter")]
    public int id;
    public int enemyStatus; // 0 = Roaming | 1 = Attacking
    public int lastTurnMove = -1;
    public int attackChargeCooldown = 0;
    public int roamTurnCount = 0;
    public int roamRerun = 0;
    public bool isPaficist = false;

    [Header("GameObject")]
    public GameObject warningGrid;
    public Transform tfDamagePopup;
    public Transform tfSkillPopup;
    public GameObject arrowPref;
    public GameObject arrowRainPref;
    public GameObject healthBar;
    AudioSource audioSource;

    [Header("Skill")]

    public int skillSelect = 0;
    public int[] skillDuration;

    [Header("Animation")]
    public Animator anim;

    [Header("TurnSystem")]
    public bool isEndturn;

    [Header("Audio")]
    public AudioClip sound_damaged;
    public AudioClip sound_arrowShoot;
    public AudioClip sound_skillSelect;
    public AudioClip sound_death;

    // Start is called before the first frame update
    void Start()
    {
        health = (health * GameMain.main.enemyHPpercentage)/100;
        attack = (attack * GameMain.main.enemyattackPercentage) / 100;
        enemyPosition = new int[] { enemyX, enemyY };
        startPosition = transform.position;
        move();
        int[] track = new int[] { enemyStatus, enemyX, enemyY, attackChargeCooldown, roamTurnCount, skillSelect };
        moveTrack.Add(track);
        audioSource = gameObject.GetComponent<AudioSource>();


    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePosition(), Time.deltaTime * 20);

    }



    public void getAction()
    {


        //if attack case
        if (enemyStatus == 1)
        {
            if (attackChargeCooldown > 1)
            {
                attackChargeCooldown--;
            }
            else
            {
                attackChargeCooldown = 0;
                Skill(skillSelect);
                enemyStatus = 0;
            }
        }
        else if (enemyStatus == 0)
        {
            if (roamTurnCount < 3) Roaming();
            else if (roamTurnCount < 7)
            {
                int chance = Random.Range(0, 100);
                if (chance < 40) Attack();
                else Roaming();
            }
            else Attack();
        }

        if (attackChargeCooldown == 1) 
        {
            showWarningGrid(skillSelect, false);
        }
        StartCoroutine(endTurn(0.25f));
    }

    void Roaming()
    {

        int randomMove = Random.Range(0, 4);
        int newX = 0;
        int newY = 0;


        switch (randomMove)
            {
                case 0:
                    if (enemyX < 5) newX += 1;
                    else newX -= 1;
                    break;
                case 1:
                    if (enemyX > 1) newX -= 1;
                    else newX += 1;
                    break;
                case 2:
                    if (enemyY < 5) newY += 1;
                    else newY -= 1;
                    break;
                case 3:
                    if (enemyY > 1) newY -= 1;
                    else newY += 1;
                    break;
            }
        int[] testLocate = new int[] { enemyX + newX, enemyY + newY};
        if (EnemyGrid.main.gridUsed[enemyX + newX-1, enemyY + newY-1] == 1 && roamRerun < 5)
        {
            roamRerun++;
            Invoke("Roaming", 0.001f);
            return;
        }
            
        else
        {
            roamRerun = 0;
            EnemyGrid.main.gridUsed[enemyX-1, enemyY-1] = 0;
            enemyX += newX;
            enemyY += newY;
            enemyPosition[0] = enemyX;
            enemyPosition[1] = enemyY;
            EnemyGrid.main.gridUsed[enemyX-1, enemyY-1] = 1;
            roamTurnCount++;
        }
        
    }

    void Attack()
    {
        enemyStatus = 1;
        skillSelect = Random.Range(0, skillDuration.Length);
        attackChargeCooldown = getSkillCooldown(skillSelect);
        castSkill();
        roamTurnCount = 0;
    }

    public virtual void Skill(int index)
    {
        Debug.Log("Use Skill");
        if (enemyID == 0)
        {
            if (skillSelect == 0)
            {
                GameObject arrow = Instantiate(arrowPref, transform.position + new Vector3(-1f, 0, -10f), arrowPref.transform.rotation);
                Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
                rb.AddForce(new Vector2(-200f, 0), ForceMode2D.Impulse);
                audioSource.clip = sound_arrowShoot;
                audioSource.Play();
            }
            if (skillSelect == 1)
            {
                audioSource.clip = sound_arrowShoot;
                audioSource.Play();
            }

        }

        else if (enemyID == 1)
        {
            if (skillSelect == 0)
            {
                audioSource.clip = sound_arrowShoot;
                audioSource.Play();
            }
            if (skillSelect == 1)
            {
                audioSource.clip = sound_arrowShoot;
                audioSource.Play();
            }
        }
 
    }

    int getSkillCooldown(int index)
    {
        return skillDuration[index];
    }

    public virtual void showWarningGrid(int index, bool isRewind)
    {
        if (enemyID == 0)
        {
            //Long Shot
            if (index == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    StartCoroutine(createWarningGrid(i, enemyY, i, 0, isRewind, 0.03f * (i + 1)));
                }
            }


            //Arrow Rain
            if (index == 1)
            {
                for (int i = 0; i < 4; i++)
                {
                    int x = Random.Range(1, 5);
                    int y = Random.Range(1, 5);
                    StartCoroutine(createWarningGrid(x, y, x, y - enemyY, isRewind, 0.03f * (i + 1)));
                }
            }
        }

        else if (enemyID == 1)
        {
                //Slash
                if (index == 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        StartCoroutine(createWarningGrid(5 - enemyX, i + 1, 5 - enemyX, i - enemyY + 1, isRewind, 0.03f * (i + 1)));
                    }
                }

                //Cross Slash
                if (index == 1)
                {
                    int[] xCross = new int[] { 1, 5, 2, 4, 3, 2, 4, 1, 5 };
                    int[] yCross = new int[] { 5, 5, 4, 4, 3, 2, 2, 1, 1 };
                    for (int i = 0; i < 9; i++)
                    {
                        int x = xCross[i] - 1;
                        int y = yCross[i];
                        StartCoroutine(createWarningGrid(x, y, x, y - enemyY, isRewind, 0.02f * (i + 1)));
                    }
                }

        }
        

    }

    private IEnumerator createWarningGrid(int x, int y,int realX, int realY, bool isRewind,float time)
    {
        yield return new WaitForSeconds(time);

        GameObject grid = Instantiate(warningGrid, transform.position + new Vector3((-(enemyX) - realX) * 3.25f - 0.05f, realY * 1.75f + 0.25f), warningGrid.transform.rotation);
        grid.GetComponent<WarningGrid>().setGridCdr(time);
        grid.GetComponent<WarningGrid>().setGrid(5 - x, y );
        grid.GetComponent<WarningGrid>().setDamage(attack);
        if (isRewind) 
        {
            Debug.Log("Rewind Grid");
            --grid.GetComponent<WarningGrid>().turnSpawn;
        }

    }

    private Vector3 movePosition()
    {
        return startPosition + new Vector3(3.25f * (enemyX - 3), 1.75f * (enemyY - 3), enemyY);

    }

    public void castSkill()
    {
        string _skillName = "";

        if(enemyID == 0)
        {
            switch (skillSelect)
            {
                case 0:
                    _skillName = "Arrow Shot";
                    break;
                case 1:
                    _skillName = "Arrow Rain";
                    break;
            }
        }

        if (enemyID == 1)
        {
            switch (skillSelect)
            {
                case 0:
                    _skillName = "Sweep";
                    break;
                case 1:
                    _skillName = "Cross Slash";
                    break;
            }
        }

        GameObject skillDes = Instantiate(tfSkillPopup.gameObject, transform.position + new Vector3(1f, 3f), tfSkillPopup.rotation);
        skillDes.GetComponent<TextMeshPro>().text = _skillName;

        audioSource.clip = sound_skillSelect;
        audioSource.Play();
    }

    public void getDamage(int damage)
    {
        health -= damage;
        GameObject pop = Instantiate(tfDamagePopup.gameObject, transform.position + new Vector3(1f,3f), tfDamagePopup.rotation);
        pop.GetComponent<TextMeshPro>().text = damage.ToString();
        anim.SetBool("isTakeDamage", true);
        Invoke("damageAnimDone", 0.25f);
        Debug.Log("TakeDamage");
        audioSource.clip = sound_damaged;
        audioSource.Play();
        if(health <= 0)
        {
            audioSource.clip = sound_death;
            audioSource.Play();
        }
        
    }

    public void isDeath()
    {
        if (health <= 0) 
        {
            if (!isPaficist) GameMain.main.killScore += killPoint;
            Destroy(gameObject); 
        }
    }
    public void damageAnimDone()
    {
        anim.SetBool("isTakeDamage", false);
    }

    public void setID(int ID)
    {
        id = ID;
    }

    public IEnumerator endTurn(float time)
    {
        yield return new WaitForSeconds(time);
        int[] track = new int[] { enemyStatus, enemyX, enemyY ,attackChargeCooldown, roamTurnCount, skillSelect};
        Debug.Log(TurnSystem.turnSystemMain.currentTurn);
        if (moveTrack.Count == TurnSystem.turnSystemMain.currentTurn + 1) moveTrack.Add(track);
        else moveTrack[TurnSystem.turnSystemMain.currentTurn + 1] = track;
        TurnSystem.turnSystemMain.turnOrder.RemoveAt(0);
        if (TurnSystem.turnSystemMain.turnOrder.Count == 0)
        {
            TurnSystem.turnSystemMain.endTurn();



        }
        else TurnSystem.turnSystemMain.enemyMove();

    }

    public void Rewind()
    {
        //{ enemyStatus, enemyX, enemyY ,attackChargeCooldown, roamTurnCount, skillSelect}
        int[] track = (int[] )moveTrack[TurnSystem.turnSystemMain.currentTurn];
        enemyStatus = track[0];
        EnemyGrid.main.gridUsed[enemyX - 1, enemyY - 1] = 0;
        enemyX = track[1];
        enemyY = track[2];
        EnemyGrid.main.gridUsed[enemyX - 1, enemyY - 1] = 1;
        attackChargeCooldown = track[3];
        roamTurnCount = track[4];
        skillSelect = track[5];
        if (attackChargeCooldown == 1) showWarningGrid(skillSelect, true);
    }

    private void move()
    {
        transform.position = startPosition + new Vector3(3.25f * (enemyX - 3), 1.75f * (enemyY - 3), enemyY);
    }

}
