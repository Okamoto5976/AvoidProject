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
        //Œ•‚ğ‘•”õ
        Debug.Log("Œ•‚ğæ“¾‚µ‚Ü‚µ‚½I");

        c.EquipWeapon(this);

        //Anim ‚â Particel
        //ReturnObject();

        return true;
    }

    private void ReturnObject()
    {
        m_stageManager.ReturnWeaponSword(this.gameObject);
    }
}
