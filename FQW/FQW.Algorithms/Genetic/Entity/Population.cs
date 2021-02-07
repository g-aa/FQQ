using FQW.Algorithms.Genetic.Enumeration;
using FQW.Algorithms.Genetic.Options;
using RDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FQW.Algorithms.Genetic.Entity
{
    public class Population
    {
        public Int32 Generation { get; private set; }

        public Int32 Size { get => this.m_models.Count; }

        public Model this[Int32 index] => this.m_models[index];

        private List<Model> m_models; // полный перечень моделий в популяции

        private List<Model> m_bestModels; // модели отобранные для скрещивания

        private REngine m_engine;

        private PopulationOptions m_options;

        public Population(PopulationOptions options, REngine engine)
        {
            this.Generation = 1;
            this.m_models = new List<Model>(options.PopulationSize);
            this.m_bestModels = new List<Model>();
            this.m_options = options;
            this.m_engine = engine;
        }

        public void CreatePrimaryPopulation()
        {
            // генерация первичного поколения:
            for (int i = 0; i < this.m_options.PopulationSize; i++)
            {
                this.m_models.Add(new Model(this.m_options, this.m_engine));
            }
        }

        public void CalculateFitness()
        {
            this.m_models.ForEach((Model m) => 
            {
                if (m.Fitness == 0) 
                {
                    m.CalculateFitness();
                }
            });
        }

        public void Selection()
        {
            // отсортировать по паказателям точности и минимальному значению параметров:
            this.m_models = this.m_models.OrderBy(m => -m.Fitness).ThenBy(m=> m.GetIndices().Count).ToList();

            // отобрать 50% лучших особей для создания новой популяции:
            this.m_bestModels = this.m_models.GetRange(0, this.m_options.PopulationSize / 2);
        }

        public void CreationNewPopulation()
        {
            this.Generation++;
            Random random = new Random();
            List<Model> newModels = new List<Model>(this.Size);

            // использование элитарного отбора:
            if (this.m_options.UseEliteIndividuals)
            {
                for (int i = 0; i < this.m_options.NumberEliteIndividuals; i++)
                {
                    newModels.Add(this.m_bestModels[i]);
                }
            }

            // получение потомков случайным образом:
            while (newModels.Count != this.Size)
            {
                // получение первого родителя:
                Model p1 = this.m_bestModels[random.Next(this.m_bestModels.Count)];

                // получение второго родителя:
                Model p2 = this.m_bestModels[random.Next(this.m_bestModels.Count)];

                // получение новой особи:
                if (!p1.Equals(p2))
                {
                    newModels.Add(p1.CrossBreeding(p2));
                }
            }
            this.m_models = newModels;
            this.m_bestModels.Clear();
        }

        public Double GetAccuracy(AccuracyType type)
        {
            switch (type)
            {
                case AccuracyType.MAX:
                    return this.m_models.Max((Model m) => { return m.Fitness; });
                case AccuracyType.MIN:
                    return this.m_models.Min((Model m) => { return m.Fitness; });
                case AccuracyType.AVG:
                    return this.m_models.Sum((Model m) => { return m.Fitness; }) / this.m_models.Count;
                case AccuracyType.HARM:
                    return this.m_models.Count / this.m_models.Sum((Model m) => { return 1 / m.Fitness; });
                case AccuracyType.RMS:
                    return Math.Sqrt(this.m_models.Sum((Model m) => { return m.Fitness * m.Fitness; }) / this.m_models.Count);
                default:
                    throw new Exception();
            }
        }

        public Int32 GetFeaturesCount(FeaturesCountType type)
        {
            switch (type)
            {
                case FeaturesCountType.MIN:
                    return this.m_models.Min((Model m) => { return m.GetIndices().Count; });
                case FeaturesCountType.MAX:
                    return this.m_models.Max((Model m) => { return m.GetIndices().Count; });
                case FeaturesCountType.AVG:
                    return this.m_models.Sum((Model m) => { return m.GetIndices().Count; }) / this.m_models.Count;
                default:
                    throw new Exception();
            }
        }

        public Model GetBestModel()
        {
            return this.m_models.First();
        }
    }
}
