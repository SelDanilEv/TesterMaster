namespace TesterMaster.Another.Enum
{
    using System;

    public enum TestEnum
    {
        one = 'O',
        two = 'T',
        three = 'H',
    }

    public class TestEnumBehavior
    {
        public TestEnum enumType { get; set; }

        public void PrintBehavior()
        {
            Console.WriteLine(enumType);
        }

    }
}
