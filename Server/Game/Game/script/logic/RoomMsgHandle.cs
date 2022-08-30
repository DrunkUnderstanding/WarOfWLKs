using System;
using System.Collections.Generic;

public partial class MsgHandler
{

	//查询战绩
	public static void MsgGetAchieve(ClientState c, MsgBase msgBase)
	{
		MsgGetAchieve msg = (MsgGetAchieve)msgBase;
		Player player = c.player;
		if (player == null) return;

		msg.score = player.score;

		player.Send(msg);
	}


	//请求房间列表
	public static void MsgGetRoomList(ClientState c, MsgBase msgBase)
	{
		MsgGetRoomList msg = (MsgGetRoomList)msgBase;
		Player player = c.player;
		if (player == null) return;

		player.Send(RoomManager.ToMsg());


	}

	//创建房间
	public static void MsgCreateRoom(ClientState c, MsgBase msgBase)
	{
		MsgCreateRoom msg = (MsgCreateRoom)msgBase;
		Player player = c.player;
		if (player == null) return;
		//已经在房间里
		if (player.roomId >= 0)
		{
			msg.result = 1;
			player.Send(msg);
			return;
		}
		//创建
		Room room = RoomManager.AddRoom();
		room.AddPlayer(player.id);

		msg.result = 0;
		player.Send(msg);
	}

	//进入房间
	public static void MsgEnterRoom(ClientState c, MsgBase msgBase)
	{
		MsgEnterRoom msg = (MsgEnterRoom)msgBase;
		Player player = c.player;
		if (player == null) return;
		//已经在房间里
		if (player.roomId >= 0)
		{
			msg.result = 1;
			player.Send(msg);
			return;
		}
		//获取房间
		Room room = RoomManager.GetRoom(msg.id);
		if (room == null)
		{
			msg.result = 1;
			player.Send(msg);
			return;
		}
		//进入
		if (!room.AddPlayer(player.id))
		{
			msg.result = 1;
			player.Send(msg);
			return;
		}
		//返回协议	
		msg.result = 0;
		player.Send(msg);
	}

	public static void MsgGetRank(ClientState c, MsgBase msgBase)
	{
		MsgGetRank msg = (MsgGetRank)msgBase;
		Player player = c.player;
		if (player == null) return;
		PlayerInfo[] playerInfos = PlayerManager.GetPlayersByRank();

		msg.playerInfos = playerInfos;
		NetManager.Send(c, msg);
	}
	//获取房间信息
	public static void MsgGetRoomInfo(ClientState c, MsgBase msgBase)
	{
		MsgGetRoomInfo msg = (MsgGetRoomInfo)msgBase;
		Player player = c.player;
		if (player == null) return;

		//判断玩家是否在该房间中，不在房间时player.roomId=-1
		Room room = RoomManager.GetRoom(player.roomId);
		//不存在该房间
		if (room == null)
		{
			player.Send(msg);
			return;
		}

		player.Send(room.ToMsg());
	}

	//离开房间
	public static void MsgLeaveRoom(ClientState c, MsgBase msgBase)
	{
		MsgLeaveRoom msg = (MsgLeaveRoom)msgBase;
		Player player = c.player;
		if (player == null) return;

		Room room = RoomManager.GetRoom(player.roomId);
		if (room == null)
		{
			msg.result = 1;
			player.Send(msg);
			return;
		}

		room.RemovePlayer(player.id);
		//返回协议
		msg.result = 0;
		player.Send(msg);
	}
	public static void MsgIsReady(ClientState c, MsgBase msgBase)
	{
		MsgIsReady msg = (MsgIsReady)msgBase;
		Player player = c.player;
		if (player == null)
		{
			return;
		}
		Room room = RoomManager.GetRoom(player.roomId);
		if (room == null)
		{
			return;
		}
		player.isReady = msg.isReady;
		msg.id = player.id;
		room.BroadcastExcept(msg, player.id);
	}
	//请求开始战斗
	public static void MsgStartBattle(ClientState c, MsgBase msgBase)
	{
		MsgStartBattle msg = (MsgStartBattle)msgBase;
		Player player = c.player;
		if (player == null) return;
		//room
		Room room = RoomManager.GetRoom(player.roomId);
		if (room == null)
		{
			msg.result = 1;
			player.Send(msg);
			return;
		}
		//是否是房主
		if (!room.isOwner(player))
		{
			msg.result = 1;
			player.Send(msg);
			return;
		}
		//玩家是否全部都已经准备
		player.isReady = true;
		if (!room.IsAllReady())
		{
			Console.WriteLine("房间:" + room.id.ToString() + "启动游戏失败，有玩家未准备");
			msg.result = 2;
			player.Send(msg);
			return;
		}
		//开战
		if (!room.StartBattle())
		{
			msg.result = 1;
			player.Send(msg);
			return;
		}
		//成功
		msg.result = 0;
		player.Send(msg);
	}
}


