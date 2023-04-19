using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Enums;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Editor
{
    public class DrawPrimitives : EditorWindow
    {
        public Rect rect;
        
        private Material _mat;
        private Shader _shader;
        private float _xMargin, _yMargin, _max, _min, _xIncrements, _yIncrements;
        private int _divisions;
        
        public float height, width;

        protected void InitialPlotState()
        {
            _max = float.MinValue;
            _min = float.MaxValue;
            _divisions = 12;
        }

        protected void InitializePlot(float usedWidth = 0, float usedHeight = 0)
        {
            width = position.width - usedWidth;
            height = position.height;
            _xIncrements = (width - _xMargin * 2) / _divisions;
            _yIncrements = (height - _yMargin * 2) / _divisions;
            
            _xMargin = 0.05f * width;
            _yMargin = 0.05f * height;
            
            _shader = Shader.Find("Hidden/Internal-Colored");
            
            _mat = new Material(_shader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };

            _mat.SetPass(0);
            GL.PushMatrix();

        }

        public void DrawAxes()
        {
            // x
            DrawLine(new Vector3(0, height / 2), new Vector3(width, height / 2), Color.red);
            
            // y
            DrawLine(new Vector3(_xMargin, 0), new Vector3(_xMargin, height), Color.green);

            var acc = 0;
            
            for (var x = _xIncrements; x < width - _xMargin; x +=_xIncrements)
            {
                Debugging.Print("x:", x);
                DrawNumber(x, height/2 + 10, 10, acc, Color.white);
                acc++;
            }
            
            for (var y = _yIncrements; y < height - _yMargin; y += _yIncrements)
            {
                Debugging.Print("y:", y);
                DrawNumber(_xMargin - 15, y, 10, acc, Color.white);
                acc++;
            }
        }
        
        public void DrawNumber(float x, float y, float size, float number, Color color)
        {
            var str = number.ToString(CultureInfo.InvariantCulture);
            
            foreach (var it in str.Select((digit, index) => new {digit, index}))
            {
                Debugging.Print("Draw number:", it.digit);
                var num = 0;
                switch (it.digit)
                {
                    case '.':
                        num = 10;
                        break;
                    case '-':
                        num = 11;
                        break;
                    default:
                        num = it.digit - '0';
                        break;
                }
                
                DrawDigit(x + size * it.index, y, size, num, color);
            }
        }
        
        private void DrawDigit(float x, float y, float size, int number, Color color) 
        {
            var horizontalSize = size * 0.6f;
            var verticalSize = size;

            var posX = x - horizontalSize * 0.5f;
            var posY = y - verticalSize * 0.5f;

            switch (number)
            {
                case 0:
                    Debugging.Print("pingou no 0");
                    GL.Begin(GL.LINES);
                    GL.Color(color);
                    GL.Vertex(new Vector3(posX, posY));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize));
                    GL.Vertex(new Vector3(posX, posY + verticalSize));
                    GL.Vertex(new Vector3(posX, posY + verticalSize));
                    GL.Vertex(new Vector3(posX, posY));
                    GL.End();
                    break;
                case 1:
                    Debugging.Print("pingou no 1");
                    GL.Begin(GL.LINES);
                    GL.Color(color);
                    GL.Vertex(new Vector3(posX + horizontalSize * 0.5f, posY));
                    GL.Vertex(new Vector3(posX + horizontalSize * 0.5f, posY + verticalSize));
                    GL.End();
                    break;
                case 2:
                    GL.Begin(GL.LINES);
                    GL.Color(color);
                    GL.Vertex(new Vector3(posX, posY));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX, posY + verticalSize));
                    GL.Vertex(new Vector3(posX, posY + verticalSize));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize));
                    GL.End();
                    break;
                case 3:
                    GL.Begin(GL.LINES);
                    GL.Color(color);
                    GL.Vertex(new Vector3(posX, posY));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize));
                    GL.Vertex(new Vector3(posX, posY + verticalSize));
                    GL.End();
                    break;
                case 4:
                    GL.Begin(GL.LINES);
                    GL.Color(color);
                    GL.Vertex(new Vector3(posX, posY));
                    GL.Vertex(new Vector3(posX, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY));
                    GL.End();
                    break;
                case 5:
                    GL.Begin(GL.LINES);
                    GL.Color(color);
                    GL.Vertex(new Vector3(posX + horizontalSize, posY));
                    GL.Vertex(new Vector3(posX, posY));
                    GL.Vertex(new Vector3(posX, posY));
                    GL.Vertex(new Vector3(posX, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize));
                    GL.Vertex(new Vector3(posX, posY + verticalSize));
                    GL.End();
                    break;
                case 6:
                    GL.Begin(GL.LINES);
                    GL.Color(color);
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX, posY + verticalSize));
                    GL.Vertex(new Vector3(posX, posY + verticalSize));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY));
                    GL.Vertex(new Vector3(posX, posY));
                    GL.Vertex(new Vector3(posX, posY));
                    GL.Vertex(new Vector3(posX, posY + verticalSize * 0.5f));
                    GL.End();
                    break;
                case 7:
                    GL.Begin(GL.LINES);
                    GL.Color(color);
                    GL.Vertex(new Vector3(posX, posY));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY));
                    GL.Vertex(new Vector3(posX + horizontalSize * 0.5f, posY + verticalSize));
                    GL.End();
                    break;
                case 8:
                    GL.Begin(GL.LINES);
                    GL.Color(color);
                    GL.Vertex(new Vector3(posX, posY));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize));
                    GL.Vertex(new Vector3(posX, posY + verticalSize));
                    GL.Vertex(new Vector3(posX, posY + verticalSize));
                    GL.Vertex(new Vector3(posX, posY));
                    GL.Vertex(new Vector3(posX, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize * 0.5f));
                    GL.End();
                    break;
                case 9:
                    GL.Begin(GL.LINES);
                    GL.Color(color);
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize));
                    GL.Vertex(new Vector3(posX, posY + verticalSize));
                    GL.Vertex(new Vector3(posX, posY));
                    GL.Vertex(new Vector3(posX, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX, posY));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX, posY + verticalSize * 0.5f));
                    GL.End();
                    break;
                case 10: // draw a point
                    GL.Begin(GL.LINES);
                    GL.Color(color);
                    GL.Vertex(new Vector3(posX, posY));
                    GL.Vertex(new Vector3(posX, posY));
                    GL.End();
                    break;
                case 11: // draw a hyphen
                    GL.Begin(GL.LINES);
                    GL.Color(color);
                    GL.Vertex(new Vector3(posX, posY + verticalSize * 0.5f));
                    GL.Vertex(new Vector3(posX + horizontalSize, posY + verticalSize * 0.5f));
                    GL.End();
                    break;
            }
        }


        public void DrawLineArray(List<float> points, Color color)
        {
            var tempMin = points.Min();
            if (_min > tempMin)
            {
                _min = tempMin;
            }

            var tempMax = points.Max();
            if (_max < tempMax)
            {
                _max = tempMax;
            }
            
            for(var i = 1; i < points.Count; i++)
            {
                var start = new Vector3( (i - 1), height / 2 - points[i - 1], 0);
                var end = new Vector3(i, height / 2 - points[i], 0);
                
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