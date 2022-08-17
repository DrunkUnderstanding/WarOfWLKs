using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncActor : Actor
{
	//预测信息，哪个时间到哪个位置
	private Vector2 lastPos;

	private Vector2 syncDestination;


	public override void Awake()
	{
		base.Awake();
	}
	public override void Init(GameObject actorObj, Vector2 vec)
	{
		base.Init(actorObj, vec);

		lastPos = new Vector2(vec.x, vec.y);

	}

	public void SyncPos(MsgSyncActor msg)
	{
		Vector2 syncPos = new Vector2(msg.x, msg.y);
		//Debug.Log(msg.x);
		//Debug.Log(msg.y);
		Vector2 curPos = new Vector2(transform.position.x, transform.position.y);
		if (syncPos == curPos) return;
		// Debug.Log("同步位置：" + syncPos + "\n当前位置" + curPos);
		float distance = Vector2.Distance(transform.position, lastPos);
		//不被击退的时候
		//并且当前位置与上一个移动点的位置的距离＜0.01时
		if (!b_isKnocked && m_moveVec == Vector2.zero)
		{
			//Debug.Log("Sync调用 Move true");
			//开始行走动画
			StartAni();
		}
		MoveTo(syncPos);
		//设置行走方向
		CheckDir();
		//设置上一次的行走节点位置为这次的行走位置
		lastPos = syncPos;
	}

	public void SyncSkill(MsgSkill msg)
	{
		//写死，之后需要修改
		int skillId = msg.skillId;
		switch (skillId)
		{
			//扔苹果技能
			case (int)SKILL.APPLE:
				{
					SyncAppleSkill(msg);
					break;
				}
			case (int)SKILL.SHOUT:
				{
					StartCoroutine(SyncShoutSkill(msg));
					break;
				}
		}
	}

	public IEnumerator SyncShoutSkill(MsgSkill msg)
	{
		ShoutSkill shoutSkill = new ShoutSkill();
		yield return new WaitForSeconds(shoutSkill.chantTime);
		//开启技能动画
		shoutAni.SetTrigger("Shout");
	}
	public void SyncAppleSkill(MsgSkill msg)
	{
		Vector2 skillPos = new Vector2(msg.x, msg.y);
		AppleSkill appleSkill = new AppleSkill();

		CastAppleSkill(skillPos, appleSkill, gameObject);
	}
	public override void Stop()
	{

		base.Stop();
	}
	public void StartAni()
	{
		ani.SetBool("Move", true);
	}

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();


	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
	}

}
