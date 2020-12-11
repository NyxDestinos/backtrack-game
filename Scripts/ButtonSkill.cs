using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSkill : MonoBehaviour
{
    public string skillName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseOver()
    {
        if (skillName == "Backward")
        {
            GameMain.main.descriptionText.text = "Return you to you lasted position without take a turn";
        }

    }
    public void OnMouseExit()
    {
            GameMain.main.descriptionText.text = "";
    }
}
