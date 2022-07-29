using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Actor : MonoBehaviour
{

    //玩家移动的速度
    [SerializeField]
    protected float m_actorSpeed;

    [SerializeField]
    public Stat m_health;

    protected SkillBase m_readySkill;

    //动画
    protected Animator ani;

    [SerializeField]
    //鼠标点击位置
    protected Vector2 m_destination;

    //鼠标点击位置与当前位置的向量
    protected Vector2 m_moveVec;

    //当前移动的方向
    protected Vector2 m_direct;

    //角色准备放技能
    protected bool b_isPrepareCast = false;

    protected bool b_isClickButtom = false;

    protected bool b_isKnocked = false;


    public bool IsActive { get; set; }
    public float MaxSpeed { get; set; }


    /// <summary>
    /// 检测玩家移动方向,并调整方向
    /// </summary>
    protected void CheckDir(Vector2 clickVec)
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
    private void MoveUpdate(Vector2 pos)
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
            b_isKnocked = false;
            //如果小于就判定到达目的地，执行待机
            m_moveVec = Vector2.zero;
            //停止播放动画
            ani.SetBool("Move", false);
        }
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        ani = this.gameObject.GetComponent<Animator>();

        //游戏开始时绑定技能给Actor
        Skills.Add(new FireSkill(this));

        m_health.Bar.Reset();

        MaxSpeed = m_actorSpeed;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        MoveUpdate(m_destination);

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
    public void HandleDamage(float damage)
    {
        Debug.Log(this.tag);
        this.m_health.CurrentVal -= damage * Time.deltaTime;
        //死亡
        if (m_health.CurrentVal <= 0)
        {

            if (this.tag == "Player1")
            {
                GameManager.Instance.ShowDie(true);
            }

            Release();
        }
    }

    /// <summary>
    /// 处理角色受伤信息
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="skill">（暂无用处，之后技能可能存在buff效果需要）</param>
    public void HandleDamage(float damage, SkillBase skill)
    {
        Debug.Log(this.tag);
        this.m_health.CurrentVal -= damage;
        //死亡
        if (m_health.CurrentVal <= 0)
        {
            if (this.tag == "Player1")
            {
                GameManager.Instance.ShowDie(true);
            }
            Release();
        }
    }
    public void KnockBack(Vector3 projectilePos, SkillBase skill)
    {
        Vector2 thisPosVec2 = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        Vector3 moveDir = (this.gameObject.transform.position - projectilePos).normalized;
        Vector2 moveVec = skill.KnockBackDistance * (new Vector2(moveDir.x, moveDir.y));
        Vector2 moveTo = thisPosVec2 + moveVec;
        m_moveVec = moveVec;
        m_destination = moveTo;
        b_isKnocked = true;
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

        GameManager.Instance.ShowDie(false);
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
