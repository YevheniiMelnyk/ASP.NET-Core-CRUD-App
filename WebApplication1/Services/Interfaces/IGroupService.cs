using WebApplication1.Models;

namespace WebApplication1.Services.Interfaces
{
    public interface IGroupService
    {
        IEnumerable<Group> GetAll(int courseId);

        IEnumerable<Group> GetAllCourseGroupsByGroupId(int groupId);

        Group Get(int groupId);

        void Add(Group group);

        void Update(Group group);

        void Delete(Group group);

        bool IsNameExist(string name);
    }
}
