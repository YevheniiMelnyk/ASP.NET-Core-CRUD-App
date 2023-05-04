using WebApplication1.Models;

namespace WebApplication1.Services.Interfaces
{
    public interface IStudentService
    {
        IEnumerable<Student> GetAll(int groupId);

        Student Get(int studentId);

        void Add(Student student);

        void Update(Student student);

        void Delete(Student student);
    }
}
