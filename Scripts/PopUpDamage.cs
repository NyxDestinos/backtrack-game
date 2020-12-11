using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpDamage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("destroyThis", 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void destroyThis()
    {
        Destroy(gameObject);
    }
}
