using UnityEngine;

public class InitializedManager : MonoBehaviour
{
    private void Awake()
    {
        Initialized();
    }

    private void Initialized()
    {
        //RuntimeDataを降ろす

        //StageTypeを受ける

        //Playerの生成
        //RunTimeDataも渡す
        //PlayerのAwake（初期化）を呼ぶ
        //PlayerのDisable

        //Runtimeから降ろした敵の生成
        //敵の初期化（RunTimeのものを初期化）
        //EnemyのDisable

        //それぞれPoolがあるので　あったPoolを各Managerに渡す

        //それぞれに　必要なManagerを渡す Attack時に必要なものとか

        //UIManagerのEventをつける
        
        

        //GameStartDirectorを呼ぶ
    }


}
