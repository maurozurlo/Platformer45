using UnityEngine;
using UnityEditor;
using System.IO;

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
            MessageReader messageReader = (MessageReader)target;
            string filePath = EditorUtility.OpenFilePanel("Select JSON File", "", "json");
            if (!string.IsNullOrEmpty(filePath))
            {
                filePathProperty.stringValue = filePath;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}