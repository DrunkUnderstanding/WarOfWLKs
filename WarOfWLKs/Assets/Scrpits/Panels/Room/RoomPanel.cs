using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : BasePanel
{
	//开战按钮
	private Button startButton;
	//准备按钮
	private Button readyButton;
	//退出按钮
	private Button closeButton;
	//列表容器
	private Transform content;
	//玩家信息物体
	private GameObject playerObj;
	//玩家准备状态
	private Dictionary<string, Text> playerReadyTexts = new Dictionary<string, Text>();

	//初始化
	public override void OnInit()
	{
		skinPath = "Prefabs/UI/Panels/RoomPanel";
		layer = PanelManager.Layer.Panel;
	}

	//显示
	public override void OnShow(params object[] args)
	{
		//寻找组件
		startButton = skin.transform.Find("CtrlPanel/StartButton").GetComponent<Button>();
		closeButton = skin.transform.Find("CtrlPanel/CloseButton").GetComponent<Button>();
		readyButton = skin.transform.Find("CtrlPanel/ReadyButton").GetComponent<Button>();
		content = skin.transform.Find("ListPanel/Scroll View/Viewport/Content");
		playerObj = skin.transform.Find("Player").gameObject;
		//不激活玩家信息
		playerObj.SetActive(false);
		//按钮事件
		startButton.onClick.AddListener(OnStartClick);
		closeButton.onClick.AddListener(OnCloseClick);
		readyButton.onClick.AddListener(OnReadyClick);
		//协议监听
		NetManager.AddMsgListener("MsgGetRoomInfo", OnMsgGetRoomInfo);
		NetManager.AddMsgListener("MsgLeaveRoom", OnMsgLeaveRoom);
		NetManager.AddMsgListener("MsgStartBattle", OnMsgStartBattle);
		NetManager.AddMsgListener("MsgIsReady", OnMsgIsReady);
		//发送查询
		MsgGetRoomInfo msg = new MsgGetRoomInfo();
		//Debug.Log(string.Format("<color=#ff0000>{0}</color>", "[Send] GetRoomInfo"));
		NetManager.Send(msg);
	}

	//关闭
	public override void OnClose()
	{
		//协议监听
		NetManager.RemoveMsgListener("MsgGetRoomInfo", OnMsgGetRoomInfo);
		NetManager.RemoveMsgListener("MsgLeaveRoom", OnMsgLeaveRoom);
		NetManager.RemoveMsgListener("MsgStartBattle", OnMsgStartBattle);
		NetManager.RemoveMsgListener("MsgIsReady", OnMsgIsReady);
	}

	//收到玩家列表协议
	public void OnMsgGetRoomInfo(MsgBase msgBase)
	{
		MsgGetRoomInfo msg = (MsgGetRoomInfo)msgBase;
		//清除玩家列表
		for (int i = content.childCount - 1; i >= 0; i--)
		{
			GameObject o = content.GetChild(i).gameObject;
			Destroy(o);
		}
		//清除玩家准备列表的信息
		playerReadyTexts.Clear();
		//重新生成列表
		if (msg.players == null)
		{
			return;
		}
		for (int i = 0; i < msg.players.Length; i++)
		{
			GeneratePlayerInfo(msg.players[i]);
		}
	}

	//创建一个玩家信息单元
	public void GeneratePlayerInfo(PlayerInfo playerInfo)
	{
		//创建物体
		GameObject o = Instantiate(playerObj);
		o.transform.SetParent(content);
		o.SetActive(true);
		o.transform.localScale = Vector3.one;
		//获取组件
		Transform trans = o.transform;
		Text idText = trans.Find("IdText").GetComponent<Text>();
		Text campText = trans.Find("CampText").GetComponent<Text>();
		Text scoreText = trans.Find("ScoreText").GetComponent<Text>();
		Text readyText = trans.Find("ReadyText").GetComponent<Text>();
		//生成玩家准备列表的信息
		playerReadyTexts[playerInfo.id] = readyText;
		//填充信息
		idText.text = playerInfo.id;
		if (playerInfo.camp == 1)
		{
			campText.text = "<color=red>Red</color>";
		}
		else
		{
			campText.text = "<color=blue>Blue</color>";
		}
		if (playerInfo.isOwner == 1)
		{
			campText.text = campText.text + " (房主)";
		}
		scoreText.text = playerInfo.score.ToString();
	}

	public void OnMsgIsReady(MsgBase msgBase)
	{
		MsgIsReady msg = (MsgIsReady)msgBase;
		//显示玩家准备好了的信息
		Text text = playerReadyTexts[msg.id];
		text.gameObject.SetActive(msg.isReady);
	}
	//收到退出房间协议
	public void OnMsgLeaveRoom(MsgBase msgBase)
	{
		MsgLeaveRoom msg = (MsgLeaveRoom)msgBase;
		//成功退出房间
		if (msg.result == 0)
		{
			PanelManager.Instance.Open<TipPanel>("退出房间");
			PanelManager.Instance.Open<RoomListPanel>();
			Close();
		}
		//退出房间失败
		else
		{
			PanelManager.Instance.Open<TipPanel>("退出房间失败");
		}
	}

	//点击开战按钮
	public void OnStartClick()
	{
		MsgStartBattle msg = new MsgStartBattle();
		NetManager.Send(msg);
	}
	//点击退出按钮
	public void OnCloseClick()
	{
		MsgLeaveRoom msg = new MsgLeaveRoom();

		LevelManager.Instance.DestoryLevel();
		PanelManager.Instance.Open<RoomListPanel>();

		Camera.main.transform.position = new Vector3(0, 0, -10);
		NetManager.Send(msg);
	}
	public void OnReadyClick()
	{
		//设置玩家已经准备的状态显示
		GameObject text = playerReadyTexts[GameManager.Instance.ctrllerId].gameObject;
		text.SetActive(!text.activeSelf);
		//发送准备消息
		MsgIsReady msg = new MsgIsReady();
		msg.isReady = text.activeSelf;
		NetManager.Send(msg);
	}
	//收到开战返回
	public void OnMsgStartBattle(MsgBase msgBase)
	{
		MsgStartBattle msg = (MsgStartBattle)msgBase;
		//开战
		if (msg.result == 0)
		{

			//等待战斗推送的协议
			Close();
		}
		else if (msg.result == 2)
		{
			PanelManager.Instance.Open<TipPanel>("开战失败！所有人都要准备！");
		}
		//开战失败
		else
		{
			PanelManager.Instance.Open<TipPanel>("开战失败！两队至少都需要一名玩家，只有队长可以开始战斗！");
		}
	}

}
