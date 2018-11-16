using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using AForge.Imaging.Filters;
using AForge.Video.DirectShow;
using System.IO.Ports;
using AForge;
using AForge.Video;
using AForge.Video.VFW;
using AForge.Math;

namespace RTVPS
{
    public partial class Histogram : Form
    {
        public Histogram(VideoCaptureDevice videocapture,VideoCaptureDevice videocapture1)
        {
            videocapture.NewFrame += new NewFrameEventHandler(video_NewFrame);
            videocapture1.NewFrame += new NewFrameEventHandler(video_NewFrame);
            videocapture.Start();
            videocapture1.Start();
            InitializeComponent();
        }
         int[,] histogram = new int[3, 260];
        int val;
        public int w = 320, h = 240;
        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            //pictureBox1.Visible = true;
            //pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            // pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
            System.Drawing.Bitmap flag = (Bitmap)eventArgs.Frame.Clone();
            BitmapData data = flag.LockBits(new Rectangle(0, 0, flag.Width, flag.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);
                byte* ptr2 = (byte*)(data.Scan0);
                /*int u, v;
                byte blue, green, red;*/
                for (int w = 0; w < 3; w++)
                {
                    for (int i = 0; i < 260; i++)
                        histogram[w, i] = 0;
                }

                for (int w = 0; w < 3; w++)
                {
                    ptr = ptr2;
                    for (int y = 0; y < (data.Height); y++)
                    {
                        for (int x = 0; x < data.Width; x++)
                        {
                            if (w == 0)
                            {
                                val = ptr[0];
                                histogram[w, val]++;
                            }
                            if (w == 1)
                            {
                                val = ptr[1];
                                histogram[w, val]++;
                            }
                            if (w == 2)
                            {
                                val = ptr[2];
                                histogram[w, val]++;
                            }
                            ptr += 3;
                        }
                    }
                }


                System.Drawing.Bitmap graph0 = new System.Drawing.Bitmap(260, 200);
                System.Drawing.Bitmap graph1 = new System.Drawing.Bitmap(260, 200);
                System.Drawing.Bitmap graph2 = new System.Drawing.Bitmap(260, 200);
                BitmapData data0 = graph0.LockBits(new Rectangle(0, 0, graph0.Width, graph0.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                BitmapData data1 = graph1.LockBits(new Rectangle(0, 0, graph1.Width, graph1.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                BitmapData data2 = graph2.LockBits(new Rectangle(0, 0, graph2.Width, graph2.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                unsafe
                {
                    byte* imgPtr0 = (byte*)(data0.Scan0);
                    byte* imgPtr1 = (byte*)(data1.Scan0);
                    byte* imgPtr2 = (byte*)(data2.Scan0);

                    for (int k = 0; k < (graph0.Width * graph0.Height); k++)
                        imgPtr0 += 3;
                    for (int k = 0; k < (graph1.Width * graph1.Height); k++)
                        imgPtr1 += 3;
                    for (int k = 0; k < (graph2.Width * graph2.Height); k++)
                        imgPtr2 += 3;

                    for (int w = 0; w < 3; w++)
                    {
                        for (int i = 0; i < graph0.Height; i++)
                        {
                            for (int j = 0; j < graph0.Width; j++)
                            {
                                if ((i < ((histogram[w, 259 - j]) / 10)) && (w == 0))
                                {
                                    imgPtr0[0] = (byte)255;
                                    imgPtr0[1] = (byte)0;
                                    imgPtr0[2] = (byte)0;
                                }
                                else
                                {
                                    if (w == 0)
                                    {
                                        imgPtr0[0] = (byte)0;
                                        imgPtr0[1] = (byte)0;
                                        imgPtr0[2] = (byte)0;
                                    }
                                }
                                if ((i < ((histogram[w, 259 - j]) / 10)) && (w == 1))
                                {
                                    imgPtr1[0] = (byte)0;
                                    imgPtr1[1] = (byte)255;
                                    imgPtr1[2] = (byte)0;
                                }
                                else
                                {
                                    if (w == 1)
                                    {
                                        imgPtr1[0] = (byte)0;
                                        imgPtr1[1] = (byte)0;
                                        imgPtr1[2] = (byte)0;
                                    }
                                }
                                if ((i < ((histogram[w, 259 - j]) / 10)) && (w == 2))
                                {
                                    imgPtr2[0] = (byte)0;
                                    imgPtr2[1] = (byte)0;
                                    imgPtr2[2] = (byte)255;
                                }
                                else
                                {
                                    if (w == 2)
                                    {
                                        imgPtr2[0] = (byte)0;
                                        imgPtr2[1] = (byte)0;
                                        imgPtr2[2] = (byte)0;
                                    }
                                }

                                //histogram[w,j] = histogram[w,j] - 1;

                                if (w == 0)
                                {
                                    imgPtr0--;
                                    imgPtr0--;
                                    imgPtr0--;
                                }
                                if (w == 1)
                                {
                                    imgPtr1--;
                                    imgPtr1--;
                                    imgPtr1--;
                                }
                                if (w == 2)
                                {
                                    imgPtr2--;
                                    imgPtr2--;
                                    imgPtr2--;
                                }
                            }
                        }
                    }

                }
                graph0.UnlockBits(data0);
                graph1.UnlockBits(data1);
                graph2.UnlockBits(data2);

                pictureBox1.Image = graph0;
                pictureBox2.Image = graph1;
                pictureBox3.Image = graph2;
                /////////////////////////////
            }
        }

    

        private void Histogram_Load(object sender, EventArgs e)
        {
            
        }

    }
}
