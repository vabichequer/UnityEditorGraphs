using System;
using System.Collections.Generic;
using Enums;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class DrawPrimitives : EditorWindow
    {
        public Rect rect;
        
        private Material _mat;
        private Shader _shader;
        public float height, width;

        protected void InitializePlot(float usedWidth = 0, float usedHeight = 0)
        {
            width = position.width - usedWidth;
            height = position.height;
            _shader = Shader.Find("Hidden/Internal-Colored");
            
            _mat = new Material(_shader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };

            _mat.SetPass(0);
            GL.PushMatrix();

        }

        public void DrawLineArray(List<float> points, Color color)
        {
            for(var i = 1; i < points.Count; i++)
            {
                var start = new Vector3(i - 1, points[i - 1], 0);
                var end = new Vector3(i, points[i], 0);
                
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
            var highLeft = new Vector3(lowLeft.x, highRight.y, 0);
            var lowRight = new Vector3(highRight.x, lowLeft.y, 0);

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
            var highLeft = new Vector3(lowLeft.x, highRight.y, 0);
            var lowRight = new Vector3(highRight.x, lowLeft.y, 0);

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
            DrawSquare(new Vector3(0, height, 0), new Vector3(width, 0, 0), color);
            switch (bgType)
            {
                case BackgroundConfig.BackgroundTypes.SOLID_COLOR:
                    break;
                case BackgroundConfig.BackgroundTypes.CHECKERED:
                    var start = new Vector3(0, 0, 0);
                    var end = new Vector3(width, 0, 0);
                    
                    for (var i = 0; i < height; i++)
                    {
                        DrawLine(start, end, Color.gray);
                        start.y += spacing;
                        end.y += spacing;
                    }

                    start = new Vector3(0, 0, 0);
                    end = new Vector3(0, height, 0);
                    
                    for (var i = 0; i < width; i++)
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