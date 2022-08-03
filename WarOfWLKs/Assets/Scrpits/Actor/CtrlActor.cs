using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CtrlActor : Actor
{
	//上次同步信息发送的时间
	private float lastSendSyncTime = 0;

	//同步帧率
	public static float syncInterval = 0.1f;

	[SerializeField]
	protected GameObject m_skillRange;


	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();


		MouseDownUpdate();
		SkillKeyDownUpdate();

		SyncUpdate();

		//测试死亡需要，绑定技能
		m_readySkill = Skills[0];
	}
	public void SkillUpdate(Vector2 vector2)
	{
		MsgSkill msg = new MsgSkill();
		msg.VecX = vector2.x;
		msg.VecY = vector2.y;

		NetManager.Send(msg);
	}
	public void SyncUpdate()
	{
		//时间间隔判断
		if (Time.time - lastSendSyncTime < syncInterval)
		{
			return;
		}
		lastSendSyncTime = Time.time;
		//发送同步协议
		MsgSyncActor msg = new MsgSyncActor();
		msg.x = transform.position.x;
		msg.y = transform.position.y;

		NetManager.Send(msg);
	}

	//获取鼠标点击信息
	private void MouseDownUpdate()
	{
		if (IsDie()) return;
		GetMouse0Down();
		GetMouse1Down();
	}
	/// <summary>
	/// 获取玩家右键点击位置
	/// </summary>
	private void GetMouse1Down()
	{
		if (b_isKnocked) return;
		//如果按下鼠标右键（0是左键、1是右键）
		if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
		{
			//向鼠标点击的位置发射射线
			m_destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			//设置移动向量
			m_moveVec = m_destination - (Vector2)transform.position;

			//设置技能范围显示
			m_skillRange.SetActive(false);

			//关闭释放技能准备

			b_isPrepareCast = false;

			//检测移动方向
			CheckDir(m_destination);

			//Debug.Log(m_destination);
			//播放动画
			ani.SetBool("Move", true);
			//发送同步消息
			SyncUpdate();

		}
	}


	/// <summary>
	/// 获取玩家左键点击位置
	/// </summary>
	public void GetMouse0Down()
	{
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())// !IsOnButtom
		{
			if (b_isPrepareCast)
			{
				//向鼠标点击的位置发射射线
				//Vector2 skillPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

				m_destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				//Debug.Log(m_destination);

				CastSkill(m_destination);


			}
		}
	}

	/// <summary>
	/// 检测是否按下技能键
	/// </summary>
	public void SkillKeyDownUpdate()
	{
		//遍历绑定的技能列表获取Click信息
		foreach (SkillBase skill in Skills)
		{
			if (Input.GetKeyDown(skill.KeyCode))
			{
				if (!skill.IsCoolDown)
				{
					CastReady(skill);
				}
			}
		}
	}
	public override Vector2 CastSkill(Vector2 skillPos)
	{
		Vector2 skillMoveVec = base.CastSkill(skillPos);
		m_readySkill.IsCoolDown = true;
		m_skillRange.SetActive(false);
		b_isPrepareCast = false;
		SkillUpdate(skillMoveVec);
		return skillMoveVec;

	}
	public void CastReady(SkillBase skill)
	{
		//设置当前已经准备好的技能
		m_readySkill = skill;
		//设置当前技能的施法距离
		m_skillRange.transform.localScale = Vector3.one * (float)(skill.CastDistance / 0.02);
		//开启技能范围显示
		m_skillRange.SetActive(!m_skillRange.activeSelf);
		//修改施法状态
		b_isPrepareCast = !b_isPrepareCast;
	}
}
