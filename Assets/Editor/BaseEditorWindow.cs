using System;
using System.Collections.Generic;
using System.Reflection;
using Enums;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;
using Object = UnityEngine.Object;

namespace Editor
{
    public class BaseEditorWindow : DrawPrimitives
    {
        // Window structure
        private VisualElement _leftPane, _rightPane;
        
        // Object's references
        private GameObject _objectToAnalyze;
        private DropdownField _componentDropDown, _variableDropDown;
        private ObjectField _gameObjectField;
        private FieldInfo[] _fields;
        private FieldInfo _selectedField;
        private object _selectedVariable;
        private bool _isVariableSelected;
        
        // Graph references
        private List<int> _valuesToPlot;

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
                    Debugging.Print(component.GetType().Name);
                    
                    // Get all the public fields of the component
                    var fields = component.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
                    var properties = component.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    
                    foreach (var field in fields)
                    {
                        Debugging.Print(field.Name);
                    }
                    
                    foreach (var property in properties)
                    {
                        Debugging.Print(property.Name, property.PropertyType, Numeric.Is(property.PropertyType));
                    }
                    
                    // If the component has any public fields, add its type to the list
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
            _selectedVariable = _objectToAnalyze.GetComponent(_componentDropDown.value);
            _selectedField = _fields[_variableDropDown.index];
            _isVariableSelected = true;

            //  Selected variable name: evt.newValue 
            //  and value: selectedField.GetValue(obj))
        }

        private void HandleDrawing()
        {
            if (Event.current.type is EventType.Repaint)
            {
                InitializePlot(_leftPane.resolvedStyle.width);
                DrawBackground(BackgroundConfig.BackgroundTypes.CHECKERED, Color.black, 50);
                
                //DrawSquare(new Vector3(width/2, height, 0), new Vector3(width, 250, 0), Color.cyan);
                
                DrawLineArray(_valuesToPlot, Color.green);
                FinalizePlot();
            }
        }

        private void UpdateVariables()
        {
            if (!_isVariableSelected)
            {
                return;
            }

//            _valuesToPlot.Add(Convert.ToInt32(_selectedField.GetValue(_selectedVariable)));
        }
        
        private void Update()
        {
            UpdateVariables();
            Repaint();
        }

        private void ClearData()
        {
            _valuesToPlot = new List<int>();
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;
            
            // Separate the window into variables and plot
            var splitView = new TwoPaneSplitView(0, 200, TwoPaneSplitViewOrientation.Horizontal);
            splitView.usageHints = UsageHints.GroupTransform;
            root.Add(splitView);
            
            // Panels from the splitview
            _leftPane = new VisualElement();
            splitView.Add(_leftPane);
            _rightPane = new VisualElement
            {
                usageHints = UsageHints.GroupTransform
            };
            splitView.Add(_rightPane);
            
            // Add a GameObject to expose possible variables
            _gameObjectField = new ObjectField("Object to analyze");
            _leftPane.Add(_gameObjectField);
            _gameObjectField.RegisterValueChangedCallback(OnObjectChanged);

            _componentDropDown = new DropdownField();
            _componentDropDown.RegisterValueChangedCallback(OnComponentDropDownSelection);
            _leftPane.Add(_componentDropDown);

            _variableDropDown = new DropdownField();
            _variableDropDown.RegisterValueChangedCallback(PlotSelectedVariable);
            _leftPane.Add(_variableDropDown);

            var glContent = new IMGUIContainer(HandleDrawing);
            glContent.usageHints = UsageHints.DynamicColor;

            _rightPane.Add(glContent);
            
            ClearData();
        }
    }
}
