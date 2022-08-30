using System;
using System.Collections.Generic;
using HeapSort;

public class PlayerManager
{
	//玩家列表
	static Dictionary<string, Player> players = new Dictionary<string, Player>();
	//玩家是否在线
	public static bool IsOnline(string id)
	{
		return players.ContainsKey(id);
	}
	//获取玩家
	public static Player GetPlayer(string id)
	{
		if (players.ContainsKey(id))
		{
			return players[id];
		}
		return null;
	}

	/// <summary>
	/// 获取所有玩家通过Score 排好序的数组
	/// </summary>
	/// <returns>排好序的玩家数组</returns>
	public static PlayerInfo[] GetPlayersByRank()
	{
		Dictionary<string, PlayerData> playerDatas = DbManager.GetPlayerDatas();
		int count = playerDatas.Count;
		PlayerInfo[] playerInfos = new PlayerInfo[count];
		int i = 0;
		foreach (string id in playerDatas.Keys)
		{
			PlayerInfo playerInfo = new PlayerInfo();
			//添加元素
			playerInfo.id = id;
			playerInfo.score = playerDatas[id].score;
			playerInfos[i] = playerInfo;
			i++;
		}
		return SortDictByScore(playerInfos);
	}

	/// <summary>
	/// 通过playerInfo.score 进行降序排序
	/// </summary>
	/// <param name="playerInfos">所有玩家的数据信息playerInfo[]</param>
	/// <returns></returns>
	public static PlayerInfo[] SortDictByScore(PlayerInfo[] playerInfos)
	{
		Heap.HeapSort(playerInfos);
		//Heap.ShowSord(playerInfos);
		return playerInfos;
	}
	//添加玩家
	public static void AddPlayer(string id, Player player)
	{
		players.Add(id, player);
	}
	//删除玩家
	public static void RemovePlayer(string id)
	{
		players.Remove(id);
	}
}



