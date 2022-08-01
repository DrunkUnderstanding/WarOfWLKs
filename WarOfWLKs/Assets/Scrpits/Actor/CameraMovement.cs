using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : Singleton<CameraMovement>
{


    //镜头的x，y可以移动的最大距离
    private float m_xMax;
    private float m_yMin;

    //相机跟随的玩家节点
    private Transform m_playerTransform;

    //设定一个角色能看到的最远值

    [SerializeField]
    private float Ahead;

    [SerializeField]
    //设置一个摄像机要移动到的点
    private Vector3 targetPos;

    [SerializeField]
    //设置一个缓动速度插值
    private float m_smoothSpeed;


    void Start()
    {


    }

    public void SetFollowDestination()
    {
        //获取当前角色的transform
        m_playerTransform = GameObject.FindGameObjectWithTag("Player1").GetComponent<Transform>();
        //Debug.Log(m_playerTransform);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(GameManager.Instance.PlayerSelf!=null) CameraMoveUpdate();
    }

    private void CameraMoveUpdate()
    {
        //this.transform.position = new Vector3(m_playerTransform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);

        targetPos = new Vector3(m_playerTransform.position.x, m_playerTransform.transform.position.y, gameObject.transform.position.z);

        if (m_playerTransform.position.x > 0f)
        {
            if (m_playerTransform.position.y > 0f)
            {
                targetPos = new Vector3(m_playerTransform.position.x + Ahead, m_playerTransform.position.y + Ahead, gameObject.transform.position.z);
            }
            else if (m_playerTransform.position.y < 0f)
            {
                targetPos = new Vector3(m_playerTransform.position.x + Ahead, m_playerTransform.position.y - Ahead, gameObject.transform.position.z);
            }

        }
        else
        {
            if (m_playerTransform.position.y > 0f)
            {
                targetPos = new Vector3(m_playerTransform.position.x - Ahead, m_playerTransform.position.y + Ahead, gameObject.transform.position.z);
            }
            else if (m_playerTransform.position.y < 0f)
            {
                targetPos = new Vector3(m_playerTransform.position.x - Ahead, m_playerTransform.position.y - Ahead, gameObject.transform.position.z);
            }

        }
        transform.position = Vector3.Lerp(transform.position, targetPos, m_smoothSpeed * Time.deltaTime);
        
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, m_xMax), Mathf.Clamp(transform.position.y, m_yMin, 0), -10);
    }
    /// <summary>
    /// 设置摄像头移动限制
    /// </summary>
    /// <param name="maxTile"></param>
    public void SetLimits(Vector3 maxTile)
    {

        //设置wp的位置为右下角（1,0）
        Vector3 wp = Camera.main.ViewportToWorldPoint(new Vector3(1, 0));

        m_xMax = maxTile.x - wp.x;
        m_yMin = maxTile.y - wp.y;
    }
}