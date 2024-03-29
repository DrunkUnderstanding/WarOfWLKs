using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Localization : ScriptableObject
{	
	public List<Sheet> sheets = new List<Sheet> ();
	 private  static Dictionary<int ,Sheet> sheetDic=new Dictionary<int, Sheet>();

	[System.SerializableAttribute]
	public class Sheet
	{
		public string name = string.Empty;
		public List<Param> list = new List<Param>();
		private static readonly Dictionary<int, Param> dicParams = new Dictionary<int, Param>();
		 /// <summary>
        ///     get one data
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public  Param GetData(int id)
        {
            if (dicParams.ContainsKey(id))
            {
                return dicParams[id];
            }
            Debug.LogError("No This id!");
            return null;
        }

        public void SetDic()
        {
            dicParams.Clear();
            for (int i = 0, iMax = list.Count; i < iMax; i++)
            {
                dicParams.Add(list[i].id, list[i]);
            }
        }
	}

	[System.SerializableAttribute]
	public class Param
	{
		
        /// <summary>
        /// 编号
        /// </summary>
        public  int id;

        /// <summary>
        /// 中文简体
        /// </summary>
        public  string value;

	}

	    public void SetDic()
    {
        for (int i = 0, iMax = sheets.Count; i < iMax; i++)
        {
            var enumId=Enum.Parse(typeof (SheetName), sheets[i].name);
            sheetDic.Add((int)enumId, sheets[i]);
            sheets[i].SetDic();
        }
    }

	 /// <summary>
    ///     get one sheet
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    public static Sheet GetSheet(int id)
    {
        if (sheetDic.ContainsKey(id))
        {
            return sheetDic[id];
        }
        Debug.LogError("No This Sheet!");
        return null;
    }

	  public enum SheetName
    {
       ChineseSimplified,
English,
Japanese,

    }
}

