using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FuzzyFramework;
using FuzzyFramework.Dimensions;
using FuzzyFramework.Sets;
using FuzzyFramework.Defuzzification;

namespace AIStudySystem
{
    public partial class Form1 : Form
   {

        List<Label> labels;
        double[] ar;
        double[,] influence;
        public enum EBelongType {
            EBTLow,
            EBTAverage,
            EBTHigh,
        };
        public Form1()
        {
            InitializeComponent();
            labels = new List<Label>();

            labels.Add(label1);
            labels.Add(label2);
            labels.Add(label3);
            labels.Add(label4);

            Random random = new Random();
            influence = new double[5, 5];
            for (int i = 0; i < influence.GetLength(0); i++)
                for (int j = 0; j < influence.GetLength(1); j++)
                {
                    influence[i, j] = random.NextDouble();
                }

                ar = new double[4];
        }

        public void fillLabels(object sender, EventArgs e)
        {
            Random random = new Random();

            int i = 0;
            foreach (Label label in labels)
            {
                ar[i] = random.NextDouble();
                label.Text =  String.Format("{0:0.00}",ar[i]);
                i++;
            }

            double max = 0;
            for (i=0; i<ar.Length; i++)
                for (int j = 0; j < ar.Length; j++)
                {
                    if (i == j) continue;
                    max = Math.Max(Math.Abs(ar[i] - ar[j]), max);
                }
            double average = 0;
            for (i = 0; i < ar.Length; i++)
            {
                average += ar[i];
            }
            average /= ar.Length;
        }

        public EBelongType calculateBelonging(double variable)
        {

            return 0;
        }
    }
}
