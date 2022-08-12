using UnityEngine;

public class Projectile : MonoBehaviour
{
	private Animator m_animator;

	private SkillBase m_skill;

	//发出技能的父亲
	public GameObject parent;

	private Vector3 m_targetPos;

	private float m_projSpeed;

	private Vector2 m_moveDir;

	//移动的距离（由SkillBase的castDistance来init）
	private float m_moveDistance;

	// Start is called before the first frame update
	void Start()
	{
		this.m_animator = this.gameObject.GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		MoveUpdate();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="parent">发出子弹的人</param>
	/// <param name="moveDir">子弹移动的方向</param>
	/// <param name="targetPos">子弹移动到的位置</param>
	/// <param name="castDistance">技能施法距离</param>
	/// <param name="projSpeed">技能（子弹）移动速度</param>
	public void InitPorjectile(GameObject parent, Vector2 moveDir, Vector2 targetPos, float castDistance, float projSpeed, SkillBase skill)
	{
		//记录发射的角色，回收需要
		this.parent = parent;

		//设置初始位置
		this.transform.position = parent.transform.position;

		//设置移动
		this.m_moveDir = moveDir;
		Debug.Log(moveDir);
		this.m_moveDistance = castDistance;
		this.m_targetPos = new Vector2(this.transform.position.x, this.transform.position.y) + moveDir * castDistance;
		//this.m_targetPos = targetPos;
		this.m_projSpeed = projSpeed;

		//找到父节点
		this.transform.SetParent(GameObject.Find("Projectiles").transform);

		//设置该技能的类型、伤害等
		m_skill = skill;
		//m_animator.SetBool("Move", true);
		//SetAngle();
	}


	private void SetAngle()
	{
		//设置子弹角度
		float angle = Mathf.Atan2(m_moveDir.y, m_moveDir.x) * Mathf.Rad2Deg;

		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	/// <summary>
	/// 子弹移动到指定位置
	/// </summary>
	private void MoveUpdate()
	{
		if (m_moveDir != Vector2.zero)
		{
			//移动过去
			transform.position = Vector2.MoveTowards(transform.position, m_targetPos, Time.deltaTime * m_projSpeed);

			Stop();
		}
	}
	private void Stop()
	{
		//计算自身和目标点的距离
		float distance = Vector2.Distance(this.transform.position, m_targetPos);
		//判断和目标点的距离是否小于0.01f
		if (distance < 0.01f)
		{
			//如果小于就判定到达目的地，执行待机
			m_moveDir = Vector2.zero;


			m_animator.SetTrigger("Disappear");
			//回收子弹
			//GameManager.Instance.Pool.ReleaseObject(gameObject);
			//停止播放动画
			//m_animator.SetBool("Move", false);
		}
	}
	/// <summary>
	/// CtrlActor发出的技能子弹处理
	/// </summary>
	/// <param name="collision"></param>
	private void OnTriggerEnter2D(Collider2D collision)
	{

		//之有当前子弹是当前客户端的控制角色发出的时候才会发送消息
		if (parent != GameManager.Instance.PlayerSelf) return;
		//如果碰到的角色是父亲，不处理
		if (collision.gameObject == this.parent.gameObject) return;
		//如果碰到的对象不是同步玩家，不处理
		if (collision.tag != "SyncActor") return;

		if (collision.gameObject.GetComponent<Actor>().camp == parent.gameObject.GetComponent<Actor>().camp) return;


		
		//Debug.Log(collision);
		//被击中的角色
		Actor hitActor = collision.gameObject.GetComponent<Actor>();
		hitActor.HandleDamage(m_skill.Damage, m_skill);
		hitActor.KnockBack(this.gameObject.transform.position, m_skill);
		//参与射击的角色
		Actor actor = parent.GetComponent<Actor>();
		SendMsgHit(actor, hitActor);
		//释放动画
		m_animator.SetTrigger("Disappear");
		//子弹停在原地不要动了
		m_moveDir = Vector2.zero;
		//GameManager.Instance.Pool.ReleaseObject(this.gameObject);
	}
	/// <summary>
	/// 发击中消息
	/// </summary>
	/// <param name="actor"></param>
	/// <param name="hitActor"></param>
	private void SendMsgHit(Actor actor, Actor hitActor)
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
		msg.skillId = m_skill.id;
		msg.damage = m_skill.Damage;
		//msg.damage = 
		msg.x = transform.position.x;
		msg.y = transform.position.y;

		NetManager.Send(msg);
	}
}
