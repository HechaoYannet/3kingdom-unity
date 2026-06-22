using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理当前战斗中启用的角色实例。
/// </summary>
public class RoleManager : MonoBehaviour
{
    public static RoleManager Instance { get; private set; }
    public List<Role> roles = new List<Role>();

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 注册并启用一个角色。
    /// </summary>
    /// <param name="role">角色组件。</param>
    public void RoleInitiation(Role role)
    {
        if (role == null)
        {
            return;
        }

        if (!roles.Contains(role))
        {
            roles.Add(role);
        }

        role.IsRoleEnabled = true;
    }

    /// <summary>
    /// 禁用角色。
    /// </summary>
    /// <param name="role">角色组件。</param>
    public void RoleDisable(Role role)
    {
        if (role == null)
        {
            return;
        }

        role.IsRoleEnabled = false;
    }

    /// <summary>
    /// 刷新角色技能使用状态。
    /// </summary>
    public void RoleSkillReset()
    {
    }

    /// <summary>
    /// 进入准备阶段时统一刷新角色状态。
    /// </summary>
    /// <returns>协程。</returns>
    public IEnumerator EnterPreparing()
    {
        RoleSkillReset();
        yield return null;
    }
}
