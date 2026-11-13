using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public virtual void InitializeData(EnemyData data)
    {
        if (!data.MatchesEnemyType(GetType()))
        {
            Debug.LogError(
                $"Enemy‚ÌŒ^‚ªˆê’v‚µ‚Ü‚¹‚ñBEnemy: {GetType().FullName}",
                this
            );
            return;
        }

        Debug.Log("EnemyData‚Ì‰Šú‰»¬Œ÷");
    }
}
