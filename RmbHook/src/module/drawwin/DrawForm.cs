using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using System.Runtime.InteropServices;
//using Microsoft.Xna.Framework.Graphics;
using ls.libs;

namespace WrittingHelper
{
    partial class DrawForm : Form
    {
        public static DrawForm mthis = null;
        /* 
        GraphicsDevice dev = null;        
        BasicEffect effect = null;
        // Wheel vertexes
        VertexPositionColor[] v = new VertexPositionColor[100];
        */
        public DrawFormMan mdfman = null;
        //public IntPtr mhwnd;

        // Wheel rotation
        float rot = 0;
        public DrawForm()
        {
            InitializeComponent();

            mthis = this;

            //mhwnd = this.Handle;
        
        }
        public void init()//Rectangle rect)
        {
            //_drawWow.Bound();

            StartPosition = FormStartPosition.CenterScreen;
            //StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0,0);// rect.Location;
            this.Size = new Size(200, 200);// rect.Width, rect.Height);

            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;  // no borders

            TopMost = true;        // make the form always on top                     
            Visible = true;        // Important! if this isn't set, then the form is not shown at all
            //Visible = false;

            // Set the form click-through
            int initialStyle = WinApis.GetWindowLong(this.Handle, -20);
            WinApis.SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);



            // Extend aero glass style on form init
            OnResize(null);

            this.Visible = false;

        }

        // size & parint,
        Graphics mgrap = null;
        protected override void OnResize(EventArgs e)
        {
            int[] margins = new int[] { 0, 0, Width, Height };

            // Extend aero glass style to whole form
            WinApis.DwmExtendFrameIntoClientArea(this.Handle, ref margins);

            //if (_drawWow != null)
            //{
            //    _drawWow.Bound();
            //}
            mgrap = this.CreateGraphics();
        }
        
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // do nothing here to stop window normal background painting
        }
        /*
        protected override void OnPaint(PaintEventArgs e)
        {
            return;
            //Rectangle rect = new Rectangle(0, 0, 100, 100);
            //e.Graphics.DrawRectangle(Pens.Red, rect);
            //return;

            if (mdfman!=null)
            {
                mdfman.onParint(e.Graphics);
            //    return;
            }

            // Redraw immediatily
            Invalidate(false);            
        }
        */




        private void DrawForm_MouseClick(object sender, MouseEventArgs e)
        {
            //_drawWow.onClick(sender, e);
        }




    }
}
