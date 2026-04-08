using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role : MonoBehaviour
{
    public string roleName { get; protected set; }
    public RoleFaction roleFaction { get; protected set; }
    public List<Skill> skills { get; protected set; } = new List<Skill>();
    public int HPmax { get; protected set; }
    public int HP { get; set; }
    public Dictionary<string, int> Tags { get; set; } = new Dictionary<string, int>();


    public bool IsRoleEnabled { get; set; } = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 添加Tag
    /// </summary>
    /// <param name="tag">Tag唯一标识“XXX|name”</param>
    /// <param name="num">层数</param>
    public void AddTag(string tag, int num = 1)
    {
        if (Tags.ContainsKey(tag))
        {
            if (Tags[tag] >= 1)
            {
                Tags[tag] = Tags[tag] + num;
            }
        }
        else
        {
            Tags.Add(tag, num);
        }
    }

    /// <summary>
    /// 刷新临时Tag
    /// </summary>
    public void RefreshTags()
    {
        if (Tags.ContainsKey("Card|JIU"))
        {
            RemoveTag("Card|JIU", isClearingTag: true);
        }
    }

    /// <summary>
    /// 移除Tag
    /// </summary>
    /// <param name="tag">Tag唯一标识“XXX|name”</param>
    /// <param name="num">层数</param>
    /// <param name="isClearingTag">是否直接清理</param>
    public void RemoveTag(string tag, int num = 1, bool isClearingTag = false)
    {
        if (Tags.ContainsKey(tag))
        {
            if (!isClearingTag)
            {
                if (Tags[tag] > 1)
                {
                    Tags[tag] = Tags[tag] - num;
                }
                else if (Tags[tag] == 1)
                {
                    Tags.Remove(tag);
                }
            }
            else
            {
                Tags.Remove(tag);
            }
        }
    }

    /// <summary>
    /// 获取最大手牌数
    /// </summary>
    /// <returns></returns>
    public virtual int GetMaxCardsNum()
    {
        return HP;
    }
}
public enum RoleFaction
{
    Shu,
    Wu,
    Wei,
    Qun
}
public class Skill
{
    public string skillName;
    public string skillDescription;
    public SkillType skillType;
    public Skill(string name, string description, SkillType type)
    {
        skillName = name;
        skillDescription = description;
        skillType = type;
    }
}
public enum SkillType
{
    Passive,
    Active
}
