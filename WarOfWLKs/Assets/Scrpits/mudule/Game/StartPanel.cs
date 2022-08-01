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
		trailBtn.onClick.AddListener(OnTrailClick);
		joinRoomBtn.onClick.AddListener(OnJoinRoomClick);
		settingBtn.onClick.AddListener(OnSettingClick);
		quitGameBtn.onClick.AddListener(OnLogOutClick);
	}

	public override void OnClose()
	{

	}
	public void OnTrailClick()
	{
		//开始游戏
		GameManager.Instance.StartGame();
		PanelManager.Instance.Open<GamingPanel>();
		Close();
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
		Close();
	}
	public void OnLogOutClick()
	{
		PanelManager.Instance.Open<LoginPanel>();
		Close();
		//发送下线消息
	}

}
