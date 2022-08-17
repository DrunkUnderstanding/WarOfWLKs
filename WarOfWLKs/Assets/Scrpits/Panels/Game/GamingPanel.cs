using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using zFrame.UI;
public class GamingPanel : BasePanel
{
	private Button skillButton0;

	private Button skillButton1;

	private Button skillButton2;

	private Button skillButton3;

	private SkillSlot skillSlot0;

	private SkillSlot skillSlot1;

	private SkillSlot skillSlot2;

	private SkillSlot skillSlot3;

	private Button settingBtn;

	private Joystick joystick;

	public override void OnInit()
	{
		skinPath = "Prefabs/UI/Panels/GamingPanel";
		layer = PanelManager.Layer.Panel;
	}
	//显示
	public override void OnShow(params object[] args)
	{
		//寻找组件
		skillButton0 = skin.transform.Find("SkillPanel/SkillButton").GetComponent<Button>();
		skillButton1 = skin.transform.Find("SkillPanel/SkillButton(1)").GetComponent<Button>();
		skillButton2 = skin.transform.Find("SkillPanel/SkillButton(2)").GetComponent<Button>();
		skillButton3 = skin.transform.Find("SkillPanel/SkillButton(3)").GetComponent<Button>();

		skillSlot0 = skin.transform.Find("SkillPanel/SkillButton").GetComponent<SkillSlot>();
		skillSlot1 = skin.transform.Find("SkillPanel/SkillButton(1)").GetComponent<SkillSlot>();
		skillSlot2 = skin.transform.Find("SkillPanel/SkillButton(2)").GetComponent<SkillSlot>();
		skillSlot3 = skin.transform.Find("SkillPanel/SkillButton(3)").GetComponent<SkillSlot>();

		settingBtn = skin.transform.Find("SettingBtn").GetComponent<Button>();
		joystick = skin.transform.Find("JoyStick").GetComponent<Joystick>();
		//往控制角色的技能槽添加技能
		CtrlActor ctrlActor = GameManager.Instance.PlayerSelf.GetComponent<CtrlActor>();
		skillSlot0.skill = ctrlActor.Skills[0];
		skillSlot1.skill = ctrlActor.Skills[1];
		skillSlot2.skill = ctrlActor.Skills[0];
		skillSlot3.skill = ctrlActor.Skills[0];
		//添加控制监听
		ctrlActor.SetJoyStick(joystick);
		//监听
		skillButton0.onClick.AddListener(OnSkill0Click);
		skillButton1.onClick.AddListener(OnSkill1Click);
		skillButton2.onClick.AddListener(OnSkill2Click);
		skillButton3.onClick.AddListener(OnSkill3Click);
		settingBtn.onClick.AddListener(OnSettingClick);


		//事件监听（鼠标移动到技能图标上的事件）
		OnPointerEnterSlot0 += OnStateShow0;
		OnPointerEnterSlot1 += OnStateShow1;
		OnPointerEnterSlot2 += OnStateShow2;
		OnPointerEnterSlot3 += OnStateShow3;
		OnPointerExitSlot += OnStateClose;
		AddEventTrigger(skillButton0.transform, EventTriggerType.PointerEnter, OnPointerEnterSlot0);
		AddEventTrigger(skillButton0.transform, EventTriggerType.PointerExit, OnPointerExitSlot);
		AddEventTrigger(skillButton1.transform, EventTriggerType.PointerEnter, OnPointerEnterSlot1);
		AddEventTrigger(skillButton1.transform, EventTriggerType.PointerExit, OnPointerExitSlot);
		AddEventTrigger(skillButton2.transform, EventTriggerType.PointerEnter, OnPointerEnterSlot2);
		AddEventTrigger(skillButton2.transform, EventTriggerType.PointerExit, OnPointerExitSlot);
		AddEventTrigger(skillButton3.transform, EventTriggerType.PointerEnter, OnPointerEnterSlot3);
		AddEventTrigger(skillButton3.transform, EventTriggerType.PointerExit, OnPointerExitSlot);

	}

	private UnityAction<BaseEventData> OnPointerEnterSlot0;
	private UnityAction<BaseEventData> OnPointerEnterSlot1;
	private UnityAction<BaseEventData> OnPointerEnterSlot2;
	private UnityAction<BaseEventData> OnPointerEnterSlot3;
	private UnityAction<BaseEventData> OnPointerExitSlot;
	public void OnSkill0Click()
	{
		CtrlActor ctrlActor = GameManager.Instance.PlayerSelf.GetComponent<CtrlActor>();
		ctrlActor.ReadySkill = skillSlot0.skill;
		ctrlActor.CastReady(ctrlActor.ReadySkill);
	}
	public void OnSkill1Click()
	{
		CtrlActor ctrlActor = GameManager.Instance.PlayerSelf.GetComponent<CtrlActor>();
		ctrlActor.ReadySkill = skillSlot1.skill;
		ctrlActor.CastReady(ctrlActor.ReadySkill);
	}
	public void OnSkill2Click()
	{

	}
	public void OnSkill3Click()
	{

	}


	private void OnStateShow0(BaseEventData p)
	{
		Show(0);
	}
	private void OnStateShow1(BaseEventData p)
	{
		Show(1);
	}
	private void OnStateShow2(BaseEventData p)
	{
		Show(0);
	}
	private void OnStateShow3(BaseEventData p)
	{
		Show(0);
	}
	private void OnStateClose(BaseEventData p)
	{
		PanelManager.Instance.Close("StatesPanel");
	}
	private void Show(int showId)
	{
		switch (showId)
		{
			case 0:
				{
					PanelManager.Instance.Open<StatesPanel>(LocalizationMgr.Instance.GetWordByDirect(skillSlot0.skill.translationNum));
					break;
				}
			case 1:
				{
					PanelManager.Instance.Open<StatesPanel>(LocalizationMgr.Instance.GetWordByDirect(skillSlot1.skill.translationNum));
					break;
				}
		}
	}

	public void OnSettingClick()
	{
		PanelManager.Instance.Open<SettingPanel>();
		Close();
	}
}
