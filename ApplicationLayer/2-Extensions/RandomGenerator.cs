namespace ApplicationLayer.Extensions
{
    public static class RandomGenerator
    {
        public static int GenerateRandomSecurityCode() =>
            new Random().Next(111111, 999999);

        public static int GenerateRandomSecurityCode(int length)
            => new Random().Next((int)Math.Pow(10, length - 1), (int)Math.Pow(10, length) - 1);
    }
}