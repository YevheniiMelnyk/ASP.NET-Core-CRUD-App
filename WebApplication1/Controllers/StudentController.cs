using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public IActionResult Students(int groupId)
        {
            ViewBag.GroupId = groupId;
            var students = _studentService.GetAll(groupId);
            return View(students);
        }

        [HttpGet]
        public IActionResult CreateStudent(int groupId)
        {
            var model = new Student { GROUP_ID = groupId };
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student, int groupId)
        {
            if (string.IsNullOrEmpty(student.FIRST_NAME) || string.IsNullOrWhiteSpace(student.FIRST_NAME) || string.IsNullOrEmpty(student.LAST_NAME) || string.IsNullOrWhiteSpace(student.LAST_NAME))
            {
                return View(student);
            }

            if (groupId != 0)
            {
                student.GROUP_ID = groupId;
            }

            if (ModelState.IsValid)
            {
                _studentService.Add(student);
                return RedirectToAction("Students", new { groupId = student.GROUP_ID });
            }
            ViewBag.ErrorMessage = "Invalid GROUP_ID";
            return View(student);
        }

        [HttpGet]
        public IActionResult UpdateStudent(int id)
        {
            Student student = _studentService.Get(id);
            return View(student);
        }

        [HttpPost]
        public IActionResult UpdateStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                _studentService.Update(student);
                return RedirectToAction("Students", new { groupId = student.GROUP_ID });
            }

            return View(student);
        }

        [HttpPost]
        public IActionResult Delete(int id, int groupId)
        {
            try
            {
                _studentService.Delete(new Student { STUDENT_ID = id });
                TempData["SuccessMessage"] = "Student has been deleted successfully.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Students", new { groupId });
        }
    }
}
