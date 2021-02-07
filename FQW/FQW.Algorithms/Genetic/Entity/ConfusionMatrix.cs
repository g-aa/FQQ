using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FQW.Algorithms.Genetic.Entity
{
    public class ConfusionMatrix
    {
        public Int32 TP { get; private set; }
        public Int32 FP { get; private set; }
        public Int32 FN { get; private set; }
        public Int32 TN { get; private set; }

        public ConfusionMatrix(Int32[,] confusionMatrix)
        {
            this.TP = confusionMatrix[0, 0];
            this.FP = confusionMatrix[0, 1];
            this.FN = confusionMatrix[1, 0];
            this.TN = confusionMatrix[1, 1];
        }

        public Double Accuracy { get => ((Double)TP + TN) / (TP + TN + FP + TN); }
        public Double Recall { get => (Double)TP / (TP + FN); }
        public Double Precision { get => (Double)TP / (TP + FP); }
        public Double Specificity { get => (Double)TN / (FP + TN); }
        public Double FallOut { get => (Double)FP / (FP + TN); }
        public Double F1_Score { get => 2 * (Precision * Recall) / (Precision + Recall); }

        public override string ToString()
        {
            String format = "0.0000";
            CultureInfo info = CultureInfo.InvariantCulture;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ConfusionMatrix:");
            sb.AppendLine(String.Format("TP:\t{0}", this.TP));
            sb.AppendLine(String.Format("FP:\t{0}", this.FP));
            sb.AppendLine(String.Format("FN:\t{0}", this.FN));
            sb.AppendLine(String.Format("TN:\t{0}", this.TN));
            sb.AppendLine(String.Format("Accuracy:\t{0}", this.Accuracy.ToString(format, info)));
            sb.AppendLine(String.Format("Recall:\t{0}", this.Recall.ToString(format, info)));
            sb.AppendLine(String.Format("Precision:\t{0}", this.Precision.ToString(format, info)));
            sb.AppendLine(String.Format("Specificity:\t{0}", this.Specificity.ToString(format, info)));
            sb.AppendLine(String.Format("FallOut:\t{0}", this.FallOut.ToString(format, info)));
            sb.AppendLine(String.Format("F1_Score:\t{0}", this.F1_Score.ToString(format, info)));
            return sb.ToString();
        }
    }
}
