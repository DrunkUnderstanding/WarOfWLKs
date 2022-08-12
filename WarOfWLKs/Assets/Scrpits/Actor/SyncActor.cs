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
		Vector2 curPos = new Vector2(msg.x, msg.y);
		MoveTo(curPos);

		float subX = curPos.x - lastPos.x;
		float subY = curPos.y - lastPos.y;
		if (Mathf.Abs(subX) >= 0.05f || Mathf.Abs(subY) >= 0.05f)
		{
			//不被击退的时候开始动画
			if (!b_isKnocked)
			{
				StartAni();

			}
			//设置行走方向
			CheckDir();
			//设置上一次的行走节点位置
			lastPos = curPos;
		}
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
					SyncShoutSkill(msg);
					break;
				}
		}
	}

	public void SyncShoutSkill(MsgSkill msg)
	{

	}
	public void SyncAppleSkill(MsgSkill msg)
	{
		Vector2 skillPos = new Vector2(msg.x, msg.y);
		AppleSkill appleSkill = new AppleSkill();

		CastAppleSkill(skillPos, appleSkill, GetSyncActor(msg.id).gameObject);
	}
	public void StartAni()
	{
		ani.SetBool("Move", true);
	}
	private void MoveTo(Vector2 destination)
	{


		if (b_isKnocked) return;

		//设置目的地
		m_destination = destination;
		//设置方向
		m_moveVec = m_destination - (Vector2)transform.position;


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
