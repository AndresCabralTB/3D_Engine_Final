using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace _3D_3ngine_V1
{
    public partial class Form1 : Form
    {

        public float z = 1f;

        Vertex rotate1 = new Vertex(0,0,0);
        Vertex translate1 = new Vertex(0f,0,14f);
        int scale1 = 1;

        Vertex rotate2 = new Vertex(0, 0, 0);
        Vertex translate2 = new Vertex(-6, 0, 14f);
        int scale2 = 1;

        Vertex rotate3 = new Vertex(0, 0, 0);
        Vertex translate3 = new Vertex(6f, 0, 14f);
        int scale3 = 1;

        public List<Vertex> rotationValues = new List<Vertex>();
        public List<Vertex> traslationValues = new List<Vertex>();
        public List<float> zValues = new List<float>();

        public TreeNode selectedNode;
        public int count = 4;
        public int playbackIndex = 0;

        
        public bool objSel = true;
        public bool recordinSet = false;

        string filename;
        string filename2;
        string filename3;

        OpenFileDialog dialog = new OpenFileDialog();
        TreeNode node1, node2, node3;

        public Form1()
        {
            InitializeComponent();
        }
        public void Init(string filename, string filename2, string filename3)
        {
            CreateNewOBJ(filename, filename2, filename3);
        }

        public void CreateNewOBJ(string filename, string filename2, string filename3)
        {
            //Get the three objects as parameters and create a new ObjParser variable from each of them
           ObjParser objectPar = new ObjParser(filename);
           ObjParser objectPar2 = new ObjParser(filename2);
           ObjParser objectPar3 = new ObjParser(filename3);

            //-----------------FIRST OBJECT--------------------------//
           Vertex[] vertices = objectPar.Vertices.Select(v => new Vertex(v[0], v[1], v[2])).ToArray();

           Triangle[] triangles = objectPar.Faces.Select(t => new Triangle(
                Array.IndexOf(vertices, vertices[t[0]]),
                Array.IndexOf(vertices, vertices[t[1]]),
                Array.IndexOf(vertices, vertices[t[2]]),
                Color.Black)).ToArray();

           Model cube = new Model(vertices, triangles, new Vertex(0, 0, 0), (float)Math.Sqrt(3));

            //-----------------SECOND OBJECT--------------------------//
           Vertex[] vertices2 = objectPar2.Vertices.Select(v => new Vertex(v[0], v[1], v[2])).ToArray();

           Triangle[] triangles2 = objectPar2.Faces.Select(t => new Triangle(
                Array.IndexOf(vertices2, vertices2[t[0]]),
                Array.IndexOf(vertices2, vertices2[t[1]]),
                Array.IndexOf(vertices2, vertices2[t[2]]),
                Color.Black)).ToArray();

           Model cube2 = new Model(vertices2, triangles2, new Vertex(0, 0, 0), (float)Math.Sqrt(3));

            //-----------------THIRD OBJECT--------------------------//
           Vertex[] vertices3 = objectPar3.Vertices.Select(v => new Vertex(v[0], v[1], v[2])).ToArray();

           Triangle[] triangles3 = objectPar3.Faces.Select(t => new Triangle(
                Array.IndexOf(vertices3, vertices3[t[0]]),
                Array.IndexOf(vertices3, vertices3[t[1]]),
                Array.IndexOf(vertices3, vertices3[t[2]]),
                Color.Black)).ToArray();

           Model cube3 = new Model(vertices3, triangles3, new Vertex(0, 0, 0), (float)Math.Sqrt(3));

           Instance[] instances = new Instance[3];
            
            //Create and add each instance of the objects to an array of instances
           instances[0] = new Instance(cube, translate1, Mtx.Rotate(rotate1), scale1);
           instances[1] = new Instance(cube2, translate2, Mtx.Rotate(rotate2), scale2);
           instances[2] = new Instance(cube3, translate3, Mtx.Rotate(rotate3), scale3);

           Rasterization raster = new Rasterization(PCT_CANVAS.Size, rotate1, translate1, trackBar4.Value, instances);


           PCT_CANVAS.Image = raster.Canvas;

        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {/*
            if (e.Node.Tag != null)
            {
                =
            }
        */
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //Select and modify the object from the tree view
            if(e.Node.Text == "1_" + Path.GetFileName(filename))
            {
                //MessageBox.Show(Path.GetFileName(filename) + " selected");
                count = 0;

            } else if(e.Node.Text == "2_" + Path.GetFileName(filename2))
            {
                //MessageBox.Show(Path.GetFileName(filename2) + " selected");
                count = 1;

            } else if(e.Node.Text == "3_" + Path.GetFileName(filename3))
            {
                //MessageBox.Show(Path.GetFileName(filename3) + " selected");
                count = 2;
            }
        }

        public void Add_OBJ_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear(); //Clears the treeView so it doesnt stack up

            //OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "OBJ files (*.obj)|*.obj";
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dialog.FileNames.Length == 3) //Allows for three objects to be made
                {
                    filename = dialog.FileNames[0];
                    filename2 = dialog.FileNames[1];
                    filename3 = dialog.FileNames[2];


                    //Add tree view
                    //TreeNode parentNode = treeView1.Nodes.Add("OBJ files");
                     node1 = treeView1.Nodes.Add("1_" + Path.GetFileName(filename));
                     node2 = treeView1.Nodes.Add("2_" + Path.GetFileName(filename2));
                     node3 = treeView1.Nodes.Add("3_" + Path.GetFileName(filename3));

                    count = 0;
                    canvasTimer.Enabled = true; //Start the timer that invalidates the PCT every milisencond
                    canvasTimer.Start();

                    Init(filename, filename2, filename3); //Inialize the objects

                }
                else
                {
                    MessageBox.Show("Please select three OBJ files.");
                }
            }

            
        }

        public void button1_Click(object sender, EventArgs e)
        {
            //String newFileReplaced;
            OpenFileDialog newDialog = new OpenFileDialog();

            if (count == 0 || count == 1 || count == 2)
            {
                if (newDialog.ShowDialog() == DialogResult.OK)
                {
                    //filename = newDialog.FileName;
                    //treeView1.SelectedNode.Get

                    switch (count)
                    {
                        case 0:
                            //treeView1.Nodes.Remove(node1);

                            filename = newDialog.FileName;

                            treeView1.Nodes[0].Text = "1_" + Path.GetFileName(filename);

                            Init(filename, filename2, filename3);

                            break;
                        case 1:
                            // treeView1.Nodes.Remove(node2);

                            filename2 = newDialog.FileName;

                            treeView1.Nodes[1].Text = "2_" + Path.GetFileName(filename2);


                            Init(filename, filename2, filename3);

                            break;
                        case 2:
                            //treeView1.Nodes.Remove(node3);

                            filename3 = newDialog.FileName;

                            treeView1.Nodes[2].Text = "3_" + Path.GetFileName(filename3);
                            Init(filename, filename2, filename3);

                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show("No OBJ file detected");
            }
        }

        //Track bars that control rotation, traslation and scale
        public void trackBar1_Scroll(object sender, EventArgs e)
        {
            YrotLabel.Text = "Y:" + trackBar1.Value.ToString();
        }

        public void trackBar2_Scroll(object sender, EventArgs e)
        {
            XrotLabel.Text = "X:" + trackBar2.Value.ToString();
        }

        public void trackBar3_Scroll(object sender, EventArgs e)
        {
            ZrotLabel.Text = "Z:" + trackBar3.Value.ToString();
        }

        public void trackBar4_Scroll(object sender, EventArgs e)
        {
            scaleLabel.Text = "Scale:" + trackBar4.Value.ToString();
        }

        public void trackBarTrY_Scroll(object sender, EventArgs e)
        {
            YTrLabel.Text ="Y:" + trackBarTrY.Value.ToString();
        }

        public void trackBarTrX_Scroll(object sender, EventArgs e)
        {
            TrXlabel.Text = "X:" + trackBarTrX.Value.ToString();
        }

        public void trackBarTrZ_Scroll(object sender, EventArgs e)
        {
            TrZlabel.Text = "Z: " + trackBarTrZ.Value.ToString();
        }

        public void canvasTimer_Tick(object sender, EventArgs e)
        {   
            //Timer that invalidates screen every milisecond
            Init(filename, filename2, filename3);
            moveOBJ(count);
            PCT_CANVAS.Invalidate();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            canvasTimer.Stop(); //Stop timer when screen is closed
        }

        private void Stop_BTN_Click(object sender, EventArgs e)
        {
            //Erase the recordings
            rotationValues.Clear();
            traslationValues.Clear();
            zValues.Clear();
            recordinSet = false;

            MessageBox.Show("Recording Deleted");
        }

        public void Record_BTN_Click(object sender, EventArgs e)
        {


            if (count == 0 || count == 1 || count == 2)
            {
                if (int.TryParse(recordValue.Text, out int timerVal))
                {
                    //Start recording timer and disable record button
                    Record_Timer.Interval = 50; //100
                    Record_Timer.Start();
                    Record_Timer.Enabled = true;
                    Record_BTN.Enabled = false;


                    //Start timer that will stop the recording in 10 seconds
                    timer1.Start();
                    timer1.Enabled = true;

                    timer1.Interval = int.Parse(recordValue.Text) * 1000;
                }
                else
                {
                    MessageBox.Show("Please enter a valid integer for the interval");
                }
            }
            else
            {
                MessageBox.Show("No OBJ file detected");
            }

        }

        private void Record_Timer_Tick(object sender, EventArgs e)
        {
            switch (count)
            {
                //Will run depending on the object chosen
                case 0:
                    rotate1 = new Vertex(trackBar2.Value, trackBar1.Value, trackBar3.Value);
                    translate1 = new Vertex(trackBarTrX.Value, trackBarTrY.Value, trackBarTrZ.Value);
                    z = trackBar4.Value;

                    rotationValues.Add(rotate1);
                    traslationValues.Add(translate1);
                    zValues.Add(z);

                    recordinSet = true;
                    break;
                case 1:
                    rotate2 = new Vertex(trackBar2.Value, trackBar1.Value, trackBar3.Value);
                    translate2 = new Vertex(trackBarTrX.Value, trackBarTrY.Value, trackBarTrZ.Value);
                    z = trackBar4.Value;

                    rotationValues.Add(rotate2);
                    traslationValues.Add(translate2);
                    zValues.Add(z);

                    recordinSet = true;
                    break;
                case 2:
                    rotate3 = new Vertex(trackBar2.Value, trackBar1.Value, trackBar3.Value);
                    translate3 = new Vertex(trackBarTrX.Value, trackBarTrY.Value, trackBarTrZ.Value);
                    z = trackBar4.Value;

                    rotationValues.Add(rotate3);
                    traslationValues.Add(translate3);
                    zValues.Add(z);

                    recordinSet = true;
                    break;

            }

            
        }

        public void Play_BTN_Click(object sender, EventArgs e)
        {
            if (!recordinSet)
            {
                MessageBox.Show("No recording available");
            }
            else
            {
                Play_Timer.Interval = 50; //125
                Play_Timer.Start();
            }
        }

        public void Play_Timer_Tick(object sender, EventArgs e)
        {
            if(playbackIndex < rotationValues.Count)
            {
                //Play the recording
                trackBar2.Value = (int)rotationValues[playbackIndex].X;
                trackBar1.Value = (int)rotationValues[playbackIndex].Y;
                trackBar3.Value = (int)rotationValues[playbackIndex].Z;

                trackBarTrX.Value = (int)traslationValues[playbackIndex].X;
                trackBarTrY.Value = (int)traslationValues[playbackIndex].Y;
                trackBarTrZ.Value = (int)traslationValues[playbackIndex].Z;

                trackBar4.Value = (int)zValues[playbackIndex];

                moveOBJ(count);

                playbackIndex++;
            }
            else
            {
                playbackIndex = 0;
                Play_Timer.Stop();
                Play_Timer.Enabled = false;
                Record_BTN.Enabled = true;
                
                MessageBox.Show("End of recording");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //This runs 5 seconds after the Record Button is clicked
            Record_Timer.Stop();
            Record_Timer.Enabled = false;
            Record_BTN.Enabled = true;

            timer1.Stop();
            timer1.Enabled = false;
            MessageBox.Show("Screen has stopped recording");

        }

        public void moveOBJ(int count)
        {
            switch (count)
            {
                case 0:

                    rotate1 = new Vertex(trackBar2.Value, trackBar1.Value, trackBar3.Value);
                    translate1 = new Vertex(trackBarTrX.Value, trackBarTrY.Value, trackBarTrZ.Value);
                    scale1 = trackBar4.Value;
                    break;

                case 1:

                    rotate2 = new Vertex(trackBar2.Value, trackBar1.Value, trackBar3.Value);
                    translate2 = new Vertex(trackBarTrX.Value, trackBarTrY.Value, trackBarTrZ.Value);
                    scale2 = trackBar4.Value;
                    break;

                case 2:

                    rotate3 = new Vertex(trackBar2.Value, trackBar1.Value, trackBar3.Value);
                    translate3 = new Vertex(trackBarTrX.Value, trackBarTrY.Value, trackBarTrZ.Value);
                    scale3 = trackBar4.Value;
                    break;
            }
        }

        private void recordValue_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void PCT_CANVAS_SizeChanged(object sender, EventArgs e)
        {
        }

        
    }
}
