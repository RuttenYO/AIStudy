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
        double average;
        double max;

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


            //Setup Fuzzy Logic Sets
           

            //Setup Rules for Fuzzy Logic

            

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

            max = 0;
            for (i=0; i<ar.Length; i++)
                for (int j = 0; j < ar.Length; j++)
                {
                    if (i == j) continue;
                    max = Math.Max(Math.Abs(ar[i] - ar[j]), max);
                }
            average = 0;
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

        private void button6_Click(object sender, EventArgs e)
        {
            ContinuousDimension dimension = new ContinuousDimension("dimension", "deviation", "points", 0, 1);
            ContinuousDimension conclusion = new ContinuousDimension("conclusion", "conclusion", "points", 0, 1);

            FuzzySet low = new RightLinearSet(dimension, "low", 0, (decimal)0.4);
            FuzzySet middle = new TriangularSet(dimension, "middle", (decimal)0.5, (decimal)0.8);
            FuzzySet high = new LeftLinearSet(dimension, "high", (decimal)0.7, (decimal)1);

            FuzzySet conclusionLow = new RightLinearSet(conclusion, "conclusionLow", 0, (decimal)0.6);
            FuzzySet conclusionHigh = new LeftLinearSet(conclusion, "conclusionHigh", (decimal)0.4, (decimal)1);

            double isDeviationLow = low.IsMember(
                new Dictionary<IDimension, decimal>{
                            { dimension, (decimal)max },   
                    }
            );

            double isDeviationMiddle = middle.IsMember(
                new Dictionary<IDimension, decimal>{
                            { dimension, (decimal)max },   
                    }
            );

            double isDeviationHigh = high.IsMember(
                new Dictionary<IDimension, decimal>{
                            { dimension, (decimal)max },   
                    }
            );

        }
    }
}
