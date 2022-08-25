using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HT.InfiniteList;
public class ResultPanel : BasePanel
{
	//胜利提示文字
	private Text winText;

	//失败提示文字
	private Text lostText;

	//确定按钮
	private Button okBtn;

	//结算无限列表
	private InfiniteListScrollRect infiniteList;

	public override void OnInit()
	{
		skinPath = "Prefabs/UI/Panels/ResultPanel";
		layer = PanelManager.Layer.Tip;
	}

	public override void OnShow(params object[] args)
	{
		//寻找组件
		winText = skin.transform.Find("WinText").GetComponent<Text>();
		lostText = skin.transform.Find("LostText").GetComponent<Text>();
		okBtn = skin.transform.Find("OkBtn").GetComponent<Button>();
		infiniteList = skin.transform.Find("RankPanel/InfiniteList").GetComponent<InfiniteListScrollRect>();
		//监听
		okBtn.onClick.AddListener(OnOkClick);
		//显示哪个图片
		//MsgBattleResult msg = (MsgBattleResult)args[0];
		if (args.Length != 0)
		{
			bool isWin = (bool)args[0];
			if (isWin)
			{
				winText.gameObject.SetActive(true);
				lostText.gameObject.SetActive(false);
			}
			else
			{
				winText.gameObject.SetActive(false);
				lostText.gameObject.SetActive(true);
			}
			ResultInfo[] resultInfos = (ResultInfo[])args[1];
			InitResult(resultInfos);
		}
		//int count = args.Length - 1;
		//ResultInfo[] resultInfos = (ResultInfo[])args[1];

/*		for (int i = 1; i < args.Length; i++)
		{
			resultInfos[i] = (ResultInfo)args[i];
		}*/
		//InitResult(resultInfos);
	}
	//关闭
	public override void OnClose()
	{

	}
	//初始化排行榜信息
	public void InitResult(ResultInfo[] results)
	{
		if (results.Length == 0) return;
		List<ResultData> datas = new List<ResultData>();
		for (int i = 0; i < results.Length; i++)
		{
			ResultData data = new ResultData();
			data.camp = results[i].camp;
			data.id = results[i].id;
			data.score = results[i].score;
			data.addScore = results[i].addScore;
			datas.Add(data);
		}
		infiniteList.AddDatas(datas);
	}
	//当按下确定按钮
	public void OnOkClick()
	{
		PanelManager.Instance.Open<RoomPanel>();
		BattleManager.Instance.DestoryActors();
		//LevelManager.Instance.DestoryLevel();
		PanelManager.Instance.Close("GamingPanel");
		PanelManager.Instance.Close("SettingPanel");
		//发送查询
		MsgGetRoomInfo msg = new MsgGetRoomInfo();
		//Debug.Log(string.Format("<color=#ff0000>{0}</color>", "[Send] GetRoomInfo"));
		NetManager.Send(msg);

		Close();
	}
}
