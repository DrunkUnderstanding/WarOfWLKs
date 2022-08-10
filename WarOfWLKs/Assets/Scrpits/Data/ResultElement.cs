using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HT.InfiniteList;

public class ResultElement : InfiniteListElement
{

	public Text id;
	public Text score;
	public Text addScore;

	private InfiniteListScrollRect _scrollRect;
	private ResultData _data;

	public override void OnUpdateData(InfiniteListScrollRect scrollRect, InfiniteListData data)
	{
		base.OnUpdateData(scrollRect, data);


		_scrollRect = scrollRect;
		_data = data as ResultData;

		if (_data.camp == 1)
		{
			id.text = "<color=red>" + _data.id + "</color>";
		}
		else
		{
			id.text = "<color=blue>" + _data.id + "</color>";
		}

		score.text = _data.score.ToString();
		if (_data.addScore <= 0)
		{
			addScore.text = "<color=red>" + _data.addScore.ToString() + "</color>";
		}
		else
		{
			addScore.text = "<color=green>" + _data.addScore.ToString() + "</color>";
		}
	}

	public override void OnClearData()
	{
		base.OnClearData();

	}
}
