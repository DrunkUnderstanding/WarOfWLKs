using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase
{
    //发射技能的角色
    Actor m_player;

    private float projSpeed;

    private bool b_isCoolDown = false;

    private float coolDown;

    //图标位置
    protected string m_iconPath;

    protected int m_damage;

    private KeyCode m_keyCode;

    //技能ID
    protected int m_skillID;

    //技能作用范围
    private float castDistance;

    protected float m_cdDuration = 0;

    public bool IsCoolDown { get => b_isCoolDown; set => b_isCoolDown = value; }
    public float CastDistance { get => castDistance; set => castDistance = value; }
    public float CoolDown { get => coolDown; set => coolDown = value; }
    public float ProjSpeed { get => projSpeed; set => projSpeed = value; }
    public KeyCode KeyCode { get => m_keyCode; set => m_keyCode = value; }

    public SkillBase(Actor player)
    {
        m_player = player;
    }
    //技能特效
    //private GameObject m_selfEffect;

    //被Update调用
    public virtual void Update()
    {
        if (IsCoolDown)
        {
            m_cdDuration += Time.deltaTime;
            //Debug.Log(m_cdDuration);
            if (m_cdDuration >= CoolDown)
            {
                m_cdDuration = 0;
                IsCoolDown = false;
            }
        }
    }
}
