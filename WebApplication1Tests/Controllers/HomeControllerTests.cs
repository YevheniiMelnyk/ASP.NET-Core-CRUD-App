using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;
using Moq;
using Assert = Xunit.Assert;

namespace WebApplication1.Controllers.Tests
{
    [TestClass()]
    public class HomeControllerTests
    {
        private Mock<ICourseService> _mockCourseService;
        private HomeController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockCourseService = new Mock<ICourseService>();
            _controller = new HomeController(_mockCourseService.Object);
        }

        [TestMethod]
        public void CreateCourse_Redirects_To_Index_Action_When_Course_Added()
        {
            // Arrange
            var course = new Course { NAME = "New Course", DESCRIPTION = "desc" };

            // Act
            var result = _controller.CreateCourse(course) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }


        [TestMethod]
        public void CreateCourse_Returns_ViewResult()
        {
            // Arrange

            // Act
            var result = _controller.CreateCourse() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [TestMethod]
        public void CreateCourse_Adds_Course_To_Service()
        {
            // Arrange
            var course = new Course { NAME = "New Course", DESCRIPTION = "desc" };

            // Act
            _controller.CreateCourse(course);

            // Assert
            _mockCourseService.Verify(m => m.Add(course), Times.Once);
        }

        [TestMethod]
        public void EditCourse_Post_WithValidModel_RedirectsToCourses()
        {
            // Arrange
            Course course = new()
            {
                COURSE_ID = 1,
                NAME = "name",
                DESCRIPTION = "desc"
            };

            _mockCourseService.Setup(x => x.Update(course)).Verifiable();

            // Act
            var result = _controller.UpdateCourse(course) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _mockCourseService.Verify();
        }

        [TestMethod]
        public void EditCourse_Post_WithInvalidModel_ReturnsView()
        {
            // Arrange
            Course course = new()
            {
                COURSE_ID = 1,
                NAME = "",
                DESCRIPTION = ""
            };

            _controller.ModelState.AddModelError("NAME", "The Name field is required.");
            _controller.ModelState.AddModelError("DESCRIPTION", "The Description field is required.");

            // Act
            var result = _controller.UpdateCourse(course) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(course, result.Model);
            Assert.False(_controller.ModelState.IsValid);
        }
    }
}