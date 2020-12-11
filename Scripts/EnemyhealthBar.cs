using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyhealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void delayedDestroy()
    {
        Invoke("destroyThis", 1.5f);
    }


    void destroyThis()
    {
        Destroy(gameObject);
    }
}
