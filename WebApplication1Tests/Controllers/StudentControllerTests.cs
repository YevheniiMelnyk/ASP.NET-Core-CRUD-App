using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;
using Moq;
using Assert = Xunit.Assert;

namespace WebApplication1.Controllers.Tests
{
    [TestClass()]
    public class StudentControllerTests
    {
        private Mock<IStudentService> _mockStudentService;
        private StudentController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockStudentService = new Mock<IStudentService>();
            _controller = new StudentController(_mockStudentService.Object);
        }

        [TestMethod()]
        public void CreateStudent_ReturnsAViewResult_WithStudents()
        {
            // Arrange
            int groupId = 1;
            var expectedStudent = new Student { GROUP_ID = groupId };

            // Act
            var result = _controller.CreateStudent(groupId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Student>(viewResult.ViewData.Model);
            Assert.Equal(expectedStudent.GROUP_ID, model.GROUP_ID);
        }

        [TestMethod()]
        public void CreateStudent_ReturnsBadRequestResult_WhenModelHasValidationError()
        {
            // Arrange
            int groupId = 1;
            var student = new Student { FIRST_NAME = "", LAST_NAME = "" };
            _controller.ModelState.AddModelError("FIRSTNAME", "Required");
            _controller.ModelState.AddModelError("LASTNAME", "Required");

            // Act
            var result = _controller.CreateStudent(student, groupId);

            // Assert
            var badRequestResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<Student>(badRequestResult.ViewData.Model);
        }

        [TestMethod()]
        public void Groups_ReturnsAViewResult_WithListOfStudents()
        {
            // Arrange
            int groupId = 4;

            _mockStudentService.Setup(repo => repo.GetAll(groupId)).Returns(GetTestStudents());

            // Act
            var result = _controller.Students(groupId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Student>>(viewResult.ViewData.Model);
            Assert.Equal(GetTestStudents().Count, model.Count());
        }

        private static List<Student> GetTestStudents()
        {
            var students = new List<Student>
            {
                new Student
                {
                    GROUP_ID = 1,
                    STUDENT_ID = 1,
                    FIRST_NAME = "first",
                    LAST_NAME = "last"
                },
                new Student
                {
                    GROUP_ID = 1,
                    STUDENT_ID = 2,
                    FIRST_NAME = "first1",
                    LAST_NAME = "last1"
                }
            };
            return students;
        }

        [TestMethod]
        public void EditStudent_Post_WithValidModel_RedirectsToStudents()
        {
            // Arrange
            Student student = new()
            {
                GROUP_ID = 1,
                STUDENT_ID = 1,
                FIRST_NAME = "first",
                LAST_NAME = "last"
            };

            _mockStudentService.Setup(x => x.Update(student)).Verifiable();

            // Act
            var result = _controller.UpdateStudent(student) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Students", result.ActionName);
            Assert.Equal(student.GROUP_ID, result.RouteValues["groupId"]);
            _mockStudentService.Verify();
        }

        [TestMethod]
        public void EditStudent_Post_WithInvalidModel_ReturnsView()
        {
            // Arrange
            Student student = new()
            {
                GROUP_ID = 1,
                STUDENT_ID = 1,
                FIRST_NAME = "",
                LAST_NAME = ""
            };

            _controller.ModelState.AddModelError("FIRSTNAME", "The Name field is required.");
            _controller.ModelState.AddModelError("LASTNAME", "The Name field is required.");

            // Act
            var result = _controller.UpdateStudent(student) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(student, result.Model);
            Assert.False(_controller.ModelState.IsValid);
        }
    }
}