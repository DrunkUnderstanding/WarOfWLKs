using System;
using System.Collections.Generic;

public class Room
{
	//id
	public int id = 0;
	//最大玩家数
	public int maxPlayer = 6;
	//玩家列表
	public Dictionary<string, bool> playerIds = new Dictionary<string, bool>();
	//房主id
	public string ownerId = "";

	//一局游戏限定的时间
	public static float gameTime = 10f;

	//出生点信息
	public static float[,,] birthConfig = new float[2, 3, 2]
	{
		{
			{1.2f,-1.1f },
			{0.9f,-1.8f },
			{1.2f,-2.5f }
		},
		{
			{3.5f,-1.1f },
			{4.2f,-1.8f },
			{3.5f,-2.5f }
		}
	};

	//状态
	public enum Status
	{
		PREPARE = 0,
		FIGHT = 1,
	}
	public Status status = Status.PREPARE;
	//初始化位置
	private void SetBirthPos(Player player, int index)
	{
		int camp = player.camp;
		player.x = birthConfig[camp - 1, index, 0];
		player.y = birthConfig[camp - 1, index, 1];
	}

	//玩家数据转成TankInfo
	public ActorInfo PlayerToActorInfo(Player player)
	{
		ActorInfo actorInfo = new ActorInfo();

		actorInfo.id = player.id;
		actorInfo.camp = player.camp;
		actorInfo.hp = player.hp;

		actorInfo.x = player.x;
		actorInfo.y = player.y;

		return actorInfo;
	}

	//开战
	public bool StartBattle()
	{
		if (!CanStartBattle())
		{
			return false;
		}
		//状态
		status = Status.FIGHT;
		//玩家战斗属性
		ResetPlayers();
		lastJudgeTime = 0;
		//返回数据
		MsgEnterBattle msg = new MsgEnterBattle();
		msg.mapId = 1;
		msg.actors = new ActorInfo[playerIds.Count];

		int i = 0;
		foreach (string id in playerIds.Keys)
		{
			Player player = PlayerManager.GetPlayer(id);
			msg.actors[i] = PlayerToActorInfo(player);
			i++;
		}
		Broadcast(msg);
		return true;
	}

	//重置玩家战斗属性
	private void ResetPlayers()
	{
		//位置和旋转
		int count1 = 0;
		int count2 = 0;
		foreach (string id in playerIds.Keys)
		{
			Player player = PlayerManager.GetPlayer(id);
			if (player.camp == 1)
			{
				SetBirthPos(player, count1);
				count1++;
			}
			else
			{
				SetBirthPos(player, count2);
				count2++;
			}
		}
		//生命值
		foreach (string id in playerIds.Keys)
		{
			Player player = PlayerManager.GetPlayer(id);
			player.hp = 100;
		}

	}

	//能否开战斗
	public bool CanStartBattle()
	{
		//战斗状态
		if (status != Status.PREPARE)
		{
			return false;
		}
		//统计阵营人数
		int count1 = 0;
		int count2 = 0;
		foreach (string id in playerIds.Keys)
		{
			Player player = PlayerManager.GetPlayer(id);
			if (player.camp == 1) { count1++; }
			else { count2++; }

		}
		//每个阵营至少一个玩家
		if (count1 < 1 || count2 < 1)
		{
			Console.WriteLine("某一方玩家人数不足 count1:" + count1.ToString() + " couint2:" + count2, ToString());
			return false;
		}
		return true;
	}

	//添加玩家
	public bool AddPlayer(string id)
	{
		//获取玩家
		Player player = PlayerManager.GetPlayer(id);
		if (player == null)
		{
			Console.WriteLine("room.AddPlayer fail, player is null");
			return false;
		}
		//房间人数
		if (playerIds.Count >= maxPlayer)
		{
			Console.WriteLine("room.AddPlayer fail, reach maxPlayer");
			return false;
		}
		//准备状态才能加人
		if (status != Status.PREPARE)
		{
			Console.WriteLine("room.AddPlayer fail, not PREPARE");
			return false;
		}
		//已经在房间里
		if (playerIds.ContainsKey(id))
		{
			Console.WriteLine("room.AddPlayer fail, already in this room");
			return false;
		}
		//加入列表
		playerIds[id] = true;
		//设置玩家数据
		player.camp = SwitchCamp();
		player.roomId = this.id;
		player.isReady = false;
		//设置房主
		if (ownerId == "")
		{
			ownerId = player.id;
		}
		//广播
		Broadcast(ToMsg());
		return true;
	}

	//分配阵营
	public int SwitchCamp()
	{
		//计数
		int count1 = 0;
		int count2 = 0;
		foreach (string id in playerIds.Keys)
		{
			Player player = PlayerManager.GetPlayer(id);
			if (player.camp == 1) { count1++; }
			if (player.camp == 2) { count2++; }
		}
		//选择
		if (count1 <= count2)
		{
			return 1;
		}
		else
		{
			return 2;
		}
	}

	//是不是房主
	public bool isOwner(Player player)
	{
		return player.id == ownerId;
	}

	//删除玩家
	public bool RemovePlayer(string id)
	{

		//获取玩家
		Player player = PlayerManager.GetPlayer(id);
		if (player == null)
		{
			Console.WriteLine("room.RemovePlayer fail, player is null");
			return false;
		}
		//没有在房间里
		if (!playerIds.ContainsKey(id))
		{
			Console.WriteLine("room.RemovePlayer fail, not in this room");
			return false;
		}
		//删除列表
		playerIds.Remove(id);
		//设置玩家数据
		player.camp = 0;
		player.roomId = -1;
		//设置房主
		if (ownerId == player.id)
		{
			ownerId = SwitchOwner();
		}

		//战斗状态退出
		if (status == Status.FIGHT)
		{
			player.data.score -= 5;
			MsgLeaveBattle msg = new MsgLeaveBattle();
			msg.id = player.id;
			Broadcast(msg);
		}
		//房间为空
		if (playerIds.Count == 0)
		{
			RoomManager.RemoveRoom(this.id);
		}
		//广播
		Broadcast(ToMsg());
		return true;
	}

	//选择房主
	public string SwitchOwner()
	{
		//选择第一个玩家
		foreach (string id in playerIds.Keys)
		{
			return id;
		}
		//房间没人
		return "";
	}


	//广播消息
	public void Broadcast(MsgBase msg)
	{
		foreach (string id in playerIds.Keys)
		{
			Player player = PlayerManager.GetPlayer(id);
			player.Send(msg);
		}
	}

	public void BroadcastExcept(MsgBase msg, string targetId)
	{
		foreach (string id in playerIds.Keys)
		{
			if (id == targetId) continue;
			Player player = PlayerManager.GetPlayer(id);
			player.Send(msg);
		}
	}

	//生成MsgGetRoomInfo协议
	public MsgBase ToMsg()
	{
		MsgGetRoomInfo msg = new MsgGetRoomInfo();
		int count = playerIds.Count;
		msg.players = new PlayerInfo[count];
		//players
		int i = 0;
		foreach (string id in playerIds.Keys)
		{
			Player player = PlayerManager.GetPlayer(id);
			PlayerInfo playerInfo = new PlayerInfo();
			//赋值
			playerInfo.id = player.id;
			playerInfo.camp = player.camp;
			playerInfo.score = player.score;
			playerInfo.isOwner = 0;
			if (isOwner(player))
			{
				playerInfo.isOwner = 1;
			}

			msg.players[i] = playerInfo;
			i++;
		}
		return msg;
	}
	public int Judgement()
	{
		int count1 = 0;
		int count2 = 0;
		foreach (string id in playerIds.Keys)
		{
			Player player = PlayerManager.GetPlayer(id);
			if (!IsDie(player))
			{
				if (player.camp == 1) { count1++; }
				if (player.camp == 2) { count2++; }
			}
		}
		if (count1 <= 0)
		{
			return 2;
		}
		else if (count2 <= 0)
		{
			return 1;
		}
		return 0;
	}
	public bool IsDie(Player player)
	{
		return player.hp <= 0;
	}
	private long lastJudgeTime = 0;

	public void Update()
	{
		//状态判断
		if (status != Status.FIGHT)
		{
			return;
		}
		//时间判断每 gamerTime 检测一次
		if (NetManager.GetTimeStamp() - lastJudgeTime < gameTime)
		{
			return;
		}
		lastJudgeTime = NetManager.GetTimeStamp();
		//胜负判断
		int winCamp = Judgement();
		//尚未分出胜负
		if (winCamp == 0)
		{
			return;
		}
		//某一方胜利，结束战斗
		status = Status.PREPARE;
		//发送战斗结果
		MsgBattleResult msg = new MsgBattleResult();
		int count = playerIds.Count;
		ResultInfo[] resultInfos = new ResultInfo[count];
		int i = 0;
		//统计信息
		foreach (string id in playerIds.Keys)
		{
			Player player = PlayerManager.GetPlayer(id);
			//角色战斗结束的结算信息
			ResultInfo resultInfo = new ResultInfo();

			if (player.camp == winCamp)
			{
				player.data.score += 10;
				resultInfo.addScore = 10;
			}
			else
			{
				player.data.score -= 5;
				resultInfo.addScore = -5;
			}
			//数据库提交
			DbManager.UpdatePlayerData(id, player.data);
			player.score = player.data.score;
			//设置结算界面resultInfo
			resultInfo.id = id;
			resultInfo.score = player.score;
			resultInfo.camp = player.camp;
			//改变玩家准备状态
			player.isReady = false;
			resultInfos[i] = resultInfo;
			i++;
		}
		//设置playerInfo信息与胜利的队伍
		msg.winCamp = winCamp;
		msg.resultInfos = resultInfos;
		Broadcast(msg);
	}
	public bool IsAllReady()
	{
		foreach (string playerId in playerIds.Keys)
		{
			Player player = PlayerManager.GetPlayer(playerId);
			if (player.isReady == false) return false;
		}
		return true;
	}
}

