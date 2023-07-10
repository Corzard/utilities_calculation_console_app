using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using DataManipulations;

namespace utilities_calculation_console_app
{
    public static class ServicesCalculator
    {
        static readonly List<TariffPlans> plans;
        static readonly UserConsumptionData previousPeriod;
        static public string PreviousPeriod { get; set; }
        static ServicesCalculator()
        {
            plans = DataLoader.LoadTariffPlans("DataBases\\tariff_plans.db");
            previousPeriod = DataLoader.LoadUserData("DataBases\\userData.db", 1111, PreviousPeriod);
        }

        static private double DefaultCalculation(double Volume, double Tariff)
        {
            return Volume * Tariff;
        }
        static private double VolumeByhMetersData(double MCurrent, double MPrevious)
        {
            return MCurrent - MPrevious;
        }
        static private double VolumeByStandard(int NumberOfPersons, double Standard)
        {
            return NumberOfPersons * Standard;
        }

        /// <summary>
        /// Вычисляет объём и стоимость потребления услуги ХВС
        /// </summary>
        /// <param name="NumberOfPersons">Количество проживающих</param>
        /// <param name="MetersData">Показания счетчиков ХВС</param>
        /// <returns>Объём и стоимость услуги ХВС</returns>
        static public (double, double) CalculateCWS(int NumberOfPersons, double MetersData)
        {
            TariffPlans CWS = plans.Find(item => item.Service == "CWS");
            double cost;
            double volume;
            if (MetersData == -1)
            {
                MetersData = CWS.Standard;
                volume = VolumeByStandard(NumberOfPersons, MetersData);
                cost = DefaultCalculation(volume, CWS.Tariff);
            }
            else
            {
                //Load previous data;
                double previousMonth = previousPeriod == null ? 0 : previousPeriod.CWSConsumption;
                volume = VolumeByhMetersData(MetersData, previousMonth);
                cost = DefaultCalculation(volume, CWS.Tariff);
            }

            return (volume, cost);

        }

        /// <summary>
        /// Вычисляет объём и стоимость потребления услуги ГВС
        /// </summary>
        /// <param name="NumberOfPersons">Количество проживающих</param>
        /// <param name="MetersData">Показания счетчиков ГВС</param>
        /// <returns>Объём и стоимость услуги ГВС теплоноситель и теплоэнергия</returns>
        static public ((double, double), (double, double)) CalculateHWS(int NumberOfPersons, double MetersData)
        {
            TariffPlans HWSC = plans.Find(p => p.Service == "HWSC");
            TariffPlans HWSTE = plans.Find(p => p.Service == "HWSTE");
            double coolantVolume;
            double TEVolume;
            double coolantCost;
            double TECost;
            if (MetersData == -1)
            {
                MetersData = HWSC.Standard;
                coolantVolume = VolumeByStandard(NumberOfPersons, MetersData);
                coolantCost = DefaultCalculation(coolantVolume, HWSC.Tariff);
                TEVolume = coolantVolume * HWSTE.Standard;
                TECost = DefaultCalculation(TEVolume, HWSTE.Tariff);
            }
            else
            {
                double previousMonthС = previousPeriod == null ? 0 : previousPeriod.CWSConsumption;

                coolantVolume = VolumeByhMetersData(MetersData, previousMonthС);
                coolantCost = DefaultCalculation(coolantVolume, HWSC.Tariff);
                TEVolume = coolantVolume * HWSTE.Standard;
                TECost = DefaultCalculation(TEVolume, HWSTE.Tariff);

            }
            return ((coolantVolume, coolantCost), (TEVolume, TECost));
        }

        /// <summary>
        /// Вычисляет объём и стоимость потребления услуги электроэнергии
        /// </summary>
        /// <param name="NumberOfPersons">Количество проживающих</param>
        /// <param name="MetersData">Показания счетчиков ГВС</param>
        /// <returns>Объём и стоимость услуги электроэнергии</returns>
        static public (double, double) CalculateEStandard(int NumberOfPersons, double MetersData)
        {
            TariffPlans E = plans.Find(item => item.Service == "E");
            double volume;
            double cost;
            MetersData = E.Standard;
            volume = VolumeByStandard(NumberOfPersons, MetersData);
            cost = DefaultCalculation(volume, E.Tariff);
            return (volume, cost);
        }
        /// <summary>
        /// Вычисляет объём и стоимость потребления услуги электроэнергии по жвойному тарифу
        /// </summary>
        /// <param name="MetersDataDay">Показания счётчиков: дневной тариф</param>
        /// <param name="MetersDataNight">Показания счетчиков: ночной тариф</param>
        /// <returns>Объём и стоимость услуги электроэнергии по двум тарифам</returns>
        static public ((double, double), (double, double)) CalculateEDN(double MetersDataDay, double MetersDataNight)
        {
            TariffPlans ED = plans.Find(p => p.Service == "ED");
            TariffPlans EN = plans.Find(p => p.Service == "EN");
            double DayVolume;
            double NightVolume;
            double DayCost;
            double NightCost;
            //Load previous data;
            double previousMonthED = previousPeriod == null ? 0 : previousPeriod.ED;
            double previousMonthEN = previousPeriod == null ? 0 : previousPeriod.EN;

            DayVolume = VolumeByhMetersData(MetersDataDay, previousMonthED);
            NightVolume = VolumeByhMetersData(MetersDataNight, previousMonthEN);
            DayCost = DefaultCalculation(DayVolume, ED.Tariff);
            NightCost = DefaultCalculation(NightVolume, EN.Tariff);

            return ((DayVolume, DayCost), (NightVolume, NightCost));

        }
    }

}
