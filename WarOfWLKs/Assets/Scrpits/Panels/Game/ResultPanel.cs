using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResultPanel : BasePanel
{
	//胜利提示文字
	private Text winText;
	
	//失败提示文字
	private Text lostText;

	//确定按钮
	private Button okBtn;

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
		//监听
		okBtn.onClick.AddListener(OnOkClick);
		//显示哪个图片
		if (args.Length == 1)
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
		}
	}
	//关闭
	public override void OnClose()
	{

	}

	//当按下确定按钮
	public void OnOkClick()
	{
		PanelManager.Instance.Open<RoomPanel>();
		Close();
	}
}
