using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BasePanel
{
	//账号输入框
	private InputField idInput;
	//密码输入框
	private InputField pwInput;
	//登陆按钮
	private Button loginBtn;
	//注册按钮
	private Button regBtn;
	//语言选项
	private Dropdown languageDropdown;

	//初始化
	public override void OnInit()
	{
		skinPath = "Prefabs/UI/Panels/LoginPanel";
		layer = PanelManager.Layer.Panel;
		SoundManager.Instance.LoadVolume();
	}

	//显示
	public override void OnShow(params object[] args)
	{
		//寻找组件
		idInput = skin.transform.Find("IdInput").GetComponent<InputField>();
		pwInput = skin.transform.Find("PwInput").GetComponent<InputField>();
		loginBtn = skin.transform.Find("LoginBtn").GetComponent<Button>();
		regBtn = skin.transform.Find("RegisterBtn").GetComponent<Button>();
		languageDropdown = skin.transform.Find("LanguageDropdown").GetComponent<Dropdown>();
		//初始化LanguageDropdown的值
		languageDropdown.value = GameManager.curLanguage;
		//监听
		loginBtn.onClick.AddListener(OnLoginClick);
		regBtn.onClick.AddListener(OnRegClick);
		languageDropdown.onValueChanged.AddListener(OnChangeLanguage);
		//网络协议监听
		NetManager.AddMsgListener("MsgLogin", OnMsgLogin);
		//网络事件监听
		NetManager.AddEventListener(NetManager.NetEvent.ConnectSucc, OnConnectSucc);
		NetManager.AddEventListener(NetManager.NetEvent.ConnectFail, OnConnectFail);
		//连接服务器
		NetManager.Connect("192.168.96.154", 8888);
	}

	//关闭
	public override void OnClose()
	{
		//网络协议监听
		NetManager.RemoveMsgListener("MsgLogin", OnMsgLogin);
		//网络事件监听
		NetManager.RemoveEventListener(NetManager.NetEvent.ConnectSucc, OnConnectSucc);
		NetManager.RemoveEventListener(NetManager.NetEvent.ConnectFail, OnConnectFail);
	}


	//连接成功回调
	void OnConnectSucc(string err)
	{
		Debug.Log("OnConnectSucc");
	}

	//连接失败回调
	void OnConnectFail(string err)
	{
		PanelManager.Instance.Open<TipPanel>(err);
	}


	public void OnChangeLanguage(int language)
	{
		GameManager.curLanguage = language;
		// Debug.Log("ChangeLanguage:" + language);
		// var temp=language;
		// temp=language==0?10:10;
		// temp=language==1?40:10;
		// temp=language==2?22:10;
		if (language == 0)
		{
			language = 10;
		}
		else if (language == 1) { language = 40; }
		else if (language == 2) { language = 22; }

		//Debug.Log("ChangeLanguage after:" + language);
		LocalizationMgr.Instance.ChangeLanguage((SystemLanguage)language);
	}

	//当按下注册按钮
	public void OnRegClick()
	{
		PanelManager.Instance.Open<RegisterPanel>();
	}



	//当按下登陆按钮
	public void OnLoginClick()
	{

		
		//用户名密码为空
		if (idInput.text == "" || pwInput.text == "")
		{
			PanelManager.Instance.Open<TipPanel>("用户名和密码不能为空");
			return;
		}
		//发送
		MsgLogin msgLogin = new MsgLogin();
		msgLogin.id = idInput.text;
		msgLogin.pw = pwInput.text;
		NetManager.Send(msgLogin);
	}

	//收到登陆协议
	public void OnMsgLogin(MsgBase msgBase)
	{
		MsgLogin msg = (MsgLogin)msgBase;
		//Debug.Log(msgBase + " " + msg.result);
		if (msg.result == 0)
		{
			Debug.Log("登陆成功");
			//进入游戏
			//添加坦克
			/*			GameObject tankObj = new GameObject("myTank");
						CtrlTank ctrlTank = tankObj.AddComponent<CtrlTank>();
						ctrlTank.Init("tankPrefab");*/
			//设置相机
			/*			tankObj.AddComponent<CameraFollow>();*/
			//设置id
			/*			GameMain.id = msg.id;*/
			//关闭界面
			GameManager.Instance.ctrllerId = msg.id;
			PanelManager.Instance.Open<StartPanel>();
			Close();
		}
		else
		{
			PanelManager.Instance.Open<TipPanel>("登陆失败");
		}
	}

}
