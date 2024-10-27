using FinPal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FinPal.Data
{
    public class SalaryDatabase : BaseDatabase<Salary>
    {
        public async Task<Salary> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<Salary>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateItemAsync(Salary item)
        {
            return await Database.UpdateAsync(item);
        }

        public async Task<bool> CheckIfMonthAndYearExistAsync(int year, int month)
        {
            await Init();
            var existingItem = await Database.Table<Salary>()
                .Where(x => x.Month == month && x.Year == year)
                .FirstOrDefaultAsync();

            return existingItem != null;  // Returns true if the month exists
        }

        public async Task<List<Salary>> GetSalariesByYear(int year)
        {
            await Init();

            if(!await CheckIfMonthAndYearExistAsync(year, 1))
            {
                await CreateNewYear(year);
            }

            return await Database.Table<Salary>()
                                  .Where(s => s.Year == year && s.Active)
                                  .ToListAsync();
        }

        public async Task<decimal> GetSalaryByMonth(int year, int month)
        {
            await Init();

            var salaries = await Database.Table<Salary>().Where(x => x.Year == year && x.Month == month && x.Active).ToListAsync();

            return salaries.FirstOrDefault().Amount;
        }

        public async Task<decimal> SumSalariesByYear(int year)
        {
            await Init();

            var salaries = await GetSalariesByYear(year);

            return salaries.Sum(x => x.Amount);
        }

        public async Task CreateNewYear(int year)
        {
            await Init();

            var salaries = new List<Salary>();

            for (int month = 1; month <= 12; month++)
            {
                salaries.Add(new Salary { Month = month, Year = year, Amount = 0, Active = true });
            }

            foreach (var salary in salaries)
            {
                await SaveItemAsync(salary);
            }

        }
    }
}
