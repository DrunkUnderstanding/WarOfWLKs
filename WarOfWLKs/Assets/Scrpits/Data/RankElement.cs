using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HT.InfiniteList;

public class RankElement : InfiniteListElement
{
	public Text rank;
	public Text id;
	public Text score;

	private InfiniteListScrollRect _scrollRect;
	private RankData _data;


	public override void OnUpdateData(InfiniteListScrollRect scrollRect, InfiniteListData data)
	{
		base.OnUpdateData(scrollRect, data);

		_scrollRect = scrollRect;
		_data = data as RankData;

		rank.text = _data.rank.ToString();
		id.text = _data.id;
		score.text = _data.score.ToString();
	}

	public override void OnClearData()
	{
		base.OnClearData();

	}
}
