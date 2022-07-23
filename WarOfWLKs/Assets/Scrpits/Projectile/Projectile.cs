using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Animator m_animator;

    private Actor parent;

    private Vector3 m_targetPos;

    private float m_projSpeed;

    private Vector2 m_moveDir;

    //移动的距离（由SkillBase的castDistance来init）
    private float m_moveDistance;

    // Start is called before the first frame update
    void Start()
    {
        this.m_animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToTarget();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parent">子弹的父节点</param>
    /// <param name="moveDir">子弹移动的方向</param>
    /// <param name="targetPos">子弹移动到的位置</param>
    /// <param name="castDistance">技能施法距离</param>
    /// <param name="projSpeed">技能（子弹）移动速度</param>
    public void InitPorjectile(Actor parent, Vector2 moveDir, Vector2 targetPos, float castDistance, float projSpeed)
    {
        //记录父亲（可能需要计分功能）
        this.parent = parent;

        //设置初始位置
        this.transform.position = parent.transform.position;

        //设置移动
        this.m_moveDir = moveDir;
        Debug.Log(moveDir);
        this.m_moveDistance = castDistance;
        this.m_targetPos = new Vector2(this.transform.position.x, this.transform.position.y) + moveDir * castDistance ;
        //this.m_targetPos = targetPos;
        this.m_projSpeed = projSpeed;

        //找到父节点
        this.transform.SetParent(GameObject.Find("Projectiles").transform);

        //SetAngle();
    }


    private void SetAngle()
    {
        //设置子弹角度
        float angle = Mathf.Atan2(m_moveDir.y, m_moveDir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    /// <summary>
    /// 子弹移动到指定位置
    /// </summary>
    private void MoveToTarget()
    {
        if (m_moveDir != Vector2.zero)
        {
            //移动过去
            transform.position = Vector2.MoveTowards(transform.position, m_targetPos, Time.deltaTime * m_projSpeed);

            Stop();
        }
    }
    private void Stop()
    {
        //计算自身和目标点的距离
        float distance = Vector2.Distance(this.transform.position, m_targetPos);
        //判断和目标点的距离是否小于0.01f
        if (distance < 0.01f)
        {
            //如果小于就判定到达目的地，执行待机
            m_moveDir = Vector2.zero;

            //回收子弹
            GameManager.Instance.Pool.ReleaseObject(gameObject);
            //停止播放动画
            m_animator.SetBool("Move", false);
        }
    }

}
