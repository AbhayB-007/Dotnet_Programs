﻿using DapperProject.Models;

namespace DapperProject.Repository
{
    public interface IEmployeeRepository
    {
        Employee Find(int id);
        List<Employee> GetAll();
        Employee Add(Employee company);
        Employee Update(Employee company);
        void Remove(int id);
    }
}
