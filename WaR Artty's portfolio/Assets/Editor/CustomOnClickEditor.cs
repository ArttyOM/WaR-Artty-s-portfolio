using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomOnClick))]
public class CustomOnClickEditor : Editor
{

    ButtonOwner buttonOwner;
    SerializedProperty ownerProp;
    SerializedProperty onGameViewEventProp;
    SerializedProperty onCityViewEventProp;

    SerializedProperty damageProp;
    SerializedProperty armorProp;


    void OnEnable()
    {
        // Setup the SerializedProperties.
        ownerProp = serializedObject.FindProperty("window");
        onGameViewEventProp = serializedObject.FindProperty("onGameViewEvent");
        onCityViewEventProp = serializedObject.FindProperty("onCityViewEvent");
        // Debug.Log("myInt " + ownerProp.);

        damageProp = serializedObject.FindProperty("damage");
        armorProp = serializedObject.FindProperty("armor");
    }

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        EditorGUILayout.PropertyField(ownerProp);
        buttonOwner = (ButtonOwner)ownerProp.enumValueIndex;
        switch (buttonOwner)
        {
            case ButtonOwner.Auto://в случае с автоматическим режимом дополнительных выпадающих списков не нужно
                break;
            case ButtonOwner.GameView:
                EditorGUILayout.PropertyField(onGameViewEventProp);//дополнительно отображаем enum основных кнопок
                break;
            case ButtonOwner.CityView:
                EditorGUILayout.PropertyField(onCityViewEventProp);
                break;
            default: break;
        }

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }

    // Custom GUILayout progress bar.
    void ProgressBar(float value, string label)
    {
        // Get a rect for the progress bar using the same margins as a textfield:
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        EditorGUI.ProgressBar(rect, value, label);
        EditorGUILayout.Space();
    }

    void AddPopup(ref SerializedProperty ourSerializedProperty, string nameOfLabel, Type typeOfEnum)
    {
        Rect ourRect = EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginProperty(ourRect, GUIContent.none, ourSerializedProperty);
        EditorGUI.BeginChangeCheck();

        int actualSelected = 1;
        int selectionFromInspector = ourSerializedProperty.intValue;
        string[] enumNamesList = System.Enum.GetNames(typeOfEnum);
        actualSelected = EditorGUILayout.Popup(nameOfLabel, selectionFromInspector, enumNamesList);
        ourSerializedProperty.intValue = actualSelected;

        EditorGUI.EndProperty();
        EditorGUILayout.EndHorizontal();
    }

}
