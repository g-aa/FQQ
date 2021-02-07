using FQW.Algorithms.Genetic.Enumeration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FQW.Algorithms.Genetic.Options
{
    public class ModelOptions : ChOptions
    {
        public ModelType ModelType { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(base.ToString());
            sb.AppendLine("ModelType:\t" + this.ModelType);
            return sb.ToString();
        }
        
        public static new ModelOptions Default
        {
            get 
            {
                ChOptions options = ChOptions.Default;
                return new ModelOptions() 
                {
                    AdaptiveCrossover = options.AdaptiveCrossover,
                    IncestControl = options.IncestControl,
                    ModelType = ModelType.KNN,
                    PGene = options.PGene,
                    PMutationMax = options.PMutationMax,
                    PMutationMin = options.PMutationMin
                };
            }
        }
    }
}
