using SCOI2V2.Classes;
using SCOI2V2.Classes.InterpolationManagers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCOI2V2
{
    public partial class FunctionWindow : System.Windows.Forms.PictureBox
    {
        Label Txt;
        PictureBox PrevueBox;
        PictureBox GistagrammBox;
        GistagrammManager GM;
        PrevueManager PM;
        ComboBox InterMode;
        DrawingManager DM;
        bool MovingFlag;
        int WI;
        bool changingFlag;

        public FunctionWindow(Label Diagnostic, string WayToTarget, PictureBox _PrevueBox,PictureBox _GistagrammBox,ComboBox _InterMode)
        {

            this.SetStyle(
                System.Windows.Forms.ControlStyles.UserPaint |
                System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
                System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer,
                true);
            Paint += p_event;
            MouseDown += Pan_MouseDown;
            MouseUp += Pan_MouseUp;
            MouseMove += Pan_MouseMove;

            Timer y = new Timer();
            y.Interval = 30;
            y.Tick += (s, a) => { this.Refresh(); };
            VisibleChanged += (s, a) => { y.Start(); };

            Txt = Diagnostic;
            PrevueBox = _PrevueBox;
            GistagrammBox = _GistagrammBox;
            
            DM = new DrawingManager();
            GM = new GistagrammManager();
            PM = new PrevueManager(WayToTarget);
       
            //SetChanges(true);
            //GistagrammBox.Image = GM.DrawGistogramm(PM.TakeGist()); 

            InterMode = _InterMode;
            MovingFlag = false;
            //changingFlag = true;
        }

        private void Pan_MouseMove(object sender, MouseEventArgs e)
        {
            Classes.Point tmp = new Classes.Point(e.Location.X, e.Location.Y);
            tmp.Normolize();
            if ((MovingFlag) && (WI != -1) && (WI >= 0))
            {
                DM.Points[WI].X = tmp.X;
                DM.Points[WI].Y = tmp.Y;
                changingFlag = true;
            }
        }

        private void Pan_MouseUp(object sender, MouseEventArgs e)
        {
            MovingFlag = false;
            WI = -1;
            changingFlag = false;
        }

        public int FIndWP(Classes.Point P)
        {
            for (int i = 0; i < DM.Points.Count; i++)
                if ((DM.Points[i].X == P.X) && (DM.Points[i].Y == P.Y))
                    return i;
            return -1;
        }

        private void Pan_MouseDown(object sender, MouseEventArgs e)
        {
            changingFlag = true;
            if (e.Button == MouseButtons.Left)
            {
                Classes.Point tmp = new Classes.Point(e.Location.X, e.Location.Y);
                tmp.Normolize();
                bool flag = true;
                for (int i = 0; (!MovingFlag) && (i < DM.Points.Count); i++)
                {
                    if (DM.Points[i].InPointCoords(tmp))
                    {
                        MovingFlag = true;
                        WI = i;
                        flag = false;
                        
                    }
                }
                if (flag)
                {
                    DM.AddPoint(tmp);
                    MovingFlag = true;
                    WI = FIndWP(tmp);
                    
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                Classes.Point tmp = new Classes.Point(e.Location.X, e.Location.Y);
                tmp.Normolize();
                bool flag = true;
                for (int i = 1; (i < DM.Points.Count) && flag; i++)
                    if (DM.Points[i].InPointCoords(tmp))
                    {
                        DM.RemovePoint(i);
                        WI = -1;
                        flag = false;
                    }
            }

        }
        public InterpolationInterface GetFunc()
        {
            return DM.ActualFunction;
        }

        public void SetChanges(bool flag = false)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            if (flag)
                changingFlag = true;
            Image = DM.DrawNewFrame(InterMode.SelectedIndex);
            if (changingFlag)
            {
                PrevueBox.Image = PM.SetChanges(DM.ActualFunction);
                GistagrammBox.Image = GM.DrawGistogramm(PM.TakeGist());

                changingFlag = false;
            }
            timer.Stop();

            Txt.Text = timer.ElapsedMilliseconds.ToString();
        }
        
        public void p_event(object sender, System.Windows.Forms.PaintEventArgs e)
        {

            SetChanges();
        }
    }
}
