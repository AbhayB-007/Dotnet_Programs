using DapperProject.Models;

namespace DapperProject.Repository
{
    public interface IBonusRepository
    {
        List<Employee> GetEmployeeWithCompany(int id);
        Company GetCompanyWithAddresses(int id);
    }
}
