using UnityEngine;

public enum PlayerState
{
    Idle,       // 待機
    Move,       // 移動
    Sprint,     // ダッシュ
    Avoid,      // 回避
    Recover,    // 回復
    Interact,   // インタラクト
    Attack,     // 攻撃
    Dead        // 死亡
}
