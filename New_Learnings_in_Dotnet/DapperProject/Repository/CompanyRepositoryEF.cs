using DapperProject.Data;
using DapperProject.Models;

namespace DapperProject.Repository
{
    public class CompanyRepositoryEF : ICompanyRepository
    {
        private readonly ApplicationDbContext db;

        public CompanyRepositoryEF(ApplicationDbContext _db)
        {
            this.db = _db;
        }

        public Company Add(Company company)
        {
            db.Companies.Add(company);
            db.SaveChanges();
            return company;            
        }

        public Company Find(int id)
        {
            return db.Companies.FirstOrDefault(x => x.CompanyId == id);            
        }

        public List<Company> GetAll()
        {
            return db.Companies.ToList();
        }

        public void Remove(int id)
        {
            Company company = db.Companies.FirstOrDefault(y => y.CompanyId == id);
            db.Companies.Remove(company);
            db.SaveChanges();
            return;
        }

        public Company Update(Company company)
        {
            db.Companies.Update(company);
            db.SaveChanges();
            return company;
        }
    }
}
