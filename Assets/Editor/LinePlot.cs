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
        private VisualElement leftPane, rightPane;
        
        // Object's references
        private GameObject objectToAnalyze;
        private DropdownField componentDropDown, variableDropDown;
        private ObjectField gameObjectField;
        private FieldInfo[] fields;
        private PropertyInfo[] properties;
        private FieldInfo selectedField;
        private PropertyInfo selectedProperty;
        private object selectedComponent;
        private bool isFieldSelected, isVariableSelected, isVector;
        private int variableLength;

        // Graph references
        private List<List<float>> valuesToPlot;
        private readonly List<Color> availableColors = new List<Color>
        {
            Color.red, Color.green, Color.blue, Color.yellow
        };
        private List<string> variableNames, vectorNames = new List<string>{"X", "Y", "Z", "W"};
        
        [MenuItem("Window/Graphs/Line Plot")]
        public static void ShowWindow()
        {
            BaseEditorWindow wnd = GetWindow<BaseEditorWindow>();
            wnd.titleContent = new GUIContent("Line Plot");
        }

        private void OnObjectChanged(ChangeEvent<Object> evt)
        {
            objectToAnalyze = (GameObject) evt.newValue;
            isVariableSelected = false;

            if (objectToAnalyze != null)
            {
                // Get all the components attached to this gameobject
                Component[] components = objectToAnalyze.GetComponents<Component>();

                ClearData(DataHandling.ClearDataModes.Initialize);
                
                // Loop through each component
                foreach (Component component in components)
                {
                    //Debugging.Print(component.GetType().Name);

                    bool addProp = false;
                    
                    // Get all the public fields of the component
                    FieldInfo[] fields = component.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
                    PropertyInfo[] properties = component.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    foreach (FieldInfo field in fields)
                    {
                        //Debugging.Print(field.Name);
                    }
                    
                    foreach (PropertyInfo property in properties)
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
                        componentDropDown.choices.Add(component.GetType().Name);
                    }
                }
            }
        }

        private void OnComponentDropDownSelection(ChangeEvent<string> evt)
        {
            string component = evt.newValue;
            
            ClearData(DataHandling.ClearDataModes.ComponentChange);

            if (evt.newValue == null)
            {
                return;
            }

            Debugging.Print("Is obj null?", objectToAnalyze == null);
            
            // Get all the public fields of the given type from the component
            fields = objectToAnalyze.GetComponent(component).GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            properties = objectToAnalyze.GetComponent(component).GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                variableDropDown.choices.Add(field.Name);
            }
            
            foreach (PropertyInfo property in properties)
            {
                if (Numeric.Is(property.PropertyType).Item1)
                {
                    variableDropDown.choices.Add(property.Name);
                }
            }
        }

        private void PlotSelectedVariable(ChangeEvent<string> evt)
        {
            selectedComponent = objectToAnalyze.GetComponent(componentDropDown.value);
            isVariableSelected = true;
            isFieldSelected = false;
            
            ClearData(DataHandling.ClearDataModes.VariableChange);
            
            if (fields.Any(field => field.Name == fields[variableDropDown.index].Name))
            {
                selectedField = fields[variableDropDown.index];
                (_, isVector) = Numeric.Is(selectedField.FieldType);
                variableLength = Numeric.Length(selectedField.FieldType);
                isFieldSelected = true;
                return;
            }

            selectedProperty = properties[variableDropDown.index];
            (_, isVector) = Numeric.Is(selectedProperty.PropertyType);
            variableLength = Numeric.Length(selectedProperty.PropertyType);
        }

        private void HandleDrawing()
        {
            if (objectToAnalyze == null)
            {
                return; 
            }
            
            if (Event.current.type is EventType.Repaint)
            {
                InitializePlot(new Vector2(leftPane.contentRect.width, 0), leftPane.contentRect.height);
                DrawBackground(BackgroundConfig.BackgroundTypes.CHECKERED, Color.black, 50);
                DrawAxes();

                if (!isVariableSelected)
                {
                    FinalizePlot();
                    return;
                }

                if (objectToAnalyze == null)
                {
                    FinalizePlot();
                    return;
                }
                
                for (int i = 0; i < variableLength; i++)
                {
                    DrawLineArray(valuesToPlot.Select(list => list[i]).ToList(), availableColors[i]);
                }
                
                DrawLegend(variableNames, availableColors);
                
                FinalizePlot();
            }
        }

        private void UpdateVariables()
        {
            if (objectToAnalyze == null)
            {
                return;
            }
            
            if (!isVariableSelected)
            {
                return;
            }

            while (valuesToPlot.Count > width)
            {
                valuesToPlot.RemoveAt(0);
            }

            if (isFieldSelected)
            {
                object field = selectedField.GetValue(selectedComponent);
                if (isVector)
                {
                    List<float> vector = Numeric.GetVector(selectedField.FieldType, field);
                    valuesToPlot.Add(vector);
                    variableNames = new List<string>();
                    for (int i = 0; i < vector.Count; i++)
                    {
                        variableNames.Add(vectorNames[i]);
                    }
                }
                else
                {
                    valuesToPlot.Add(new List<float>(){(float) field});
                    variableNames = new List<string>{ selectedField.Name };
                }
                return;
            }

            object property = selectedProperty.GetValue(selectedComponent);
            if (isVector)
            {
                List<float> vector = Numeric.GetVector(selectedProperty.PropertyType, property);
                valuesToPlot.Add(vector);
                variableNames = new List<string>();
                for (int i = 0; i < vector.Count; i++)
                {
                    variableNames.Add(vectorNames[i]);
                }
            }
            else
            {
                valuesToPlot.Add(new List<float>(){(float) property});
                variableNames = new List<string> { selectedProperty.Name };
            }
        }
        
        private void Update()
        {
            UpdateVariables();
            Repaint();
        }

        private void ClearData(DataHandling.ClearDataModes mode)
        {
            switch (mode)
            {
                case DataHandling.ClearDataModes.Complete:
                    valuesToPlot = new List<List<float>>();
                    variableNames = new List<string>();
                    for (int i = 0; i < width; i++)
                    {
                        valuesToPlot.Add(new List<float>(){0, 0, 0, 0});
                    }
                    componentDropDown.choices = new List<string>();
                    variableDropDown.choices = new List<string>();
                    objectToAnalyze = null;
                    isVariableSelected = false;
                    isVector = false;
                    isFieldSelected = false;
                    variableLength = 0;
                    break;
                case DataHandling.ClearDataModes.Initialize:
                    valuesToPlot = new List<List<float>>();
                    variableNames = new List<string>();
                    for (int i = 0; i < width; i++)
                    {
                        valuesToPlot.Add(new List<float>(){0, 0, 0, 0});
                    }
                    componentDropDown.choices = new List<string>();
                    variableDropDown.choices = new List<string>();
                    isVariableSelected = false;
                    isVector = false;
                    isFieldSelected = false;
                    variableLength = 0;
                    break;
                case DataHandling.ClearDataModes.ComponentChange:
                    valuesToPlot = new List<List<float>>();
                    variableNames = new List<string>();
                    for (int i = 0; i < width; i++)
                    {
                        valuesToPlot.Add(new List<float>(){0, 0, 0, 0});
                    }
                    variableDropDown.choices = new List<string>();
                    isVariableSelected = false;
                    isVector = false;
                    isFieldSelected = false;
                    variableLength = 0;
                    break;
                case DataHandling.ClearDataModes.VariableChange:
                    valuesToPlot = new List<List<float>>();
                    variableNames = new List<string>();
                    for (int i = 0; i < width; i++)
                    {
                        valuesToPlot.Add(new List<float>(){0, 0, 0, 0});
                    }
                    isVector = false;
                    isFieldSelected = false;
                    variableLength = 0;
                    break;
            }
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;
            
            // Separate the window into variables and plot
            TwoPaneSplitView splitView = new TwoPaneSplitView(0, 200, TwoPaneSplitViewOrientation.Horizontal);
            splitView.usageHints = UsageHints.GroupTransform;
            root.Add(splitView);
            
            // Panels from the splitview
            leftPane = new VisualElement();
            splitView.Add(leftPane);
            rightPane = new VisualElement
            {
                usageHints = UsageHints.GroupTransform
            };
            splitView.Add(rightPane);
            
            // Add a GameObject to expose possible variables
            gameObjectField = new ObjectField("Object to analyze");
            leftPane.Add(gameObjectField);
            gameObjectField.RegisterValueChangedCallback(OnObjectChanged);

            componentDropDown = new DropdownField();
            componentDropDown.RegisterValueChangedCallback(OnComponentDropDownSelection);
            leftPane.Add(componentDropDown);

            variableDropDown = new DropdownField();
            variableDropDown.RegisterValueChangedCallback(PlotSelectedVariable);
            leftPane.Add(variableDropDown);

            IMGUIContainer glContent = new IMGUIContainer(HandleDrawing);
            glContent.usageHints = UsageHints.DynamicColor;

            rightPane.Add(glContent);
            
            ClearData(DataHandling.ClearDataModes.Initialize);
            InitialPlotState();
        }
    }
}
