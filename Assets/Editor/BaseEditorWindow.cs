using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Editor
{
    public class BaseEditorWindow : EditorWindow
    {
        private GameObject _objectToAnalyze;
        private DropdownField _dropDown;
        
        [MenuItem("Window/Graphs/Base window")]
        public static void ShowWindow()
        {
            BaseEditorWindow wnd = GetWindow<BaseEditorWindow>();
            wnd.titleContent = new GUIContent("Base window");
        }

        private void OnObjectChanged(ChangeEvent<Object> evt)
        {
            _objectToAnalyze = (GameObject) evt.newValue;

            if (_objectToAnalyze != null)
            {
                // Get all the components attached to this gameobject
                var components = _objectToAnalyze.GetComponents<Component>();

                // Loop through each component
                foreach (var component in components)
                {
                    // Get all the public fields of the component
                    var fields = component.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                    // If the component has any public fields, print its type to the console
                    if (fields.Length > 0)
                    {
                        _dropDown.choices.Add(component.GetType().Name);
                    }
                }
            }
        }
        
        private FieldInfo[] CheckVariablesExposure()
        {
            if (_objectToAnalyze != null)
            {
                return _objectToAnalyze.GetComponent<CircularMotion>().GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            }

            return null;
        }
        
        private void OnDropDownSelection(ChangeEvent<Object> evt)
        {
            
        }
        
        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Separate the window into variables and plot
            var splitView = new TwoPaneSplitView(0, 200, TwoPaneSplitViewOrientation.Horizontal);
            root.Add(splitView);
            
            // Panels from the splitview
            var leftPane = new VisualElement();
            splitView.Add(leftPane);
            var rightPane = new VisualElement();
            splitView.Add(rightPane);
            
            // Add a GameObject to expose possible variables
            var gameObjectField = new ObjectField("Object to analyze");
            leftPane.Add(gameObjectField);
            gameObjectField.RegisterValueChangedCallback(OnObjectChanged);
            
            // Exposing variables
            var fields = CheckVariablesExposure();

            _dropDown = new DropdownField();
            _dropDown.RegisterValueChangedCallback(OnDropDownSelection());
            leftPane.Add(_dropDown);
        }
    }
}
