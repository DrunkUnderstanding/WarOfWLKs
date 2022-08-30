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

	//技能 id
	public int skillId = 0;
	//移动的方向
	public float x = 0f;
	public float y = 0f;

	//服务器补充
	public string id = "";
}

//击中
public class MsgHit : MsgBase
{
	public MsgHit() { protoName = "MsgHit"; }
	//客户端补充
	//击中谁
	public string targetId = "";
	//技能ID
	public int skillId = 0;
	//被击中角色血量
	public int hp = 0;
	//击中点	
	public float x = 0f;
	public float y = 0f;
	//受到的伤害
	public int damage = 0;

	//服务端补充
	public string id = "";      //哪个角色攻击


}

public class MsgBurned : MsgBase
{
	public MsgBurned() { protoName = "MsgBurned"; }
	public string id = "";  //哪个人被烧
	public float damage = 0; //受到的伤害
	public int hp = 0;          //被烧的人的血量
}
public class MsgDie : MsgBase
{
	public MsgDie() { protoName = "MsgDie"; }

	public string id = "";
}