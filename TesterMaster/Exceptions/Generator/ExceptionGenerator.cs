namespace TesterMaster.Exceptions.Generator
{
    using System;

    public static class ExceptionGenerator
    {
        private static int _chanceOfError = 50;

        public static void SetChanceOfError(int value)
        { _chanceOfError = value; }

        public static void ThrowRandomException()
        {
            var random = new Random().Next(0, 100);

            if (random < _chanceOfError)
            {
                Console.WriteLine("Error! Throwing Exception");
                throw new Exception("Random exception");
            }
        }
    }
}
