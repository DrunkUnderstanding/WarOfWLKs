using System;


public partial class MsgHandler
{
	public static void MsgMove(ClientState c, MsgBase msgBase)
	{
		MsgMove msgMove = (MsgMove)msgBase;
		/*		Console.WriteLine(msgMove.x);
				msgMove.x++;*/
/*		int roomId = c.player.roomId;
		Room room = RoomManager.GetRoom(roomId);
		room.Broadcast(msgMove);*/
		NetManager.Send(c, msgMove);
	}


}


