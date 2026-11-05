using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyData))]
public class EnemyDataEditor : Editor
{
    private SerializedProperty m_enemyScript;
    private SerializedProperty m_enemyTypeName;
    private SerializedProperty m_attackSettings;

    private void OnEnable()
    {
        m_enemyScript = serializedObject.FindProperty("m_enemyScript");
        m_enemyTypeName = serializedObject.FindProperty("m_enemyTypeName");
        m_attackSettings = serializedObject.FindProperty("m_attackSettings");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Enemyのスクリプトを選択
        EditorGUILayout.PropertyField(
            m_enemyScript,
            new GUIContent("Enemy Script")
        );

        MonoScript script = m_enemyScript.objectReferenceValue as MonoScript;

        if (script == null)
        {
            EditorGUILayout.HelpBox(
                "Enemyのスクリプトを設定してください。",
                MessageType.Info
            );

            serializedObject.ApplyModifiedProperties();
            return;
        }

        Type enemyType = script.GetClass();

        // Enemyを継承しているか確認
        if (enemyType == null ||
            enemyType.IsAbstract ||
            !typeof(Enemy).IsAssignableFrom(enemyType))
        {
            EditorGUILayout.HelpBox(
                "Enemyを継承した具象クラスを設定してください。",
                MessageType.Error
            );

            serializedObject.ApplyModifiedProperties();
            return;
        }

        // 実行時の型チェック用にクラス名を保存
        m_enemyTypeName.stringValue = enemyType.FullName;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField(
            "Attack Settings",
            EditorStyles.boldLabel
        );

        // Attributeを取得
        List<AttackPointInfo> attackPoints =
            GetAttackPoints(enemyType);

        if (attackPoints.Count == 0)
        {
            EditorGUILayout.HelpBox(
                "EnemyAttackPointAttributeが付いたメソッドがありません。",
                MessageType.Warning
            );
        }

        // 攻撃ポイントを表示
        foreach (var point in attackPoints)
        {
            DrawAttackPoint(point);
        }

        // 登録されていない設定がある場合の警告
        DrawUnknownSettings(attackPoints);

        serializedObject.ApplyModifiedProperties();
    }

    private class AttackPointInfo
    {
        public string Id;
        public string DisplayName;
        public string Description;
    }

    // EnemyクラスからAttributeを取得
    private List<AttackPointInfo> GetAttackPoints(System.Type enemyType)
    {
        var result = new List<AttackPointInfo>();

        var methods = enemyType.GetMethods(
            BindingFlags.Instance |
            BindingFlags.Public |
            BindingFlags.NonPublic
        );

        var usedIds = new HashSet<string>();

        foreach (var method in methods)
        {
            var attribute =
                method.GetCustomAttribute<EnemyAttackPointAttribute>(true);

            if (attribute == null)
                continue;

            if (string.IsNullOrWhiteSpace(attribute.Id))
            {
                Debug.LogError(
                    $"{method.Name} のAttributeにIDが設定されていません。"
                );
                continue;
            }

            if (!usedIds.Add(attribute.Id))
            {
                Debug.LogError(
                    $"AttackPointのIDが重複しています: {attribute.Id}"
                );
                continue;
            }

            result.Add(new AttackPointInfo
            {
                Id = attribute.Id,
                DisplayName = attribute.DisplayName,
                Description = attribute.Description
            });
        }

        return result;
    }

    //private void AddAttack(AttackData data)
    //{
    //    int index = m_attackSettings.arraySize;

    //    m_attackSettings.InsertArrayElementAtIndex(index);

    //    SerializedProperty element = m_attackSettings.GetArrayElementAtIndex(index);

    //    element.managedReferenceValue = data;
    //}

    // 攻撃ポイントを表示
    private void DrawAttackPoint(AttackPointInfo point)
    {
        EditorGUILayout.Space(8);

        EditorGUILayout.LabelField(
            point.DisplayName,
            EditorStyles.boldLabel
        );

        EditorGUILayout.HelpBox(
            point.Description,
            MessageType.Info
        );

        int index = FindSettingIndex(point.Id);

        // 設定が存在しない場合
        if (index < 0)
        {
            if (GUILayout.Button("Attack Settingを追加"))
            {
                AddSetting(point.Id);
            }

            return;
        }

        SerializedProperty setting =
            m_attackSettings.GetArrayElementAtIndex(index);

        SerializedProperty attackData =
            setting.FindPropertyRelative("m_attackData");

        // AttackDataが未設定の場合
        if (attackData.managedReferenceValue == null)
        {
            if (GUILayout.Button("AttackDataを選択"))
            {
                ShowAttackDataMenu(attackData.propertyPath);
            }
        }
        else
        {
            //EditorGUILayout.PropertyField(
            //    attackData,
            //    new GUIContent("Attack Data"),
            //    true
            //);

            DrawAttackData(attackData);

            if (GUILayout.Button("AttackDataを変更"))
            {
                ShowAttackDataMenu(attackData.propertyPath);
            }
        }
    }

    // IDから設定を検索
    private int FindSettingIndex(string id)
    {
        for (int i = 0; i < m_attackSettings.arraySize; i++)
        {
            SerializedProperty setting =
                m_attackSettings.GetArrayElementAtIndex(i);

            SerializedProperty idProperty =
                setting.FindPropertyRelative("m_attackPointId");

            if (idProperty.stringValue == id)
            {
                return i;
            }
        }

        return -1;
    }

    // 攻撃設定を追加
    private void AddSetting(string id)
    {
        int index = m_attackSettings.arraySize;

        m_attackSettings.InsertArrayElementAtIndex(index);

        SerializedProperty setting =
            m_attackSettings.GetArrayElementAtIndex(index);

        setting.FindPropertyRelative("m_attackPointId").stringValue = id;

        setting.FindPropertyRelative("m_attackData").managedReferenceValue = null;
    }

    // AttackDataの派生クラスを選択
    private void ShowAttackDataMenu(string propertyPath)
    {
        GenericMenu menu = new GenericMenu();

        var types = TypeCache.GetTypesDerivedFrom<AttackData>();

        foreach (Type type in types)
        {
            if (type.IsAbstract || type.IsGenericType)
                continue;

            // publicな引数なしコンストラクタが必要
            if (type.GetConstructor(Type.EmptyTypes) == null)
                continue;

            Type selectedType = type;

            menu.AddItem(
                new GUIContent(type.Name),
                false,
                () => SetAttackData(propertyPath, selectedType)
            );
        }

        menu.ShowAsContext();
    }

    // 選択したAttackDataを設定
    private void SetAttackData(string propertyPath, Type type)
    {
        serializedObject.Update();

        SerializedProperty property =
            serializedObject.FindProperty(propertyPath);

        property.managedReferenceValue =
            Activator.CreateInstance(type);

        serializedObject.ApplyModifiedProperties();
    }

    // 現在のEnemyに存在しないIDの設定を確認
    private void DrawUnknownSettings(List<AttackPointInfo> points)
    {
        var validIds = new HashSet<string>();

        foreach (var point in points)
        {
            validIds.Add(point.Id);
        }

        for (int i = 0; i < m_attackSettings.arraySize; i++)
        {
            SerializedProperty setting =
                m_attackSettings.GetArrayElementAtIndex(i);

            string id = setting
                .FindPropertyRelative("m_attackPointId")
                .stringValue;

            if (!validIds.Contains(id))
            {
                EditorGUILayout.HelpBox(
                    $"現在のEnemyには存在しないAttackPointです: {id}",
                    MessageType.Warning
                );
            }
        }
    }

    #region AttackData

    private void DrawAttackData(SerializedProperty attackData)
    {
        Type attackType =
            attackData.managedReferenceValue.GetType();

        SerializedProperty iterator = attackData.Copy();
        SerializedProperty end = iterator.GetEndProperty();

        bool enterChildren = true;

        while (iterator.NextVisible(enterChildren) &&
               !SerializedProperty.EqualContents(iterator, end))
        {
            enterChildren = false;

            if (iterator.name == "m_actionList")
            {
                DrawActionList(iterator, attackType);
            }
            else
            {
                EditorGUILayout.PropertyField(iterator, true);
            }
        }
    }

    private void DrawActionList(
    SerializedProperty actionList,
    Type attackType)
    {
        EditorGUILayout.Space(5);

        actionList.isExpanded = EditorGUILayout.Foldout(
            actionList.isExpanded,
            "Action List",
            true
        );

        if (!actionList.isExpanded)
            return;

        EditorGUI.indentLevel++;

        for (int i = 0; i < actionList.arraySize; i++)
        {
            SerializedProperty element =
                actionList.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginVertical("box");

            if (element.managedReferenceValue == null)
            {
                EditorGUILayout.LabelField("未設定");

                if (GUILayout.Button("Actionを選択"))
                {
                    ShowActionDataMenu(
                        actionList.propertyPath,
                        i,
                        attackType
                    );
                }
            }
            else
            {
                EditorGUILayout.LabelField(
                    element.managedReferenceValue.GetType().Name,
                    EditorStyles.boldLabel
                );

                EditorGUILayout.PropertyField(element, true);

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("変更"))
                {
                    ShowActionDataMenu(
                        actionList.propertyPath,
                        i,
                        attackType
                    );
                }

                if (GUILayout.Button("↑") && i > 0)
                {
                    actionList.MoveArrayElement(i, i - 1);
                }

                if (GUILayout.Button("↓") &&
                    i < actionList.arraySize - 1)
                {
                    actionList.MoveArrayElement(i, i + 1);
                }

                if (GUILayout.Button("削除"))
                {
                    actionList.DeleteArrayElementAtIndex(i);
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Actionを追加"))
        {
            ShowActionDataMenu(
                actionList.propertyPath,
                -1,
                attackType
            );
        }

        EditorGUI.indentLevel--;
    }

    private void ShowActionDataMenu(
    string propertyPath,
    int index,
    Type attackType)
    {
        GenericMenu menu = new GenericMenu();

        var types = TypeCache.GetTypesDerivedFrom<ActionData>();

        foreach (Type type in types)
        {
            if (type.IsAbstract || type.IsGenericType)
                continue;

            if (type.GetConstructor(Type.EmptyTypes) == null)
                continue;

            var attributes =
                type.GetCustomAttributes<ActionDataForAttribute>();

            bool canUse = attributes.Any(
                x => x.AttackDataType.IsAssignableFrom(attackType)
            );

            if (!canUse)
                continue;

            Type selectedType = type;

            menu.AddItem(
                new GUIContent(type.Name),
                false,
                () => SetActionData(
                    propertyPath,
                    index,
                    selectedType
                )
            );
        }

        menu.ShowAsContext();
    }

    private void SetActionData(
    string propertyPath,
    int index,
    Type type)
    {
        serializedObject.Update();

        SerializedProperty list =
            serializedObject.FindProperty(propertyPath);

        if (list == null)
            return;

        if (index < 0)
        {
            index = list.arraySize;

            list.InsertArrayElementAtIndex(index);
        }

        SerializedProperty element =
            list.GetArrayElementAtIndex(index);

        element.managedReferenceValue =
            Activator.CreateInstance(type);

        serializedObject.ApplyModifiedProperties();
    }

    #endregion
}
