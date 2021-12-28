using System;
using System.IO;
using TestMasterInfrastructure.Models.Types;

namespace TesterMaster.TestTypes
{
    class TestTypes
    {
        public void Start()
        {
            PrintTypes();
        }

        public void PrintTypes()
        {
            var one = new Class1();
            var two = new Class2();

            Console.WriteLine("Type name 1 " + one.GetType().Name);
            Console.WriteLine("Type name 2 " + two.GetType().Name);

            Console.WriteLine("Type name 1 (version 2) " + typeof(Class1).Name);
            Console.WriteLine("Type name 2 (version 2) " + typeof(Class2).Name);
        }
    }
}