using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks; // need .net 4.0;
using ls.libs;

namespace WoW.wow
{
    class WowWorker
    {
        public static WowWorker mthis = null;
        Random random = new Random();

        public WowWorker()
        {
            mthis = this;

            this.Enable = false;
        }

        public static void doWork(object sender, EventArgs e)
        {

        }
        public static void doneWork(object sender, EventArgs e)
        {

        }
        public static void reportWork(object sender, EventArgs e)
        {

        }

        public bool Enable { get; set; }
        public Process TargetProcess { get; set; }
        //private ILogger logger;

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

            //logger.LogInformation($"KeyDown {key} " + description);
            NativeMethods.PostMessage(TargetProcess.MainWindowHandle, NativeMethods.WM_KEYDOWN, (int)key, 0);

            keyDict[key] = true;
        }

        private Dictionary<ConsoleKey, bool> keyDict = new Dictionary<ConsoleKey, bool>();

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

            //logger.LogInformation($"KeyUp {key}");
            NativeMethods.PostMessage(TargetProcess.MainWindowHandle, NativeMethods.WM_KEYUP, (int)key, 0);

            keyDict[key] = false;
        }

        public async void RightClickMouseBehindPlayer()
        {
            //var rect = GetWindowRect();

            //await RightClickMouse(new Point(rect.right / 2, (rect.bottom * 2) / 3));
        }

        public async Task KeyPress(ConsoleKey key, int milliseconds, string description = "")
        {
            var keyDescription = string.Empty;
            if (!string.IsNullOrEmpty(description)) { keyDescription = $"{description} "; }
            //logger.LogInformation($"{keyDescription}[{key}] pressing for {milliseconds}ms");

            NativeMethods.PostMessage(TargetProcess.MainWindowHandle, NativeMethods.WM_KEYDOWN, (int)key, 0);
            await Delay(milliseconds);
            NativeMethods.PostMessage(TargetProcess.MainWindowHandle, NativeMethods.WM_KEYUP, (int)key, 0);
        }

        public void KeyPressSleep(ConsoleKey key, int milliseconds, string description = "")
        {
            if (milliseconds < 1) { return; }
            var keyDescription = string.Empty;
            if (!string.IsNullOrEmpty(description)) { keyDescription = $"{description} "; }
            //logger.LogInformation($"{keyDescription}[{key}] pressing for {milliseconds}ms");

            NativeMethods.PostMessage(TargetProcess.MainWindowHandle, NativeMethods.WM_KEYDOWN, (int)key, 0);
            Thread.Sleep(milliseconds);
            NativeMethods.PostMessage(TargetProcess.MainWindowHandle, NativeMethods.WM_KEYUP, (int)key, 0);
        }

        public async Task TapStopKey(string description = "")
        {
            Debug.WriteLine("TapStopKey: " + description);
            await KeyPress(ConsoleKey.UpArrow, 51);
        }

        public void SetKeyState(ConsoleKey key, bool pressDown, bool forceClick, string description)
        {
            Debug.WriteLine("SetKeyState: " + description);
            if (pressDown) { KeyDown(key, description); } else { KeyUp(key, forceClick); }
        }

        public static void SetCursorPosition(System.Drawing.Point position)
        {
            NativeMethods.SetCursorPos(position.X, position.Y);
        }

        public async Task RightClickMouse(System.Drawing.Point position)
        {
            SetCursorPosition(position);
            NativeMethods.PostMessage(TargetProcess.MainWindowHandle, NativeMethods.WM_RBUTTONDOWN, NativeMethods.VK_RMB, 0);
            await Delay(101);
            NativeMethods.PostMessage(TargetProcess.MainWindowHandle, NativeMethods.WM_RBUTTONUP, NativeMethods.VK_RMB, 0);
        }

        public async Task LeftClickMouse(System.Drawing.Point position)
        {
            SetCursorPosition(position);
            await Delay(101);
            NativeMethods.PostMessage(TargetProcess.MainWindowHandle, NativeMethods.WM_LBUTTONDOWN, NativeMethods.VK_RMB, 0);
            await Delay(101);
            NativeMethods.PostMessage(TargetProcess.MainWindowHandle, NativeMethods.WM_LBUTTONUP, NativeMethods.VK_RMB, 0);
            await Delay(101);
        }

        public async Task Delay(int milliseconds)
        {
            await Task.Delay(milliseconds + random.Next(1, 200));
        }
        //public RECT GetWindowRect()
        //{
        //    var handle = this.WarcraftProcess.MainWindowHandle;
        //    RECT rect = new RECT();
        //    NativeMethods.GetWindowRect(handle, ref rect);

        //    if (rect.right == 2048)
        //    {
        //        scale = 125;

        //        rect.right = (rect.right * scale) / 100;
        //        rect.bottom = (rect.bottom * scale) / 100;
        //    }

        //    return rect;
        //}

    }
}
