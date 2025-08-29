using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Order;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.All_Benchmarks
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn(NumeralSystem.Arabic)]
    public class DBCalls
    {
        string connectionString = "Server=.\\SqlExpress; Database=WebAPIProject_In_Dotnet_7; Trusted_Connection=true; TrustServerCertificate=true;";

        [Benchmark]        
        public void PoorlyPerformingQuery()
        {
            string sql = "SELECT * FROM Users WHERE Username = 'Abhay'";

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var command = new SqlCommand(sql, conn);
                var ds = new DataSet();
                var da = new SqlDataAdapter(command);

                da.Fill(ds);
                var result = ds.Tables[0].Rows[0][1];


            }
        }

    }
}
