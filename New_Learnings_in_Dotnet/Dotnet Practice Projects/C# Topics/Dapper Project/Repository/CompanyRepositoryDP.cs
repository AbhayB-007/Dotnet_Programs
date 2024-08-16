using Dapper;
using DapperProject.Data;
using DapperProject.Models;
using Microsoft.Data.SqlClient;
using System.ComponentModel.Design;
using System.Data;

namespace DapperProject.Repository
{
    public class CompanyRepositoryDP : ICompanyRepository
    {
        private readonly IDbConnection db;

        public CompanyRepositoryDP(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public Company Add(Company company)
        {
            var sql = "INSERT INTO Companies(Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode);"
                + "SELECT CAST(SCOPE_IDENTITY() as int);";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter{ParameterName = "@Name", Value = company.Name},
                new SqlParameter{ParameterName = "@Address", Value = company.Address},
                new SqlParameter{ParameterName = "@City", Value = company.City},
                new SqlParameter{ParameterName = "@State", Value = company.State},
                new SqlParameter{ParameterName = "@PostalCode", Value = company.PostalCode}
            };

            // using dynamic parameters
            company.CompanyId = db.Query<int>(sql, new
            {
                company.Name,
                company.Address,
                company.City,
                company.State,
                company.PostalCode
            }).Single();

            //// using SqlParameters
            //company.CompanyId = db.Query<int>(sql, parameters).Single();
            return company;
        }

        public Company Find(int id)
        {
            var sql = "select * from Companies where CompanyId = @CompanyId";
            return db.Query<Company>(sql, new { @CompanyId = id }).Single();

            //var sql = "select * from Companies where CompanyId = @id";
            //return db.Query<Company>(sql, new { id }).Single();
        }

        public List<Company> GetAll()
        {
            var sql = "select * from Companies";
            return db.Query<Company>(sql).ToList();
        }

        public void Remove(int id)
        {
            var sql = "delete from Companies where CompanyId = @CompanyId";
            db.Execute(sql, new { @CompanyId = id});
            //db.Query(sql, new { @CompanyId = id });
        }

        public Company Update(Company company)
        {
            var sql = "update Companies set Name = @Name, Address = @Address, City = @City," +
                "State = @State, PostalCode = @PostalCode where Compan" +
                "yId = @CompanyId";
            db.Execute(sql, company);
            return company;
        }
    }
}
