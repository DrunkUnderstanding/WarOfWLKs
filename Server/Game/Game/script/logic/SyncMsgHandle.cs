using System;


public partial class MsgHandler
{
	public static void MsgDie(ClientState c,MsgBase msgBase)
	{
		MsgDie msg = (MsgDie)msgBase;
		Player player = c.player;
		if (player == null) return;
		//targetPlayer
		Player diedPlayer = PlayerManager.GetPlayer(msg.id);
		if (diedPlayer == null)
		{
			return;
		}
		Room room = RoomManager.GetRoom(player.roomId);
		if (room==null)
		{
			return;
		}
		if (room.status != Room.Status.FIGHT)
		{
			return;
		}
		diedPlayer.hp = 0;
		room.Broadcast(msg);
	}
	//同步位置协议
	public static void MsgSyncActor(ClientState c, MsgBase msgBase)
	{
		//Console.WriteLine("SyncAcotr");
		MsgSyncActor msg = (MsgSyncActor)msgBase;
		Player player = c.player;
		if (player == null) return;
		//room
		Room room = RoomManager.GetRoom(player.roomId);
		if (room == null)
		{
			return;
		}
		//status
		if (room.status != Room.Status.FIGHT)
		{
			return;
		}
/*		//是否作弊
		if (Math.Abs(player.x - msg.x) > 5 ||
			Math.Abs(player.y - msg.y) > 5)
		{
			Console.WriteLine("疑似作弊 " + player.id);
		}*/
		//更新信息
		player.x = msg.x;
		player.y = msg.y;

		//广播
		msg.id = player.id;
		room.Broadcast(msg);
	}

	//技能协议
	public static void MsgSkill(ClientState c, MsgBase msgBase)
	{
		MsgSkill msg = (MsgSkill)msgBase;
		Player player = c.player;
		if (player == null) return;
		//room
		Room room = RoomManager.GetRoom(player.roomId);
		if (room == null)
		{
			return;
		}
		//status
		if (room.status != Room.Status.FIGHT)
		{
			return;
		}
		//广播
		msg.id = player.id;
		room.Broadcast(msg);
	}

	//击中协议
	public static void MsgHit(ClientState c, MsgBase msgBase)
	{
		MsgHit msg = (MsgHit)msgBase;
		Player player = c.player;
		if (player == null) return;
		//targetPlayer
		Player targetPlayer = PlayerManager.GetPlayer(msg.targetId);
		if (targetPlayer == null)
		{
			return;
		}
		//room
		Room room = RoomManager.GetRoom(player.roomId);
		if (room == null)
		{
			return;
		}
		//status
		if (room.status != Room.Status.FIGHT)
		{
			return;
		}
		//发送者校验
		if (player.id != msg.id)
		{
			return;
		}
		//状态
		int damage = msg.damage;
		targetPlayer.hp -= damage;
		//广播
		msg.id = player.id;
		msg.hp = player.hp;

		room.BroadcastExcept(msg, msg.id);

	}

}


