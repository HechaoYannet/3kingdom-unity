using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; set; }
    //public Dictionary<int, PlayerManager> players;
    public List<Player> players = new List<Player>();


    public Player TEMP_PLAYER;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        TEMP_PLAYER.SetPlayerID(2);//¸Ä
    }

    // Update is called once per frame
    void Update()
    {

    }
    public Player GetPlayerInstanceByID(int id)
    {
        if (id == 0 || id == 1)//0:ÅÆ¶Ñ£¬1:ÆúÅÆ¶Ñ
        {
            return null;
        }
        foreach (Player p in players)
        {
            if (p.PlayerID == id)
            {
                return p;
            }
        }
        return null;
    }
}
