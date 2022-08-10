using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncActor : Actor
{
	//预测信息，哪个时间到哪个位置
	private Vector2 lastPos;

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
		Vector2 destination = new Vector2(msg.x, msg.y);
		MoveTo(destination);
		float subX = destination.x - lastPos.x;
		float subY = destination.y - lastPos.y;
		if (Mathf.Abs(subX) >= 0.05f || Mathf.Abs(subY) >= 0.05f)
		{
			//设置行走方向
			CheckDir(m_destination);

			//开始动画
			StartAni();

			//设置上一次的行走节点位置
			lastPos = destination;
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
