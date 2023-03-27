using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class BaseEditorWindow : EditorWindow
    {
        private GameObject _objectToAnalyze;
        private DropdownField _componentDropDown, _variableDropDown;
        private FieldInfo[] _fields;
        
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
                        _componentDropDown.choices.Add(component.GetType().Name);
                    }
                }
            }
        }

        private void OnComponentDropDownSelection(ChangeEvent<string> evt)
        {
            var component = evt.newValue;

            if (evt.newValue == null)
            {
                return;
            }

            // Get all the public fields of the given type from the component
            _fields = _objectToAnalyze.GetComponent(component).GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in _fields)
            {
                _variableDropDown.choices.Add(field.Name);
            }
        }

        private void PlotSelectedVariable(ChangeEvent<string> evt)
        {
            object obj = _objectToAnalyze.GetComponent(_componentDropDown.value);
            var selectedField = _fields[_variableDropDown.index];
            
            //  Selected variable name: evt.newValue 
            //  and value: selectedField.GetValue(obj))
            
            
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

            _componentDropDown = new DropdownField();
            _componentDropDown.RegisterValueChangedCallback(OnComponentDropDownSelection);
            leftPane.Add(_componentDropDown);

            _variableDropDown = new DropdownField();
            _variableDropDown.RegisterValueChangedCallback(PlotSelectedVariable);
            leftPane.Add(_variableDropDown);
        }
    }
}
