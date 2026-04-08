using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public abstract string cardName { get; protected set; }
    public abstract Sprite cardImage { get; protected set; }
    public abstract string description { get; protected set; }

    public abstract string CardType { get; protected set; }
    /*
     * Type:
     * Sha
     * Shan
     * Tao
     */

    public int OwnerID { get; protected set; }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetOwnerByID(int ownerID)
    {
        OwnerID = ownerID;
    }
    public virtual void DoCardsAction(Player user, Player target)
    {
        //可响应，声明协程，并管理usedCards
        //CardManager.instance.RemoveCard(this);
    }
    public virtual void ResponseTrigger()
    {
        //协程Trigger
    }
    public virtual bool IsCardAvailable()
    {
        return true;
    }
    public virtual bool IsCardResponsible(string cardType)
    {
        return false;
    }
    public virtual bool IsCardResponsible()
    {
        return false;
    }

}

