using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleManager : MonoBehaviour
{
    public static RoleManager Instance { get; private set; }
    public List<Role> roles = new List<Role>();

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void RoleInitiation(Role role)
    {
        roles.Add(role);
        role.IsRoleEnabled = true;
    }
    public void RoleDisable(Role role)
    {
        role.IsRoleEnabled = false;
    }
    public void RoleSkillReset()
    {

    }
    public IEnumerator EnterPreparing()
    {
        RoleSkillReset();
        yield return null;
    }
}
