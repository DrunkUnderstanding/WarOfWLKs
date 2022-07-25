using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{

    //玩家移动的速度
    [SerializeField]
    private float m_actorSpeed;

    [SerializeField]
    private GameObject m_skillRange;

    private SkillBase m_readySkill;

    [SerializeField]
    public Stat m_health;

    //动画
    private Animator ani;

    //鼠标点击位置
    private Vector2 m_destination;

    //鼠标点击位置与当前位置的向量
    private Vector2 m_moveVec;

    //当前移动的方向
    private Vector2 m_direct;

    //角色准备放技能
    private bool b_isPrepareCast = false;

    private bool b_isClickButtom = false;

    public bool IsActive { get; set; }
    public float MaxSpeed { get; set; }
    /*    /// <summary>
        /// 当单位移动时,触发的事件
        /// </summary>
        /// <param name="pos">单位要移动到的目标位置</param>
        public delegate void MoveHandler(Actor actor, Vector3 pos);

        // 当单位移动时,触发的事件
        public event MoveHandler OnMove;*/

    /// <summary>
    /// 获取玩家右键点击位置
    /// </summary>
    private void GetMouse1Down()
    {

        //如果按下鼠标右键（0是左键、1是右键）
        if (Input.GetMouseButtonDown(1))
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
    private void GetMouse0Down()
    {
        if (Input.GetMouseButtonDown(0) && !IsOnButtom)
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
    private void CastSkill(Vector2 skillPos)
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
    public void GetSkillClick()
    {
        //遍历绑定的技能列表获取Click信息
        foreach (SkillBase skill in Skills)
        {
            if (!skill.IsCoolDown)
            {
                if (Input.GetKeyDown(skill.KeyCode))
                {
                    CastReady(skill);
                }
            }
        }
    }



    public void CastReady(SkillBase skill)
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

    /// <summary>
    /// 检测玩家移动方向,并调整方向
    /// </summary>
    private void CheckDir(Vector2 clickVec)
    {
        if (m_moveVec.x <= 0)
        {
            if (m_moveVec.y >= 0)
            {
                m_direct = new Vector2(-1, 1);
            }
            else
            {
                m_direct = new Vector2(-1, -1);
            }
            this.transform.rotation = Quaternion.Euler(0, 180, 0);

            this.transform.GetChild(1).rotation = Quaternion.Euler(0, 0, 0);
        }
        if (m_moveVec.x > 0)
        {
            if (m_moveVec.y >= 0)
            {
                m_direct = new Vector2(1, 1);
            }
            else
            {
                m_direct = new Vector2(1, -1);
            }
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
            this.transform.GetChild(1).rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    private void MoveTo(Vector2 pos)
    {
        //移动
        //移动向量！=（0,0）才能说明有地方可以去，不然就是点自己脚底板了
        if (m_moveVec != Vector2.zero)
        {
            //移动过去
            transform.position = Vector2.MoveTowards(transform.position, m_destination, m_actorSpeed * Time.deltaTime);
            Stop();
        }
    }

    //停止移动
    public void Stop()
    {
        //计算自身和目标点的距离
        float distance = Vector2.Distance(transform.position, m_destination);
        //判断和目标点的距离是否小于0.01f
        if (distance < 0.01f)
        {
            //如果小于就判定到达目的地，执行待机
            m_moveVec = Vector2.zero;
            //停止播放动画
            ani.SetBool("Move", false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ani = this.gameObject.GetComponent<Animator>();

        //游戏开始时绑定技能给Actor
        Skills.Add(new FireSkill(this));
        //测试死亡需要，绑定技能
        m_readySkill = Skills[0];

        m_health.Bar.Reset();

        MaxSpeed = m_actorSpeed;
    }
    //
    void GetMouseClick()
    {
        //设置在本地
        if (this.gameObject.tag == "Player1")
        {
            GetMouse0Down();
            GetMouse1Down();
        }

    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        GetMouseClick();
        MoveTo(m_destination);
        GetSkillClick();
        HandleSkills();
        m_health.Initialize();
        //测试生命条的代码
        //m_health.CurrentVal -= 0.1f;
        //HandleDamage(20f, m_readySkill);
    }

    //技能列表，一般只有4个技能，可加
    private List<SkillBase> m_skills = new List<SkillBase>();

    public List<SkillBase> Skills { get => m_skills; set => m_skills = value; }
    public bool IsOnButtom { get => b_isClickButtom; set => b_isClickButtom = value; }

    /// <summary>
    /// 处理角色的释放技能CD、等信息
    /// </summary>
    private void HandleSkills()
    {
        foreach (SkillBase skill in Skills)
        {
            skill.Update();
        }
    }
    /// <summary>
    /// 处理角色受伤信息
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="skill"></param>
    public void HandleDamage(float damage, SkillBase skill)
    {
        Debug.Log(this.tag);
        this.m_health.CurrentVal -= damage;
        //死亡
        if (m_health.CurrentVal <= 0)
        {
            GameManager.Instance.ShowDie();

            Release();
        }
    }

    /// <summary>
    /// 释放当前 Actor，并且将当前 Monster 放入 Pool
    /// </summary>
    public void Release()
    {

        //需要这一句，其他代码位置不存在速度修改，会导致bug
        m_actorSpeed = MaxSpeed;

        //让这个对象进入 isn't active 不活跃状态
        IsActive = false;


        //释放对象以后再放入对象池
        GameManager.Instance.Pool.ReleaseObject(gameObject);


    }
    public void Rebirth()
    {
        //当我们需要重新启用当前资源时，将这个资源的初始位置设置到GridPosition
        Reset(m_health.MaxVal);

        this.gameObject.SetActive(true);
    }

    public void Reset(float maxHealth)
    {
        transform.position = LevelManager.Instance.Tiles[LevelManager.Instance.BirthPoint[1]].transform.position;

        this.m_health.Bar.Reset();

        this.m_health.MaxVal = maxHealth;

        this.m_health.CurrentVal = this.m_health.MaxVal;

        this.m_destination = transform.position;
    }


}
