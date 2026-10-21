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
    Console.Write("id студента: ");
    int idUser = int.Parse(Console.ReadLine()!);

    bool studentExists = await db.Students.AnyAsync(s => s.Id == idUser);
    if (studentExists == false)
    {
        Console.WriteLine("студент не найден");
        return;
    }

    var student = await db.Students
    .Include(s => s.Group)
    .FirstOrDefaultAsync(s => s.Id ==idUser);

    // 2
    Console.WriteLine($"\nгруппа: {student.Group.Name} id группы: {student.Group.Id}\n");

    // 3
    var groups = await db.Groups.ToListAsync();

    Console.WriteLine("\nвсе доступные группы: ");
    foreach (var gr in groups)
    {
        Console.WriteLine($"id: {gr.Id} название: {gr.Name}");
    }

    // 4
    Console.Write("id новой группы: ");
    int idGroupUser = int.Parse(Console.ReadLine()!);

    // 5
    var groupExists = await db.Groups.AnyAsync(g => g.Id == idGroupUser);
    if (groupExists == false)
    {
        Console.WriteLine("группа не найдена");
        return;
    }

    // 6
    student.GroupId = idGroupUser;

    // 7
    await db.SaveChangesAsync();
    Console.WriteLine($"Студент {student.Id} в группе: {student.Group.Name} id - {student.Group.Id}");
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
}