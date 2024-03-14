using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(CSVReader))]
public class CSVReaderEditor : Editor
{
    private SerializedProperty filePathsProperty;

    private void OnEnable()
    {
        filePathsProperty = serializedObject.FindProperty("filePaths");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(filePathsProperty, true);

        if (GUILayout.Button("Add CSV File"))
        {
            string filePath = EditorUtility.OpenFilePanel("Select File", "", "csv");
            if (!string.IsNullOrEmpty(filePath))
            {
                // Add the file path to the serialized property
                filePathsProperty.arraySize++;
                string path = "Dialogue" + filePath.Split(new string[] { "Dialogue" }, StringSplitOptions.None)[1];
                string noExtension = path.Split(new string[] { "." }, StringSplitOptions.None)[0];
                filePathsProperty.GetArrayElementAtIndex(filePathsProperty.arraySize - 1).stringValue = noExtension;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
