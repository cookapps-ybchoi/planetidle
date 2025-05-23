using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.UI;

[CustomEditor(typeof(UEButton), true)]
[CanEditMultipleObjects]
public class UEButtonEditor : ButtonEditor
{
    private SerializedProperty _onPressProperty;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        _onPressProperty = serializedObject.FindProperty("_onPress");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        UEButton button = (UEButton) target;
        
        EditorGUILayout.Space();
        serializedObject.Update();
        EditorGUILayout.PropertyField(_onPressProperty);
        serializedObject.ApplyModifiedProperties();
        
        UEReactionType oldType = button.ReactionType;
        UEReactionType newType = (UEReactionType) EditorGUILayout.EnumPopup("Reaction Type", button.ReactionType);

        if (newType != oldType)
        {
            foreach (Object obj in targets)
            {
                button = (UEButton) obj;
                button.ReactionType = newType;
                EditorUtility.SetDirty(obj);
            }
        }
        
        UEButtonSoundType oldSoundType = button.SoundType;
        UEButtonSoundType newSoundType = (UEButtonSoundType) EditorGUILayout.EnumPopup("Click Sound", button.SoundType);

        if (newSoundType != oldSoundType)
        {
            foreach (Object obj in targets)
            {
                button = (UEButton) obj;
                button.SoundType = newSoundType;
                EditorUtility.SetDirty(obj);
            }
        }
        
        bool curValue = button.UseContinueClick;
        bool newValue = EditorGUILayout.Toggle("Use Continue Click", curValue);

        if (curValue != newValue)
        {
            foreach (Object obj in targets)
            {
                button = (UEButton) obj;
                button.UseContinueClick = newValue;
                EditorUtility.SetDirty(obj);
            }
        }
    }
}