using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyData))]
public class EnemyDataEditor : Editor
{
    private SerializedProperty m_attackList;

    private void OnEnable()
    {
        m_attackList = serializedObject.FindProperty("m_attackList");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //EditorGUILayout.PropertyField(m_attackList, true);
        for (int i = 0; i < m_attackList.arraySize; i++)
        {
            SerializedProperty element =
                m_attackList.GetArrayElementAtIndex(i);

            string className =
                element.managedReferenceValue?.GetType().Name ?? "–¢Ý’è";

            EditorGUILayout.LabelField(className, EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(element, true);
        }

        if (GUILayout.Button("Add Melee Attack"))
        {
            AddAttack(new MeleeAttackData());
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void AddAttack(AttackData data)
    {
        int index = m_attackList.arraySize;

        m_attackList.InsertArrayElementAtIndex(index);

        SerializedProperty element = m_attackList.GetArrayElementAtIndex(index);

        element.managedReferenceValue = data;
    }
}
