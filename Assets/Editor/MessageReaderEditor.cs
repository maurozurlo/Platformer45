using UnityEngine;
using UnityEditor;
using System.IO;
using System;

[CustomEditor(typeof(MessageReader))]
public class MessageReaderEditor : Editor
{
    private SerializedProperty filePathProperty;

    private void OnEnable()
    {
        filePathProperty = serializedObject.FindProperty("filePath");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(filePathProperty);

        if (GUILayout.Button("Load JSON File"))
        {
            //MessageReader messageReader = (MessageReader)target;
            string filePath = EditorUtility.OpenFilePanel("Select JSON File", "", "json");
            if (!string.IsNullOrEmpty(filePath))
            {
                string path = "Dialogue" + filePath.Split(new string[] { "Dialogue" }, StringSplitOptions.None)[1];
                filePathProperty.stringValue = path.Substring(0,path.Length - 5);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}