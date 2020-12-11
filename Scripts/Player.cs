using JetBrains.Annotations;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player main;
    [Header("Stat")]
    public int maxHealth = 100;
    public int maxEnergy = 100;
    public int health = 100;
    public int energy = 100;
    public int attack = 10;
    
    public int energyPerRewind = 15;

    [Header("Position")]
    public int playerX = 3;
    public int playerY = 3;
    public int[] playerPosition = new int[2];
    public ArrayList playerTrack = new ArrayList();
    public Vector3 startPosition;

    [Header("GameObject")]
    public GameObject trackObj;
    public GameObject attackObj;
    public GameObject skillList;
    public GameObject arrowPref;
    public Image healthBar;
    public Image energyBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI energyText;
    public GameObject tfDamagePopup;
    public Animator anim;
    

    [Header("TurnSystem")]
    public bool isEndturn;
    public bool isSkillTrigger;

    [Header("Skill")]
    public string skill1Name;
    public string skill2Name;
    public string skill3Name;
    public int skill1Cost;
    public int skill2Cost;
    public int skill3Cost;
    public TextMeshProUGUI skill1Text;
    public TextMeshProUGUI skill2Text;
    public TextMeshProUGUI skill3Text;

    [Header("SkillCounter")]
    public ArrayList ShootTracker = new ArrayList();


    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip sound_damaged;
    public AudioClip sound_arrowShoot;





    // Start is called before the first frame update
    void Start()
    {
        main = gameObject.GetComponent<Player>();
        playerPosition = new int[] { playerX, playerY };
        startPosition = transform.position;
        fakeTakeDamage();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameMain.main.currentState == GameMain.gameState.Battle)
        {

            if (!TurnSystem.turnSystemMain.isPlayerMove && !TurnSystem.turnSystemMain.isRewind)
            {
                if (Input.GetKeyDown("w") && playerY < 5)
                {
                    MoveAction(0, 1);
                }

                if (Input.GetKeyDown("s") && playerY > 1)
                {
                    MoveAction(0, -1);
                }

                if (Input.GetKeyDown("a") && playerX > 1)
                {
                    MoveAction(-1, 0);
                }

                if (Input.GetKeyDown("d") && playerX < 5)
                {
                    MoveAction(1, 0);
                }

                if (Input.GetKeyDown("e"))
                {
                    skillSetTrigger();
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Attack();
                    GameMain.main.isPlayerAttack = true;
                    Debug.Log(GameMain.main.isPlayerAttack);
                }

            }

            transform.position = Vector3.MoveTowards(transform.position, movePosition(), Time.deltaTime * 20);
        }
        

    }

    private void Attack()
    {
        //GameObject attack = Instantiate(attackObj, transform.position + new Vector3(3.25f * attackRange , 0), attackObj.transform.rotation);
        GameObject arrow = Instantiate(arrowPref, transform.position + new Vector3(-1f, 2f, -10f), arrowPref.transform.rotation);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(200f, 0), ForceMode2D.Impulse);
        audioSource.clip = sound_arrowShoot;
        audioSource.Play();

        GameObject[] allEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in allEnemy)
        {
            if (playerY == enemy.GetComponent<Enemy>().enemyY)
            {
                enemy.GetComponent<Enemy>().getDamage(attack);
            }
        }
        TurnSystem.turnSystemMain.nextTurn();
    }

    public void useSkill(int i)
    {
        if (!TurnSystem.turnSystemMain.isPlayerMove && !TurnSystem.turnSystemMain.isRewind)
        {
            string skillUse = "";
            switch (i)
            {
                case 0:
                    skillUse = skill1Name;
                    break;
                case 1:
                    skillUse = skill2Name;
                    break;
                case 2:
                    skillUse = skill3Name;
                    break;
            }

            switch (skillUse)
            {
                case "Backward":
                    int[] track;
                    if (playerTrack.Count > 3)
                        track = (int[])playerTrack[playerTrack.Count - 4];
                    else track = (int[])playerTrack[playerTrack.Count - 1];
                    playerX = track[0];
                    playerY = track[1];
                    playerTrack.Add(new int[] { playerX, playerY });
                    transform.position = startPosition + new Vector3(3.25f * (track[0] - 3), 1.75f * (track[1] - 3), 0);

                    break;
                default:
                    Debug.LogError("Can't find skill");
                    break;
            }
            skillSetTrigger();

            //TurnSystem.turnSystemMain.nextTurn();
        }



    }

    public void skillSetTrigger()
    {
        isSkillTrigger = !isSkillTrigger;
        skillList.SetActive(isSkillTrigger);

        if(skill1Name == "")
        {
            skill1Text.gameObject.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            skill1Text.gameObject.transform.parent.gameObject.SetActive(true);
            skill1Text.gameObject.transform.parent.gameObject.GetComponent<ButtonSkill>().skillName = skill1Name;
            skill1Text.text = skill1Name;
        }

        if (skill2Name == "")
        {
            skill2Text.gameObject.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            skill2Text.gameObject.transform.parent.gameObject.SetActive(true);
            skill2Text.gameObject.transform.parent.gameObject.GetComponent<ButtonSkill>().skillName = skill2Name;
            skill2Text.text = skill2Name;
        }

        if (skill3Name == "")
        {
            skill3Text.gameObject.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            skill3Text.gameObject.transform.parent.gameObject.SetActive(true);
            skill3Text.gameObject.transform.parent.gameObject.GetComponent<ButtonSkill>().skillName = skill3Name;
            skill3Text.text = skill3Name;
        }
    }

    private void MoveAction(int x, int y)
    {
        isSkillTrigger = false;
        skillList.SetActive(isSkillTrigger);

        playerX += x;
        playerY += y;
        playerTrack.Add(new int[] { playerX, playerY });
        movePosition();
        TurnSystem.turnSystemMain.nextTurn();
    }

    public int[] getPlayerPosition()
    {
        return playerPosition;
    }

    public Vector3 movePosition()
    {
        return startPosition + new Vector3(3.25f * (playerX - 3), 1.75f * (playerY - 3), 0);
    }

    public void fakeTakeDamage()
    {
        healthText.text = health.ToString();
        energyText.text = energy.ToString();
    }
    public void takeDamage(int damage)
    {
        GameMain.main.isPlayerDamage = true;
        health -= damage;
        healthText.text = health.ToString();
        healthBar.fillAmount = (float)health / (float)maxHealth;
        GameObject pop = Instantiate(tfDamagePopup, transform.position + new Vector3(1f, 3f), tfDamagePopup.transform.rotation);
        pop.GetComponent<TextMeshPro>().text = damage.ToString();
        anim.SetBool("isTakeDamage", true);
        Invoke("damageAnimDone", 0.25f);
        Debug.Log("TakeDamage");
        audioSource.clip = sound_damaged;
        audioSource.Play();
    }

    public void energyUse(int cost)
    {
        energy -= cost;
        if (energy > maxEnergy)
        {
            energy = maxEnergy;
        }
        energyText.text = energy.ToString();
        energyBar.fillAmount = (float)energy / (float)maxEnergy;
        healthText.text = health.ToString();
        healthBar.fillAmount = (float)health / (float)maxHealth;
    }

    public void heal(int value)
    {
        health += value;
        if (health > maxHealth) health = maxHealth;
    }

    public void damageAnimDone()
    {
        anim.SetBool("isTakeDamage", false);
    }
}
