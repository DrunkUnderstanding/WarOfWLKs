using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private Actor m_playerSelf;

    [SerializeField]
    private GameObject statesPanel;

    [SerializeField]
    private Text statesTxt;

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

    // Update is called once per frame
    void Update()
    {

    }

}
