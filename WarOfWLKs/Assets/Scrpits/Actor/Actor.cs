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
    private float m_ActorSpeed;

    private void GetInput()
    {
        //绑定键位
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * m_ActorSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * m_ActorSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * m_ActorSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * m_ActorSpeed * Time.deltaTime);
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, m_xMax), Mathf.Clamp(transform.position.y, m_yMin, 0), -10);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    /// <summary>
    /// 设置摄像头移动限制
    /// </summary>
    /// <param name="maxTile"></param>
    public void SetLimits(Vector3 maxTile)
    {
        m_ActorSpeed = 5;

        //设置wp的位置为右下角（1,0）
        Vector3 wp = Camera.main.ViewportToWorldPoint(new Vector3(1, 0));

        m_xMax = maxTile.x - wp.x;
        m_yMin = maxTile.y - wp.y;
    }
}
