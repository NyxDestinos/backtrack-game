using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMain : MonoBehaviour
{
    public GameObject player;
    public static Camera main;
    // Start is called before the first frame update
    void Start()
    {
        main = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!TurnSystem.turnSystemMain.isPlayerMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, player.transform.position.y,transform.position.z) , Time.deltaTime * 20);
            if (this.GetComponent<Camera>().orthographicSize > 8) this.GetComponent<Camera>().orthographicSize -= Time.deltaTime * 5;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 0, -10), Time.deltaTime * 20);
            if (this.GetComponent<Camera>().orthographicSize < 12) this.GetComponent<Camera>().orthographicSize += Time.deltaTime * 5;
        }*/
    }
}
