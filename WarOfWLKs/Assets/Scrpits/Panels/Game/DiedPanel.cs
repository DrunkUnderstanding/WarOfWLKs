using UnityEngine.UI;

public class DiedPanel : BasePanel
{
	//死亡文本
	private Text diedText;

	//跟随者文本
	private Text followerText;

	public override void OnInit()
	{
		skinPath = "Prefabs/UI/Panels/DiedPanel";
		layer = PanelManager.Layer.Tip;
		//LocalizationMgr.Instance.ChangeLanguage((SystemLanguage)GameManager.curLanguage);
	}
	public override void OnShow(params object[] para)
	{
		diedText = skin.transform.Find("DiedText").GetComponent<Text>();
		followerText = skin.transform.Find("FollowerText").GetComponent<Text>();

	}

	public void ChangeFollowerText(string id)
	{
		followerText.text = "FOLLOWER: " + id;
	}
}