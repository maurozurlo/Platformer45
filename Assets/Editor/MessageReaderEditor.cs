using UnityEngine;
using UnityEditor;
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
            string filePath = EditorUtility.OpenFilePanel("Select File", "", "json");
            if (!string.IsNullOrEmpty(filePath))
            {
                string path = "Dialogue" + filePath.Split(new string[] { "Dialogue" }, StringSplitOptions.None)[1];
                string noExtension = path.Split(new string[] { "." }, StringSplitOptions.None)[0];
                filePathProperty.stringValue = noExtension;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}