using FQW.Algorithms.Genetic.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace FQW.Algorithms.Genetic
{
    public class BestResult
    {
        public String BaseSet { get; set; }
        public String TrainingSet { get; set; }
        public String TestSet { get; set; }
        public Int32 BaseSetFeaturesCount => this.BaseSetFeatures.Count;
        public List<String> BaseSetFeatures { get; set; }
        public Int32 OutFeaturesCount => this.OutFeatures.Count;
        public List<Int32> OutFeaturesIndices { get; set; }
        public List<String> OutFeatures { get; set; }
        public ConfusionMatrix Matrix { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("BaseSet:\t{0}", this.BaseSet)).AppendLine();
            sb.AppendLine(String.Format("TrainingSet:\t{0}", this.TrainingSet)).AppendLine();
            sb.AppendLine(String.Format("TestSet:\t{0}", this.TestSet)).AppendLine();
            sb.AppendLine(String.Format("BaseSetFeaturesCount:\t{0}", this.BaseSetFeaturesCount)).AppendLine();
            sb.AppendLine(String.Format("BaseSetFeatures:\t{0}", String.Join(',', this.BaseSetFeatures))).AppendLine();
            sb.AppendLine(String.Format("OutFeaturesCount:\t{0}", String.Join(',', this.OutFeaturesCount))).AppendLine();
            sb.AppendLine(String.Format("OutFeaturesIndices:\t{0}", String.Join(',', this.OutFeaturesIndices))).AppendLine();
            sb.AppendLine(String.Format("OutFeatures:\t{0}", String.Join(',', this.OutFeatures))).AppendLine();
            sb.AppendLine(this.Matrix.ToString());
            return sb.ToString();
        }
    }
}
