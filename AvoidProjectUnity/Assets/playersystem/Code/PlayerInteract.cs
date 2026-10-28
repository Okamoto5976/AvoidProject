using UnityEngine;

public class PlayerInteract
{
    public void Interact()
    {
        Debug.Log("アイテム取得処理を実行！");
        //剣を拾うため　剣のオブジェクトを探す必要がある

        //その前の　Interact長押しは近くに　剣がある場合のみになるため
        //まず押し初めにあるかどうか確認　ある場合　Playerの動きを一部制限
        //押し続けることではじめ取得した剣を　そのままInteractを呼ぶ（剣のIInteractableを呼ぶ）
    }
}
