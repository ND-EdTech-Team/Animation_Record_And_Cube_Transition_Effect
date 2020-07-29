using UnityEditor;
using UnityEngine;

namespace Plugins.Cube.Editor
{
    [CustomEditor(typeof(CompositeCubeAnimator))]
    public class CompositeCubeAnimatorEditor : UnityEditor.Editor
    {
        private SerializedProperty _filePathProperty;
        private SerializedProperty _prefabCubeProperty;
        private SerializedProperty _speedProperty;
        private SerializedProperty _totalTimeProperty;
        private SerializedProperty _sampleTimeProperty;
        private CompositeCubeAnimator _animator;

        private void OnEnable()
        {
            _animator = (CompositeCubeAnimator) target;
            
            _filePathProperty = serializedObject.FindProperty("filePath");
            _prefabCubeProperty = serializedObject.FindProperty(nameof(CompositeCubeAnimator.prefabCube));
            _speedProperty = serializedObject.FindProperty(nameof(CompositeCubeAnimator.speed));
            _totalTimeProperty = serializedObject.FindProperty(nameof(CompositeCubeAnimator.totalTime));
            _sampleTimeProperty = serializedObject.FindProperty(nameof(CompositeCubeAnimator.sampleTime));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_filePathProperty);
            if (EditorGUI.EndChangeCheck())
                _animator.Load(_filePathProperty.stringValue);
            
            if (GUILayout.Button("Open", GUILayout.Width(60)))
            {
                OnOpenFile();
                GUIUtility.ExitGUI();
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.PropertyField(_prefabCubeProperty);
            EditorGUILayout.PropertyField(_speedProperty);
            EditorGUILayout.PropertyField(_sampleTimeProperty);
            GUI.enabled = false;
            EditorGUILayout.PropertyField(_totalTimeProperty);
            GUI.enabled = true;

            serializedObject.ApplyModifiedProperties();
        }

        private void OnOpenFile()
        {
            var path = EditorUtility.OpenFilePanel("Open File", string.Empty, string.Empty);
            if (!string.IsNullOrEmpty(path))
                _animator.Load(path);
        }
    }
}
