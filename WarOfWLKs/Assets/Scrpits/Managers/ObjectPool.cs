using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objectPrefabs;

    private List<GameObject> pooledObjects = new List<GameObject>();
    /// <summary>
    /// 获取缓冲池当中的对象
    /// </summary>
    /// <param name="type">获取的对象名</param>
    /// <returns></returns>
    public GameObject GetObject(string type)
    {
        foreach (GameObject go in pooledObjects)
        {
            if (go.name == type && !go.activeInHierarchy)
            {
                go.SetActive(true);
                return go;
            }
        }
        //如果池中没有这对象物体，那我们就创建一个对象物体
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            //如果我们有一个 用来创建对象的 prefab
            if (objectPrefabs[i].name == type)
            {
                //我们创建一个类型正确的 prefab
                GameObject newObject = Instantiate(objectPrefabs[i]);
                newObject.name = type;

                pooledObjects.Add(newObject);

                return newObject;
            }

        }

        return null;
    }

    /// <summary>
    /// 释放当前的这个对象
    /// </summary>
    /// <param name="gameObject"></param>
    public void ReleaseObject(GameObject gameObject)
    {
        gameObject.SetActive(false);

    }
}
