using Dapper;
using DapperProject.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperProject.Repository
{
    public class BonusRepository : IBonusRepository
    {
        private IDbConnection db;

        public BonusRepository(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public Company GetCompanyWithAddresses(int id)
        {
            var p = new
            {
                CompanyID = id,
            };
            var sql = "Select * from Companies where CompanyId = @CompanyId;" +
                " Select * from Employees where CompanyId = @CompanyId;";

            Company company;

            using (var lists = db.QueryMultiple(sql, new { @CompanyId = id }))
            {
                company = lists.Read<Company>().ToList().FirstOrDefault();
                company.Employees = lists.Read<Employee>().ToList();
            }
            return company;
        }

        public List<Employee> GetEmployeeWithCompany(int id)
        {
            var sql = "select E.*, C.* from Employees as E Inner Join Companies as C on E.CompanyId = C.CompanyId";
            if (id != 0)
            {
                sql += " Where E.CompanyId = @Id";
            }
            // one to one mapping in dapper
            var employee = db.Query<Employee, Company, Employee>(sql, (e, c) =>
            {
                e.Company = c;
                return e;
            }, new { id }, splitOn: "CompanyId").ToList();
            return employee;
        }
    }
}
