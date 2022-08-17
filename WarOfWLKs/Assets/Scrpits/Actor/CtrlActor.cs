using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using zFrame.UI;

public class CtrlActor : Actor
{
	//上次同步信息发送的时间
	private float lastSendSyncTime = 0;

	//同步帧率
	public static float syncInterval = 0.05f;

	public GameObject skillRangeGo;

	public SkillRange skillRange;

	public Joystick joystick;
	public override void Awake()
	{
		base.Awake();
		skillRangeGo = transform.Find("SkillRange").gameObject;
		skillRange = skillRangeGo.GetComponent<SkillRange>();
		skillRangeGo.SetActive(false);
	}
	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		joystick.OnValueChanged.AddListener(v =>
		{
			if (v.magnitude != 0)
			{
				//Debug.Log("hori " + v.x + " verti " + v.y);
				Vector2 direction = new Vector2(v.x, v.y);
				Vector2 destination = direction * m_actorSpeed * Time.deltaTime + new Vector2(transform.position.x, transform.position.y);
				ani.SetBool("Move", true);
				MoveTo(destination);
				CheckDir();
				//Move(direction * speed * Time.deltaTime);
				//transform.rotation = Quaternion.LookRotation(new Vector3(v.x, 0, v.y));
			}
		});
	}
	public void SetJoyStick(Joystick joystick)
	{
		this.joystick = joystick;

	}
	// Update is called once per frame
	protected override void Update()
	{
		base.Update();


		MouseDownUpdate();
		SkillKeyDownUpdate();

		SyncUpdate();

		//测试死亡需要，绑定技能
		//ReadySkill = Skills[0];
	}
	public void SendMsgSkill(Vector2 vec, int skillId)
	{
		MsgSkill msg = new MsgSkill();
		msg.x = vec.x;
		msg.y = vec.y;
		msg.skillId = skillId;
		Debug.Log(vec);
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
		//如果点到的位置是UI则直接return
		if (EventSystem.current.IsPointerOverGameObject()) return;
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
		if (Input.GetMouseButtonDown(1))
		{
			//向鼠标点击的位置发射射线
			m_destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			//设置移动向量
			m_moveVec = m_destination - (Vector2)transform.position;

			//设置技能范围显示
			if (ReadySkill != null)
			{
				if (ReadySkill.id != (int)SKILL.SHOUT) skillRangeGo.SetActive(false);
			}

			//检测移动方向
			CheckDir();

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
		if (ReadySkill == null) return;
		if (Input.GetMouseButtonDown(0))// !IsOnButtom
		{

			//向鼠标点击的位置发射射线
			//Vector2 skillPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			m_destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//Debug.Log(m_destination);

			CastAppleSkill(m_destination, ReadySkill, gameObject);

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
				ReadySkill = skill;
				CastReady(skill);

			}
		}
	}
	public override void CastAppleSkill(Vector2 skillPos, SkillBase skillBase, GameObject gameObject)
	{

		base.CastAppleSkill(skillPos, skillBase, this.gameObject);

		//技能冷却
		ReadySkill.IsCoolDown = true;
		//关闭技能准备,关闭范围显示
		skillRangeGo.SetActive(false);

		//腾出ReadySkill置为null（前有判断需要使用
		ReadySkill = null;
		//发送技能消息
		SendMsgSkill(skillPos, (int)SKILL.APPLE);

		return;
	}
	public void CastShoutSkill(SkillBase skillBase)
	{

		//Debug.Log("CastShoutSkill");
		ShoutSkill shoutSkill = (ShoutSkill)skillBase;
		//获取技能范围内的Actor

		List<Actor> actors = skillRange.GetActorsInsideRange();

		//开启技能动画
		shoutAni.SetTrigger("Shout");
		//关闭技能准备,关闭范围显示
		skillRangeGo.SetActive(false);


		//技能效果
		foreach (Actor hitActor in actors)
		{
			if (hitActor.camp == this.camp) continue;
			//伤害
			hitActor.HandleDamage(shoutSkill.Damage, shoutSkill);
			//击退
			hitActor.KnockBack(this.transform.position, shoutSkill);
			//发消息
			SendMsgHit(actor, hitActor, shoutSkill);
		}
		//腾出ReadySkill置为null（前有判断需要使用
		ReadySkill = null;
	}

	public void CastReady(SkillBase skill)
	{
		if (skill.IsCoolDown) return;
		//设置当前已经准备好的技能
		int skillId = skill.id;
		switch (skillId)
		{
			case (int)SKILL.APPLE:
				{
					CastAppleReady(Skills[(int)SKILL.APPLE - 1]);
					break;
				}
			case (int)SKILL.SHOUT:
				{
					StartCoroutine(CastShoutReady(Skills[(int)SKILL.SHOUT - 1]));
					break;
				}
		}
	}

	public IEnumerator CastShoutReady(SkillBase skillBase)
	{
		//Debug.Log("CastShoutReady");
		ShoutSkill shoutSkill = (ShoutSkill)skillBase;
		//设置当前技能的作用范围
		skillRangeGo.transform.localScale = Vector3.one * (float)(shoutSkill.SkillRange / 0.02);
		skillRange.ChangeSkillRange(shoutSkill.SkillRange);
		//开启范围显示
		skillRangeGo.SetActive(true);
		//发送技能消息
		Vector2 skillPos = new Vector2(transform.position.x, transform.position.y);
		SendMsgSkill(skillPos, (int)SKILL.SHOUT); ;
		//设置动画播放


		//技能冷却
		ReadySkill.IsCoolDown = true;
		//技能延迟释放，技能吟唱时间
		yield return new WaitForSeconds(shoutSkill.chantTime);
		//经过 chantTime 时间 后释放技能
		CastShoutSkill(skillBase);
	}
	public void CastAppleReady(SkillBase skillBase)
	{
		AppleSkill appleSkill = (AppleSkill)skillBase;
		//设置当前技能的作用范围
		skillRangeGo.transform.localScale = Vector3.one * (float)(appleSkill.SkillRange / 0.02);
		skillRange.ChangeSkillRange(appleSkill.SkillRange);
		//开启技能范围显示
		skillRangeGo.SetActive(!skillRangeGo.activeSelf);
	}

	/// <summary>
	/// 发出被击消息
	/// </summary>
	/// <param name="actor">发起者</param>
	/// <param name="hitActor">被击者</param>
	/// <param name="skillBase">技能类型</param>
	private void SendMsgHit(Actor actor, Actor hitActor, SkillBase skillBase)
	{
		if (hitActor == null || actor == null)
		{
			return;
		}
		//不是自己发的子弹击中自己
		if (actor.id != GameManager.Instance.ctrllerId)
		{
			return;
		}
		//发消息
		MsgHit msg = new MsgHit();

		msg.targetId = hitActor.id;
		msg.id = actor.id;
		msg.skillId = skillBase.id;
		msg.damage = skillBase.Damage;

		msg.x = transform.position.x;
		msg.y = transform.position.y;

		NetManager.Send(msg);
	}
}
