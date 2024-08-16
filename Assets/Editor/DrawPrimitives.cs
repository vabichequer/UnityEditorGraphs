using System;
using System.Collections.Generic;
using System.Linq;
using Components;
using Enums;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class DrawPrimitives : EditorWindow
    {
        public Rect rect;
        
        private Material mat;
        private Shader shader;
        private float xMargin, yMargin, xIncrements, yIncrements;
        private int yMax, yMin;
        
        // Viewport parameters
        //private const int TopOffset = 21;
        private float top, bottom;
        public float height, width;
        
        // Legend parameters
        private const int ItemSize = 15;
        private const int ItemMargin = 5;
        private const int BoxMargin = 25;
        private const int BoxSize = 100;
        private const float NumberOfBrackets = 6;

        protected void InitialPlotState()
        {
            yMax = int.MinValue;
            yMin = int.MaxValue;
            
            xMargin = 0.05f * width;
            yMargin = 0.05f * height;
        }
        
        protected void InitializePlot(Vector2 offset = default, float usedHeight = 0)
        {
            width = position.width - offset.x;
            height = usedHeight;
            
            yIncrements = (height - yMargin * 2) / (Mathf.Max(yMax, yMin * -1) * 2 + 1);
            
            shader = Shader.Find("Hidden/Internal-Colored");
            
            mat = new Material(shader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            
            mat.SetPass(0);
            GL.PushMatrix();

            top = (height + yMargin) / 2;
            bottom = -top + yMargin / 2;

            Matrix4x4 proj = Matrix4x4.Ortho(-offset.x, width, bottom, top, -1, 1);
            GL.LoadProjectionMatrix(proj);
        }
        
        private Vector2 ScaledCoords(Vector2 c)
        {
            return new Vector2(c.x, c.y * yIncrements);
        }
        
        public void DrawAxes()
        {
            // x
            DrawLine(new Vector3(0, 0), new Vector3(width, 0), Color.red);
            
            // y
            DrawLine(new Vector3(xMargin, height / 2), new Vector3(xMargin, -height / 2), Color.green);

            if (yMax <= 0)
            {
                return;
            }

            int bracket = (int)Math.Ceiling(yMax / NumberOfBrackets);
            
            for (float y = 0f; y < NumberOfBrackets; y += 1)
            {
                float yBracketed = bracket * y;
                WriteOnScreen.DrawNumber(ScaledCoords(new Vector2(xMargin - 15, yBracketed)), 10, yBracketed, Color.white);
                WriteOnScreen.DrawNumber(ScaledCoords(new Vector2(xMargin - 15, -yBracketed)), 10, -yBracketed, Color.white);
            }
        }

        public void DrawLegend(List<string> itemNames, List<Color> itemColors)
        {
            int numberOfItems = itemNames.Count;
            
            // Box
            float boxXl = width - (BoxSize + BoxMargin);
            float boxXh = width - BoxMargin;
            float boxYh = height / 2 - BoxMargin;
            float boxYl = boxYh - ItemSize * numberOfItems - ItemMargin * (numberOfItems + 1);

            // Item
            float itemXl = width - (BoxMargin + BoxSize) + ItemMargin;
            float itemXh = itemXl + ItemSize;
            
            Vector2 ll = new Vector2(boxXl, boxYl);
            Vector2 hr = new Vector2(boxXh, boxYh);
            DrawHollowSquare(ll, hr, Color.white);

            for (int i = 0; i < itemNames.Count; i++)
            {
                float itemYl = boxYh - (ItemMargin + ItemSize + (ItemMargin + ItemSize) * i);
                float itemYh = boxYh - (ItemMargin + (ItemMargin + ItemSize) * i);
                
                DrawSquare(
                    lowLeft:new Vector2(itemXl, itemYl),
                    highRight:new Vector2(itemXh, itemYh),
                    color: itemColors[i]
                    );
                
                WriteOnScreen.DrawWord(
                    new Vector2(itemXh + ItemMargin * 2, (itemYh + itemYl) / 2), 
                    10, 
                    itemNames[i], 
                    Color.white);
            }
        }

        protected void DrawLineArray(List<float> points, Color color)
        {
            int tempMin = (int)points.Min();
            if (yMin > tempMin)
            {
                yMin = tempMin;
            }

            int tempMax = (int)points.Max();
            if (yMax < tempMax)
            {
                yMax = tempMax;
            }
            
            for(int i = 1; i < points.Count; i++)
            {
                Vector2 start = ScaledCoords(new Vector3( (i - 1),   points[i - 1], 0));
                Vector2 end = ScaledCoords(new Vector3(i, points[i], 0));
                
                DrawLine(start, end,  color);
            }
        }
        
        public void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            GL.Begin(GL.LINES);
            GL.Color(color);
            GL.Vertex(start);
            GL.Vertex(end);
            GL.End();
        }

        public void DrawSquare(Vector3 lowLeft, Vector3 highRight, Color color)
        {
            Vector3 highLeft = new Vector3(lowLeft.x, highRight.y, 0);
            Vector3 lowRight = new Vector3(highRight.x, lowLeft.y, 0);

            GL.Begin(GL.QUADS);
            GL.Color(color);
            GL.Vertex(lowLeft);
            GL.Vertex(highLeft);
            GL.Vertex(highRight);
            GL.Vertex(lowRight);
            GL.End();
        }

        public void DrawHollowSquare(Vector3 lowLeft, Vector3 highRight, Color color)
        {
            Vector3 highLeft = new Vector3(lowLeft.x, highRight.y, 0);
            Vector3 lowRight = new Vector3(highRight.x, lowLeft.y, 0);

            GL.Begin(GL.LINE_STRIP);
            GL.Color(color);
            GL.Vertex(lowLeft);
            GL.Vertex(highLeft);
            GL.Vertex(highRight);
            GL.Vertex(lowRight);
            GL.Vertex(lowLeft);
            GL.End();
        }

        protected void DrawBackground(BackgroundConfig.BackgroundTypes bgType, Color color, int spacing=0)
        {
            DrawSquare(new Vector3(0, -height / 2, 0), new Vector3(width, height / 2, 0), color);
            switch (bgType)
            {
                case BackgroundConfig.BackgroundTypes.SOLID_COLOR:
                    break;
                case BackgroundConfig.BackgroundTypes.CHECKERED:
                    Vector3 start = new Vector3(0, height / 2, 0);
                    Vector3 end = new Vector3(width, height / 2, 0);
                    
                    for (float i = -height; i < height; i++)
                    {
                        DrawLine(start, end, Color.gray);
                        start.y -= spacing;
                        end.y -= spacing;
                    }

                    start = new Vector3(0, height / 2, 0);
                    end = new Vector3(0, -height / 2, 0);
                    
                    for (int i = 0; i < width; i++)
                    {
                        DrawLine(start, end, Color.gray);
                        start.x += spacing;
                        end.x += spacing;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(bgType), bgType, "Invalid background type.");
            }
        }

        protected void FinalizePlot()
        {
            GL.PopMatrix();
            Handles.EndGUI();
        }
    }
}