using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellFactory : SingletonMonoBehaviour<ShellFactory>
{
    [SerializeField]
    List<Shell> shellList = new List<Shell>();

    /// <summary>
    /// 貝殻を生成する
    /// </summary>
    public GameObject InstanceShell()
    { 
        int ramdom = Random.Range(0, shellList.Count - 1);
        return Instantiate( shellList[ramdom].gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    public GameObject InstanceShell(int index)
    {
        return null;
    }
}
