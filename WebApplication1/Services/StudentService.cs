using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _appDbContext;

        public StudentService(AppDbContext context)
        {
            _appDbContext = context;
        }

        public IEnumerable<Student> GetAll(int groupId)
        {
            return _appDbContext.Student.Where(g => g.GROUP_ID == groupId);
        }

        public Student Get(int studentId)
        {
            return _appDbContext.Student.FirstOrDefault(x => x.STUDENT_ID == studentId);
        }

        public void Add(Student student)
        {
            if (string.IsNullOrEmpty(student.FIRST_NAME) || string.IsNullOrEmpty(student.LAST_NAME))
            {
                throw new InvalidOperationException("Name cannot be empty");
            }

            _appDbContext.Student.Add(student);
            _appDbContext.SaveChanges();
        }

        public void Update(Student student)
        {
            _appDbContext.Student.Update(student);
            _appDbContext.SaveChanges();
        }

        public void Delete(Student student)
        {
            _appDbContext.Student.Remove(student);
            _appDbContext.SaveChanges();
        }
    }
}
