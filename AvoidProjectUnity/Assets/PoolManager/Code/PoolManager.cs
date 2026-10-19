using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PoolManager : MonoBehaviour
{
    [Header("プールオブジェクト生成元と生成数")]
    [SerializeField] private GameObject m_poolObj;
    [SerializeField] private int m_count;

    [Header("確認用")]
    [SerializeField] private List<GameObject> m_Pool = new();
    //[SerializeField] private List<GameObject> test;

    private void Awake()
    {
        Initialized();
    }

    //private void Update()
    //{
    //    //テスト
    //    if (Mouse.current.leftButton.wasPressedThisFrame)
    //    {
    //        test.Add(GenerateFromPool());
    //    }
    //    if (Mouse.current.rightButton.wasPressedThisFrame)
    //    {
    //        ReturnToPool(test[0]);
    //        test.Remove(test[0]);
    //    }
    //}

    //初期化
    //m_countの数　m_poolObjを　Instanciateを使う　（生成されたオブジェクトが　このオブジェクトを親にするように)

    //生成したObjを
    //ReturnPoolをつける　ここは後につくるのでコメントアウト
    //Listに生成したものを格納する
    //SetActiveをfalseにする

    private void Initialized()
    {
        //for分でカウントの分だけ
        for (int i = 0; i < m_count; i++)
        {
            m_Pool.Add(GeneratePoolObject());
        }
    }

    private GameObject GeneratePoolObject()
    {
        GameObject obj = Instantiate(m_poolObj, transform);
        //objにReturnPoolをつける　ここは後につくる
        obj.SetActive(false);
        return obj;
    }

    //生成の処理
    //関数を　値を返すものにする
    //格納したListから一個だけ抜き取って　返す


    /// <summary>
    /// プールからオブジェクトを取り出す関数
    /// </summary>
    /// <returns>取り出したオブジェクト</returns>
    public GameObject GenerateFromPool()
    {
        GameObject generateObj = null;
        foreach (GameObject obj in m_Pool)
        {
            if (!obj.activeInHierarchy)
            {
                generateObj = obj;
                m_Pool.Remove(generateObj);
                generateObj.SetActive(true);
                return generateObj;
            }
        }
        //もし足りなかった場合
        generateObj = GeneratePoolObject();
        generateObj.SetActive(true);
        return generateObj;
    }

    //Returnの処理
    //返されたオブジェクトをSetActivefalseにもどして　Listに戻す

    /// <summary>
    /// オブジェクトをプールに戻す関数
    /// </summary>
    /// <param name="obj">戻したいオブジェクト</param>
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        m_Pool.Add(obj);
    }

    //初期化をするやつ
    //生成するやつ
    //オブジェクトを渡すやつ
    //オブジェクトをＬｉｓｔに入れるやつ

}
