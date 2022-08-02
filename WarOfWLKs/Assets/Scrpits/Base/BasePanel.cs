using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class BasePanel : MonoBehaviour
{
	//皮肤路径
	public string skinPath;
	//皮肤
	public GameObject skin;
	//层级
	public PanelManager.Layer layer = PanelManager.Layer.Panel;
	//初始化
	public void Init()
	{
		//皮肤
		GameObject skinPrefab = ResourceManager.Instance.LoadRes<GameObject>(skinPath);
		skin = (GameObject)Instantiate(skinPrefab);
	}
	//关闭
	public void Close()
	{
		string name = this.GetType().ToString();
		PanelManager.Instance.Close(name);
	}

	//初始化时
	public virtual void OnInit()
	{
	}
	//显示时
	public virtual void OnShow(params object[] para)
	{
	}
	//关闭时
	public virtual void OnClose()
	{
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

}
