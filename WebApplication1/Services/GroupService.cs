using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class GroupService : IGroupService
    {
        private readonly AppDbContext _appDbContext;

        public GroupService(AppDbContext context)
        {
            _appDbContext = context;
        }

        public IEnumerable<Group> GetAll(int courseId)
        {
            return _appDbContext.Group.Where(g => g.COURSE_ID == courseId);
        }

        public IEnumerable<Group> GetAllCourseGroupsByGroupId(int groupId)
        {
            var courseId = _appDbContext.Group
            .Where(g => g.GROUP_ID == groupId)
            .Select(g => g.COURSE_ID)
            .FirstOrDefault();

            return GetAll(courseId);
        }

        public Group Get(int groupId)
        {
            return _appDbContext.Group.FirstOrDefault(x => x.GROUP_ID == groupId);
        }

        public void Add(Group group)
        {
            if (string.IsNullOrEmpty(group.NAME))
            {
                throw new InvalidOperationException("Name cannot be empty");
            }

            _appDbContext.Group.Add(group);
            _appDbContext.SaveChanges();
        }

        public void Update(Group group)
        {
            _appDbContext.Group.Update(group);
            _appDbContext.SaveChanges();
        }

        public void Delete(Group group)
        {
            if (_appDbContext.Student.Any(g => g.GROUP_ID == group.GROUP_ID))
            {
                throw new InvalidOperationException("The group cannot be deleted because it has students associated with it.");
            }

            _appDbContext.Group.Remove(group);
            _appDbContext.SaveChanges();
        }

        public bool IsNameExist(string name)
        {
            return _appDbContext.Group.Any(c => c.NAME.Trim().ToLower() == name.Trim().ToLower());
        }
    }
}
