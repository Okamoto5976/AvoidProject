using UnityEngine;

public class Test : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"TriggeräJén: {other.name}");
    }
    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"TriggerèIóπ: {other.name}");
    }
}
