using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : MonoBehaviour
{

    public CardRarity rarity;
    public string templateID;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

public enum CardRarity
{
    Common, Uncommon, Rare, Epic, Legendary
}