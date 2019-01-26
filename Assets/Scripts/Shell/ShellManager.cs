using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellManager : SingletonMonoBehaviour<ShellManager>
{
    /// <summary>
    /// 殻の最大生成
    /// </summary>
    public int shellCount;


    [SerializeField,Header("生成する間隔")]
    float instanceInterval;

    [SerializeField]
    Vector3 leftUp;
    [SerializeField]
    Vector3 rightDown;

    [SerializeField]
    List<Shell> shellList = new List<Shell>();

    /// <summary>
    /// Shell作成開始
    /// </summary>
    public void StartInstanceShell()
    {
        StartCoroutine(InstanceShell());
    }

    IEnumerator InstanceShell()
    {
        for(int i = 0; i< 1; i += 0)
        {
            if (Input.GetKeyDown(KeyCode.B))
                break;

            if(shellList.Count <= shellCount)
            {
                GameObject obj = ShellFactory.Instance.InstanceShell();
                //座標ランダムに
                float x = Random.Range(rightDown.x, leftUp.x);
                float z = Random.Range(rightDown.z,leftUp.z);
                Vector3 randamPos = new Vector3(x, 4, z);
                obj.transform.position = randamPos;
                //リストに加える
                shellList.Add(obj.GetComponent<Shell>());
            }
            yield return new WaitForSeconds(instanceInterval);
        }
    }

    /// <summary>
    /// Shell削除するよ
    /// </summary>
    /// <param name="obj"></param>
    public void DeleteShell(GameObject obj)
    {
        shellList.Remove(obj.GetComponent<Shell>());
        Destroy(obj.gameObject);
    }
}
