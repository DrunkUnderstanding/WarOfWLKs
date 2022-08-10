using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
	private Button closeBtn;

	private Slider musicSlider;

	private Slider sfxSlider;

	private Dropdown languageDropdown;

	//private Button backBtn;

	//初始化
	public override void OnInit()
	{
		skinPath = "Prefabs/UI/Panels/SettingPanel";
		layer = PanelManager.Layer.Panel;

	}

	public override void OnShow(params object[] para)
	{
		closeBtn = skin.transform.Find("CloseBtn").GetComponent<Button>();
		musicSlider = skin.transform.Find("MusicSlider").GetComponent<Slider>();
		sfxSlider = skin.transform.Find("SFXSlider").GetComponent<Slider>();
		languageDropdown = skin.transform.Find("LanguageDropdown").GetComponent<Dropdown>();
		//backBtn = skin.transform.Find("BackBtn").GetComponent<Button>();
		
		//初始化语言值
		languageDropdown.value = GameManager.curLanguage;
		
		//添加监听
		closeBtn.onClick.AddListener(OnCloseBtnClick);
		SoundManager.Instance.AddSliderLisener(musicSlider, sfxSlider);
		languageDropdown.onValueChanged.AddListener(OnChangeLanguage);
		//backBtn.onClick.AddListener(OnBackBtnClick);
	}

	public void OnBackBtnClick()
	{
		PanelManager.Instance.Open<StartPanel>();
		Close();
	}
	public void OnCloseBtnClick()
	{
		if (BattleManager.hasCreateLevel)
		{
			PanelManager.Instance.Open<GamingPanel>();
		}
		else
		{
			PanelManager.Instance.Open<StartPanel>();
		}
		Close();
	}
	public void OnChangeLanguage(int language)
	{
		GameManager.curLanguage = language;
		// Debug.Log("ChangeLanguage:" + language);
		// var temp=language;
		// temp=language==0?10:10;
		// temp=language==1?40:10;
		// temp=language==2?22:10;
		if (language == 0)
		{
			language = 10;
		}
		else if (language == 1) { language = 40; }
		else if (language == 2) { language = 22; }

		//Debug.Log("ChangeLanguage after:" + language);
		LocalizationMgr.Instance.ChangeLanguage((SystemLanguage)language);
	}

}
