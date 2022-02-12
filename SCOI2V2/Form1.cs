using SCOI2V2.Classes.PicturesAndFrames;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCOI2V2
{
    public partial class Form1 : Form
    {
        FunctionWindow Canvas;
        String WayToTarget;
        public Form1()
        {
            InitializeComponent();  
            comboBox1.Items.Add("линейная"); 
            comboBox1.Items.Add("кубическим сплайном");
            comboBox1.SelectedIndex = 0;
            comboBox1.Visible = false;
            button1.Visible = false;
            label1.Visible = false;
            label2.Text = "Перетащите изображение на форму с помощью мышки(Drag And Drop)";
            MinimumSize = Size;
            MaximumSize = Size;
        }





        private void Form1_DragEnter_1(object sender, DragEventArgs e)
        {
            try
            {
                
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
            }
            catch
            {
                MessageBox.Show("Ошибка");

            }
        }

        private void Form1_DragDrop_1(object sender, DragEventArgs e)
        {
            try
            {
                label2.Text = "Чтобы изменить обрабатываемое изображение перетащите новое" +
                    " изображение на форму с помощью мышки(Drag And Drop)";
                if (Canvas != null)
                    Canvas.Dispose();
                string[] objects = (string[])e.Data.GetData(DataFormats.FileDrop);
                WayToTarget = objects[0];
                using (Bitmap tmp = new Bitmap(objects[0]))
                {
                    
                    Prevue.Image = new Bitmap(tmp, Prevue.Width, Prevue.Height);
                    Canvas = new FunctionWindow(label1, objects[0],Prevue,GistBox,comboBox1);
                    Canvas.Size = Prevue.Size;
                    Canvas.Location = new Point(0, Prevue.Location.Y);
                    Controls.Add(Canvas);
                    Canvas.SetChanges(true);
                    Canvas.SetChanges(true);
                }
                comboBox1.Visible = true;
                button1.Visible = true;
                label1.Visible = true;
            }
            catch
            {
                MessageBox.Show("Ошибка");

            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            
                TargetPicture Result = new TargetPicture(WayToTarget);
                Result.SetChanges(Canvas.GetFunc());
                Bitmap Image = Result.TakePicture();
                SaveFileDialog saveFileFialog = new SaveFileDialog();
                saveFileFialog.InitialDirectory = Directory.GetCurrentDirectory();
                saveFileFialog.Filter = "Картинки (png, jpg, bmp, gif) |*.png;*.jpg;*.bmp;*.gif|All files (*.*)|*.*";
                saveFileFialog.RestoreDirectory = true;

                if (saveFileFialog.ShowDialog() == DialogResult.OK)
                {
                    if (Image != null)
                    {
                        Image.Save(saveFileFialog.FileName);


                    }
                }
                Image.Dispose();
                saveFileFialog.Dispose();
        }

        private void Form1_DragLeave(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(Canvas!=null)
            Canvas.SetChanges(true);
        }
    }
}
