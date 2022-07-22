using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 资源管理器
/// </summary>
public class ResourceManager : Singleton<ResourceManager>
{

    Dictionary<string, object> m_res = new Dictionary<string, object>();
    /// <summary>
    /// 获取资源，存放到Dic中（节约查找资源所需时间）
    /// </summary>
    /// <typeparam name="T">需要的资源类型</typeparam>
    /// <param name="resPath">资源路径</param>
    /// <returns></returns>
    public T LoadRes<T>(string resPath) where T : Object
    {
        if (m_res.ContainsKey(resPath))
        {
            return m_res[resPath] as T;
        }
        T t = Resources.Load<T>(resPath);
        m_res[resPath] = t;
        return t;
    }


}
