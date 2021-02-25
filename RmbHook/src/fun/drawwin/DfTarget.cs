using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace KeyMouseDo
{
    class DfTarget
    {
        public static DfTarget mthis = null;

        public string mwinname = "World of Warcraft";
        public int mhwnd = 0;
        public IntPtr mhWnd = (IntPtr)null;

        //public string mwinname = "Signal";
        bool mistargetfound = false;
        public Point mposition = new Point(0, 0);
        public WinApis.RECT mrect = new WinApis.RECT() { left = 0, right = 0, top = 0, bottom = 0 };
        public Point mclientcenter = new Point(0, 0);

        public DfTarget()
        {
            mthis = this;
        }

        public bool findTarget()
        {
            IntPtr hwndtarget = (IntPtr)null;
            hwndtarget = WinApis.FindWindow(null, mwinname);

            if (hwndtarget != (IntPtr)null)
            {
                DbMsg.Msg("target win handle=" + hwndtarget.ToString("x"));
                mhWnd = hwndtarget;
                mhwnd = (int)hwndtarget;
                mistargetfound = true;
            }
            else
                return false;

            //_targetpos = new Point();
            mposition.X = 0;mposition.Y = 0;
            bool found = WinApis.ClientToScreen(hwndtarget, ref mposition);
            if (found)
            {
                //WinApis.RECT _rect;
                WinApis.GetClientRect(hwndtarget, out mrect);
                
                mclientcenter.X = (mrect.right - mrect.left) / 2;
                mclientcenter.Y = (mrect.bottom - mrect.top) / 2;

                DbMsg.Msg(mposition.X.ToString() + "," + mposition.Y.ToString());

            }

            return found;
        }

        public void screenToClient(ref Point pt)
        {
            WinApis.ScreenToClient(mhWnd, ref pt);
            //return pt;
        }
        


        void initDx()
        { /*
            // Create device presentation parameters
            PresentationParameters p = new PresentationParameters();
            p.IsFullScreen = false;
            p.DeviceWindowHandle = this.Handle;
            p.BackBufferFormat = SurfaceFormat.Vector4;
            p.PresentationInterval = PresentInterval.One;

            // Create XNA graphics device
            dev = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, GraphicsProfile.Reach, p);

            // Init basic effect
            effect = new BasicEffect(dev);
            */
        }
        void paintDx()
        {
            /*
            // Clear device with fully transparent black
            dev.Clear(new Microsoft.Xna.Framework.Color(0, 0, 0, 0.0f));

            // Rotate wheel a bit
            rot+=0.1f;

            // Make the wheel vertexes and colors for vertexes
            for (int i = 0; i < v.Length; i++)
            {                    
                if (i % 3 == 1)
                    v[i].Position = new Microsoft.Xna.Framework.Vector3((float)Math.Sin((i + rot) * (Math.PI * 2f / (float)v.Length)), (float)Math.Cos((i + rot) * (Math.PI * 2f / (float)v.Length)), 0);
                else if (i % 3 == 2)
                    v[i].Position = new Microsoft.Xna.Framework.Vector3((float)Math.Sin((i + 2 + rot) * (Math.PI * 2f / (float)v.Length)), (float)Math.Cos((i + 2 + rot) * (Math.PI * 2f / (float)v.Length)), 0);

                v[i].Color = new Microsoft.Xna.Framework.Color(1 - (i / (float)v.Length), i / (float)v.Length, 0, i / (float)v.Length);
            }

            // Enable position colored vertex rendering
            effect.VertexColorEnabled = true;
            foreach (EffectPass pass in effect.CurrentTechnique.Passes) pass.Apply();

            // Draw the primitives (the wheel)
            dev.DrawUserPrimitives(PrimitiveType.TriangleList, v, 0, v.Length / 3, VertexPositionColor.VertexDeclaration);

            // Present the device contents into form
            dev.Present();
            */
        }
    }
}
