using FQW.Algorithms.Genetic.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FQW.Algorithms.Genetic
{
    public class IterationResult : IiterationResult
    {
        public int Generation { get; set; }
        public BestModel Model { get; set; }
        public Accuracy PopulationAccuracy { get; set; }
        public Features PopulationFeatures { get; set; }

        public static string ResultHeader
        {
            get => "Generation;";
        }

        public override string ToString()
        {
            return this.Generation + ";";
        }

        public class BestModel
        {
            public double Accuracy { get; set; }
            public double Recall { get; set; }
            public double Precision { get; set; }
            public double Specificity { get; set; }
            public double FallOut { get; set; }
            public double F1_Score { get; set; }
            public int FeatureCount { get; set; }
            public int[] ParameterIndices { get; set; }

            public static string BestModelHeader
            {
                get => "Accuracy;Recall;Precision;Specificity;FallOut;F1_Score;FeatureCount;ParameterIndices";
            }

            public override string ToString()
            {
                string format = "0.000";
                CultureInfo cinfo = CultureInfo.InvariantCulture;
                return string.Format("{0};{1};{2};{3};{4};{5};{6};{7}",
                    this.Accuracy.ToString(format, cinfo),
                    this.Recall.ToString(format, cinfo),
                    this.Precision.ToString(format, cinfo),
                    this.Specificity.ToString(format, cinfo),
                    this.FallOut.ToString(format, cinfo),
                    this.F1_Score.ToString(format, cinfo),
                    this.FeatureCount,
                    string.Join(',', this.ParameterIndices)
                    );
            }
        }

        public class Accuracy
        {
            public double MAX_Accuracy { get; set; }
            public double MIN_Accuracy { get; set; }
            public double RMS_Accuracy { get; set; }
            public double AVG_Accuracy { get; set; }
            public double HARM_Accuracy { get; set; }

            public static string AccuracyHeader
            {
                get => "MAX-Accuracy;RMS-Accuracy;AVG-Accuracy;HARM-Accuracy;MIN-Accuracy";
            }

            public override string ToString()
            {
                string format = "0.000";
                CultureInfo cinfo = CultureInfo.InvariantCulture;
                return string.Format("{0};{1};{2};{3};{4}",
                    this.MAX_Accuracy.ToString(format, cinfo),
                    this.RMS_Accuracy.ToString(format, cinfo),
                    this.AVG_Accuracy.ToString(format, cinfo),
                    this.HARM_Accuracy.ToString(format, cinfo),
                    this.MIN_Accuracy.ToString(format, cinfo)
                    );
            }
        }

        public class Features
        {
            public int MIN_Features { get; set; }
            public int MAX_Features { get; set; }
            public int AVG_Features { get; set; }

            public static string FeaturesHeader
            {
                get => "MIN-Features;AVG-Features;MAX-Features";
            }

            public override string ToString()
            {
                return string.Format("{0};{1};{2}",
                    this.MIN_Features,
                    this.AVG_Features,
                    this.MAX_Features);
            }
        }
    }
}
