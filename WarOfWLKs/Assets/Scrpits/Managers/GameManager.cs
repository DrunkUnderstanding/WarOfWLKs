using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
	public string ctrllerId = "";

	//初始语言默认简中
	public static int curLanguage = 1;

	[SerializeField]
	private GameObject m_playerSelf;

	[SerializeField]
	private GameObject m_diedPanel;

	[SerializeField]
	private Button m_reBirthBtn;

	[SerializeField]
	private Camera m_mainCamera;

	public ObjectPool Pool { get; set; }
	public GameObject PlayerSelf { get => m_playerSelf; set => m_playerSelf = value; }
	public bool IsPlaying { get => b_isPlaying; set => b_isPlaying = value; }

	private bool b_isPlaying = false;


	private void Awake()
	{
		Pool = GetComponent<ObjectPool>();

		LocalizationMgr.Instance.Init();
	}

	// Start is called before the first frame update
	void Start()
	{
		//网络监听
		NetManager.AddEventListener(NetManager.NetEvent.Close, OnConnectClose);
		NetManager.AddMsgListener("MsgKick", OnMsgKick);
		//初始化
		PanelManager.Instance.Init();
		BattleManager.Instance.Init();
		//打开登陆面板
		PanelManager.Instance.Open<LoginPanel>();
	}
	public void RebirthClick()
	{

		PlayerSelf.GetComponent<CtrlActor>().Rebirth();
	}
	public void OnClickQ()
	{
		CtrlActor actor = PlayerSelf.GetComponent<CtrlActor>();
		if (actor.Skills[0].IsCoolDown)
		{
			actor.CastReady(actor.Skills[0]);
		}

	}

	/// <summary>
	/// 显示死亡提示
	/// </summary>
	public void ShowDie(bool setShow)
	{
		m_diedPanel.SetActive(setShow);
		m_reBirthBtn.gameObject.SetActive(setShow);
	}
	// Update is called once per frame


	public void StartGame()
	{
		LevelManager.Instance.CreateLevel();
		IsPlaying = true;
		//m_skillPanel.SetActive(true);
		//m_setBtn.gameObject.SetActive(true);
		m_mainCamera.gameObject.SetActive(true);

		CameraMovement.Instance.Init();

		PlayerSelf = GameObject.FindGameObjectWithTag("Player1");

		//m_startMenu.SetActive(false);
	}

	/*	public void QuitGame()
		{
			Application.Quit();
		}

		public void JoinRoom()
		{

		}*/
	void Update()
	{
		NetManager.Update();
	}
	void OnMsgKick(MsgBase msgBase)
	{
		PanelManager.Instance.Open<TipPanel>("被踢下线");
	}
	//关闭连接
	void OnConnectClose(string err)
	{
		Debug.Log("断开连接");
	}
}
