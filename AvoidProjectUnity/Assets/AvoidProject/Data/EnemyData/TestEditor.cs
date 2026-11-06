using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestSO))]
public class TestEditor : Editor
{
    private SerializedProperty m_value;
    private SerializedProperty m_values;

    private void OnEnable()
    {
        m_value = serializedObject.FindProperty("value");
        m_values = serializedObject.FindProperty("values");
    }

    public override void OnInspectorGUI()
    {
        //TestSO data = (TestSO)target;

        //Undo.RecordObject(data, "Change TestData");

        serializedObject.Update();

        EditorGUILayout.PropertyField(m_value);
        EditorGUILayout.PropertyField(m_values, true);

        if(GUILayout.Button("要素を追加"))
        {
            int index = m_values.arraySize;
            m_values.InsertArrayElementAtIndex(index);

            SerializedProperty element = m_values.GetArrayElementAtIndex(index);

            element.intValue = 0;
        }

        if(GUILayout.Button("０番目を削除"))
        {
            if(m_values.arraySize > 0)
            {
                m_values.DeleteArrayElementAtIndex(0);
            }
        }

        if(GUILayout.Button("1と0の順番を入れ替える"))
        {
            if(m_values.arraySize >= 2)
            {
                m_values.MoveArrayElement(0, 1);
            }
        }

        if(GUILayout.Button("メソッドをチェック"))
        {
            System.Type enemyType = typeof(TEnemy);

            MethodInfo[] methods = enemyType.GetMethods(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic
                );

            foreach (var method in methods)
            {
                EnemyAttackPointAttribute attribute = method.GetCustomAttribute<EnemyAttackPointAttribute>();

                if (attribute == null) return;

                Debug.Log($"メソッド名:{method.Name}");
                Debug.Log($"ID:{attribute.Id}");

                TEnemy enemy = new();

                method.Invoke(enemy, null);
            }
        }

        

        //EditorGUILayout.LabelField("テスト用Inspector");

        //m_value = EditorGUILayout.IntField("現在の値", m_value);

        if(GUILayout.Button("値を100にする"))
        {
            m_value.intValue = 100;
        }

        EditorGUILayout.HelpBox(
              "Enemyのスクリプトを設定してください。",
              MessageType.Info
          );

        EditorGUILayout.HelpBox(
              "Enemyのスクリプトを設定してください。",
              MessageType.Error
          );

        //EditorUtility.SetDirty(data);
        serializedObject.ApplyModifiedProperties();
    }
}
