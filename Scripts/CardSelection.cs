using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelection : MonoBehaviour
{
    public ArrayList deck = new ArrayList();
    // Start is called before the first frame update
    void Start()
    {
        startCard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int draw()
    {
        Debug.Log("Work");
        int cardDraw = Random.Range(0, deck.Count);
        int cardGet = (int) deck[cardDraw];
        deck.RemoveAt(cardDraw);
        return cardGet;
    }

    public void startCard()
    {
        for (int i = 0; i< 8; i++)
        {
            deck.Add(0);
        }

        for (int i = 0; i < 12; i++)
        {
            deck.Add(1);
        }
        /*
        for (int i = 0; i < 2; i++)
        {
            deck.Add(2);
        }*/



    }
}
