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
    private GameObject m_diedPanel;

    [SerializeField]
    private GameObject m_skillPanel;

    [SerializeField]
    private Text m_stateText;

    [SerializeField]
    private Button m_reBirthBtn;

    [SerializeField]
    private GameObject m_optionsMenu;
    [SerializeField]
    private GameObject m_startMenu;

    [SerializeField]
    private Camera m_mainCamera;
    [SerializeField]
    private Button m_setBtn;
    public ObjectPool Pool { get; set; }
    public Actor PlayerSelf { get => m_playerSelf; set => m_playerSelf = value; }

    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    public void RebirthClick()
    {

        PlayerSelf.Rebirth();
    }
    public void OnClickQ()
    {
        if (!PlayerSelf.Skills[0].IsCoolDown)
        {
            PlayerSelf.CastReady(PlayerSelf.Skills[0]);
        }

    }
    public void MouseEnter()
    {
        PlayerSelf.IsOnButtom = true;
        //Debug.Log(m_playerSelf.IsOnButtom);
    }
    public void MouseLeave()
    {
        PlayerSelf.IsOnButtom = false;
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

    /// <summary>
    /// 显示死亡提示
    /// </summary>
    public void ShowDie(bool setShow)
    {
        m_diedPanel.SetActive(setShow);
        m_reBirthBtn.gameObject.SetActive(setShow);
    }
    // Update is called once per frame

    public void ShowOptionsMenu()
    {
        m_optionsMenu.SetActive(true);
        m_startMenu.SetActive(false);
    }
    public void CloseOptionsMenu()
    {
        if(this.m_playerSelf == true )m_optionsMenu.SetActive(false);
    }

    public void StartGame()
    {
       LevelManager.Instance.CreateLevel();

        m_skillPanel.SetActive(true);
        m_setBtn.gameObject.SetActive(true);
        m_mainCamera.gameObject.SetActive(true);
        CameraMovement.Instance.SetFollowDestination();

        PlayerSelf = GameObject.FindGameObjectWithTag("Player1").GetComponent<Actor>();

        m_startMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackStartManu()
    {
        m_optionsMenu.SetActive(false);
        m_startMenu.SetActive(true);
        //游戏结束代码↓


    }
    void Update()
    {

    }

}
