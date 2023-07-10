using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using DataManipulations.Context;

namespace DataManipulations
{

    public static class DataLoader 
    {
        public static List<TariffPlans> LoadTariffPlans(string path)
        {
            List<TariffPlans> data;
            using (TariffContext db = new(path))
            {
                data = db.TariffPlans.ToList();
            }
            return data;
        }
        public static UserConsumptionData LoadUserData(string path, int ID, string period)
        {
            UserConsumptionData data;
            using (UserContext db = new(path))
            {
                data = db.UserConsumptionData.FindAsync(ID,period).Result;
            }
            return data;
        }

    }
}