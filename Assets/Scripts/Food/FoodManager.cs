using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : SingletonMonoBehaviour<FoodManager>
{
    /// <summary>
    /// ご飯の最大生成
    /// </summary>
    [Header("ご飯の最大生成数")]
    public int foodCount;

    [SerializeField, Header("生成する間隔")]
    float instanceInterval;

    [SerializeField]
    Vector3 leftUp;
    [SerializeField]
    Vector3 rightDown;

    [SerializeField,Header("ご飯の種類")]
    List<Food> foodList = new List<Food>();

    [SerializeField,Header("現在生成中のご飯")]
    List<Food> nowFoodList = new List<Food>();

    [SerializeField,Header("親オブジェクト")]
    Transform parent;


    private void Start()
    {
        StartInstanceFood();
    }

    /// <summary>
    /// InstanceFood作成開始メソッド
    /// </summary>
    public void StartInstanceFood()
    {
        StartCoroutine(ForeverInstanceFood());
    }

    IEnumerator ForeverInstanceFood()
    {
        for (int i = 0; i < 1; i += 0)
        {
#if UNITY_EDITOR
            if ( Input.GetKeyDown(KeyCode.B))
                break;
#endif

            if (nowFoodList.Count <= foodCount)
            {
                GameObject obj = InstanceFood();
                //座標ランダムに
                float x = Random.Range(rightDown.x, leftUp.x);
                float z = Random.Range(rightDown.z, leftUp.z);
                Vector3 randamPos = new Vector3(x, 2, z);
                obj.transform.position = randamPos;
                //リストに加える
                nowFoodList.Add(obj.GetComponent<Food>());
            }
            yield return new WaitForSeconds(instanceInterval);
        }
    }


    /// <summary>
    /// ご飯を生成する
    /// </summary>
    GameObject InstanceFood()
    {
        int ramdom = Random.Range(0, foodList.Count);
        GameObject obj = Instantiate(foodList[0].gameObject);
        obj.transform.parent = parent;
        return obj;
    }

    /// <summary>
    /// ご飯をを指定して生成する
    /// </summary>
    /// <param name="index"></param>
     GameObject InstanceFood(int index)
    {
        return Instantiate(foodList[index].gameObject);
    }

    /// <summary>
    /// Foodを生成する
    /// </summary>
    /// <param name="pos">生成する地点</param>
    public void InstanceFood(int index,Vector3 pos)
    {
       GameObject obj = InstanceFood(index);
        obj.transform.position = pos;
    }

    /// <summary>
    /// ご飯を削除する
    /// </summary>
    /// <param name="food"></param>
    public void DeleteFood(Food food)
    {
        nowFoodList.Remove(food);
        Destroy(food.gameObject);
    }

    /// <summary>
    /// Foodの最大数を決定する
    /// </summary>
    /// <param name="value"></param>
    public void SetFoodCount(int value)
    {
        foodCount = value;
    }
}
