using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillBase
{
    //发射技能的角色
    Actor m_player;

    private string m_projName;

    private string m_skillName;

    private float m_projSpeed;

    private bool b_isCoolDown = false;

    private float m_coolDown;

    //图标位置
    protected string m_iconPath;


    [SerializeField]
    private int m_damage;

    private KeyCode m_keyCode;

    //技能ID
    protected int m_skillID;

    //技能作用范围
    private float castDistance;

    protected float m_cdDuration = 0;

    public bool IsCoolDown { get => b_isCoolDown; set => b_isCoolDown = value; }
    public float CastDistance { get => castDistance; set => castDistance = value; }
    public float CoolDown { get => m_coolDown; set => m_coolDown = value; }
    public float ProjSpeed { get => m_projSpeed; set => m_projSpeed = value; }
    public KeyCode KeyCode { get => m_keyCode; set => m_keyCode = value; }
    public string SkillName { get => m_skillName; set => m_skillName = value; }
    public string ProjName { get => m_projName; set => m_projName = value; }
    public int Damage { get => m_damage; set => m_damage = value; }

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
