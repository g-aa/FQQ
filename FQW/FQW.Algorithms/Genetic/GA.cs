using FQW.Algorithms.Genetic.Entity;
using FQW.Algorithms.Genetic.Enumeration;
using FQW.Algorithms.Genetic.Interfaces;
using FQW.Algorithms.Genetic.Options;
using RDotNet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;

using System.Threading;

namespace FQW.Algorithms.Genetic
{
    public class GA
    {
        private string fName = "C:\\MyRepository\\Realization\\data\\outputData\\" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss_") + "CalcResult";

        private AlgorithmOptions m_options;

        private REngine m_engine;
        
        private readonly List<IiterationResult> m_statistics;

        private BestResult m_bestResult;

        public GA(AlgorithmOptions options)
        {
            this.m_options = options;
            this.m_statistics = new List<IiterationResult>(options.MaxGenerations);
        }

        public void Solve()
        {
            try
            {
                // инициализация алгоритма:
                this.m_engine = this.Initialization();

                // основной этап работы:
                Population population = new Population(this.m_options, this.m_engine);
                
                // создание первичной популяции:
                population.CreatePrimaryPopulation();

                    Console.WriteLine("Start time: " + DateTime.Now);

                // расчитать приспособленность особей в популяции:
                population.CalculateFitness();

                // запуск ГА:
                for (int i = 0; i < this.m_options.MaxGenerations; i++)
                {
                    // отбор моделей:
                    population.Selection();

                        var s = String.Format("Generations:\t{0}, time:\t{1}, F1_Score(max):\t{2}", i, DateTime.Now, population.GetAccuracy(AccuracyType.MAX));
                        Console.WriteLine(s);
                    
                    // запись статистики по работе ГА:
                    this.m_statistics.Add(this.IiterationResult(population));

                    // проверка на критерий остановки:
                    if (this.m_options.Accuracy < population.GetAccuracy(this.m_options.AccuracyType))
                    {
                        break;
                    }

                    // создание новой популяции моделей:
                    population.CreationNewPopulation();

                    // расчитать приспособленность моделей в популяции:
                    population.CalculateFitness();
                }

                // получить лучший результат:
                Model model = population.GetBestModel();
                this.m_bestResult = new BestResult()
                {
                    BaseSet = this.m_engine.GetSymbol("fileName").AsCharacter().First(),
                    BaseSetFeatures = this.m_engine.GetSymbol("baseSetFeatures").AsCharacter().ToList(),
                    Matrix = model.Matrix,
                    OutFeatures = new List<string>(model.ColumnNames),
                    OutFeaturesIndices = model.GetIndices(),
                    TestSet = this.m_engine.GetSymbol("testSet_fName").AsCharacter().First(),
                    TrainingSet = this.m_engine.GetSymbol("trainSet_fName").AsCharacter().First()
                };
                this.m_bestResult.OutFeatures.Add("Class");
                this.m_bestResult.OutFeaturesIndices.Add(this.m_engine.GetSymbol("classIdx").AsInteger().First());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally 
            {
                // завершающий метод:
                this.Completion(this.m_engine);
            }
        }

        public BestResult GetBestResult()
        {
            return this.m_bestResult;
        }

        public List<IiterationResult> GetStatisticsForAllIterations()
        {
            return this.m_statistics;
        }



        private REngine Initialization()
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();

            StringBuilder sb = new StringBuilder();

            // импорт библиотек:
            sb.AppendLine("library(class)");

            // загрузка исходного набора данных из файла:
            sb.AppendLine("fileName <- file.choose()");
            sb.AppendLine("dataSet <- read.table(fileName, sep = ';', header = TRUE)");

            // получение вектора классов:
            sb.AppendLine("classIdx <- which(names(dataSet)==\"Class\")");
            sb.AppendLine("classes <- dataSet[[\"Class\"]]");
            sb.AppendLine("dataSet[[\"Class\"]] <- NULL");
            sb.AppendLine("featuresCount <- dim(dataSet)[2]");
            sb.AppendLine("baseSetFeatures <- dimnames(dataSet)[[2]]");

            // получение обучающего набора данных:
            sb.AppendLine("trainSet <- dataSet");
            sb.AppendLine("trainSet_fName <- fileName");
            // sb.AppendLine("trainSet_fName <- file.choose()");
            // sb.AppendLine("trainSet <- read.table(trainSet_fName, sep = ',', header = TRUE)");
            // sb.AppendLine("classes <- trainSet[[\"Class\"]]");
            // sb.AppendLine("trainSet[[\"Class\"]] <- NULL");

            // получение тестируемого набора данных:
            sb.AppendLine("testSet <- dataSet");
            sb.AppendLine("testSet_fName <- fileName");
            // sb.AppendLine("testSet_fName <- file.choose()");
            // sb.AppendLine("testSet <- read.table(testSet_fName, sep = ',', header = TRUE)");
            // sb.AppendLine("testSet[[\"Class\"]] <- NULL");

            // запуск на выполнение:
            engine.Evaluate(sb.ToString());

            var ds = engine.GetSymbol("dataSet").AsDataFrame();
            var ds_train = engine.GetSymbol("trainSet").AsDataFrame();
            var ds_test = engine.GetSymbol("testSet").AsDataFrame();

            return engine;
        }

        private void Completion(REngine engine)
        {
            if (engine != null || !engine.Disposed)
            {
                engine.Dispose();
            }
        }

        private IiterationResult IiterationResult(Population population)
        {
            Model model = population.GetBestModel();
            IterationResult result = new IterationResult()
            {
                Generation = population.Generation,
                Model = new IterationResult.BestModel()
                {
                    Accuracy = model.Matrix.Accuracy,
                    F1_Score = model.Matrix.F1_Score,
                    FallOut = model.Matrix.FallOut,
                    FeatureCount = model.GetIndices().Count,
                    ParameterIndices = model.GetIndices().ToArray(),
                    Precision = model.Matrix.Precision,
                    Recall = model.Matrix.Recall,
                    Specificity = model.Matrix.Specificity
                },
                PopulationAccuracy = new IterationResult.Accuracy()
                {
                    MAX_Accuracy = population.GetAccuracy(AccuracyType.MAX),
                    MIN_Accuracy = population.GetAccuracy(AccuracyType.MIN),
                    RMS_Accuracy = population.GetAccuracy(AccuracyType.RMS),
                    AVG_Accuracy = population.GetAccuracy(AccuracyType.AVG),
                    HARM_Accuracy = population.GetAccuracy(AccuracyType.HARM),
                },
                PopulationFeatures = new IterationResult.Features()
                {
                    MAX_Features = population.GetFeaturesCount(FeaturesCountType.MAX),
                    MIN_Features = population.GetFeaturesCount(FeaturesCountType.MIN),
                    AVG_Features = population.GetFeaturesCount(FeaturesCountType.AVG)
                }
            };

            try
            {
                if (!File.Exists(fName + ".IterationAccuracy.txt"))
                {
                    using (StreamWriter sw = File.CreateText(fName + ".IterationAccuracy.txt"))
                    {
                        sw.WriteLine(IterationResult.ResultHeader + IterationResult.Accuracy.AccuracyHeader);
                        sw.WriteLine(result.ToString() + result.PopulationAccuracy.ToString());
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(fName + ".IterationAccuracy.txt"))
                    {
                        sw.WriteLine(result.ToString() + result.PopulationAccuracy.ToString());
                    }
                }

                if (!File.Exists(fName + ".IterationFeatures.txt"))
                {
                    using (StreamWriter sw = File.CreateText(fName + ".IterationFeatures.txt"))
                    {
                        sw.WriteLine(IterationResult.ResultHeader + IterationResult.Features.FeaturesHeader);
                        sw.WriteLine(result.ToString() + result.PopulationFeatures.ToString());
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(fName + ".IterationFeatures.txt"))
                    {
                        sw.WriteLine(result.ToString() + result.PopulationFeatures.ToString());
                    }
                }

                if (!File.Exists(fName + ".IterationBestModel.txt"))
                {
                    using (StreamWriter sw = File.CreateText(fName + ".IterationBestModel.txt"))
                    {
                        sw.WriteLine(IterationResult.ResultHeader + IterationResult.BestModel.BestModelHeader);
                        sw.WriteLine(result.ToString() + result.Model.ToString());
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(fName + ".IterationBestModel.txt"))
                    {
                        sw.WriteLine(result.ToString() + result.Model.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Thread.Sleep(2_000);

            return result;
        }
    }
}
