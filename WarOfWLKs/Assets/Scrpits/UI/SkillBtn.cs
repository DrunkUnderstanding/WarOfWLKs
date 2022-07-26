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
                tooltip = string.Format("     <color=#ffa500ff><size=20><b>技能：发射！</b></size></color>     \n" +
                    "发射你的专属子弹\n" +
                    "造成伤害\n" +
                    "多少多少\n" +
                    "技能描述");
                break;
        }

        GameManager.Instance.SetTooltipText(tooltip);
        GameManager.Instance.ShowStats();
    }

}
