using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 线程安全单例模式，优点：1、可以更好的访问需要共享并且访问的数据
/// 2、使用null判断，static、抽象（abstract）类、管理器继承保证了一个模式下只有一个单例
/// </summary>
/// <typeparam name="T"> </typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    //线程安全锁
    private static readonly object padlock = new object();
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                //加锁
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = FindObjectOfType<T>();
                    }
                }
            }
            return instance;
        }
    }
}
