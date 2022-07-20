using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    //x，y可以移动的最大距离
    private float m_xMax;
    private float m_yMin;

    //玩家移动的速度
    [SerializeField]
    private float m_actorSpeed;

    //动画
    private Animator ani;

    //鼠标点击位置
    private Vector2 m_destination;

    //鼠标点击位置与当前位置的向量
    private Vector2 m_moveVec;

    //当前移动的方向
    private Vector2 m_direct;

    /// <summary>
    /// 获取玩家点击位置
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

            CheckDir();

            Debug.Log(m_destination);
            //播放动画
            ani.SetBool("Move", true);
        }
    }

    private void CheckDir()
    {
        if (m_moveVec.x < 0)
        {
            if (m_moveVec.y > 0)
            {
                m_direct = new Vector2(-1, 1);
            }
            else
            {
                m_direct = new Vector2(-1, -1);
            }
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (m_moveVec.x > 0)
        {
            if (m_moveVec.y > 0)
            {
                m_direct = new Vector2(1, 1);
            }
            else
            {
                m_direct = new Vector2(1, -1);
            }
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    private void Move()
    {
        //移动
        //移动向量！=（0,0）才能说明有地方可以去，不然就是点自己脚底板了
        if (m_moveVec != Vector2.zero)
        {
            //移动过去
            transform.position = Vector2.MoveTowards(transform.position, m_destination, m_actorSpeed * Time.deltaTime);
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
    }

    // Update is called once per frame
    void Update()
    {
        GetMouse1Down();
        Move();
        Stop();
    }

}
