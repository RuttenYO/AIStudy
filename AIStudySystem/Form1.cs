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
        List<Button> buttons;
        double[] ar;
        double[,] influence;
        double average;
        double max;
        ContinuousDimension dimension, conclusion;
        FuzzySet low, middle, high, conclusionLow, conclusionHigh;

        int buttonNumber;

        public enum EBelongType {
            EBTLow,
            EBTAverage,
            EBTHigh,
        };


        public struct FuzzyBelong
        {
            public EBelongType belongType;
            public double value;
        }

        FuzzyBelong DeviationType, AverageType, InfluenceType1, InfluenceType2; 

        public Form1()
        {
            InitializeComponent();
            labels = new List<Label>();
            buttons = new List<Button>();

            buttons.Add(button1);
            buttons.Add(button2);
            buttons.Add(button3);
            buttons.Add(button4);

            labels.Add(label1);
            labels.Add(label2);
            labels.Add(label3);
            labels.Add(label4);

            //Setup Fuzzy sets
            dimension = new ContinuousDimension("dimension", "deviation", "points", 0, 1);
            conclusion = new ContinuousDimension("conclusion", "conclusion", "points", 0, 1);

            low = new RightLinearSet(dimension, "low", 0, (decimal)0.4);
            middle = new TriangularSet(dimension, "middle", (decimal)0.5, (decimal)0.8);
            high = new LeftLinearSet(dimension, "high", (decimal)0.7, (decimal)1);

            conclusionLow = new RightLinearSet(conclusion, "conclusionLow", 0, (decimal)0.6);
            conclusionHigh = new LeftLinearSet(conclusion, "conclusionHigh", (decimal)0.4, (decimal)1);

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

            buttonNumber = buttons.IndexOf((Button)sender);

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


            ////////

            //Mu (Deviation)
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

            DeviationType = calculateBelonging(isDeviationLow, isDeviationMiddle, isDeviationHigh);

            //Mu (Average)
            double isAverageLow = low.IsMember(
                new Dictionary<IDimension, decimal>{
                            { dimension, (decimal)average },   
                    }
            );

            double isAverageMiddle = middle.IsMember(
                new Dictionary<IDimension, decimal>{
                            { dimension, (decimal)average },   
                    }
            );

            double isAverageHigh = high.IsMember(
                new Dictionary<IDimension, decimal>{
                            { dimension, (decimal)average},   
                    }
            );

            AverageType = calculateBelonging(isAverageLow, isAverageMiddle, isAverageHigh);

            

            //Mu (Influence)
            double isInfluenceLow = low.IsMember(
                new Dictionary<IDimension, decimal>{
                            { dimension, (decimal)influence[buttonNumber,buttonNumber+1] },   
                    }
            );

            double isInfluenceHigh = high.IsMember(
                new Dictionary<IDimension, decimal>{
                            { dimension, (decimal)influence[buttonNumber,buttonNumber+1]},   
                    }
            );

            InfluenceType1 = calculateBelonging(isInfluenceLow, isInfluenceHigh);

            if (buttonNumber > 0)
            {
                //Mu (Influence)
                isInfluenceLow = low.IsMember(
                   new Dictionary<IDimension, decimal>{
                            { dimension, (decimal)influence[buttonNumber,buttonNumber-1] },   
                    }
               );

                isInfluenceHigh = high.IsMember(
                   new Dictionary<IDimension, decimal>{
                            { conclusion, (decimal)influence[buttonNumber,buttonNumber-1]},   
                    }
               );

                InfluenceType2 = calculateBelonging(isInfluenceLow, isInfluenceHigh);
            }


            /////////////////////// Rules

            double lowFromY1 = 2, lowFromY2 =2;

            if (DeviationType.belongType == EBelongType.EBTLow && AverageType.belongType == EBelongType.EBTHigh && InfluenceType1.belongType == EBelongType.EBTHigh)
            {
                lowFromY1 = Math.Min(lowFromY1, DeviationType.value);
                lowFromY1 = Math.Min(lowFromY1, AverageType.value);
                lowFromY1 = Math.Min(lowFromY1, InfluenceType1.value);
            }


            if (DeviationType.belongType == EBelongType.EBTAverage && AverageType.belongType == EBelongType.EBTHigh && InfluenceType1.belongType == EBelongType.EBTHigh)
            {
                lowFromY1 = Math.Min(lowFromY1, DeviationType.value);
                lowFromY1 = Math.Min(lowFromY1, AverageType.value);
                lowFromY1 = Math.Min(lowFromY1, InfluenceType1.value);
            }

            if (DeviationType.belongType == EBelongType.EBTLow && AverageType.belongType == EBelongType.EBTAverage && InfluenceType1.belongType == EBelongType.EBTHigh)
            {
                lowFromY1 = Math.Min(lowFromY1, DeviationType.value);
                lowFromY1 = Math.Min(lowFromY1, AverageType.value);
                lowFromY1 = Math.Min(lowFromY1, InfluenceType1.value);
            }

            if (DeviationType.belongType == EBelongType.EBTAverage && AverageType.belongType == EBelongType.EBTLow && InfluenceType1.belongType == EBelongType.EBTHigh)
            {
                lowFromY2 = Math.Min(lowFromY2, DeviationType.value);
                lowFromY2 = Math.Min(lowFromY2, AverageType.value);
                lowFromY2 = Math.Min(lowFromY2, InfluenceType1.value);
            }

            

        }




        public FuzzyBelong calculateBelonging(double low, double middle, double high)
        {
            FuzzyBelong result = new FuzzyBelong();
            if (low > middle && low > high)
            {
                result = new FuzzyBelong();
                result.belongType = EBelongType.EBTLow;
                result.value = low;
            }
            if (middle > low && middle > high)
            {
                result = new FuzzyBelong();
                result.belongType = EBelongType.EBTAverage;
                result.value = middle;
            }
            if (high > low && high > middle)
            {
                result = new FuzzyBelong();
                result.belongType = EBelongType.EBTHigh;
                result.value = high;
            }


            return result;
        }

        public FuzzyBelong calculateBelonging(double low, double high)
        {
            FuzzyBelong result = new FuzzyBelong();
            if (low > high)
            {
                result = new FuzzyBelong();
                result.belongType = EBelongType.EBTLow;
                result.value = low;
            };
            if (high > low)
            {
                result = new FuzzyBelong();
                result.belongType = EBelongType.EBTHigh;
                result.value = high;
            }
            return result;
        }

        private void button6_Click(object sender, EventArgs e)
        {



        }
    }
}
