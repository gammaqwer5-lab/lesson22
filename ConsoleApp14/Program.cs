using ConsoleApp14;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Не найден Default");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

using var host = builder.Build();
using var scope = host.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
 
try
{
    // 1
    var students = await db.Students
     .Include(s => s.Group)
     .OrderBy(s => s.LastName)
     .ToListAsync();

    Console.WriteLine("\nвсе студенты: ");
    foreach (var st in students)
    {
        Console.WriteLine($"{st.FirstName} {st.LastName} - {st.Group.Name}");
    }

    // 2
    Console.WriteLine("\nдобавление студента\n");
    var newStudent = new Student
    {
        FirstName = "Александр",
        LastName = "Кузнецов",
        Age = 22,
        GroupId = 3
    };
    db.Students.Add(newStudent);
    await db.SaveChangesAsync();

    Console.WriteLine($"{newStudent.FirstName} {newStudent.LastName} - {newStudent.Group.Name}");

    // 3
    Console.WriteLine("\nизменение возраста\n");
    var updateStudent = await db.Students
    .FirstAsync(s => s.Id == 6);

    updateStudent.Age = 22;
    await db.SaveChangesAsync();

    // 4
    Console.WriteLine("\nудаление студента\n");
    var delStudent = await db.Students
    .FirstAsync(s => s.Id == 9);

    db.Students.Remove(delStudent);
    await db.SaveChangesAsync();

    // 5
    var newStudentsBD = await db.Students
     .Include(s => s.Group)
     .OrderBy(s => s.LastName)
     .ToListAsync();

    Console.WriteLine("\nвсе студенты после обновления: ");
    foreach (var st in newStudentsBD)
    {
        Console.WriteLine($"{st.FirstName} {st.LastName} - {st.Group.Name}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
}