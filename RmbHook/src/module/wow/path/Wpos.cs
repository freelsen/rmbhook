using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrittingHelper.wow
{
    class Wpos
    {

        public Wpos()
        {
            InitMap();
        }

        // what data structure should I use for locations?
        static int _mapxsize=1000;
        static int _mapysize=1000;
        int[,] _map = new int[_mapxsize, _mapysize];    // type of locations;
        //ArrayList<Point> _locations=new ar
        List<Point> _locations = new List<Point>();

        // the x,y from Addon is 1k times,
        // the range of wow coordinate is (100,100), 
        // but the coordinate is a float value, e.g., 35.123,
        // how accurate should I use?
        // for the narrow road, 1.0 resolution is not enough, 
        // at least x10;
        public void AddPos(int x, int y)
        {
            int c = this.GetPosC(x);
            int r = this.GetPosR(y);
            if (_map[r, c] < 1)
            {
                _map[r, c] = 1;
                Lslog.log($"pos=({x.ToString()},{y.ToString()})");
                _locations.Add(new Point(x, y));
            } 
        }
        public void LoadPos()
        {

        }
        public void SavePos()
        {
            //FileStream fs = new FileStream("path.txt", FileMode.Append)
            DateTime dt = DateTime.Now;
            string str = dt.ToString("yyyy-MM-dd_");
            string str1 = dt.ToString("hh_mm_ss");

            TextMan text = new TextMan($"path_{str}{str1}.txt");
            text.Open();
            foreach (var point in _locations )
            {
                text.WriteLine($"{point.X.ToString()}\t{point.Y.ToString()}");
            }
            text.Close();
        }


        float _mapscalex = 100;
        float _mapscaley = 100;
        public int GetPosC(int d)
        {
            float f = d / _mapscalex;
            //int idx = (int)f;
            return (int)f;
        }
        public int GetPosR(int d)
        {
            float f = d / _mapscaley;
            //int idx = (int)f;
            return (int)f;
        }



        void InitMap()
        {
            for (int r=0;r<_mapysize; r++)
            {
                for (int c=0;c<_mapxsize;c++)
                {
                    this._map[r, c] = 0;
                }
            }
        }
    }
}
