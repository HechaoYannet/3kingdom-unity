using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuangGai : Role
{
    void Awake()
    {
        roleName = "黄盖";
        HPmax = 4;
        HP = HPmax;
        roleFaction = RoleFaction.Wu;
        skills = new List<Skill>()
        {
            new("苦肉计","出牌阶段，你可以失去1点体力，令一名其他角色摸两张牌，然后弃置两张牌。每阶段限一次。",SkillType.Active)
        };
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
