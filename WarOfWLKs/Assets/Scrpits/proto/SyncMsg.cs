//同步玩家信息
public class MsgSyncActor : MsgBase
{
	public MsgSyncActor() { protoName = "MsgSyncActor"; }

	public float x = 0f;
	public float y = 0f;

	//服务器补充
	public string id = "";
}

//开火
public class MsgSkill : MsgBase
{
	public MsgSkill() { protoName = "MsgSkill"; }

	//移动的方向
	public float VecX = 0f;
	public float VecY = 0f;

	//服务器补充
	public string id = "";
}

//击中
public class MsgHit : MsgBase
{
	public MsgHit() { protoName = "MsgHit"; }
	//击中谁
	public string targetId = "";
	//击中点	
	public float x = 0f;
	public float y = 0f;


	//服务端补充
	public string id = "";      //哪个坦克攻击
	public int hp = 0;          //被击中坦克血量
	public int damage = 0;      //受到的伤害
}