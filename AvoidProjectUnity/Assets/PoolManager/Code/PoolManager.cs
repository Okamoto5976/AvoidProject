using UnityEngine;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private GameObject m_poolObj;
    [SerializeField] private int m_count;

    private List<GameObject> m_Pool = new();
    //EnumTypeの変数

    private void Awake()
    {
        Initialized();
    }

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

    private GameObject GenerateFromPool()
    {
        GameObject generateObj = null;
        foreach (GameObject obj in m_Pool)
        {
            if (!obj.activeInHierarchy)
            {
                generateObj = obj;
                m_Pool.Remove(generateObj);
                return generateObj;
            }
        }
        //もし足りなかった場合
        generateObj = GenerateFromPool();
        return generateObj;
    }

    //Returnの処理
    //返されたオブジェクトをSetActivefalseにもどして　Listに戻す
    private void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        m_Pool.Add(obj);
    }

    //初期化をするやつ
    //生成するやつ
    //オブジェクトを渡すやつ
    //オブジェクトをＬｉｓｔに入れるやつ

}
