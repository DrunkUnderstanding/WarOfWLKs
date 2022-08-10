using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{

	public SkillBase skill = null;

	public Image skill_Image;

	public Image fill_Image;

	public Text keyCodeText;

	private void Awake()
	{
		//寻找组件
		fill_Image = this.gameObject.transform.Find("Mask/Fill_Image").GetComponent<Image>();
		skill_Image = this.gameObject.transform.Find("Mask/Skill_Image").GetComponent<Image>();
		keyCodeText = this.gameObject.transform.Find("KeyCodeImage/Text").GetComponent<Text>();
	}
	// Start is called before the first frame update
	void Start()
	{
		//设置图片
		fill_Image.sprite = ResourceManager.Instance.LoadRes<Sprite>(skill.iconPath);
		skill_Image.sprite = ResourceManager.Instance.LoadRes<Sprite>(skill.iconPath);

		keyCodeText.text = skill.KeyCode.ToString();
	}
	private void Update()
	{
		if (skill == null) return;
		if (skill.IsCoolDown == false) return;
		skill_Image.fillAmount = skill.cdDuration / skill.CoolDownTime;
	}


}
