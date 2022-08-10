using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 状态类，保存值信息，作为角色控制生命值条的类存在（不可作为组件实例化）
/// </summary>
[Serializable]
public class Stat
{

    [SerializeField]
    private Actor actor;

    [SerializeField]
    private BarScript bar;

    [SerializeField]
    private float maxVal;

    [SerializeField]
    private Text m_healthTxt;

    [SerializeField]
    private Text m_nameTxt;

    [SerializeField]
    private float currentVal;

    //设置、获取角色当前生命值
    public float CurrentVal
    {
        get => currentVal;
        set
        {
            this.currentVal = Mathf.Clamp(value, 0, MaxVal);
            Bar.Value = currentVal;
            HandleTxt();
        }
    }

    //设置、获取角色最大生命值
    public float MaxVal
    {
        get { return maxVal; }
        set
        {
            this.maxVal = value;
            Bar.MaxValue = maxVal;
        }
    }
    //角色的生命条控制脚本
    public BarScript Bar { get => bar; }
	public Text NameTxt { get => m_nameTxt; set => m_nameTxt = value; }

	public void Initialize()
    {
        this.MaxVal = maxVal;
        this.CurrentVal = currentVal;
    }

    //控制文本显示
    public void HandleTxt()
    {
        m_healthTxt.text =currentVal.ToString("#0") + "%";
    }
}
