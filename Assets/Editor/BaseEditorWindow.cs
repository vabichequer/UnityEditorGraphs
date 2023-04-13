using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private PropertyInfo[] _properties;
        private FieldInfo _selectedField;
        private PropertyInfo _selectedProperty;
        private object _selectedComponent;
        private bool _isFieldSelected, _isVariableSelected, _isVector;

        // Graph references
        private List<List<float>> _valuesToPlot;
        private readonly List<Color> _availableColors = new List<Color>
        {
            Color.green, Color.blue, Color.red, Color.yellow
        };

        [MenuItem("Window/Graphs/Base window")]
        public static void ShowWindow()
        {
            BaseEditorWindow wnd = GetWindow<BaseEditorWindow>();
            wnd.titleContent = new GUIContent("Base window");
        }

        private void OnObjectChanged(ChangeEvent<Object> evt)
        {
            _objectToAnalyze = (GameObject) evt.newValue;
            _isVariableSelected = false;

            if (_objectToAnalyze != null)
            {
                // Get all the components attached to this gameobject
                var components = _objectToAnalyze.GetComponents<Component>();

                ClearData(DataHandling.ClearDataModes.Initialize);
                
                // Loop through each component
                foreach (var component in components)
                {
                    //Debugging.Print(component.GetType().Name);

                    var addProp = false;
                    
                    // Get all the public fields of the component
                    var fields = component.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
                    var properties = component.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    foreach (var field in fields)
                    {
                        //Debugging.Print(field.Name);
                    }
                    
                    foreach (var property in properties)
                    {
                        //Debugging.Print(property.Name, property.PropertyType, numProp);
                        if (Numeric.Is(property.PropertyType).Item1)
                        {
                            addProp = true;
                        }
                    }
                    
                    // If the component has any public fields, add its type to the list
                    if (fields.Length > 0 || addProp)
                    {
                        _componentDropDown.choices.Add(component.GetType().Name);
                    }
                }
            }
        }

        private void OnComponentDropDownSelection(ChangeEvent<string> evt)
        {
            var component = evt.newValue;
            
            ClearData(DataHandling.ClearDataModes.ComponentChange);

            if (evt.newValue == null)
            {
                return;
            }

            Debugging.Print("Is obj null?", _objectToAnalyze == null);
            
            // Get all the public fields of the given type from the component
            _fields = _objectToAnalyze.GetComponent(component).GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            _properties = _objectToAnalyze.GetComponent(component).GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in _fields)
            {
                _variableDropDown.choices.Add(field.Name);
            }
            
            foreach (var property in _properties)
            {
                if (Numeric.Is(property.PropertyType).Item1)
                {
                    _variableDropDown.choices.Add(property.Name);
                }
            }
        }

        private void PlotSelectedVariable(ChangeEvent<string> evt)
        {
            _selectedComponent = _objectToAnalyze.GetComponent(_componentDropDown.value);
            _isVariableSelected = true;
            _isFieldSelected = false;
            
            ClearData(DataHandling.ClearDataModes.VariableChange);
            
            if (_fields.Any(field => field.Name == _fields[_variableDropDown.index].Name))
            {
                _selectedField = _fields[_variableDropDown.index];
                (_, _isVector) = Numeric.Is(_selectedField.FieldType);
                _isFieldSelected = true;
                return;
            }
            
            Debugging.Print("e ai:", _isVariableSelected);

            _selectedProperty = _properties[_variableDropDown.index];
            (_, _isVector) = Numeric.Is(_selectedProperty.PropertyType);
        }

        private void HandleDrawing()
        {
            if (Event.current.type is EventType.Repaint)
            {
                InitializePlot(_leftPane.resolvedStyle.width);
                DrawBackground(BackgroundConfig.BackgroundTypes.CHECKERED, Color.black, 50);

                if (!_isVariableSelected)
                {
                    FinalizePlot();
                    return;
                }

                if (_objectToAnalyze == null)
                {
                    FinalizePlot();
                    return;
                }
                
                for (var i = 0; i < _valuesToPlot[0].Count; i++)
                {
                    DrawLineArray(_valuesToPlot.Select(list => list[i]).ToList(), _availableColors[i]);
                }
                FinalizePlot();
            }
        }

        private void UpdateVariables()
        {
            if (_objectToAnalyze == null)
            {
                
                return;
            }
            
            if (!_isVariableSelected)
            {
                return;
            }

            while (_valuesToPlot.Count > width)
            {
                _valuesToPlot.RemoveAt(0);
            }

            if (_isFieldSelected)
            {
                var field = _selectedField.GetValue(_selectedComponent);
                if (_isVector)
                {
                    _valuesToPlot.Add(Numeric.GetVector(_selectedField.FieldType, field));
                }
                else
                {
                    _valuesToPlot.Add(new List<float>(){(float) field});
                }
                return;
            }

            var property = _selectedProperty.GetValue(_selectedComponent);
            if (_isVector)
            {
                _valuesToPlot.Add(Numeric.GetVector(_selectedProperty.PropertyType, property));
            }
            else
            {
                _valuesToPlot.Add(new List<float>(){(float) property});
            }
        }
        
        private void Update()
        {
            UpdateVariables();
            Repaint();
        }

        private void ClearData(DataHandling.ClearDataModes mode)
        {
            Debugging.Print("ClearData() called with mode", mode.ToString());
            switch (mode)
            {
                case DataHandling.ClearDataModes.Complete:
                    _valuesToPlot = new List<List<float>>();
                    _componentDropDown.choices = new List<string>();
                    _variableDropDown.choices = new List<string>();
                    _objectToAnalyze = null;
                    _isVariableSelected = false;
                    _isVector = false;
                    _isFieldSelected = false;
                    break;
                case DataHandling.ClearDataModes.Initialize:
                    _valuesToPlot = new List<List<float>>();
                    _componentDropDown.choices = new List<string>();
                    _variableDropDown.choices = new List<string>();
                    _isVariableSelected = false;
                    _isVector = false;
                    _isFieldSelected = false;
                    break;
                case DataHandling.ClearDataModes.ComponentChange:
                    _valuesToPlot = new List<List<float>>();
                    _variableDropDown.choices = new List<string>();
                    _isVariableSelected = false;
                    _isVector = false;
                    _isFieldSelected = false;
                    break;
                case DataHandling.ClearDataModes.VariableChange:
                    _valuesToPlot = new List<List<float>>();
                    _isVector = false;
                    _isFieldSelected = false;
                    break;
            }
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
            
            ClearData(DataHandling.ClearDataModes.Initialize);
        }
    }
}
