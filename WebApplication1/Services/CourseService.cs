using WebApplication1.Services.Interfaces;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _appDbContext;

        public CourseService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Course> GetAll()
        {
            return _appDbContext.Course;
        }

        public Course Get(int courseId)
        {
            return _appDbContext.Course.FirstOrDefault(x => x.COURSE_ID == courseId);
        }

        public void Add(Course course)
        {
            _appDbContext.Course.Add(course);
            _appDbContext.SaveChanges();
        }

        public void Update(Course course)
        {
            _appDbContext.Course.Update(course);
            _appDbContext.SaveChanges();
        }

        public void Delete(Course course)
        {
            if (_appDbContext.Group.Any(g => g.COURSE_ID == course.COURSE_ID))
            {
                throw new InvalidOperationException("The course cannot be deleted because it has Group associated with it.");
            }

            _appDbContext.Course.Remove(course);
            _appDbContext.SaveChanges();
        }

        public IEnumerable<Group> GetAllByCourseId(int courseId)
        {
            return _appDbContext.Group.Where(g => g.COURSE_ID == courseId).ToList();
        }

        public bool IsNameExist(string name)
        {
            return _appDbContext.Course.Any(c => c.NAME.Trim().ToLower() == name.Trim().ToLower());
        }
    }
}
