using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileElement { WATER, MUD, NONE }

public class TileScript : MonoBehaviour
{
    //地板的类型（站在上面是否掉血）
    private TileElement m_tileType;

    public TileElement TileType { get => m_tileType; }
    /// <summary>
    /// 这个地板砖的网格位置
    /// </summary>
    public Point GridPosition { get; private set; }

    /// <summary>
    /// 返回当前script绑定地板在世界当中的位置
    /// </summary>
    public Vector2 WorldPosition
    {
        get
        {
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2), transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2));
        }
    }

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 设置地板砖块的位置
    /// </summary>
    /// <param name="gridPos"></param>
    /// <param name="worldPos"></param>
    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
        /*        //地砖设置时不存在防御塔，设置IsEmpty
                IsEmpty = true;
                WalkAble = true;
        */
        this.GridPosition = gridPos;
        transform.position = worldPos;
        //每有一个新生成的地板砖，就把地板砖加入Tiles（DIctionary）
        LevelManager.Instance.Tiles.Add(gridPos, this);
        transform.SetParent(parent);
    }
}
