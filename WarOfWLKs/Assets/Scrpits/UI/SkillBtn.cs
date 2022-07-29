using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBtn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowInfo(string type)
    {
        string tooltip = string.Empty;
        switch (type)
        {
            case "Fire":
                string text= LocalizationMgr.Instance.GetWordByDirect(11);
                tooltip = string.Format(text);
                break;
        }

        GameManager.Instance.SetTooltipText(tooltip);
        GameManager.Instance.ShowStats();
    }

}
