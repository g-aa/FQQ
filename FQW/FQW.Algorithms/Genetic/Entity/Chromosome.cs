using FQW.Algorithms.Genetic.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace FQW.Algorithms.Genetic.Entity
{
    internal class Chromosome
    {
        private readonly Random m_random;

        private readonly ChOptions m_options;

        private readonly List<Boolean> m_genes;

        public Boolean this[Int32 index] => this.m_genes[index];

        public Int32 Size => this.m_genes.Count;

        public Double Fitness { get; set; }

        private Chromosome(ChOptions options)
        {
            this.m_random = new Random();
            this.m_options = options;
            this.Fitness = 1;
        }

        private Chromosome(List<Boolean> genes, ChOptions options) : this(options)
        {
            this.m_genes = genes;
        }

        public Chromosome(Int32 length, ChOptions options) : this(options)
        {
            this.m_genes = new List<Boolean>(length);
            for (Int32 i = 0; i < length; i++)
            {
                this.m_genes.Add(this.m_random.NextDouble() < options.PGene ? true : false);
            }
        }

        public Chromosome CrossingOver(Chromosome other)
        {
            Int32 distance = HammingDistance(this, other);
            ChOptions options = new ChOptions()
            {
                AdaptiveCrossover = this.m_options.AdaptiveCrossover,
                IncestControl = this.m_options.IncestControl,
                PGene = this.m_options.PGene,
                PMutationMax = this.m_options.PMutationMax,
                PMutationMin = this.m_options.PMutationMin
            };

            Double pK = 0.5;
            if (this.m_options.AdaptiveCrossover)
            {
                // коэффициент предпочтения гена для this на основании Fitness:
                pK = this.Fitness / (this.Fitness + other.Fitness);
            }
            
            List<Boolean> genes = new List<Boolean>(this.Size);
            for (Int32 g = 0; g < this.Size; g++)
            {
                genes.Add(this.m_random.NextDouble() < pK ? this[g] : other[g]);
            }
            
            Chromosome ch = new Chromosome(genes, options);
            ch.Mutation(distance);
            return ch;
        }

        public static Int32 HammingDistance(Chromosome ch1, Chromosome ch2)
        {
            Int32 distance = 0;
            for (int g = 0; g < ch1.Size; g++)
            {
                if (ch1[g] != ch2[g])
                {
                    distance++;
                }
            }
            return distance;
        }

        private void Mutation(Int32 distance)
        {
            Int32 gs = 2;
            Double pM = this.m_options.PMutationMin;
            if (this.m_options.IncestControl && distance < 0.4 * this.m_genes.Count)
            {
                gs = this.m_genes.Count / 2;
                pM = this.m_options.PMutationMax;
            }
            
            for (int g = 0; g < gs; g++)
            {
                Int32 index = this.m_random.Next(this.m_genes.Count);
                Double p = this.m_random.NextDouble();
                if (p < pM)
                {
                    this.m_genes[index] = !this.m_genes[index];
                }
            }
        }

        private void Inversion()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return String.Format("size: {0}; genes: {1}", this.Size, String.Join(',', this.m_genes.Select((Boolean gene) =>
            {
                return gene ? 1 : 0;
            })));
        }
    }
}
