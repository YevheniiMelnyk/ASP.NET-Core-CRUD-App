using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseService _courseService;

        public HomeController(ICourseService courseService)
        {
            _courseService = courseService;
        }
                
        public IActionResult Index()
        {
            var course = _courseService.GetAll();
            return View(course);
        }
                
        public IActionResult CreateCourse()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCourse(Course course)
        {
            if (ModelState.IsValid)
            {
                if (_courseService.IsNameExist(course.NAME))
                {
                    ModelState.AddModelError("NAME", "A course with this name already exists.");
                    return View(course);
                }

                _courseService.Add(course);
                return RedirectToAction("Index");
            }
            return View(course);
        }

        public IActionResult UpdateCourse(int id)
        {
            Course course = _courseService.Get(id);
            return View(course);
        }

        [HttpPost]
        public IActionResult UpdateCourse(Course course)
        {
            if (ModelState.IsValid)
            {
                if (_courseService.IsNameExist(course.NAME))
                {
                    ModelState.AddModelError("NAME", "A course with this name already exists.");
                    return View(course);
                }

                _courseService.Update(course);
                return RedirectToAction("Index");
            }

            return View(course);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _courseService.Delete((new Course { COURSE_ID = id }));
                TempData["SuccessMessage"] = "The course has been deleted successfully.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        public IActionResult GetAllByCourseId(int courseId)
        {
            var Group = _courseService.GetAllByCourseId(courseId);
            return View(Group);
        }

        [Route("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}