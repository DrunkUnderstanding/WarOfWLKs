using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public enum SKILL
{
	APPLE = 1, SHOUT = 2
}

public class Actor : MonoBehaviour
{
	private GameObject skin;
	//玩家ID
	public string id = "";
	//阵营
	public int camp = 0;

	//玩家移动的速度
	[SerializeField]
	protected float m_actorSpeed;

	//当前的代码
	protected Actor actor;
	//玩家的生命值条
	[SerializeField]
	public Stat m_health;

	//技能列表，一般只有4个技能，可加
	private List<SkillBase> m_skills = new List<SkillBase>();

	public List<SkillBase> Skills { get => m_skills; set => m_skills = value; }


	//已经准备好使用的技能
	private SkillBase readySkill;

	//动画
	protected Animator ani;
	//吼叫特效
	protected Animator shoutAni;

	[SerializeField]
	//鼠标点击位置
	protected Vector2 m_destination;

	//鼠标点击位置与当前位置的向量
	protected Vector2 m_moveVec;

	//当前移动的方向
	protected Vector2 m_direct;

	//角色正在被击退
	protected bool b_isKnocked = false;

	public float MaxSpeed { get; set; }
	public SkillBase ReadySkill { get => readySkill; set => readySkill = value; }

	public virtual void Awake()
	{
		//在awake把所有绑定的代码绑定好，以免奇怪的事情发生
		ani = this.gameObject.GetComponent<Animator>();
		shoutAni = transform.Find("Shout").GetComponent<Animator>();
		actor = this.gameObject.GetComponent<Actor>();
		//游戏开始时绑定技能给Actor
		Skills.Add(new AppleSkill());
		Skills.Add(new ShoutSkill());
	}

	public virtual void Init(GameObject actorObj, Vector2 vec)
	{
		skin = actorObj;

		//ReadySkill = new AppleSkill();
	}

	/// <summary>
	/// 检测玩家移动方向,并调整方向
	/// </summary>
	protected void CheckDir()
	{
		if (b_isKnocked) return;
		if (m_moveVec.x < 0)
		{
			if (m_moveVec.y >= 0)
			{
				m_direct = new Vector2(-1, 1);
			}
			else
			{
				m_direct = new Vector2(-1, -1);
			}
			this.transform.rotation = Quaternion.Euler(0, 180, 0);

			this.transform.GetChild(1).rotation = Quaternion.Euler(0, 0, 0);
		}
		if (m_moveVec.x > 0)
		{
			if (m_moveVec.y >= 0)
			{
				m_direct = new Vector2(1, 1);
			}
			else
			{
				m_direct = new Vector2(1, -1);
			}
			this.transform.rotation = Quaternion.Euler(0, 0, 0);
			this.transform.GetChild(1).rotation = Quaternion.Euler(0, 0, 0);
		}
	}
	private void MoveUpdate()
	{
		//移动
		//移动向量！=（0,0）才能说明有地方可以去，不然就是点自己脚底板了
		if (m_moveVec != Vector2.zero)
		{

			Stop();
			//Debug.Log("1——" + transform.position.ToString("f5"));
			//移动过去
			transform.position = Vector2.MoveTowards(transform.position, m_destination, m_actorSpeed * Time.deltaTime);
			//Debug.Log("2——" + transform.position.ToString("f5"));

			//Debug.Log(m_destination.ToString("f5"));

		}
	}

	//停止移动
	public virtual void Stop()
	{


		//计算自身和目标点的距离
		float distance = Vector2.Distance(transform.position, m_destination);
		//Debug.Log(distance.ToString("f5"));
		//判断和目标点的距离是否小于0.01f
		if (distance < 0.0001f)
		{
			b_isKnocked = false;
			//如果小于就判定到达目的地，执行待机
			m_moveVec = Vector2.zero;
			//Debug.Log(distance.ToString("f5"));
			//Debug.Log(string.Format("<color=green>Stop调用 Move false</color>"));
			//停止播放动画
			ani.SetBool("Move", false);
			ani.SetBool("Knocked", false);
		}
	}
	// Start is called before the first frame update
	protected virtual void Start()
	{


		m_health.Bar.Reset();

		//游戏开始时设置动画
		ani.SetBool("Move", false);

		//设置名字
		if (this.camp == 1)
		{
			this.m_health.NameTxt.text = "<color=red>" + this.id + "</color>";
		}
		else
		{
			this.m_health.NameTxt.text = "<color=blue>" + this.id + "</color>";
		}

		MaxSpeed = m_actorSpeed;
	}

	// Update is called once per frame
	protected virtual void Update()
	{
		//Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));

		MoveUpdate();

		SkillsUpdate();

		//m_health.Initialize();
		//测试生命条的代码
		//m_health.CurrentVal -= 0.1f;
		//HandleDamage(20f, m_readySkill);
	}

	public void MoveTo(Vector2 destination)
	{


		if (b_isKnocked) return;

		//设置目的地
		m_destination = destination;
		//设置方向
		m_moveVec = m_destination - (Vector2)transform.position;


	}

	/// <summary>
	/// 处理角色的释放技能CD、等信息
	/// </summary>
	private void SkillsUpdate()
	{
		foreach (SkillBase skill in Skills)
		{
			skill.Update();
		}
	}

	public bool IsDie()
	{
		return m_health.CurrentVal <= 0;
	}
	public void Die()
	{
		if (this.id == GameManager.Instance.ctrllerId)
		{

			//GameManager.Instance.ShowDiePanel(true);
		}
		//只要有某一个玩家的客户端的玩家死亡，则直接发送玩家的死亡消息
		MsgDie msg = new MsgDie();
		msg.id = this.id;
		NetManager.Send(msg);
		Release();
	}
	/// <summary>
	/// 处理角色受烧伤信息
	/// </summary>
	/// <param name="damage"></param>
	public void HandleBurned(float damage)
	{
		//Debug.Log(this.tag);
		this.m_health.CurrentVal -= damage * Time.deltaTime;

		//死亡
		if (IsDie())
		{
			Die();
		}
	}

	/// <summary>
	/// 处理角色受攻击的信息
	/// </summary>
	/// <param name="damage"></param>
	/// <param name="skill">（暂无用处，之后技能可能存在buff效果需要）</param>
	public void HandleDamage(float damage, SkillBase skill)
	{
		Debug.Log(this.tag);
		this.m_health.CurrentVal -= damage;
		//死亡
		if (IsDie())
		{

			Die();
		}
	}
	/// <summary>
	/// 被击后 后退
	/// </summary>
	/// <param name="knockPos"></param>
	/// <param name="skill"></param>
	public void KnockBack(Vector3 knockPos, SkillBase skill)
	{
		//当前位置
		Vector2 thisPosVec2 = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
		//移动方向
		Vector3 moveDir = (this.gameObject.transform.position - knockPos).normalized;
		//移动的向量
		Vector2 moveVec = skill.KnockBackDistance * (new Vector2(moveDir.x, moveDir.y));
		//移动到的最终位置
		Vector2 moveTo = thisPosVec2 + moveVec;
		m_moveVec = moveVec;
		m_destination = moveTo;
		b_isKnocked = true;

		//设置动画
		ani.SetBool("Knocked", true);
		ani.SetBool("Move", false);
	}

	/// <summary>
	/// 释放当前 Actor，并且将当前 Actor 放入 Pool
	/// </summary>
	public void Release()
	{

		//需要这一句，其他代码位置不存在速度修改，会导致bug
		m_actorSpeed = MaxSpeed;

		//释放对象以后再放入对象池
		GameManager.Instance.Pool.ReleaseObject(gameObject);

	}
	/// <summary>
	/// 释放Apple技能
	/// </summary>
	/// <param name="skillPos">//暂时不需要使用技能要到达的位置</param>
	public virtual void CastAppleSkill(Vector2 skillPos, SkillBase skillBase, GameObject parent)
	{
		//Debug.Log(skillPos);
		//激活子弹对象实例，并且获得其代码
		Projectile projectile = GameManager.Instance.Pool.GetObject(skillBase.ProjName).GetComponent<Projectile>();

		Vector2 skillMoveVec = new Vector2(skillPos.x - this.transform.position.x, skillPos.y - this.transform.position.y);

		//初始化技能投射
		projectile.InitPorjectile(parent, skillMoveVec.normalized, skillPos, skillBase.SkillRange, skillBase.ProjSpeed, skillBase);

		return;
	}

	/// <summary>
	/// 已经弃用
	/// </summary>
	public void Rebirth()
	{
		//当我们需要重新启用当前资源时，将这个资源的初始位置设置到GridPosition
		Reset(m_health.MaxVal);

		this.gameObject.SetActive(true);

		GameManager.Instance.ShowDiePanel(false);
	}

	/// <summary>
	/// 已经弃用
	/// </summary>
	/// <param name="maxHealth"></param>
	public void Reset(float maxHealth)
	{
		transform.position = LevelManager.Instance.Tiles[LevelManager.Instance.BirthPoint[1]].transform.position;

		this.m_health.Bar.Reset();

		this.m_health.MaxVal = maxHealth;

		this.m_health.CurrentVal = this.m_health.MaxVal;


		this.m_destination = transform.position;
	}
	public void SyncHit(MsgHit msg)
	{
		//写死，之后需要修改
		int skillId = msg.skillId;
		switch (skillId)
		{
			case (int)SKILL.APPLE:
				{
					SyncAppleHit(msg);
					break;
				}
			case 2:
				{
					SyncShoutHit(msg);
					break;
				}
		}
	}
	public void SyncShoutHit(MsgHit msg)
	{
		//伤害效果
		HandleDamage(msg.damage, new ShoutSkill());
		//击退效果
		Vector3 knockPos = new Vector3(msg.x, msg.y, 0);
		KnockBack(knockPos, new ShoutSkill());
	}
	public void SyncAppleHit(MsgHit msg)
	{
		//回收子弹
		RecycleProjectile(msg.id);
		//处理伤害效果
		HandleDamage(msg.damage, new AppleSkill());
		//处理击退效果
		Vector3 knockPos = new Vector3(msg.x, msg.y, 0);
		KnockBack(knockPos, new AppleSkill());
	}
	/// <summary>
	/// 回收子弹
	/// </summary>
	/// <param name="id"></param>
	public void RecycleProjectile(string id)
	{
		//及时回收子弹
		GameObject projectliesFather = GameObject.Find("Projectiles");

		Projectile projectile = null;
		//在Projectiles下寻找子弹
		int count = projectliesFather.transform.childCount;
		for (int i = 0; i < count; i++)
		{
			//当前对象是否是 Apple 子弹
			GameObject gameObject = projectliesFather.transform.GetChild(i).gameObject;
			if (gameObject.tag != "Apple") continue;
			//当前对象是 Apple 子弹
			//找到发射子弹的父亲的id
			string fatherId = gameObject.transform.GetComponent<Projectile>().parent.GetComponent<Actor>().id;
			if (fatherId == id)
			{
				projectile = projectliesFather.transform.GetChild(i).GetComponent<Projectile>();
				break;
			}

		}
		if (projectile == null)
		{
			Debug.Log("Projectile's father is null !");
			return;
		}
		projectile.m_animator.SetTrigger("Disappear");
	}
	public SyncActor GetSyncActor(string id)
	{
		GameObject ActorsFather = GameObject.Find("Actors");
		SyncActor syncActor = null;
		foreach (string actor in BattleManager.actors.Keys)
		{
			string syncActorId = actor;
			if (syncActorId == id)
			{
				syncActor = (SyncActor)BattleManager.Instance.GetActor(syncActorId);
				break;
			}
		}

		if (syncActor == null)
		{
			Debug.Log("syncActor is null!");
			return null;
		}
		return syncActor;
	}
}
