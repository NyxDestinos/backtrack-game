using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameAudio : MonoBehaviour
{
    public AudioSource sound;
    bool isLoad = false;
    bool isLoadComplete = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!isLoad)
        {
            sound.time = PlayerPrefs.GetInt("IntroLength");
            isLoad = !isLoad;
        }
        if (!isLoadComplete && sound.volume < 1)
        {
            sound.volume += 0.001f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
