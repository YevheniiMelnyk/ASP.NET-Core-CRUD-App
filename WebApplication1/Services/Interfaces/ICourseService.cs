using WebApplication1.Models;

namespace WebApplication1.Services.Interfaces
{
    public interface ICourseService
    {
        IEnumerable<Course> GetAll();

        Course Get(int courseId);

        void Add(Course course);

        void Update(Course course);

        void Delete(Course course);

        IEnumerable<Group> GetAllByCourseId(int courseId);

        bool IsNameExist(string name);
    }
}
