using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var provider = MySqlProvider.Provider.CreateProvider())
            {
                provider.OpenConnection();
                var result = provider.ExecuteSelectCommand("SELECT * FROM world.city;");
                int rowEffectedCount = provider.ExecuteUpdateComand("update world.city set population=1000000 where id =1");
            }
        }
    }
}
