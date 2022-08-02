using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GamingPanel : BasePanel
{
	private Button skillBtn1;

	private Button skillBtn2;

	private Button skillBtn3;

	private Button skillBtn4;

	private Button settingBtn;
	public override void OnInit()
	{
		skinPath = "Prefabs/UI/Panels/GamingPanel";
		layer = PanelManager.Layer.Panel;
	}
	//显示
	public override void OnShow(params object[] args)
	{
		//寻找组件
		skillBtn1 = skin.transform.Find("SkillPanel/SkillBtn_Q").GetComponent<Button>();
		skillBtn2 = skin.transform.Find("SkillPanel/SkillBtn_Q (1)").GetComponent<Button>();
		skillBtn3 = skin.transform.Find("SkillPanel/SkillBtn_Q (2)").GetComponent<Button>();
		skillBtn4 = skin.transform.Find("SkillPanel/SkillBtn_Q (3)").GetComponent<Button>();
		settingBtn = skin.transform.Find("SettingBtn").GetComponent<Button>();
		//监听
		skillBtn1.onClick.AddListener(OnSkill1Click);
		skillBtn2.onClick.AddListener(OnSkill2Click);
		skillBtn3.onClick.AddListener(OnSkill3Click);
		skillBtn4.onClick.AddListener(OnSkill4Click);
		settingBtn.onClick.AddListener(OnSettingClick);


		//事件监听
		OnPointerEnterBtn += OnStateShow;
		OnPointerExitBtn += OnStateClose;
		AddEventTrigger(skillBtn1.transform, EventTriggerType.PointerEnter, OnPointerEnterBtn);
		AddEventTrigger(skillBtn1.transform, EventTriggerType.PointerExit, OnPointerExitBtn);
		AddEventTrigger(skillBtn2.transform, EventTriggerType.PointerEnter, OnPointerEnterBtn);
		AddEventTrigger(skillBtn2.transform, EventTriggerType.PointerExit, OnPointerExitBtn);
		AddEventTrigger(skillBtn3.transform, EventTriggerType.PointerEnter, OnPointerEnterBtn);
		AddEventTrigger(skillBtn3.transform, EventTriggerType.PointerExit, OnPointerExitBtn);
		AddEventTrigger(skillBtn4.transform, EventTriggerType.PointerEnter, OnPointerEnterBtn);
		AddEventTrigger(skillBtn4.transform, EventTriggerType.PointerExit, OnPointerExitBtn);

	}

	public void OnSkill1Click()
	{
		CtrlActor ctrlActor = GameManager.m_playerSelf;
		ctrlActor.CastReady(ctrlActor.Skills[0]);
	}
	public void OnSkill2Click()
	{

	}
	public void OnSkill3Click()
	{

	}
	public void OnSkill4Click()
	{

	}
	public void OnSettingClick()
	{
		PanelManager.Instance.Open<SettingPanel>();
		Close();
	}


	private UnityAction<BaseEventData> OnPointerEnterBtn;
	private UnityAction<BaseEventData> OnPointerExitBtn;
	private void OnStateShow(BaseEventData p)
	{
		Show();
	}
	private void OnStateClose(BaseEventData p)
	{
		PanelManager.Instance.Close("StatesPanel");
	}
	private void Show()
	{
		PanelManager.Instance.Open<StatesPanel>(LocalizationMgr.Instance.GetWordByDirect(4));

	}
}
