using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using EmpMng.Applications.Domains;
using EmpMng.Infrastructures.Adapters;
using EmpMng.Infrastructures.Context;
using EmpMng.Infrastructures.Repositories;
using Microsoft.Extensions.Logging.Abstractions;

namespace Tests;

[DoNotParallelize]
[TestClass]
public class DepartmentRepositoryTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=csharp_training_202605;Username=postgres;Password=training;";

    private DepartmentRepository _repository = null!;
    private AppDbContext _context = null!;

    [TestInitialize]
    public void Setup()
    {
        var departmentAdapter = new DepartmentEntityAdapter();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        _context = new AppDbContext(options);

        var path = Path.Combine(AppContext.BaseDirectory, "sql", "init.sql");
        var sql = File.ReadAllText(path);
        _context.Database.ExecuteSqlRaw(sql);

        _repository = new DepartmentRepository(_context, departmentAdapter);
    }

    [TestMethod]
    public void Create_WhenCorrect()
    {
        var beforeCount = _context.Departments.Count();

        var department = new Department(null, "企画部", 1);

        _repository.Create(department);

        var afterCount = _context.Departments.Count();
        AreEqual(beforeCount + 1, afterCount);

        var created = _context.Departments
            .FirstOrDefault(d => d.DeptName == "企画部");

        IsNotNull(created);
    }
    [TestMethod]
    public void FindAll_WhenCorrect()
    {
        var actual = _repository.FindAll();

        IsTrue(actual.Count >= 5);

        IsTrue(actual.Any(c => c.Name == "総務部" && c.Area == 1));
        IsTrue(actual.Any(c => c.Name == "経理部" && c.Area == 1));
        IsTrue(actual.Any(c => c.Name == "人事部" && c.Area == 1));
        IsTrue(actual.Any(c => c.Name == "開発部" && c.Area == 2));
        IsTrue(actual.Any(c => c.Name == "営業部" && c.Area == 2));

    }
    [TestMethod]
    public void FindById_WhenIdCorrect()
    {
        var expected = new Department(1, "総務部", 1);
        var actual = _repository.FindById(1);

        AreEqual(expected, actual);
        AreEqual("総務部", actual?.Name);
    }

}