using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TestSO", menuName = "Scriptable Objects/TestSO")]
public class TestSO : ScriptableObject
{
    public int value = 10;

    public List<int> values = new();
}
