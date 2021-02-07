using FQW.Algorithms.Genetic;
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

namespace FQW.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Application is run!");

            string txt =
                "D27,D66,D106,D14,D8,D18,D16,D88,D10,D2,D6,D201,D15,D19,D9,D95,D200,D7,D78,D469,D5,D104,D103,D747,D146,D21,D87,D105,D20,D17,D91,D607,D196,D26,D107,D69,D173,D84,D99,D47,D30,D86,D911,D74,D25,D46,D90,D89,D100,D102,D32,D660,D739,D175,D64,D61,D31,D11,D80,D218,D207,D177,D181,D204,D659,D48,D131,D45,D65,D182,D71,D70,D39,D38,D67,D199,D76,D52,D68,D208,D83,D101,D951,D33,D60,D202,D126,D55,D180,D3,D152,D49,D75,D56,D63,D139,D198,D130,D1036,D217,D145,D595,D62,D73,D214,D209,D947,D187,D158,D53,D43,D164,D54,D211,D162,D58,D57,D979,D44,D504,D194,D741,D505,D927,D205,D688,D36,D142,D96,Class";

            string[] titles = txt.Split(',');


            string data_source = "C:\\Users\\Eridanus e\\Desktop\\res2\\phpSSK7iA_15%.csv";
            string data_result = "C:\\Users\\Eridanus e\\Desktop\\res2\\RF_best_2.csv";

            using (StreamWriter sw = new StreamWriter(data_result))
            {
                Boolean flag = true;
                List<int> idxs = new List<int>(titles.Length);

                StringBuilder sb = new StringBuilder();
                using (StreamReader sr = new StreamReader(data_source))
                {
                    string str = null;
                    while ((str = sr.ReadLine()) != null)
                    {
                        string[] items = str.Split(';');
                        if (flag)
                        {
                            var k = items.Where((s, i) =>
                            {
                                if (titles.Contains(s))
                                {
                                    idxs.Add(i);
                                }
                                return titles.Contains(s);
                            });
                            sb.AppendJoin(';', k).AppendLine();
                            flag = false;
                        }
                        else
                        {
                            sb.AppendJoin(';', items.Where((s, i) => idxs.Contains(i)).ToArray()).AppendLine();
                        }
                    }
                }
                sw.WriteLine(sb);
            }

            try
            {
                Program.GeneticFunction();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR:\t" + ex.Message);
            }

            Console.WriteLine("Press any key for  exit...");
            Console.ReadKey();
        }

        static void GeneticFunction()
        {
            AlgorithmOptions options = new AlgorithmOptions()
            {
                Accuracy = 0.950,
                AccuracyType = AccuracyType.MAX,
                AdaptiveCrossover = true,
                IncestControl = true,
                MaxGenerations = 1500,
                ModelType = ModelType.KNN,
                NumberEliteIndividuals = 5,
                PGene = 0.70,
                PMutationMax = 0.30,
                PMutationMin = 0.15,
                PopulationSize = 30,
                UseEliteIndividuals = true
            };

            GA algorithm = new GA(options);

            // запуск ГА:
            algorithm.Solve();

            // получить результат:
            BestResult result = algorithm.GetBestResult();

            // запись результатов работы в файл:
            Program.SaveResult(options, algorithm.GetStatisticsForAllIterations(), result);
        }

        static void SaveResult(AlgorithmOptions options, List<IiterationResult> statistics, BestResult bestModel)
        {
            string fName = "C:\\MyRepository\\Realization\\data\\outputData\\" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss_") + "CalcResult";

            // статистика точности работы ГА:
            StringBuilder sbAccuracy = new StringBuilder();
            sbAccuracy.AppendLine(IterationResult.ResultHeader + IterationResult.Accuracy.AccuracyHeader);

            // статистика отбора параметров:
            StringBuilder sbFeatures = new StringBuilder();
            sbFeatures.AppendLine(IterationResult.ResultHeader + IterationResult.Features.FeaturesHeader);

            // статистика параметров лучшей модели:
            StringBuilder sbModel = new StringBuilder();
            sbModel.AppendLine(IterationResult.ResultHeader + IterationResult.BestModel.BestModelHeader);

            foreach (IiterationResult item in statistics)
            {
                IterationResult s = item as IterationResult;
                sbAccuracy.AppendLine(s.ToString() + s.PopulationAccuracy.ToString());
                sbModel.AppendLine(s.ToString() + s.Model.ToString());
                sbFeatures.AppendLine(s.ToString() + s.PopulationFeatures.ToString());
            }

            using (StreamWriter sw = new StreamWriter(fName + ".Options.txt"))
            {
                sw.WriteLine(options.ToString());
            }
            using (StreamWriter sw = new StreamWriter(fName + ".IterationAccuracy.txt"))
            {
                sw.WriteLine(sbAccuracy);
            }
            using (StreamWriter sw = new StreamWriter(fName + ".IterationBestModel.txt"))
            {
                sw.WriteLine(sbModel);
            }
            using (StreamWriter sw = new StreamWriter(fName + ".IterationFeatures.txt"))
            {
                sw.WriteLine(sbFeatures);
            }
            using (StreamWriter sw = new StreamWriter(fName + ".FinalResult.txt"))
            {
                sw.WriteLine(bestModel.ToString());
            }
            using (StreamWriter sw = new StreamWriter(fName + ".ResultSet.csv"))
            {
                StringBuilder sb = new StringBuilder();
                using (StreamReader sr = new StreamReader(bestModel.BaseSet))
                {
                    string str = null;
                    while ((str = sr.ReadLine()) != null)
                    {
                        string[] resSh = str.Split(';').Where((s, i) => bestModel.OutFeaturesIndices.Contains(i + 1)).ToArray();
                        sb.AppendJoin(';', resSh).AppendLine();
                    }
                }
                sw.WriteLine(sb);
            }
        }
    }
}
