
public class MsgMove : MsgBase
{
	public MsgMove() { protoName = "MsgMove"; }

	public int x = 0;
	public int y = 0;
	public int z = 0;
}


public class MsgAttack : MsgBase
{
	public MsgAttack() { protoName = "MsgAttack"; }

	public string desc = "127.0.0.1:6543";
}

[System.Serializable]
public class ActorInfo
{
	public string id = "";
	public int camp = 0;
	public int hp = 0;

	//战场信息
	public float x = 0;
	public float y = 0;

}

//进入战场（服务器）
public class MsgEnterBattle : MsgBase
{
	public MsgEnterBattle() { protoName = "MsgEnterBattle"; }

	public ActorInfo[] actors;

	//地图ID 之后可以设计地图切换
	public int mapId = 1;
}

//战斗结果（服务器）
public class MsgBattleResult : MsgBase
{
	public MsgBattleResult() { protoName = "MsgBattleResult"; }

	//获胜的阵营
	public int winCamp = 0;
}

//退出战斗（一般为意外）
public class MsgLeaveBattle : MsgBase
{
	public MsgLeaveBattle() { protoName = "MsgLeaveBattle"; }

	//退出的玩家ID
	public string id = "";
}
