using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogErrorPopPanel : BaseUiFrame
{
    public static LogErrorPopPanel Instance;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this ;
        CurrentUiType.UiShowMode = UiShowMode.Normal;
        CurrentUiType.UiWindType = UiWindType.PopUp;
        RigisterButtonObjectEvent("Confirm_btn", P => {  });
        RigisterButtonObjectEvent("Canel_btn", P => {  });
        
    }
    public void HidePanel()
    {
        Invoke("Hiding", 0.5f);
    }
}
