using System;
using Enums;
using Extensions;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class DrawPrimitives : EditorWindow
    {
        public Rect rect;
        
        private Material _mat;
        private Shader _shader;
        private float _height, _width;

        protected void InitializePlot()
        {
            _width = position.width;
            _height = position.height;
            _shader = Shader.Find("Hidden/Internal-Colored");
            
            _mat = new Material(_shader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };

            _mat.SetPass(0);

            //GUI.BeginClip(rect);
            GL.PushMatrix();

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
            DrawSquare(new Vector3(0, _height, 0), new Vector3(_width, 0, 0), color);
            switch (bgType)
            {
                case BackgroundConfig.BackgroundTypes.SOLID_COLOR:
                    break;
                case BackgroundConfig.BackgroundTypes.CHECKERED:
                    var start = new Vector3(0, 0, 0);
                    var end = new Vector3(_width, 0, 0);
                    
                    for (var i = 0; i < _height; i++)
                    {
                        DrawLine(start, end, Color.gray);
                        start.y += spacing;
                        end.y += spacing;
                    }

                    start = new Vector3(0, 0, 0);
                    end = new Vector3(0, _height, 0);
                    
                    for (var i = 0; i < _width; i++)
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
            //GUI.EndClip();
            Handles.EndGUI();
        }
    }
}