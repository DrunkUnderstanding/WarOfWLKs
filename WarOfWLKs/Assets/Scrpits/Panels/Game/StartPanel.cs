using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : BasePanel
{
	//新手试玩
	private Button trailBtn;

	//加入房间
	private Button joinRoomBtn;

	//设置
	private Button settingBtn;

	//退出游戏
	private Button quitGameBtn;

	public override void OnInit()
	{
		skinPath = "Prefabs/UI/Panels/StartPanel";
		layer = PanelManager.Layer.Panel;
		//LocalizationMgr.Instance.ChangeLanguage((SystemLanguage)GameManager.curLanguage);
	}
	public override void OnShow(params object[] para)
	{
		trailBtn = skin.transform.Find("PanelImage/Panel/TrailBtn").GetComponent<Button>();
		joinRoomBtn = skin.transform.Find("PanelImage/Panel/JoinRoomBtn").GetComponent<Button>();
		settingBtn = skin.transform.Find("PanelImage/Panel/SettingBtn").GetComponent<Button>();
		quitGameBtn = skin.transform.Find("PanelImage/Panel/QuitGameBtn").GetComponent<Button>();

		//监听
		trailBtn.onClick.AddListener(OnIntroClick);
		joinRoomBtn.onClick.AddListener(OnJoinRoomClick);
		settingBtn.onClick.AddListener(OnSettingClick);
		quitGameBtn.onClick.AddListener(OnLogoutClick);

		//网络监听
		NetManager.AddMsgListener("MsgLogout", OnMsgLogout);
	}

	public override void OnClose()
	{
		NetManager.RemoveMsgListener("MsgLogout", OnMsgLogout);
	}
	public void OnIntroClick()
	{
		//开始游戏
		PanelManager.Instance.Open<TipPanel>("这是个往别人脸上糊球球的游戏");
	}
	public void OnJoinRoomClick()
	{
		//打开加入房间面板
		PanelManager.Instance.Open<RoomListPanel>();
		Close();
	}
	public void OnSettingClick()
	{
		//打开设置面板
		PanelManager.Instance.Open<SettingPanel>();
		//Close();
	}
	public void OnLogoutClick()
	{
		//发送下线消息
		MsgLogout msg = new MsgLogout();
		msg.id = GameManager.Instance.ctrllerId;
		NetManager.Send(msg);
	}

	//登出消息返回
	public void OnMsgLogout(MsgBase msgBase)
	{
		MsgLogout msg = (MsgLogout)msgBase;
		if (msg.result == 1)
		{
			PanelManager.Instance.Open<TipPanel>("登出失败！");
			return;
		}
		//关闭连接
		NetManager.Close();
		PanelManager.Instance.Open<LoginPanel>();
		PanelManager.Instance.Open<TipPanel>("登出成功！");

		Close();
	}
}
