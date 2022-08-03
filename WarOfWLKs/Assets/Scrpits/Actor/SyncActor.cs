using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncActor : Actor
{
	//预测信息，哪个时间到哪个位置
	private Vector2 lastPos;

	private Vector2 forcastPos;

	private float forecastTime;

	public override void Init(GameObject actorObj)
	{
		base.Init(actorObj);
		//设置物理运动影响
		Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
		rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
		rigidbody.simulated = false;
	}
	public void SyncHit(MsgHit msg)
	{
		HandleDamage(msg.damage, new FireSkill());
		Vector3 vector3 = new Vector3(msg.x, msg.y, 0);
		KnockBack(vector3, new FireSkill());
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
		FireSkill fireSkill = new FireSkill();

		Vector2 skillPos = new Vector2(msg.VecX, msg.VecY);

		CastSkill(skillPos);

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
