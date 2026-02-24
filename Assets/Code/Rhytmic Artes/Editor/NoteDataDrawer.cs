using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(NoteData))]
public class NoteDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Draw background
        GUI.Box(position, GUIContent.none);

        // Start drawing fields
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        Rect line = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        // Beat field
        var beatProp = property.FindPropertyRelative("beat");
        EditorGUI.PropertyField(line, beatProp);
        line.y += EditorGUIUtility.singleLineHeight + 2;

        // Type field
        var typeProp = property.FindPropertyRelative("type");
        EditorGUI.PropertyField(line, typeProp);
        line.y += EditorGUIUtility.singleLineHeight + 2;

        if ((NoteType)typeProp.enumValueIndex == NoteType.Button)
        {
            // Draw a header-like label
            EditorGUI.LabelField(line, "Button Settings", EditorStyles.boldLabel);
            line.y += EditorGUIUtility.singleLineHeight + 4; // extra spacing below header

            var approachProp = property.FindPropertyRelative("approachBeats");
            EditorGUI.PropertyField(line, approachProp);
            line.y += EditorGUIUtility.singleLineHeight + 2;

            var posProp = property.FindPropertyRelative("spawnPosition");
            EditorGUI.PropertyField(line, posProp);
            line.y += EditorGUIUtility.singleLineHeight + 2;
    
            var prefabIndexProp = property.FindPropertyRelative("buttonPrefabIndex");
            EditorGUI.PropertyField(line, prefabIndexProp);
            line.y += EditorGUIUtility.singleLineHeight + 2;

        }
        if ((NoteType)typeProp.enumValueIndex == NoteType.Special)
        {
            // Draw a header-like label
            EditorGUI.LabelField(line, "Special Settings", EditorStyles.boldLabel);
            line.y += EditorGUIUtility.singleLineHeight + 4; // extra spacing below header

            var approachProp = property.FindPropertyRelative("approachBeats");
            EditorGUI.PropertyField(line, approachProp);
            line.y += EditorGUIUtility.singleLineHeight + 2;

            var posProp = property.FindPropertyRelative("spawnPosition");
            EditorGUI.PropertyField(line, posProp);
            line.y += EditorGUIUtility.singleLineHeight + 2;

        }
        if ((NoteType)typeProp.enumValueIndex == NoteType.CircleSlider)
        {
            EditorGUI.LabelField(line, "Circle Slider Settings", EditorStyles.boldLabel);
            line.y += EditorGUIUtility.singleLineHeight + 4; // extra spacing below header

            var finalSpinProp = property.FindPropertyRelative("isFinalSpin");
            EditorGUI.PropertyField(line, finalSpinProp);
            line.y += EditorGUIUtility.singleLineHeight + 2;

        }


        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int lines = 2; // beat + type always visible
        var typeProp = property.FindPropertyRelative("type");
        if ((NoteType)typeProp.enumValueIndex == NoteType.Button)
            lines += 5;
        if ((NoteType)typeProp.enumValueIndex == NoteType.Special)
            lines += 4;  
        if ((NoteType)typeProp.enumValueIndex == NoteType.CircleSlider)
            lines += 3; 
        return (EditorGUIUtility.singleLineHeight + 2) * lines + 4; // add a little buffer
    }

}
