using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
	public static Dictionary<string, Actor> actors = new Dictionary<string, Actor>();

	public Transform actortsFather;

	public static bool hasFinished = false;

	public static bool hasCreateLevel = false;
	public void Init()
	{
		NetManager.AddMsgListener("MsgEnterBattle", OnMsgEnterBattle);
		NetManager.AddMsgListener("MsgBattleResult", OnMsgBattleResult);
		NetManager.AddMsgListener("MsgLeaveBattle", OnMsgLeaveBattle);

		NetManager.AddMsgListener("MsgSyncActor", OnMsgSyncActor);
		NetManager.AddMsgListener("MsgSkill", OnMsgSkill);
		NetManager.AddMsgListener("MsgHit", OnMsgHit);
		NetManager.AddMsgListener("MsgDie", OnMsgDie);
	}
	public void OnMsgDie(MsgBase msgBase)
	{
		MsgDie msg = (MsgDie)msgBase;
		Actor actor = GetActor(msg.id);
		if (actor == null) return;
		if (actor.IsDie()) return;
		actor.m_health.CurrentVal = 0;
		if (this.tag == "Player1")
		{
			GameManager.Instance.ShowDiePanel(true);
		}
		actor.Release();
	}

	public void DestoryActors()
	{
		for (int i = 0; i < actortsFather.childCount; i++)
		{
			Destroy(actortsFather.GetChild(i).gameObject);
		}
	}
	public void AddActor(string id, Actor actor)
	{
		actors[id] = actor;
	}
	public void RemoveActor(string id)
	{
		actors.Remove(id);
	}
	public Actor GetActor(string id)
	{
		if (actors.ContainsKey(id))
		{
			return actors[id];
		}
		return null;
	}
	public Actor GetCtrlActor()
	{
		return GetActor(GameManager.Instance.ctrllerId);
	}

	public void Reset()
	{
		foreach (Actor actor in actors.Values)
		{
			actor.Release();
		}
		actors.Clear();
	}
	//收到可以进入战斗的消息
	public void OnMsgEnterBattle(MsgBase msgBase)
	{
		MsgEnterBattle msg = (MsgEnterBattle)msgBase;
		//加入战斗
		EnterBattle(msg);
	}

	//进入战斗
	public void EnterBattle(MsgEnterBattle msg)
	{
		//重置
		BattleManager.Instance.Reset();
		//关闭界面
		PanelManager.Instance.Close("RoomPanel");
		PanelManager.Instance.Close("ResultPanel");

		//打开游玩过程中的界面
		PanelManager.Instance.Open<GamingPanel>();
		hasFinished = false;
		//产生角色
		for (int i = 0; i < msg.actors.Length; i++)
		{
			GenerateActor(msg.actors[i]);
		}

		//设置摄像头
		CameraMovement.Instance.Init();

		//产生地图
		if (!hasCreateLevel)
		{
			LevelManager.Instance.CreateLevel();
			hasCreateLevel = true;
		}

	}
	//生成角色
	public void GenerateActor(ActorInfo actorInfo)
	{
		string objName = "Actor_" + actorInfo.id;
		//创建游戏实体
		GameObject actorObj;
		if (actorInfo.id == GameManager.Instance.ctrllerId)
		{
			actorObj = ResourceManager.Instance.LoadRes<GameObject>("Prefabs/Actors/CtrlMaskAborigine");
			actorObj = Instantiate(actorObj);
			GameManager.Instance.PlayerSelf = actorObj;
		}
		else
		{
			actorObj = ResourceManager.Instance.LoadRes<GameObject>("Prefabs/Actors/SyncMaskAborigine");
			actorObj = Instantiate(actorObj);
		}



		//获取代码组件
		Actor actor = actorObj.GetComponent<Actor>();

		actor.Init(actorObj);

		//属性设置
		actor.camp = actorInfo.camp;
		actor.id = actorInfo.id;
		actor.m_health.CurrentVal = actorInfo.hp;
		actor.m_health.MaxVal = actorInfo.hp;


		//修改在管理器内的显示
		actorObj.name = objName;
		Transform father = GameObject.Find("Actors").transform;
		actorObj.transform.SetParent(father);

		//pos、transform设置
		Vector2 pos = new Vector2(actorInfo.x, actorInfo.y);
		actorObj.transform.position = pos;
		//actorObj.transform.SetParent(actorNameObj.transform);

		//加入列表
		AddActor(actorInfo.id, actor);
	}
	//战斗结束信息
	public void OnMsgBattleResult(MsgBase msgBase)
	{
		MsgBattleResult msg = (MsgBattleResult)msgBase;
		//判断战斗胜利还是失败
		bool isWin = false;
		Actor actor = GetCtrlActor();
		if (actor != null && actor.camp == msg.winCamp)
		{
			isWin = true;
		}
		//游戏是否结束
		hasFinished = true;
		//关闭死亡界面
		PanelManager.Instance.Close("DiedPanel");
		//显示胜利
		PanelManager.Instance.Open<ResultPanel>(isWin);
	}

	public void OnMsgLeaveBattle(MsgBase msgBase)
	{
		MsgLeaveBattle msg = (MsgLeaveBattle)msgBase;
		//查找玩家
		Actor actor = GetActor(msg.id);
		if (actor == null)
		{
			return;
		}
		RemoveActor(msg.id);
		Destroy(actor.gameObject);
	}

	public void OnMsgSyncActor(MsgBase msgBase)
	{
		MsgSyncActor msg = (MsgSyncActor)msgBase;
		//不同步自己
		if (msg.id == GameManager.Instance.ctrllerId)
		{
			return;
		}
		SyncActor actor = (SyncActor)GetActor(msg.id);
		if (actor == null) return;
		actor.SyncPos(msg);
	}

	public void OnMsgSkill(MsgBase msgBase)
	{
		MsgSkill msg = (MsgSkill)msgBase;
		//不同步自己
		if (msg.id == GameManager.Instance.ctrllerId)
		{
			return;
		}
		SyncActor actor = (SyncActor)GetActor(msg.id);
		if (actor == null) return;
		actor.SyncSkill(msg);
	}
	public void OnMsgHit(MsgBase msgBase)
	{
		MsgHit msg = (MsgHit)msgBase;

		Actor actor = (Actor)GetActor(msg.targetId);
		if (actor == null) return;
		actor.SyncHit(msg);

	}
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
