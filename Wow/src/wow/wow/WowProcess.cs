using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ls.libs;
using WoW.libs;
using PInvoke;

namespace WoW.wow
{
    class WowProcess
    {

        public IntPtr GetHwnd()
        {
            return _wowproc.MainWindowHandle;
        }
        public int Start()
        {
            this._wowproc = this.GetProcess();
            return 0;
        }
        

        // process & windows,
        Process _wowproc=null;
        List<string> _wowprocnames = new List<string> { "WoW", "WowClassic", "Wow-64" };
        Process GetProcess()
        {
            var proclist = Process.GetProcesses();
            foreach(var p in proclist)
            {
                if (_wowprocnames.Contains(p.ProcessName))
                    return p;
            }
            return null;
        }
        

        Random random = new Random();
        int _delaytime = 101;

        // key,
        public void SetKeyState(ConsoleKey key, bool pressDown, bool forceClick, string description)
        {
            Debug.WriteLine("SetKeyState: " + description);
            if (pressDown) { KeyDown(key, description); } else { KeyUp(key, forceClick); }
        }
        public void KeyPressSleep(ConsoleKey key, int milliseconds, string description = "")
        {
            if (milliseconds < 1) { return; }
            var keyDescription = string.Empty;
            if (!string.IsNullOrEmpty(description)) { keyDescription = $"{description} "; }
            Lslog.log($"{keyDescription}[{key}] pressing for {milliseconds}ms");

            NativeMethods.PostMessage(GetHwnd(), NativeMethods.WM_KEYDOWN, (int)key, 0);
            Thread.Sleep(milliseconds);
            NativeMethods.PostMessage(GetHwnd(), NativeMethods.WM_KEYUP, (int)key, 0);
        }

        // mouse,
        public void GetPosScreen(ref Point point)
        {
            WinApis.ClientToScreen(this.GetHwnd(), ref point);
        }
        public void SetCursorPosClientDelay(Point point)
        {
            this.GetPosScreen(ref point);
            WowProcess.SetCursorPosition(point);
            DelaySleep(_delaytime);
        }
        public void LeftClickMouseClient(Point position)
        {
            WinApis.ClientToScreen(this.GetHwnd(), ref position);
            this.LeftClickMouseSleep(position);
        }        
        public void RightClickMouseSleep(Point position)
        {
            SetCursorPosition(position);
            NativeMethods.PostMessage(GetHwnd(), NativeMethods.WM_RBUTTONDOWN, NativeMethods.VK_RMB, 0);
            DelaySleep(_delaytime);
            NativeMethods.PostMessage(GetHwnd(), NativeMethods.WM_RBUTTONUP, NativeMethods.VK_RMB, 0);
            DelaySleep(_delaytime);
        }
        public void LeftClickMouseSleep(Point position)
        {
            SetCursorPosition(position);
            DelaySleep(_delaytime);
            NativeMethods.PostMessage(GetHwnd(), NativeMethods.WM_LBUTTONDOWN, NativeMethods.VK_RMB, 0);
            DelaySleep(_delaytime);
            NativeMethods.PostMessage(GetHwnd(), NativeMethods.WM_LBUTTONUP, NativeMethods.VK_RMB, 0);
            DelaySleep(_delaytime);
        }
        public static void SetCursorPosition(Point position)
        {
            NativeMethods.SetCursorPos(position.X, position.Y);
        }
        public void DelaySleep(int milliseconds)
        {
            Thread.Sleep(milliseconds + random.Next(1, 200));
        }

        // window
        int scale = 100;
        public RECT GetWindowRect()
        {
            var handle = this.GetHwnd();
            RECT rect = new RECT();
            NativeMethods.GetWindowRect(handle, ref rect);

            if (rect.right == 2048)
            {
                scale = 125;

                rect.right = (rect.right * scale) / 100;
                rect.bottom = (rect.bottom * scale) / 100;
            }

            return rect;
        }

        //--- async method, ----------------------------------------------
        public async void KeyPressSleep(ConsoleKey key)
        {
            //this.KeyPressSleep(key, _delaytime, "");

            await this.KeyPress(key, _delaytime, "");
        }
        public async Task KeyPress(ConsoleKey key, int milliseconds, string description = "")
        {
            var keyDescription = string.Empty;
            if (!string.IsNullOrEmpty(description)) { keyDescription = $"{description} "; }
            Lslog.log($"{keyDescription}[{key}] pressing for {milliseconds}ms");

            NativeMethods.PostMessage(GetHwnd(), NativeMethods.WM_KEYDOWN, (int)key, 0);
            await Delay(milliseconds);
            NativeMethods.PostMessage(GetHwnd(), NativeMethods.WM_KEYUP, (int)key, 0);
        }
        public async Task TapStopKey(string description = "")
        {
            Debug.WriteLine("TapStopKey: " + description);
            await KeyPress(ConsoleKey.UpArrow, 51);
        }
        private Dictionary<ConsoleKey, bool> keyDict = new Dictionary<ConsoleKey, bool>();
        private void KeyDown(ConsoleKey key, string description)
        {
            if (keyDict.ContainsKey(key))
            {
                //if (keyDict[key] == true) { return; }
            }
            else
            {
                keyDict.Add(key, true);
            }

            Lslog.log($"KeyDown {key} " + description);
            NativeMethods.PostMessage(GetHwnd(), NativeMethods.WM_KEYDOWN, (int)key, 0);

            keyDict[key] = true;
        }
        private void KeyUp(ConsoleKey key, bool forceClick)
        {
            if (keyDict.ContainsKey(key))
            {
                if (!forceClick)
                {
                    if (keyDict[key] == false) { return; }
                }
            }
            else
            {
                keyDict.Add(key, false);
            }

            Lslog.log($"KeyUp {key}");
            NativeMethods.PostMessage(GetHwnd(), NativeMethods.WM_KEYUP, (int)key, 0);

            keyDict[key] = false;
        }
        



        // mouse;
        public async void RightClickMouseBehindPlayer()
        {
            var rect = GetWindowRect();

            await RightClickMouse(new Point(rect.right / 2, (rect.bottom * 2) / 3));
        }
        public async Task RightClickMouse(System.Drawing.Point position)
        {
            SetCursorPosition(position);
            NativeMethods.PostMessage(GetHwnd(), NativeMethods.WM_RBUTTONDOWN, NativeMethods.VK_RMB, 0);
            await Delay(_delaytime);
            NativeMethods.PostMessage(GetHwnd(), NativeMethods.WM_RBUTTONUP, NativeMethods.VK_RMB, 0);
        }
        public async Task LeftClickMouse(System.Drawing.Point position)
        {
            SetCursorPosition(position);
            await Delay(_delaytime);
            NativeMethods.PostMessage(GetHwnd(), NativeMethods.WM_LBUTTONDOWN, NativeMethods.VK_RMB, 0);
            await Delay(_delaytime);
            NativeMethods.PostMessage(GetHwnd(), NativeMethods.WM_LBUTTONUP, NativeMethods.VK_RMB, 0);
            await Delay(_delaytime);
        }

        
        public async Task Delay(int milliseconds)
        {
            await Task.Delay(milliseconds + random.Next(1, 200));
        }
    }
}
