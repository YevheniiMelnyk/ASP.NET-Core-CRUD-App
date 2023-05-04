using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        public IActionResult Groups(int courseId)
        {
            ViewBag.CourseId = courseId;
            var group = _groupService.GetAll(courseId);
            return View(group);
        }

        public IActionResult GroupsByGroupId(int groupId)
        {
            var group = _groupService.GetAllCourseGroupsByGroupId(groupId);
            ViewBag.CourseIdFromStudents = group.FirstOrDefault(c => true)?.COURSE_ID;
            return View("Groups", group);
        }

        [HttpGet]
        public IActionResult CreateGroup(int courseId)
        {
            var model = new Group { COURSE_ID = courseId };
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateGroup(Group group, int courseId)
        {
            if (string.IsNullOrEmpty(group.NAME))
            {
                return View(group);
            }

            if (courseId != 0)
            {
                group.COURSE_ID = courseId;
            }

            if (ModelState.IsValid)
            {
                if (_groupService.IsNameExist(group.NAME))
                {
                    ModelState.AddModelError("NAME", "A group with this name already exists.");
                    return View(group);
                }

                _groupService.Add(group);
                return RedirectToAction("Groups", new { courseId = group.COURSE_ID });
            }
            ViewBag.ErrorMessage = "Invalid COURSE_ID";
            return View(group);
        }

        [HttpGet]
        public IActionResult UpdateGroup(int id)
        {
            Group group = _groupService.Get(id);
            return View(group);
        }

        [HttpPost]
        public IActionResult UpdateGroup(Group group)
        {
            if (ModelState.IsValid)
            {
                if (_groupService.IsNameExist(group.NAME))
                {
                    ModelState.AddModelError("NAME", "A group with this name already exists.");
                    return View(group);
                }

                _groupService.Update(group);
                return RedirectToAction("Groups", new { courseId = group.COURSE_ID });
            }

            return View(group);
        }

        [HttpPost]
        public IActionResult Delete(int id, int courseId)
        {
            try
            {
                _groupService.Delete(new Group { GROUP_ID = id });
                TempData["SuccessMessage"] = "The group has been deleted successfully.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Groups", new { courseId });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}