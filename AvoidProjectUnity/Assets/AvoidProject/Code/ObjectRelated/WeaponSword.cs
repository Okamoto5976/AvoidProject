using UnityEngine;

public class WeaponSword : MonoBehaviour, IInteractable
{
    private StageManager m_stageManager;

    public void Initialize(StageManager stageManager)
    {
        m_stageManager = stageManager;
    }

    public bool Interact(PlayerController c)
    {
        //剣を装備
        Debug.Log("剣を取得しました！");

        // プレイヤーの剣所持状態を変更
        c.SetHasWeapon(true);

        //Anim や Particel
        ReturnObject();


        return true;
    }

    private void ReturnObject()
    {
        m_stageManager.ReturnWeaponSword(this.gameObject);
    }
}
