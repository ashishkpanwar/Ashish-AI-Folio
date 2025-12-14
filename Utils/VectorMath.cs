namespace Utils
{
    public static class VectorMath
    {
        public static double CosineSimilarity(
            float[] vectorA,
            float[] vectorB)
        {
            if (vectorA.Length != vectorB.Length)
                throw new ArgumentException("Vectors must be same length");

            double dot = 0.0;
            double magA = 0.0;
            double magB = 0.0;

            for (int i = 0; i < vectorA.Length; i++)
            {
                dot += vectorA[i] * vectorB[i];
                magA += vectorA[i] * vectorA[i];
                magB += vectorB[i] * vectorB[i];
            }

            return dot / (Math.Sqrt(magA) * Math.Sqrt(magB));
        }
    }

}
