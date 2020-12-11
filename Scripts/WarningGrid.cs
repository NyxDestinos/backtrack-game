using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningGrid : MonoBehaviour
{
    public int x;
    public int y;
    public int damage;
    public int turnSpawn;
    public float destroyTime;
    // Start is called before the first frame update
    void Start()
    {
        turnSpawn += TurnSystem.turnSystemMain.currentTurn;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void checkDestroy()
    {
        Debug.Log("Checking");
        if (turnSpawn + 2 <= TurnSystem.turnSystemMain.currentTurn || turnSpawn > TurnSystem.turnSystemMain.currentTurn - 1) StartCoroutine(killGrid());
    }

    public void setGridCdr(float time)
    {
        destroyTime = time;
    }

    public void setGrid(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public void setDamage(int _damage)
    {
        damage = _damage;
    }

    public IEnumerator killGrid()
    {
        yield return new WaitForSeconds(destroyTime);
        if (Player.main.playerX == x && Player.main.playerY == y && !TurnSystem.turnSystemMain.isRewind){
            Debug.LogWarning("Player Take Damage");
            Player.main.takeDamage(damage) ;
        }
        Destroy(gameObject);

    }
}
