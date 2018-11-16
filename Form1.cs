using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO.Ports;
using AForge.Video;
using AForge.Video.VFW;
using AForge.Video.DirectShow;
using AForge.Imaging.Filters;

namespace RTVPS
{
    public partial class Form1 : Form
    {
         //////////////////////////////////////GLOBAL DECLARATIONS/////// 
        public int w = 640, h = 480;
        public Bitmap image;
        int close_en = 0; 
        int selection = 0; 
        int choice = 0;
        int ch = 0;
        //////FORM LOAD AND OTHER INITIALIZATION//////////// 
        //INITIALIZING VIDEO SOURCE FROM FILE AND STREAMING DEVICES LIKE WEBCAM
        FileVideoSource videoSource = new FileVideoSource("C:\\tom.avi"); 
        static FilterInfoCollection videoDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice); 
        static VideoCaptureDevice videoCapture;
        static VideoCaptureDevice videoCapture1;
       
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox2.Visible = true;
            try
            {
                videoCapture = new VideoCaptureDevice(videoDevice[0].MonikerString);
                videoCapture1 = new VideoCaptureDevice(videoDevice[0].MonikerString);
            }
            catch
            {
                if (videoDevice.Count == 0) MessageBox.Show("No Cameras Detected");
                fromCameraToolStripMenuItem.Enabled = false; 
                close_en = 1;
            }

        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            videoCapture.Stop();
            videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
            videoSource.Start();
            colorFiltersToolStripMenuItem.Enabled = true;
            binarizationToolStripMenuItem.Enabled = true;
            edgeDetectorsToolStripMenuItem.Enabled = true;
            morphologyToolStripMenuItem.Enabled = true;
            otherFiltersToolStripMenuItem.Enabled = true;
        }

        private void fromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            videoCapture.Stop();
            videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
            videoSource.Start();
            colorFiltersToolStripMenuItem.Enabled = true;
            binarizationToolStripMenuItem.Enabled = true;
            edgeDetectorsToolStripMenuItem.Enabled = true;
            morphologyToolStripMenuItem.Enabled = true;
            otherFiltersToolStripMenuItem.Enabled = true;
        }

        private void fromCameraToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            videoSource.Stop();
            videoCapture.NewFrame += new NewFrameEventHandler(video_NewFrame);
            videoCapture.DesiredFrameSize = new Size(w, h);
            videoCapture.Start();
            colorFiltersToolStripMenuItem.Enabled = true;
            binarizationToolStripMenuItem.Enabled = true;
            edgeDetectorsToolStripMenuItem.Enabled = true;
            morphologyToolStripMenuItem.Enabled = true;
            otherFiltersToolStripMenuItem.Enabled = true;
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            videoSource.Stop();
            videoCapture.NewFrame += new NewFrameEventHandler(video_NewFrame);
            videoCapture.DesiredFrameSize = new Size(w, h);
            videoCapture.Start();
            colorFiltersToolStripMenuItem.Enabled = true;
            binarizationToolStripMenuItem.Enabled = true;
            edgeDetectorsToolStripMenuItem.Enabled = true;
            morphologyToolStripMenuItem.Enabled = true;
            otherFiltersToolStripMenuItem.Enabled = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
            videoSource.Stop();
            videoCapture.Stop();
            videoCapture1.Stop();
       
        }
        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            videoCapture1.Stop();
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
            System.Drawing.Bitmap flag = (Bitmap)eventArgs.Frame.Clone();
            BitmapData data = flag.LockBits(new Rectangle(0, 0, flag.Width, flag.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte blue, green, red;
            int nrVal, nrBrightness = 100;
            int ngVal, ngBrightness = 100;
            int nbVal, nbBrightness = 100;
            byte t;
            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);
                for (int i = 0; i777 < data.Height; i++)
                {
                    for (int j = 0; j < data.Width; j++)
                    {
                        switch (selection)
                        {
                            //No Operation 
                            case 0:
                                {
                                }
                                break;
                            // Gray Scale 
                            case 1:
                                {
                                    blue = ptr[0];
                                    green = ptr[1];
                                    red = ptr[2];
                                    ptr[0] = (byte)(0.3 * red + 0.59 * green + 0.11 * blue);
                                    ptr[1] = (byte)(0.3 * red + 0.59 * green + 0.11 * blue);
                                    ptr[2] = (byte)(0.3 * red + 0.59 * green + 0.11 * blue);
                                    ptr += 3;
                                }
                                break;
                            // Lighting 
                            case 2:
                                {
                                    nbVal = (int)(ptr[0] + nbBrightness);
                                    if (nbVal < 0)
                                        nbVal = 0;
                                    if (nbVal > 255)
                                        nbVal = 255;
                                    ptr[0] = (byte)nbVal;
                                    //ptr += 3;
                                    ngVal = (int)(ptr[1] + ngBrightness);
                                    if (ngVal < 0)
                                        ngVal = 0;
                                    if (ngVal > 255)
                                        ngVal = 255;
                                    ptr[1] = (byte)ngVal;
                                    //ptr += 3;
                                    nrVal = (int)(ptr[2] + nrBrightness);
                                    if (nrVal < 0)
                                        nrVal = 0;
                                    if (nrVal > 255)
                                        nrVal = 255;
                                    ptr[2] = (byte)nrVal;
                                    ptr += 3;
                                }
                                break;
                            //Darkening 
                            case 3:
                                {
                                    nbVal = (int)(ptr[0] - nbBrightness);
                                    if (nbVal < 0)
                                        nbVal = 0;
                                    if (nbVal > 255)
                                        nbVal = 255;
                                    ptr[0] = (byte)nbVal;
                                    //ptr += 3; 
                                    ngVal = (int)(ptr[1] - ngBrightness);
                                    if (ngVal < 0)
                                        ngVal = 0;
                                    if (ngVal > 255)
                                        ngVal = 255;
                                    ptr[1] = (byte)ngVal;
                                    //ptr += 3; 
                                    nrVal = (int)(ptr[2] - nrBrightness);
                                    if (nrVal < 0)
                                        nrVal = 0;
                                    if (nrVal > 255)
                                        nrVal = 255;
                                    ptr[2] = (byte)nrVal;
                                    ptr += 3;
                                }
                                break;
                            //Negative
                            case 4:
                                {
                                    ptr[0] = (byte)(255 - ptr[0]);
                                    ptr[1] = (byte)(255 - ptr[1]);
                                    ptr[2] = (byte)(255 - ptr[2]);
                                    ptr += 3;
                                }
                                break;
                            //red
                            case 5:
                                {
                                    ptr[0] = 0;
                                    ptr[1] = 0;
                                    ptr += 3;
                                } break;
                            //green
                            case 6:
                                {
                                    ptr[0] = 0;
                                    ptr[2] = 0;
                                    ptr += 3;

                                } break;

                            //blue
                            case 7:
                                {
                                    ptr[1] = 0;
                                    ptr[2] = 0;
                                    ptr += 3;

                                } break;
                            //cyan
                            case 8:
                                {
                                    ptr[2] = 0;
                                    ptr += 3;
                                } break;
                            //magenta
                            case 9:
                                {
                                    ptr[1] = 0;
                                    ptr += 3;
                                } break;
                            //yellow
                            case 10:
                                {
                                    ptr[0] = 0;
                                    ptr += 3;
                                } break;
                            //Sepia
                            case 11:
                                {
                                    byte p = (byte)(0.299 * ptr[2]);
                                    byte q = (byte)(0.587 * ptr[1]);
                                    byte r = (byte)(0.114 * ptr[0]);
                                    t = (byte)(p + q + r);
                                    // red
                                    ptr[2] = (byte)((t > 206) ? 255 : t + 49);
                                    // green
                                    ptr[1] = (byte)((t < 14) ? 0 : t - 14);
                                    // blue
                                    ptr[0] = (byte)((t < 56) ? 0 : t - 56);
                                    ptr += 3;
                                }
                                break;
                        }
                    }
                    ptr += data.Stride - data.Width * 3;
                }
                flag.UnlockBits(data);
            }
            pictureBox2.Image = flag;
            switch (ch)
            {
                case 0:
                    {
                    }break;
                case 1:
                    {
                        applyfilter(new HueModifier(180));
                    }break;


            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //videoCapture = null;
            videoCapture.Stop();
            //videoSource = null;
            videoSource.Stop();
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (close_en == 0)
                videoCapture.SignalToStop();
            videoSource.SignalToStop();
        }

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selection = 1;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            videoCapture1.Stop();
            videoCapture.Start();
            videoCapture.NewFrame += new NewFrameEventHandler(video_NewFrame);
            videoCapture.DesiredFrameSize = new Size(w, h);
            selection = 1;
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selection = 11;
        }

        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selection = 5;
        }

        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selection = 7;
        }

        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selection = 6;
        }

        private void cyanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selection = 8;
        }

        private void magentaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selection = 9;
        }

        private void yellowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selection = 10;
        }  
   
      private void darkeningToolStripMenuItem_Click(object sender, EventArgs e)
      {
          selection = 3;
      }

      private void lightingToolStripMenuItem_Click(object sender, EventArgs e)
      {
          selection = 2;
      }

      private void invertToolStripMenuItem_Click(object sender, EventArgs e)
      {
          selection = 4;
      }
      private void video_NewFrame1(object sender, NewFrameEventArgs eventArgs)
        {
            videoCapture.Stop();
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
            //pictureBox2.Image = (Bitmap)eventArgs.Frame.Clone();
            image = (Bitmap)eventArgs.Frame.Clone();
            byte[,] matrix = new byte[4, 4]
            {
            {  95, 233, 127, 255 },
            { 159,  31, 191,  63 },
            { 111, 239,  79, 207 },
            { 175,  47, 143,  15 }
             };
            switch (choice)
            {
                case 0:
                    {
                    }
                    break;
                    //Threshold
                case 1:
                    {
                        Grayscale g = new Grayscale(0.2125, 0.7154, 0.0721);
                        image = g.Apply(image);
                        Threshold filter = new Threshold(100);
                        applyfilter(filter);
                    }
                    break;
                    //OrderedDithering
                case 2:
                    {
                        Grayscale g = new Grayscale(0.2125, 0.7154, 0.0721);
                        image = g.Apply(image);
                        OrderedDithering filter = new OrderedDithering(matrix);
                        applyfilter(filter);
                    }
                    break;
                    //BayerDithering
                case 3:
                    {
                        Grayscale g = new Grayscale(0.2125, 0.7154, 0.0721);
                        image = g.Apply(image);
                        BayerDithering filter = new BayerDithering();
                        applyfilter(filter);
                    }
                    break;
                //floyd
                case 4:
                    {
                        Grayscale g = new Grayscale(0.2125, 0.7154, 0.0721);
                        image = g.Apply(image);
                        FloydSteinbergDithering filter = new FloydSteinbergDithering();
                        applyfilter(filter);

                    }
                    break;
                //burkes
                case 5:
                    {
                        Grayscale g = new Grayscale(0.2125, 0.7154, 0.0721);
                        image = g.Apply(image);
                        BurkesDithering filter = new BurkesDithering();
                        applyfilter(filter);
                    }
                    break;
                //jarvis
                case 6:
                    {
                        Grayscale g = new Grayscale(0.2125, 0.7154, 0.0721);
                        image = g.Apply(image);
                        JarvisJudiceNinkeDithering filter = new JarvisJudiceNinkeDithering();
                        applyfilter(filter);
                    }
                    break;
                //sierra
                case 7:
                    {
                        Grayscale g = new Grayscale(0.2125, 0.7154, 0.0721);
                        image = g.Apply(image);
                        SierraDithering filter = new SierraDithering();
                        applyfilter(filter);
                    }
                    break;
                //stucki
                case 8:
                    {
                        Grayscale g = new Grayscale(0.2125, 0.7154, 0.0721);
                        image = g.Apply(image);
                        StuckiDithering filter = new StuckiDithering();
                        applyfilter(filter);
                    }
                    break;
                    //Homogenity
                case 9:
                    {
                        Grayscale g = new Grayscale(0.2125, 0.7154, 0.0721);
                        image = g.Apply(image);
                        HomogenityEdgeDetector filter = new HomogenityEdgeDetector();
                        applyfilter(filter);
                    }
                    break;
                //rotate
                case 10:
                    {
                        applyfilter(new RotateChannels());
                    }break;
                    //sobel
                case 11:
                    {
                        Grayscale g = new Grayscale(0.2125, 0.7154, 0.0721);
                        image = g.Apply(image);
                        SobelEdgeDetector filter = new SobelEdgeDetector();
                        applyfilter(filter);

                    }
                    break;
                    //canny
                case 12:
                    {
                        Grayscale g = new Grayscale(0.2125, 0.7154, 0.0721);
                        image = g.Apply(image);
                        CannyEdgeDetector filter = new CannyEdgeDetector();
                        applyfilter(filter);
                    }
                    break;
                case 13:
                    {
                        applyfilter(new YCbCrFiltering());
                    }break;
                case 14:
                    {
                        applyfilter(new HueModifier(180));
                    }break;
                    //SIS Threshold
                case 15:
                    {
                        Grayscale g = new Grayscale(0.2125, 0.7154, 0.0721);
                        image = g.Apply(image);
                        SISThreshold filter = new SISThreshold();
                        applyfilter(filter);

                    }break;
                    //Difference
                case 16:
                    {
                        Grayscale g = new Grayscale(0.2125, 0.7154, 0.0721);
                        image = g.Apply(image);
                        DifferenceEdgeDetector filter = new DifferenceEdgeDetector();
                        applyfilter(filter);

                    }break;
                    //mirror
                case 17:
                    {
                        applyfilter(new Mirror(false, true));
                    }break;
                    //flip
                case 18:
                    {
                        applyfilter(new RotateBilinear(180, true));
                    }break;
                    //Erosion
                case 19:
                    {
                         applyfilter1(new Erosion());
                    }break;
                    //Dilatation
                case 20:
                    {
                        applyfilter1(new Dilatation());
                    }break;
                    //Opening
                case 21:
                    {
                        applyfilter1(new Opening());
                    }break;
                    //closing
                case 22:
                    {
                        applyfilter1(new Closing());
                    }break;
                    //jitter
                case 23:
                    {
                        applyfilter(new Jitter(15));
                    }break;
                    //OilPainting
                case 24:
                    {
                        applyfilter(new OilPainting(10));
                    }break;
                    //pixellate
                case 25:
                    {
                        applyfilter(new Pixellate(10));
                    }break;
            }
        }

        public void applyfilter(IFilter filter)
        {
            Bitmap newimage = filter.Apply(image);
            image = newimage;
            pictureBox2.Image = image;
        }
        public void applyfilter1(IFilter filter)
        {
            try
            {
                Grayscale g = new Grayscale(0.2125, 0.7154, 0.0721);
                image = g.Apply(image);
                Bitmap newImage = filter.Apply(image);
                pictureBox2.Image = newImage;

            }
            catch (ArgumentException)
            {
                MessageBox.Show("Selected filter can not be applied to the image", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
        }
        private void binarizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            videoCapture.Stop();
            videoCapture1.NewFrame += new NewFrameEventHandler(video_NewFrame1);
            videoCapture1.DesiredFrameSize = new Size(w, h);
            videoCapture1.Start();
            
        }

        private void thresholdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choice = 1;
        }

        private void orderedDitherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choice = 2;
        }

        private void bayerOrderedDitherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choice = 3;
        }

        private void floydsteinbergToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choice = 4;
        }

        private void burkerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choice = 5;
        }

        private void stuckiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choice = 8;
        }

        private void jarvisjudiceNinkeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choice = 6;
        }

        private void sierraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choice = 7;
        }

        private void sISThresholdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choice = 15;
        }

        private void colorFiltersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            videoCapture1.Stop();
            videoCapture.Start();
            videoCapture.NewFrame += new NewFrameEventHandler(video_NewFrame);
            videoCapture.DesiredFrameSize = new Size(w, h);
        }

        private void homogenityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choice = 9;
        }

        private void edgeDetectorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            videoCapture.Stop();
            videoCapture1.NewFrame += new NewFrameEventHandler(video_NewFrame1);
            videoCapture1.DesiredFrameSize = new Size(w, h);
            videoCapture1.Start();
            
        }

        private void sobelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choice = 11;
        }

        private void cannyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choice = 12;
        }

        private void differenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choice = 16;
        }

        private void operationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            videoCapture.Stop();
            videoCapture1.NewFrame += new NewFrameEventHandler(video_NewFrame1);
            videoCapture1.DesiredFrameSize = new Size(w, h);
            videoCapture1.Start();
        }

        private void mirrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choice = 17;
        }
        private void flipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choice = 18;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            videoCapture.Stop();
            videoCapture1.NewFrame += new NewFrameEventHandler(video_NewFrame1);
            videoCapture1.DesiredFrameSize = new Size(w, h);
            videoCapture1.Start();
            choice = 1;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Histogram hs = new Histogram(videoCapture, videoCapture1);
            hs.Show();
        }

        private void histogramToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Histogram hs = new Histogram(videoCapture, videoCapture1);
            hs.Show();
        }

        private void morphologyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            videoCapture.Stop();
            videoCapture1.NewFrame += new NewFrameEventHandler(video_NewFrame1);
            videoCapture1.DesiredFrameSize = new Size(w, h);
            videoCapture1.Start();
        }

        private void erosionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choice = 19;
        }

        private void dilatationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choice = 20;
        }

        private void openingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choice = 21;
        }

        private void closingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choice = 22;
        }

        private void otherFiltersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            videoCapture.Stop();
            videoCapture1.NewFrame += new NewFrameEventHandler(video_NewFrame1);
            videoCapture1.DesiredFrameSize = new Size(w, h);
            videoCapture1.Start();
        }

        private void jitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choice = 23;
        }

        private void oilPaintingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choice = 24;
        }

        private void pixellateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choice = 25;
        }

        private void hueModifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ch = 1;
        }

        private void darkeningToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            selection = 3;
        }

        private void lightingToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            selection = 2;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aboutform ab = new aboutform();
            ab.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
