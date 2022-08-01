using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CtrlActor : Actor
{


    [SerializeField]
    protected GameObject m_skillRange;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {

        base.Update();
        MouseDownUpdate();
        SkillKeyDownUpdate();
        //测试死亡需要，绑定技能
        m_readySkill = Skills[0];
    }
    //
    private void MouseDownUpdate()
    {
        //设置在本地
        if (this.gameObject.tag == "Player1")
        {
            GetMouse0Down();
            GetMouse1Down();
        }

    }
    /// <summary>
    /// 获取玩家右键点击位置
    /// </summary>
    private void GetMouse1Down()
    {
        if (b_isKnocked) return;
        //如果按下鼠标右键（0是左键、1是右键）
        if (Input.GetMouseButtonDown(1)  && !EventSystem.current.IsPointerOverGameObject())
        {
            //向鼠标点击的位置发射射线
            m_destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //设置移动向量
            m_moveVec = m_destination - (Vector2)transform.position;

            //设置技能范围显示
            m_skillRange.SetActive(false);

            //关闭释放技能准备

            b_isPrepareCast = false;

            //检测移动方向
            CheckDir(m_destination);

            //Debug.Log(m_destination);
            //播放动画
            ani.SetBool("Move", true);
        }
    }


    /// <summary>
    /// 获取玩家左键点击位置
    /// </summary>
    public void GetMouse0Down()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())// !IsOnButtom
        {
            if (b_isPrepareCast)
            {
                //向鼠标点击的位置发射射线
                //Vector2 skillPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                m_destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //Debug.Log(m_destination);

                CastSkill(m_destination);

            }
        }
    }
    /// <summary>
    /// 释放技能
    /// </summary>
    /// <param name="skillPos">//暂时不需要使用技能要到达的位置</param>
    protected void CastSkill(Vector2 skillPos)
    {
        Projectile projectile = GameManager.Instance.Pool.GetObject(m_readySkill.ProjName).GetComponent<Projectile>();

        m_readySkill.IsCoolDown = true;
        Vector2 skillMoveVec = new Vector2(skillPos.x - this.transform.position.x, skillPos.y - this.transform.position.y);
        projectile.InitPorjectile(this, skillMoveVec.normalized, skillPos, m_readySkill.CastDistance, m_readySkill.ProjSpeed, m_readySkill);
        m_skillRange.SetActive(false);
        b_isPrepareCast = false;
    }

    /// <summary>
    /// 检测是否按下技能键
    /// </summary>
    public void SkillKeyDownUpdate()
    {
        //遍历绑定的技能列表获取Click信息
        foreach (SkillBase skill in Skills)
        {
            if (Input.GetKeyDown(skill.KeyCode))
            {
                if (!skill.IsCoolDown)
                {
                    CastReady(skill);
                }
            }
        }
    }

    public  void CastReady(SkillBase skill)
    {
        //设置当前已经准备好的技能
        m_readySkill = skill;
        //设置当前技能的施法距离
        m_skillRange.transform.localScale = Vector3.one * (float)(skill.CastDistance / 0.02);
        //开启技能范围显示
        m_skillRange.SetActive(!m_skillRange.activeSelf);
        //修改施法状态
        b_isPrepareCast = !b_isPrepareCast;
    }
}
