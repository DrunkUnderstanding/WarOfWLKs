using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    //地板材质瓷砖
    [SerializeField]
    private GameObject[] m_tilePrefabs;

    //所有Tile的父节点Map物件节点
    [SerializeField]
    private Transform m_tileFather;

    [SerializeField]
    private Transform m_actorsFather;

    private Point[] m_birthPoint;

    [SerializeField]
    private CameraMovement m_cameraMovement;    //摄像头移动脚本

    /// <summary>
    /// 获取出生点位置，禁止外界修改
    /// </summary>
    public Point[] BirthPoint
    {
        get => m_birthPoint;
        private set
        {
            this.m_birthPoint = value;
        }
    }

    //地图大小
    private Point m_mapSize;

    /// <summary>
    /// 储存信息的字典，方便的获取每一个TileScript以便于使用每个TileScript里面的数据
    /// </summary>
    public Dictionary<Point, TileScript> Tiles { get; set; }

    /// <summary>
    /// 返回地板大小的属性
    /// </summary>
    public float TileSize
    {
        get { return m_tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

    private void Awake()
    {
/*        CreateLevel();
        GameObject go = ResourceManager.Instance.LoadRes<GameObject>("Prefabs/Actors/MaskAborigine");
        go = GameObject.Instantiate(go);
        go.transform.position = LevelManager.Instance.Tiles[BirthPoint[1]].transform.position;
        go.transform.SetParent(m_actorsFather);*/
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateLevel()
    {

        Tiles = new Dictionary<Point, TileScript>();

        //地图信息读取
        string[] mapData = ReadLevelText("mapType");
        string[] birData = ReadLevelText("birType");

        //获取地图最大的X、Y，放入 Point 
        m_mapSize = new Point(mapData[0].ToCharArray().Length, mapData.Length);

        //map x size
        int mapX = m_mapSize.X;

        //map y size
        int mapY = m_mapSize.Y;

        //世界开始的起点位置
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        //生成地图
        for (int y = 0; y < mapY; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();
            for (int x = 0; x < mapX; x++)
            {
                PlaceTile(newTiles[x].ToString(), x, y, worldStart);
            }
        }
        Vector3 maxTile = Vector3.zero;

        maxTile = Tiles[new Point(mapX - 1, mapY - 1)].transform.position;

        m_cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));

        //设置出生点
        m_birthPoint = SetBirthPoint(birData);


        //创建游戏角色
        GameObject go = ResourceManager.Instance.LoadRes<GameObject>("Prefabs/Actors/MaskAborigine");
        go = GameObject.Instantiate(go);
        go.transform.position = LevelManager.Instance.Tiles[BirthPoint[1]].transform.position;
        go.transform.SetParent(m_actorsFather);
    }

    /// <summary>
    /// 解析出的出生点位置字符串数组
    /// </summary>
    /// <param name="birPointStrs"></param>
    /// <returns>出生点Point数组</returns>
    private Point[] SetBirthPoint(string[] birPointStrs)
    {
        Point[] points = new Point[birPointStrs.Length];
        for (int i = 0; i < birPointStrs.Length; i++)
        {
            string str = birPointStrs[i];
            int index = str.IndexOf(",");
            points[i].X = int.Parse(str.Substring(0, index));
            points[i].Y = int.Parse(str.Substring(index + 1, str.Length - index - 1));
        }
        return points;
    }
    /// <summary>
    /// 创建地板方块
    /// </summary>
    /// <param name="tileType">地板预设体类型</param>
    /// <param name="x">地板所在位置x</param>
    /// <param name="y">地板所在位置y</param>
    /// <param name="worldStart">世界起点</param>
    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        int tileIndex = int.Parse(tileType);

        //获取新的Object以便于修改position
        TileScript newTile = Instantiate(m_tilePrefabs[tileIndex]).GetComponent<TileScript>();

        //修改positon,Point的位置
        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0), m_tileFather);

    }

    private string[] ReadLevelText(string txtType)
    {
        //载入地板图层的信息
        TextAsset t_mapdata = ResourceManager.Instance.LoadRes<TextAsset>("Levels/Map1");
        TextAsset t_birData = ResourceManager.Instance.LoadRes<TextAsset>("Levels/Bir1");
        //把'/n'换行符去除
        string mapData = t_mapdata.text.Replace(Environment.NewLine, string.Empty);
        string birthData = t_birData.text.Replace(Environment.NewLine, string.Empty);
        //使用约定好的'-'作为每一行的末尾把字符串分割为多个字符串
        if (txtType == "birType") return birthData.Split('-');

        return mapData.Split('-');
    }
}
