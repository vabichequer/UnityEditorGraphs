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
        
        private Material _mat;
        private Shader _shader;
        private float _xMargin, _yMargin, _yMax, _yMin, _xIncrements, _yIncrements;
        private const int TopOffset = 21;
        
        public float height, width;
        
        // Legend parameters
        private const int ItemSize = 15;
        private const int ItemMargin = 5;
        private const int BoxMargin = 25;
        private const int BoxSize = 100;

        protected void InitialPlotState()
        {
            _yMax = float.MinValue;
            _yMin = float.MaxValue;
        }

        protected void InitializePlot(Vector2 offset = default, float usedHeight = 0)
        {
            width = position.width - offset.x;
            height = usedHeight;

            if (_yMax > float.MinValue)
            {
                _yIncrements = (height - _yMargin * 2) / (_yMax * 2);
            }
            else
            {
                _yIncrements = (height - _yMargin * 2) / 12;
            }

            _xMargin = 0.05f * width;
            _yMargin = 0.05f * height;
            
            _shader = Shader.Find("Hidden/Internal-Colored");
            
            _mat = new Material(_shader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            
            _mat.SetPass(0);
            GL.PushMatrix();

            var proj = Matrix4x4.Ortho(-offset.x, width, -height / 2, height / 2 - TopOffset, -1, 1);
            GL.LoadProjectionMatrix(proj);
        }
        
        private Vector2 UnscaledCoords(Vector2 c)
        {
            if (c.y > height / 2 - TopOffset)
            {
                return new Vector2(c.x, height/2 - 2 * TopOffset);
            }

            return new Vector2(c.x, c.y - TopOffset);
        }
        
        private Vector2 ScaledCoords(Vector2 c)
        {
            return new Vector2(c.x, c.y * _yIncrements);
        }
        
        public void DrawAxes()
        {
            // x
            DrawLine(new Vector3(0, 0), new Vector3(width, 0), Color.red);
            
            // y
            DrawLine(new Vector3(_xMargin, height / 2), new Vector3(_xMargin, -height / 2), Color.green);

            var acc = 0;
            for (var y = _yIncrements; y < height / 2 - _yMargin - TopOffset; y += _yIncrements)
            {
                WriteOnScreen.DrawNumber(UnscaledCoords(new Vector2(_xMargin - 15, y)), 10, acc, Color.white);
                WriteOnScreen.DrawNumber(UnscaledCoords(new Vector2(_xMargin - 15, -y)), 10, -acc, Color.white);
                acc++;
            }
        }

        public void DrawLegend(List<string> itemNames, List<Color> itemColors)
        {
            const int itemTotalHeight = ItemSize + ItemMargin * 2;

            // Box
            var boxXl = width - (BoxSize + BoxMargin);
            var boxXh = width - BoxMargin;
            var boxYl = height / 2 - itemTotalHeight * itemNames.Count - BoxMargin + ItemMargin * 2;
            var boxYh = height / 2 - BoxMargin;

            // Item
            var itemXl = width - (BoxMargin + BoxSize) + ItemMargin;
            var itemXh = itemXl + ItemSize;
            
            var ll = UnscaledCoords(new Vector2(boxXl, boxYl));
            var hr = UnscaledCoords(new Vector2(boxXh, boxYh));
            DrawHollowSquare(ll, hr, Color.white);

            for (var i = 0; i < itemNames.Count; i++)
            {
                var itemYl = boxYh - (ItemMargin + ItemSize + (ItemMargin + ItemSize) * i);
                var itemYh = boxYh - (ItemMargin + (ItemMargin + ItemSize) * i);
                
                DrawSquare(
                    lowLeft:UnscaledCoords(new Vector2(itemXl, itemYl)),
                    highRight: UnscaledCoords(new Vector2(itemXh, itemYh)),
                    color: itemColors[i]
                    );
                
                WriteOnScreen.DrawWord(
                    UnscaledCoords(new Vector2(itemXh + ItemMargin * 2, (itemYh + itemYl) / 2 - ItemSize - ItemMargin)), 
                    10, 
                    itemNames[i], 
                    Color.white);
            }
        }
        
        public void DrawLineArray(List<float> points, Color color)
        {
            var tempMin = points.Min();
            if (_yMin > tempMin)
            {
                _yMin = tempMin;
            }

            var tempMax = points.Max();
            if (_yMax < tempMax)
            {
                _yMax = tempMax;
            }
            
            for(var i = 1; i < points.Count; i++)
            {
                var start = ScaledCoords(new Vector3( (i - 1),   points[i - 1], 0));
                var end = ScaledCoords(new Vector3(i, points[i], 0));
                
                DrawLine(start, end,  color);
            }
        }
        
        public void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            start = UnscaledCoords(start);
            end = UnscaledCoords(end);
            
            GL.Begin(GL.LINES);
            GL.Color(color);
            GL.Vertex(start);
            GL.Vertex(end);
            GL.End();
        }

        public void DrawSquare(Vector3 lowLeft, Vector3 highRight, Color color)
        {
            lowLeft = UnscaledCoords(lowLeft);
            highRight = UnscaledCoords(highRight);
            
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
            lowLeft = UnscaledCoords(lowLeft);
            highRight = UnscaledCoords(highRight);
            
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