using System;
using System.Collections.Generic;
using TestMasterInfrastructure.Interfaces;

namespace TesterMaster.Dictionary
{
    public class TestDictionary : ITest
    {
        private readonly Dictionary<string, string> _emmsSubstitutionFieldsMapping = new Dictionary<string, string>()
        {
            { "local_access_number", "local_access_number"},
            { "plan_expiration", "enddate"},
            { "customer_support_number", "customer_support_number"},
            { "combo_transaction_id", "transaction_id"},
            { "recipient_number", "recipient_number"},
            { "card_pin", "cc_num"},
            { "confirmation_number", "confirmation number"}
        };

        public void StartTest()
        {
            TestValueAccessors();
        }

        private void TestValueAccessors()
        {
            var correctValue = _emmsSubstitutionFieldsMapping["card_pin"];
            Console.WriteLine($"CorrectValue: {correctValue}");

            try
            {
                var incorrectValue = _emmsSubstitutionFieldsMapping["card_pin123"];
                Console.WriteLine($"IncorrectValue: {incorrectValue}");
            }
            catch { }

            _emmsSubstitutionFieldsMapping.TryGetValue("card_pin123",out var tryValue);
            tryValue = tryValue ?? "card_pin123";
            Console.WriteLine($"TryValue: {tryValue}");
        }
    }
}
