using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FQW.Algorithms.Genetic.Options
{
    public class ChOptions
    {
        public Double PGene { get; set; }
        public Double PMutationMin { get; set; }
        public Double PMutationMax { get; set; }
        public Boolean IncestControl { get; set; }
        public Boolean AdaptiveCrossover { get; set; }

        public override string ToString()
        {
            string format = "0.000";
            CultureInfo cInfo = CultureInfo.InvariantCulture;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("pGene:\t" + this.PGene.ToString(format, cInfo));
            sb.AppendLine("pMutationMin:\t" + this.PMutationMin.ToString(format, cInfo));
            sb.AppendLine("pMutationMax:\t" + this.PMutationMax.ToString(format, cInfo));
            sb.AppendLine("IncestControl:\t" + this.IncestControl);
            sb.AppendLine("AdaptiveCrossingOver:\t" + this.AdaptiveCrossover);
            return sb.ToString();
        }

        public static ChOptions Default
        {
            get => new ChOptions()
            {
                PGene = 0.7,
                PMutationMin = 0.15,
                PMutationMax = 0.3,
                IncestControl = true,
                AdaptiveCrossover = true
            };
        }
    }
}
