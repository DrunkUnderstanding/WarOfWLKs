using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//引入无限滑动复用的脚本
using HT.InfiniteList;

public class RankPanel : BasePanel
{
	public InfiniteListScrollRect infiniteList;

	public Button closeButton;



	public override void OnInit()
	{
		skinPath = "Prefabs/UI/Panels/RankPanel";
		layer = PanelManager.Layer.Panel;
	}

	public override void OnShow(params object[] args)
	{
		//寻找组件
		closeButton = skin.transform.Find("CloseButton").GetComponent<Button>();
		infiniteList = skin.transform.Find("InfiniteList").GetComponent<InfiniteListScrollRect>();
		//监听
		infiniteList.onValueChanged.AddListener(OnScollChanged);
		closeButton.onClick.AddListener(OnCloseClick);
		//信息更改
		PlayerInfo[] players;
		//玩家排名信息
		if (args.Length != 0)
		{
			players = (PlayerInfo[])args;
			InitRank(players);
		}
	}
	//初始化排行榜信息
	public void InitRank(PlayerInfo[] players)
	{
		if (players.Length == 0) return;
		List<RankData> datas = new List<RankData>();
		for (int i = 0; i < players.Length; i++)
		{
			RankData data = new RankData();
			data.rank = i + 1;
			data.id = players[i].id;
			data.score = players[i].score;
			datas.Add(data);
		}
		infiniteList.AddDatas(datas);
	}
	public void OnCloseClick()
	{
		Close();
	}
	public void OnScollChanged(Vector2 value)
	{
		Debug.Log(value);
	}
}
