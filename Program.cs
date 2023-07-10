using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Models;
namespace utilities_calculation_console_app
{
    class Program
    {
        static void Main(string[] args)
        {
            UserCunsumptionData userProcessed = new();
            userProcessed.ID = 1111;
            Dictionary<string, double> prices = new();
            Console.WriteLine("Введите test, чтобы изменить месяц передачи показаний.");
            if (Console.ReadLine().Trim() == "test")
            {
                Console.WriteLine("Введите период в формате mm/yyyy, поумолчанию берётся текущий месяц и год.");
                string inputDate = userProcessed.Period = Console.ReadLine().Trim();
                ServicesCalculator.PreviousPeriod = $"{Convert.ToDateTime(inputDate).Month - 1}/{Convert.ToDateTime(inputDate).Year}";
            }
            else
            {
                userProcessed.Period = $"{DateTime.Now.Month}/{DateTime.Now.Year}";
                ServicesCalculator.PreviousPeriod = $"{DateTime.Now.Month - 1}/{DateTime.Now.Year}";
            }
            int persons = 0;
            do
            {
                Console.WriteLine("Введите количество проживающих.");
                string temp = Console.ReadLine().Trim();
                int.TryParse(temp, out persons);
            } while (persons < 1);
            userProcessed.Persons = persons;
            #region Рассчёт ХВС
        CWS:
            string input;
            ReadServiceInput("ХВС", out input);
            double CWSMeters = Convert.ToDouble(input);
            (userProcessed.CWSCunsumption, prices["ХВС"]) = ServicesCalculator.CalculateCWS(persons, CWSMeters);
            Console.WriteLine($"Ваш расход составил {userProcessed.CWSCunsumption}, если всё верно, введите 1.");
            if (CheckInputKeyFailed()) goto CWS;
            #endregion

            #region Рассчёт ГВС
            HWS:
            input = "";
            ReadServiceInput("ГВС", out input);
            //Console.WriteLine("Введите показания ГВС, если прибор учёта не установлен, введите -1");
            double HWSMeters = Convert.ToDouble(input);
            ((userProcessed.HWSCCunsumption, prices["ГВС теплоноситель"]), (userProcessed.HWSTECunsumption, prices["ГВС нагрев"])) = ServicesCalculator.CalculateHWS(persons, HWSMeters);
            Console.WriteLine($"Ваш расход теплоносителя составил {userProcessed.HWSCCunsumption},\n" +
                $"Расход теплоэнергии составил {userProcessed.HWSTECunsumption} если всё верно, введите 1.");
            if (CheckInputKeyFailed()) goto HWS;
            #endregion

            #region Рассчёт Электроэнергии
            E:
            Console.WriteLine("Введите показания электроэнергии дневной тариф, если прибор учёта не установлен, введите -1");
            string[] inputs = new string[2];
            inputs[0] = Console.ReadLine().Trim();
            if (inputs[0].Contains('-'))
            {
                double EMeters = Convert.ToInt32(inputs[0]);
                (userProcessed.E, prices["Электроэнергия"]) = ServicesCalculator.CalculateEStandart(persons, EMeters);
                userProcessed.EN = userProcessed.ED = default;
                Console.WriteLine($"Ваш расход электроэенергии составил {userProcessed.E}, если всё верно, введите 1.");
                if (CheckInputKeyFailed()) goto E;
            }
            else
            {
                Console.WriteLine("Введите показания электроэнергии ночной тариф.");
                inputs[1] = Console.ReadLine().Trim();
                double day = double.Parse(inputs[0]);
                double night = double.Parse(inputs[1]);
                userProcessed.E = default;
                ((userProcessed.ED, prices["Электроэнергия день"]), (userProcessed.EN, prices["Электроэнергия ночь"])) = ServicesCalculator.CalculateEDN(day, night);
                Console.WriteLine($"Ваш расход электроэнергии день составил {userProcessed.ED},\n" +
                    $"расход электроэнергии ночь составил {userProcessed.EN}, если всё верно, введите 1.");
                if (CheckInputKeyFailed()) goto E;
            }
            #endregion

            //Вывод цен и суммы
            Console.WriteLine(new string('-', 20));
            Console.WriteLine($"Стоимость услуг за {userProcessed.Period} составила:");
            foreach (var item in prices)
            {
                Console.WriteLine($"{item.Key} - {item.Value:f4}руб");
            }
            Console.WriteLine(new string('-', 20));
            Console.WriteLine($"Итоговая сумма составила: {prices.Values.Sum()}руб");

            //Сохранение данных
            if (DataManipulations.DataSaver.SaveUserData("DataBases\\userData.db", userProcessed))
                Console.WriteLine("Показания сохранены.");
            else Console.WriteLine("Ошибка сохранения.");
        }
        private static bool CheckInputKeyFailed()
        {
            ConsoleKey inputKey = Console.ReadKey(true).Key;
            return (!(inputKey == ConsoleKey.D1 ||
                        inputKey == ConsoleKey.NumPad1));
        }
        private static bool MetersCheck(string input)
        {
            double temp;
            if (double.TryParse(input.Replace(',', '.'), NumberStyles.Any,
                CultureInfo.InvariantCulture, out temp) && temp >= -1)
                return true;
            else return false;
        }
        private static void ReadServiceInput(string service, out string input)
        {
            do
            {
                Console.WriteLine($"Введите показания {service}, если прибор учёта не установлен, введите -1.");
                input = Console.ReadLine().Trim();
                if (!MetersCheck(input)) Console.WriteLine("Ошибка ввода.");
            }
            while (!MetersCheck(input));
        }
    }

}
