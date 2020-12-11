using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrid : MonoBehaviour
{
    public static EnemyGrid main;

    public int[,] gridUsed = new int[5,5];
    // Start is called before the first frame update
    void Start()
    {
        main = gameObject.GetComponent<EnemyGrid>();
        for (int i = 0; i < 5; i++)
        {
            for(int j =0; j<5; j++)
            {
                gridUsed[i, j] = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addGrid(int x, int y)
    {
        gridUsed[x, y] = 1;
    }

    public void resetGrid()
    {
        gridUsed = new int[5, 5];
    }
}
