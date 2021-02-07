using FQW.Algorithms.Genetic.Enumeration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FQW.Algorithms.Genetic.Options
{
    public class AlgorithmOptions : PopulationOptions
    {
        public Int32 MaxGenerations { get; set; }
        public Double Accuracy { get; set; }
        public AccuracyType AccuracyType { get; set; }

        public override string ToString()
        {
            string format = "0.000";
            CultureInfo cInfo = CultureInfo.InvariantCulture;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(base.ToString());
            sb.AppendLine("MaxGenerations:\t" + this.MaxGenerations);
            sb.AppendLine("Accuracy:\t" + this.Accuracy.ToString(format, cInfo));
            sb.AppendLine("AccuracyType:\t" + this.AccuracyType);
            return sb.ToString();
        }

        public static new AlgorithmOptions Default
        {
            get
            {
                PopulationOptions options = PopulationOptions.Default;
                return new AlgorithmOptions()
                {
                    Accuracy = 0.85,
                    AccuracyType = AccuracyType.AVG,
                    AdaptiveCrossover = options.AdaptiveCrossover,
                    IncestControl = options.IncestControl,
                    MaxGenerations = 1000,
                    ModelType = options.ModelType,
                    NumberEliteIndividuals = options.NumberEliteIndividuals,
                    PGene = options.PGene,
                    PMutationMax = options.PMutationMax,
                    PMutationMin = options.PMutationMin,
                    PopulationSize = options.PopulationSize,
                    UseEliteIndividuals = options.UseEliteIndividuals
                };
            }
        }
    }
}
