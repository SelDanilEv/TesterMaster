using System;

namespace TesterMaster.Predict
{
    public class PredictNextNumber
    {
        /* predicts random numbers, given 2 state descriptors */
        public double computeDiffAndOffset(double r1, double r2)
        {
            double diff = r1 - r2;

            if (diff == Int32.MaxValue)
                diff = -1 / (double) Int32.MaxValue;
            if (diff < 0)
                return diff + 1;
            else
                return diff;
        }

        public void Start()
        {
            /* this we break */
            Random r = new Random();

            /* describes the state of the subtractive generator */
            double[] SeedArray = new double[56];

            /* leaking the state by observing the first 55 random numbers */
            for (int i = 1; i < 56; i++)
                SeedArray[i] = r.NextDouble();

            /* the offset is known from the original implementation */
            int offset = 21;

            /* from the theory part: i = index1, j = index2 */
            int index1 = 1, index2 = index1 + offset;

            /* running a few tests */
            for (int i = 0; i < 1000; i++)
            {
                /* handling the circular array limits */
                if (index1 >= 56)
                    index1 = 1;

                if (index2 >= 56)
                    index2 = 1;

                /* this is the predicted random number */
                double predictedValue = computeDiffAndOffset(SeedArray[index1], SeedArray[index2]);

                /* this is the correct random number */
                double correctRandom = r.NextDouble();

                /* we compare them as doubles */
                if (Math.Abs(predictedValue - correctRandom) > 0.00001)
                    throw new Exception(String.Format("Failed at {0} vs {1}", predictedValue, correctRandom));

                /* printing the results */
                Console.WriteLine("Predicted: " + predictedValue + " | Correct: " + correctRandom);

                /* updating the state of the generator */
                SeedArray[index1] = predictedValue;

                index1++;
                index2++;
            }
        }
    }
}