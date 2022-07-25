using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private Actor m_playerSelf;

    [SerializeField]
    private GameObject m_statesPanel;

    [SerializeField]
    private Text m_stateText;

    public ObjectPool Pool { get; set; }

    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_playerSelf = GameObject.FindGameObjectWithTag("Player1").GetComponent<Actor>();
    }
    public void RebirthClick()
    {

        m_playerSelf.Rebirth();
    }
    public void OnClickQ()
    {
        if (!m_playerSelf.Skills[0].IsCoolDown)
        {
            m_playerSelf.CastReady(m_playerSelf.Skills[0]);
        }

    }
    public void MouseEnter()
    {
        m_playerSelf.IsOnButtom = true;
        //Debug.Log(m_playerSelf.IsOnButtom);
    }
    public void MouseLeave()
    {
        m_playerSelf.IsOnButtom = false;
       // Debug.Log(m_playerSelf.IsOnButtom);
    }
    /// <summary>
    /// 显示左下方介绍栏
    /// </summary>
    public void ShowStats()
    {
        m_statesPanel.SetActive(!m_statesPanel.activeSelf);
    }


    /// <summary>
    /// 设置左下方介绍栏的文本
    /// </summary>
    /// <param name="txt"></param>
    public void SetTooltipText(string txt)
    {
        m_stateText.text = txt;
    }

    public void ShowDie()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }

}
