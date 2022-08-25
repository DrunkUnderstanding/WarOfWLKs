using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillBase
{
	//发射技能的角色
	private Actor m_castPlayer;

	//技能发射子弹名（可能不存在）
	private string m_projName;
	//技能名
	private string m_skillName;

	//翻译文本所在Excel文件的行号
	public int translationNum;

	private float m_projSpeed;

	private bool b_isCoolDown = false;

	private float m_coolDownTime;

	//图标位置
	public string iconPath;

	[SerializeField]
	private int m_damage;

	//技能快捷键
	private KeyCode keyCode;

	//技能ID
	public int id;

	//技能作用范围
	private float skillRange;

	public float cdDuration = 0;

	private float knockBackDistance;

	public bool IsCoolDown { get => b_isCoolDown; set => b_isCoolDown = value; }
	public float SkillRange { get => skillRange; set => skillRange = value; }
	public float CoolDownTime { get => m_coolDownTime; set => m_coolDownTime = value; }
	public float ProjSpeed { get => m_projSpeed; set => m_projSpeed = value; }
	public KeyCode KeyCode { get => keyCode; set => keyCode = value; }
	public string SkillName { get => m_skillName; set => m_skillName = value; }
	public string ProjName { get => m_projName; set => m_projName = value; }
	public int Damage { get => m_damage; set => m_damage = value; }
	public float KnockBackDistance { get => knockBackDistance; set => knockBackDistance = value; }

	public SkillBase()
	{

	}
	//技能特效
	//private GameObject m_selfEffect;

	//被Update调用
	public virtual void Update()
	{
		CoolDownUpdate();
	}
	private void CoolDownUpdate()
	{
		if (IsCoolDown)
		{

			cdDuration += Time.deltaTime;
			//Debug.Log(m_cdDuration);
			if (cdDuration >= CoolDownTime)
			{
				cdDuration = 0;
				IsCoolDown = false;
			}
		}
	}
}
