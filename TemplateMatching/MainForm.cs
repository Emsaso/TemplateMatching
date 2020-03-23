using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Accord;
using Accord.Imaging;
using System.Drawing.Imaging;
using Accord.Imaging.Filters;

namespace TemplateMatching
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Filter = "Jpeg|*.jpg";
                DialogResult dr = openFileDialog1.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    textBox1.Text = openFileDialog1.FileName;
                    PictureInput.Load(openFileDialog1.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.ToString());
            }
        }

        private void btnMatch_Click(object sender, EventArgs e)
        {
            //Converting Template into GrayScale Image            
            Bitmap templateImage = new Bitmap(textBox2.Text);
            Grayscale gg = new GrayscaleBT709();
            Bitmap grayTemplate = gg.Apply(templateImage);

            // create template matching algorithm's instance
            ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0);
            // find all matchings with specified above similarity
            Bitmap sourceImage = new Bitmap(textBox1.Text);
            Bitmap graySource = gg.Apply(sourceImage);
            
            TemplateMatch[] matchings = tm.ProcessImage(graySource, grayTemplate);
            
            Graphics g = Graphics.FromImage(sourceImage);
            if (matchings[0].Similarity > 0.8f)
            {
                int X = matchings[0].Rectangle.X;
                int Y = matchings[0].Rectangle.Y;

                g.DrawRectangle(new Pen(Color.Red, 3), X, Y, matchings[0].Rectangle.Width, matchings[0].Rectangle.Height);                
                PicTemplate.Image = sourceImage;
                MessageBox.Show("Match found...");
            }
            else
            {
                MessageBox.Show("Match Not Found...");
            }           
        }

        private void btnTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Filter = "Jpeg|*.jpg";
                DialogResult dr = openFileDialog1.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    textBox2.Text = openFileDialog1.FileName;
                    //PictureInput.Load(openFileDialog1.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.ToString());
            }
        }
    }
}
    