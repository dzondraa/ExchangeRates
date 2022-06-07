using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRatesTests
{
    public class RandomDateFactory
    {
        DateTime start;
        Random gen;
        int range;

        public RandomDateFactory()
        {
            start = new DateTime(2020, 1, 1);
            gen = new Random();
            range = (DateTime.Today - start).Days;
        }

        private DateTime Next()
        {
            return start.AddDays(gen.Next(range)).AddHours(gen.Next(0, 24)).AddMinutes(gen.Next(0, 60)).AddSeconds(gen.Next(0, 60));
        }

        public IEnumerable<DateTime> GetNRandomDates(int n)
        {
            var list = new List<DateTime>();
            for (int i = 0; i < n; i++)
            {
                list.Add(this.Next());
            }
            return list;
        }
    }
}
