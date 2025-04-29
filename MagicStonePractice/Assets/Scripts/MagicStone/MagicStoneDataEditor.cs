using UnityEditor;
using UnityEngine;

namespace MagicStone
{
    [CustomEditor(typeof(MagicStoneData))]
    public class MagicStoneDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            MagicStoneData item = (MagicStoneData)target;

            // 일반 속성
            item.stoneName = EditorGUILayout.TextField("Stone Name", item.stoneName);
            item.skillName = EditorGUILayout.TextField("Stone SkillName", item.skillName);
            item.description = EditorGUILayout.TextField("Stone Desc", item.description);
            item.stoneIcon = (Sprite)EditorGUILayout.ObjectField("Stone Icon", item.stoneIcon, typeof(Sprite), false);
            item.width = EditorGUILayout.IntField("Width", item.width);
            item.height = EditorGUILayout.IntField("Height", item.height);

            // 사이즈 맞게 shape 초기화
            while (item.shape.Count < item.height)
                item.shape.Add(new MagicStoneData.Cell());

            if (item.shape.Count > item.height)
            {
                int countToRemove = item.shape.Count - item.height;
                item.shape.RemoveRange(item.height, countToRemove);
            }

            for (int y = 0; y < item.height; y++)
            {
                while (item.shape[y].cols.Count < item.width)
                    item.shape[y].cols.Add(false);

                if (item.shape[y].cols.Count > item.width)
                {
                    int countToRemove = item.shape[y].cols.Count - item.width;
                    item.shape[y].cols.RemoveRange(item.width, countToRemove);
                }
            }

            EditorGUILayout.LabelField("Stone Shape:");

            // 그리드 UI
            for (int y = 0; y < item.height; y++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < item.width; x++)
                {
                    item.shape[y][x] = GUILayout.Toggle(item.shape[y][x], "", "Button", GUILayout.Width(25),
                        GUILayout.Height(25));
                }

                EditorGUILayout.EndHorizontal();
            }

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(item);
        }
    }
}