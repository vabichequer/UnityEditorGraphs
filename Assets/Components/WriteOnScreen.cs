using System.Globalization;
using System.Linq;
using UnityEngine;

namespace Components
{
    public abstract class WriteOnScreen
    {
        public static void DrawWord(Vector2 pos, float size, string word, Color color)
        {
            foreach (var (letter, index) in word.Select((letter, index) => (letter, index)))
            {
                var upperLetter = char.ToUpper(letter);
                
                DrawLetter(new Vector2(pos.x + size * index, pos.y), size, upperLetter, color);
            }
        }

        private static void DrawLetter(Vector2 c, float size, char letter, Color color)
        {
            var horizontalSize = size * 0.6f;
            var verticalSize = size;

            var posX = c.x - horizontalSize / 2;
            var posY = c.y - verticalSize / 2;
            var ll = new Vector2(posX, posY);
            var lr = new Vector2(posX + horizontalSize, posY);
            var ml = new Vector2(posX, posY + verticalSize / 2);
            var mr = new Vector2(posX + horizontalSize, posY + verticalSize / 2);
            var um = new Vector2(posX + horizontalSize / 2, posY + verticalSize);
            var lm = new Vector2(posX + horizontalSize / 2, posY);
            var hl = new Vector2(posX, posY + verticalSize);
            var hr = new Vector2(posX + horizontalSize, posY + verticalSize);

            switch (letter)
            {
                case 'A':
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(ll);
                    GL.Vertex(um);
                    GL.Vertex(lr);
                    GL.Vertex(new Vector2(lr.x - horizontalSize / 5, lr.y + verticalSize / 5));
                    GL.Vertex(new Vector2(ll.x + horizontalSize / 5, ll.y + verticalSize / 5));
                    GL.End();
                    break;
                case 'B':
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(ll);
                    GL.Vertex(hl);
                    GL.Vertex(new Vector2(hr.x, hr.y - verticalSize / 4));
                    GL.Vertex(ml);
                    GL.Vertex(new Vector2(hr.x, lr.y + verticalSize / 4));
                    GL.Vertex(ll);
                    GL.End();
                    break;
                case 'C':
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(hr);
                    GL.Vertex(ml);
                    GL.Vertex(lr);
                    GL.End();
                    break;
                case 'D':
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(ll);
                    GL.Vertex(hl);
                    GL.Vertex(mr);
                    GL.Vertex(ll);
                    GL.End();
                    break;
                case 'E':
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(hr);
                    GL.Vertex(hl);
                    GL.Vertex(ml);
                    GL.Vertex(mr);
                    GL.Vertex(ml);
                    GL.Vertex(ll);
                    GL.Vertex(lr);
                    GL.End();
                    break;
                case 'F':
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(ll);
                    GL.Vertex(hl);
                    GL.Vertex(hr);
                    GL.Vertex(hl);
                    GL.Vertex(ml);
                    GL.Vertex(mr);
                    GL.Vertex(ml);
                    GL.Vertex(ml);
                    GL.Vertex(ll);
                    GL.End();
                    break;
                case 'G':
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(new Vector3(um.x + horizontalSize / 3, um.y));
                    GL.Vertex(um);
                    GL.Vertex(ml);
                    GL.Vertex(lm);
                    GL.Vertex(new Vector3(lr.x, lr.y + verticalSize/4));
                    GL.Vertex(mr);
                    GL.Vertex(new Vector3(lm.x, lm.y  + verticalSize / 2));
                    GL.End();
                    break;
                case 'H':
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(hr);
                    GL.Vertex(lr);
                    GL.Vertex(mr);
                    GL.Vertex(ml);
                    GL.Vertex(hl);
                    GL.Vertex(ll);
                    GL.End();
                    break;
                case 'I':
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(um);
                    GL.Vertex(lm);
                    GL.End();
                    break;
                case 'J':
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(hr);
                    GL.Vertex(new Vector3(lr.x, lr.y + verticalSize / 4));
                    GL.Vertex(lm);
                    GL.Vertex(new Vector3(ll.x, ll.y + verticalSize / 4));
                    GL.End();
                    break;
                case 'K':
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(ll);
                    GL.Vertex(hl);
                    GL.Vertex(ml);
                    GL.Vertex(hr);
                    GL.Vertex(ml);
                    GL.Vertex(lr);
                    GL.End();
                    break;
                case 'L':
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(hl);
                    GL.Vertex(ll);
                    GL.Vertex(lr);
                    GL.End();
                    break;
                case 'M':
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(ll);
                    GL.Vertex(hl);
                    GL.Vertex(new Vector3(um.x, um.y - verticalSize / 2));
                    GL.Vertex(hr);
                    GL.Vertex(lr);
                    GL.End();
                    break;
                case 'N':
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(ll);
                    GL.Vertex(hl);
                    GL.Vertex(lr);
                    GL.Vertex(hr);
                    GL.End();
                    break;
                case 'O':
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(lm);
                    GL.Vertex(ml);
                    GL.Vertex(um);
                    GL.Vertex(mr);
                    GL.Vertex(lm);
                    GL.End();
                    break;
                case 'P':
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(ll);
                    GL.Vertex(hl);
                    GL.Vertex(new Vector3(hr.x, hr.y - verticalSize / 3));
                    GL.Vertex(new Vector3(ll.x, ll.y + verticalSize / 3));
                    GL.Vertex(ll);
                    GL.End();
                    break;
                case 'Q':
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(lm);
                    GL.Vertex(ml);
                    GL.Vertex(um);
                    GL.Vertex(mr);
                    GL.Vertex((lm + mr) / 2);
                    GL.Vertex(lr);
                    GL.Vertex((lm + mr) / 2);
                    GL.Vertex(lm);
                    GL.End();
                    break;
                case 'R':
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(ll);
                    GL.Vertex(hl);
                    var a = new Vector3(hr.x, hr.y - verticalSize / 3);
                    GL.Vertex(a);
                    var b = new Vector3(ll.x, ll.y + verticalSize / 3);
                    GL.Vertex((a + b) / 2);
                    GL.Vertex(lr);
                    GL.Vertex((a + b) / 2);
                    GL.Vertex(b);
                    GL.Vertex(ll);
                    GL.End();
                    break;
                case 'S':
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(new Vector3(hr.x, hr.y - verticalSize / 4 ));
                    GL.Vertex(um);
                    GL.Vertex(new Vector3(hl.x, hl.y - verticalSize / 3));
                    GL.Vertex(new Vector3(lr.x, lr.y + verticalSize / 3));
                    GL.Vertex(lm);
                    GL.Vertex(new Vector3(ll.x, ll.y + verticalSize / 4));
                    GL.End();
                    break;
                case 'T':
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(hl);
                    GL.Vertex(hr);
                    GL.Vertex(um);
                    GL.Vertex(lm);
                    GL.End();
                    break;
                case 'U':
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(hl);
                    GL.Vertex(ll);
                    GL.Vertex(lr);
                    GL.Vertex(hr);
                    GL.End();
                    break;
                case 'V':
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(hl);
                    GL.Vertex(lm);
                    GL.Vertex(hr);
                    GL.End();
                    break;
                case 'X':
                    GL.Begin(GL.LINES);
                    GL.Color(color);
                    GL.Vertex(hl);
                    GL.Vertex(lr);
                    GL.Vertex(hr);
                    GL.Vertex(ll);
                    GL.End();
                    break;
                case 'Z':
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(hl);
                    GL.Vertex(hr);
                    GL.Vertex(ll);
                    GL.Vertex(lr);
                    GL.End();
                    break;
                case 'Y':
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(hl);
                    GL.Vertex(new Vector3(um.x, um.y - verticalSize / 3));
                    GL.Vertex(hr);
                    GL.Vertex(new Vector3(um.x, um.y - verticalSize / 3));
                    GL.Vertex(lm);
                    GL.End();
                    break;
                case 'W':
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex(hl);
                    GL.Vertex(new Vector3(ml.x - horizontalSize / 5, ml.y));
                    GL.Vertex(new Vector3(ml.x, ml.y + verticalSize / 2));
                    GL.Vertex(new Vector3(ml.x + horizontalSize / 5, ml.y));
                    GL.Vertex(hr);
                    GL.End();
                    break;
            }
        }
        
        public static void DrawNumber(Vector2 pos, float size, float number, Color color)
        {
            var str = number.ToString(CultureInfo.InvariantCulture);
            
            foreach (var it in str.Select((digit, index) => new {digit, index}))
            {
                var num = it.digit switch
                {
                    '.' => 10,
                    '-' => 11,
                    _ => it.digit - '0'
                };

                DrawDigit(new Vector2(pos.x + size * it.index, pos.y), size, num, color);
            }
        }
        
        private static void DrawDigit(Vector2 c, float size, int number, Color color)
        {
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
    }
}