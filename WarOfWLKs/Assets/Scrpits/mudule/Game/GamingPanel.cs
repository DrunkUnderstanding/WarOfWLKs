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
		OnEnterBtn += OnStateShow;
		OnExitBtn += OnStateClose;
		AddEventTrigger(skillBtn1.transform, EventTriggerType.PointerEnter, OnEnterBtn);
		AddEventTrigger(skillBtn1.transform, EventTriggerType.PointerExit, OnExitBtn);
		AddEventTrigger(skillBtn2.transform, EventTriggerType.PointerEnter, OnEnterBtn);
		AddEventTrigger(skillBtn2.transform, EventTriggerType.PointerExit, OnExitBtn);
		AddEventTrigger(skillBtn3.transform, EventTriggerType.PointerEnter, OnEnterBtn);
		AddEventTrigger(skillBtn3.transform, EventTriggerType.PointerExit, OnExitBtn);
		AddEventTrigger(skillBtn4.transform, EventTriggerType.PointerEnter, OnEnterBtn);
		AddEventTrigger(skillBtn4.transform, EventTriggerType.PointerExit, OnExitBtn);

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

	/*为eventTrigger添加事件(参数1:添加事件的物体;参数2:事件类型;参数3:需要调用的事件函数)*/
	public void AddEventTrigger(Transform insObject, EventTriggerType eventType, UnityAction<BaseEventData> myFunction)//泛型委托
	{
		EventTrigger eventTri = insObject.GetComponent<EventTrigger>();

		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = eventType;

		entry.callback.AddListener(myFunction);
		eventTri.triggers.Add(entry);
	}

	private UnityAction<BaseEventData> OnEnterBtn;
	private UnityAction<BaseEventData> OnExitBtn;
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
		PanelManager.Instance.Open<StatesPanel>("Fire!!!!!!");

	}
}
