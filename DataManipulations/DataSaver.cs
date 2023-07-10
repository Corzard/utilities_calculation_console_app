using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using DataManipulations.Context;

namespace DataManipulations
{

    public static class DataSaver 
    {
        public static bool SaveUserData(string path, UserConsumptionData userData)
        {
            using (UserContext db = new(path))
            {
                try
                {
                    db.UserConsumptionData.AddAsync(userData);
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