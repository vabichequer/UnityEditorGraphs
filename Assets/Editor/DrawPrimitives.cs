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
        private Vector2 _offset;
        
        public float height, width;

        protected void InitialPlotState()
        {
            _max = float.MinValue;
            _min = float.MaxValue;
            _divisions = 12;
        }

        protected void InitializePlot(Vector2 offset = default, float usedHeight = 0)
        {
            width = position.width;
            height = usedHeight;
            _xIncrements = (width - _xMargin * 2) / _divisions;
            _yIncrements = (height - _yMargin * 2) / _divisions;

            _offset = offset;
            
            _xMargin = 0.05f * width;
            _yMargin = 0.05f * height;
            
            _shader = Shader.Find("Hidden/Internal-Colored");
            
            _mat = new Material(_shader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            
            _mat.SetPass(0);
            GL.PushMatrix();

            var proj = Matrix4x4.Ortho(0, width, -height / 2, height / 2, -1, 1);
            GL.LoadProjectionMatrix(proj);
        }

        private Vector2 Coord(Vector2 c)
        {
            return new Vector2(c.x + _offset.x, c.y + _offset.y - 21);
        }
        
        public void DrawAxes()
        {
            // x
            DrawLine(new Vector3(0, 0), new Vector3(width, 0), Color.red);
            
            // y
            DrawLine(new Vector3(_xMargin, height / 2), new Vector3(_xMargin, -height / 2), Color.green);

            var acc = 0;
            
            for (var x = _xIncrements; x < width - _xMargin; x +=_xIncrements)
            {
                DrawNumber(x, 10, 10, acc, Color.white);
                acc++;
            }
            
            for (var y = _yIncrements; y < height / 2 - _yMargin; y += _yIncrements)
            {
                DrawNumber(_xMargin - 15, y, 10, acc, Color.white);
                acc++;
            }
        }
        
        public void DrawNumber(float x, float y, float size, float number, Color color)
        {
            var str = number.ToString(CultureInfo.InvariantCulture);
            
            foreach (var it in str.Select((digit, index) => new {digit, index}))
            {
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
            var c = Coord(new Vector2(x, y));
            
            var horizontalSize = size * 0.6f;
            var verticalSize = size;

            var posX = c.x - horizontalSize / 2;
            var posY = c.y - verticalSize / 2;
            var ll = new Vector2(posX, posY);
            var lr = new Vector2(posX + horizontalSize, posY);
            var ml = new Vector2(posX, posY + verticalSize / 2);
            var mr = new Vector2(posX + horizontalSize, posY + verticalSize / 2);
            var hl = new Vector2(posX, posY + verticalSize);
            var hr = new Vector2(posX + horizontalSize, posY + verticalSize);
            

            switch (number)
            {
                case 0:
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(ll);
                    GL.Vertex(lr);
                    GL.Vertex(hr);
                    GL.Vertex(hl);
                    GL.Vertex(ll);
                    GL.End();
                    break;
                case 1:
                    GL.Begin(GL.LINES);
                    GL.Color(color);
                    GL.Vertex(new Vector3(posX + horizontalSize / 2, posY));
                    GL.Vertex(new Vector3(posX + horizontalSize / 2, posY + verticalSize));
                    GL.End();
                    break;
                case 2:
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(hl);
                    GL.Vertex(hr);
                    GL.Vertex(mr);
                    GL.Vertex(ml);
                    GL.Vertex(ll);
                    GL.Vertex(lr);
                    GL.End();
                    break;
                case 3:
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(hl);
                    GL.Vertex(hr);
                    GL.Vertex(mr);
                    GL.Vertex(ml);
                    GL.Vertex(mr);
                    GL.Vertex(lr);
                    GL.Vertex(ll);
                    GL.End();
                    break;
                case 4:
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(lr);
                    GL.Vertex(hr);
                    GL.Vertex(mr);
                    GL.Vertex(ml);
                    GL.Vertex(hl);
                    GL.End();
                    break;
                case 5:
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(hr);
                    GL.Vertex(hl);
                    GL.Vertex(ml);
                    GL.Vertex(mr);
                    GL.Vertex(lr);
                    GL.Vertex(ll);
                    GL.End();
                    break;
                case 6:
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(hr);
                    GL.Vertex(hl);
                    GL.Vertex(ll);
                    GL.Vertex(lr);
                    GL.Vertex(mr);
                    GL.Vertex(ml);
                    GL.End();
                    break;
                case 7:
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(hl);
                    GL.Vertex(hr);
                    GL.Vertex(new Vector3(posX + horizontalSize * 0.5f, posY));
                    GL.End();
                    break;
                case 8:
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(mr);
                    GL.Vertex(ml);
                    GL.Vertex(hl);
                    GL.Vertex(hr);
                    GL.Vertex(lr);
                    GL.Vertex(ll);
                    GL.Vertex(ml);
                    GL.End();
                    break;
                case 9:
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(ll);
                    GL.Vertex(lr);
                    GL.Vertex(hr);
                    GL.Vertex(hl);
                    GL.Vertex(ml);
                    GL.Vertex(mr);
                    GL.End();
                    break;
                case 10: // draw a point
                    GL.Begin(GL.LINES);
                    GL.Color(color);
                    GL.Vertex(ll);
                    GL.Vertex(ll);
                    GL.End();
                    break;
                case 11: // draw a hyphen
                    GL.Begin(GL.LINES);
                    GL.Color(color);
                    GL.Vertex(mr);
                    GL.Vertex(ml);
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
            start = Coord(start);
            end = Coord(end);
            
            GL.Begin(GL.LINES);
            GL.Color(color);
            GL.Vertex(start);
            GL.Vertex(end);
            GL.End();
        }

        public void DrawSquare(Vector3 lowLeft, Vector3 highRight, Color color)
        {
            lowLeft = Coord(lowLeft);
            highRight = Coord(highRight);
            
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
            lowLeft = Coord(lowLeft);
            highRight = Coord(highRight);
            
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
            DrawSquare(new Vector3(0, -height / 2, 0), new Vector3(width, height / 2, 0), color);
            switch (bgType)
            {
                case BackgroundConfig.BackgroundTypes.SOLID_COLOR:
                    break;
                case BackgroundConfig.BackgroundTypes.CHECKERED:
                    var start = new Vector3(0, height / 2, 0);
                    var end = new Vector3(width, height / 2, 0);
                    
                    for (var i = -height; i < height; i++)
                    {
                        DrawLine(start, end, Color.gray);
                        start.y -= spacing;
                        end.y -= spacing;
                    }

                    start = new Vector3(0, height / 2, 0);
                    end = new Vector3(0, -height / 2, 0);
                    
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