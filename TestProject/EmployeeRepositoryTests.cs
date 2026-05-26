using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using EmpMng.Applications.Domains;
using EmpMng.Exceptions;
using EmpMng.Infrastructures.Adapters;
using EmpMng.Infrastructures.Context;
using EmpMng.Infrastructures.Repositories;
using Microsoft.Extensions.Logging.Abstractions;

namespace Tests;

[DoNotParallelize]
[TestClass]
public class EmployeeRepositoryTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=csharp_training_202605;Username=postgres;Password=training;";

    private EmployeeRepository _repository = null!;
    private AppDbContext _context = null!;

    [TestInitialize]
    public void Setup()
    {
        var employeeAdapter = new EmployeeEntityAdapter();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        _context = new AppDbContext(options);

        var path = Path.Combine(AppContext.BaseDirectory, "sql", "init.sql");
        var sql = File.ReadAllText(path);
        _context.Database.ExecuteSqlRaw(sql);

        _repository = new EmployeeRepository(_context, employeeAdapter);
    }

    [TestMethod]
    public void Create_WhenCorrect()
    {
        var beforeCount = _context.Employees.Count();

        var employee = new Employee(null, "森鷗外", null, 1);

        _repository.Create(employee);

        var afterCount = _context.Employees.Count();
        AreEqual(beforeCount + 1, afterCount);

        var created = _context.Employees
            .Include(e => e.Department)
            .FirstOrDefault(e => e.EmpName == "森鷗外");

        IsNotNull(created);
    }

    [TestMethod]
    public void FindAll_WhenCorrect()
    {
        var actual = _repository.FindAll();

        AreEqual(8, actual.Count);
        IsTrue(actual.Any(c => c.Equals(new Employee(1, "田中太郎", null, 1))));
        IsTrue(actual.Any(c => c.Equals(new Employee(2, "鈴木三郎", null, 1))));
        IsTrue(actual.Any(c => c.Equals(new Employee(3, "佐藤花子", null, 2))));
        IsTrue(actual.Any(c => c.Equals(new Employee(3, "中田彩子", null, 2))));
        IsTrue(actual.Any(c => c.Equals(new Employee(3, "佐藤圭太", null, 1))));
        IsTrue(actual.Any(c => c.Equals(new Employee(3, "松本良太", null, 1))));
        IsTrue(actual.Any(c => c.Equals(new Employee(3, "山下孝輔", null, 1))));
        IsTrue(actual.Any(c => c.Equals(new Employee(3, "渡辺大輔", null, 1))));
    }
}