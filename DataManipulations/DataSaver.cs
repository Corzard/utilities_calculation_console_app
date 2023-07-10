using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using DataManipulations.Context;

namespace DataManipulations
{

    public static class DataSaver 
    {
        public static bool SaveUserData(string path, UserCunsumptionData userData)
        {
            //List<TariffPlans> data;
            using (UserContext db = new(path))
            {
                try
                {
                    db.UserCunsumptionData.AddAsync(userData);
                    db.SaveChanges();
                    return true;
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException)
                {
                    try
                    {
                        db.Update(userData);
                        db.SaveChanges();
                        return true;
                    }
                    catch(Exception innerEx)
                    {
                        Console.WriteLine(innerEx.Message);
                        return false;
                    }
                    
                }
                
            }
        }
    }
}