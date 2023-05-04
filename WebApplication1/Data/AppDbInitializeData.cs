using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class AppDbInitializeData
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                context.Database.EnsureCreated();

               
                if (!context.Course.Any())
                {
                    var courses = new List<Course>
                    {
                        new Course {NAME = "C#", DESCRIPTION = "C# course"},
                        new Course {NAME = "Javascript", DESCRIPTION = "Javascript course"},
                        new Course {NAME = "Java", DESCRIPTION = "Java course"},
                        new Course {NAME = "Python", DESCRIPTION = "Python course"},
                        new Course {NAME = "C++", DESCRIPTION = "C++ course"},
                        new Course {NAME = "C", DESCRIPTION = "C course"},
                        new Course {NAME = "PHP", DESCRIPTION = "PHP course"},
                        new Course {NAME = "Ruby", DESCRIPTION = "Ruby course"},
                        new Course {NAME = "Rust", DESCRIPTION = "Rust course"},
                        new Course {NAME = "Go", DESCRIPTION = "Go course"},
                    };
                    context.Course.AddRange(courses);
                    context.SaveChanges();

                }

                
                if (!context.Group.Any())
                {
                    var random = new Random();
                    var courses = context.Course.ToList();

                    for (int i = 0; i < 20; i++)
                    {
                        var group = new Group
                        {
                            COURSE_ID = courses[random.Next(0, courses.Count)].COURSE_ID,
                            NAME = $"Group {i + 1}"
                        };
                        context.Group.Add(group);
                    }
                    context.SaveChanges();
                }

                
                if (!context.Student.Any())
                {
                    var random = new Random();
                    var groups = context.Group.ToList();

                    var firstNames = new string[] { "John", "Mary", "David", "Sarah", "Michael", "Jennifer", "Christopher", "Jessica", "Matthew", "Elizabeth" };
                    var lastNames = new string[] { "Smith", "Johnson", "Brown", "Taylor", "Anderson", "Wilson", "Jones", "Miller", "Davis", "Garcia" };

                    for (int i = 0; i < 100; i++)
                    {
                        var student = new Student
                        {
                            GROUP_ID = groups[random.Next(1, 20)].GROUP_ID, 
                            FIRST_NAME = firstNames[random.Next(0, firstNames.Length)], 
                            LAST_NAME = lastNames[random.Next(0, lastNames.Length)] 
                        };
                        context.Student.Add(student);
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
