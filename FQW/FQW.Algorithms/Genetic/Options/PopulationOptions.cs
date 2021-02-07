using System;
using System.Collections.Generic;
using System.Text;

namespace FQW.Algorithms.Genetic.Options
{
    public class PopulationOptions : ModelOptions
    {
        public Int32 PopulationSize { get; set; }
        public Boolean UseEliteIndividuals { get; set; }
        public Int32 NumberEliteIndividuals { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(base.ToString());
            sb.AppendLine("PopulationSize:\t" + this.PopulationSize);
            sb.AppendLine("UseEliteIndividuals:\t" + this.UseEliteIndividuals);
            sb.AppendLine("NumberEliteIndividuals:\t" + this.NumberEliteIndividuals);
            return sb.ToString();
        }

        public static new PopulationOptions Default
        {
            get
            {
                ModelOptions options = ModelOptions.Default;
                return new PopulationOptions()
                {
                    AdaptiveCrossover = options.AdaptiveCrossover,
                    IncestControl = options.IncestControl,
                    ModelType = options.ModelType,
                    PGene = options.PGene,
                    PMutationMax = options.PMutationMax,
                    PMutationMin = options.PMutationMin,
                    PopulationSize = 30,
                    NumberEliteIndividuals = 5,
                    UseEliteIndividuals = true
                };
            }
        }
    }
}
