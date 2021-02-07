using FQW.Algorithms.Genetic.Options;
using RDotNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace FQW.Algorithms.Genetic.Entity
{
    public class Model
    {
        private readonly Chromosome m_chromosome;

        private readonly REngine m_engine;
        
        private readonly ModelOptions m_options;

        public Double Fitness { get; private set; }

        public ConfusionMatrix Matrix { get; private set; }

        public String[] ColumnNames { get; private set; }

        public Model(ModelOptions options, REngine engine)
        {
            this.m_options = options;
            this.m_engine = engine;
            Int32 featuresCount = engine.GetSymbol("featuresCount").AsInteger()[0];
            this.m_chromosome = new Chromosome(featuresCount, this.m_options);
        }

        private Model(Chromosome chromosome, REngine engine, ModelOptions options)
        {
            this.m_chromosome = chromosome;
            this.m_engine = engine;
            this.m_options = options;
        }

        internal Model CrossBreeding(Model other)
        {
            // кроссинговер хромосом родителей, получение хромосомы потомка:
            Chromosome chromosome = this.m_chromosome.CrossingOver(other.m_chromosome);
            return new Model(chromosome, this.m_engine, this.m_options);
        }

        internal void CalculateFitness()
        {
            StringBuilder sb = new StringBuilder();

            // получить индексы из хромосомы: 
            String sIdxs = String.Join(',', this.GetIndices());

            // сформировать подмножества для классификации:
            sb.AppendLine(String.Format("subTrainSet <- trainSet[ , c({0})]", sIdxs));
            sb.AppendLine(String.Format("subTestSet <- testSet[ , c({0})]", sIdxs));

            // запустить классификацию:
            sb.AppendLine("knnResult <- knn(train = subTrainSet, cl = classes, test = subTestSet, k = 7)");
            sb.AppendLine("confusionMatrix <- table(knnResult, classes)");
            sb.AppendLine("colsName <- dimnames(subTestSet)[[2]]");

            this.m_engine.Evaluate(sb.ToString());

            // получить результат классификации:
            this.Matrix = new ConfusionMatrix(this.m_engine.GetSymbol("confusionMatrix").AsIntegerMatrix().ToArray());
            this.ColumnNames = this.m_engine.GetSymbol("colsName").AsCharacter().ToArray();

            // вычисление приспособленности:
            this.Fitness = this.Matrix.F1_Score;
            this.m_chromosome.Fitness = this.Fitness;
        }

        public List<Int32> GetIndices()
        {
            List<Int32> indices = new List<Int32>();
            for (Int32 i = 0; i < this.m_chromosome.Size; i++)
            {
                if (this.m_chromosome[i] != false)
                {
                    indices.Add(i + 1);
                }
            }
            return indices;
        }

        public override string ToString()
        {
            return String.Format("Size: {0}; Fitness: {1}; Idxs: {2}", this.GetIndices().Count, this.Fitness.ToString("0.0000"), String.Join(',', this.GetIndices()));
        }
    }
}
