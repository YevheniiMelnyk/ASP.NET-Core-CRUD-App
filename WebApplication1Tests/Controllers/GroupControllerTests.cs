using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApplication1.Controllers;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;
using Assert = Xunit.Assert;

namespace WebApplication1.Tests.Controllers
{
    [TestClass()]
    public class GroupControllerTests
    {
        private Mock<IGroupService> _mockGroupService;
        private GroupController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockGroupService = new Mock<IGroupService>();
            _controller = new GroupController(_mockGroupService.Object);
        }

        [TestMethod()]
        public void CreateGroup_ReturnsAViewResult_WithAGroup()
        {
            // Arrange
            int courseId = 1;
            var controller = new GroupController(Mock.Of<IGroupService>());
            var expectedGroup = new Group { COURSE_ID = courseId };

            // Act
            var result = controller.CreateGroup(courseId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Group>(viewResult.ViewData.Model);
            Assert.Equal(expectedGroup.COURSE_ID, model.COURSE_ID);
        }

        [TestMethod()]
        public void CreateGroup_ReturnsBadRequestResult_WhenModelHasValidationError()
        {
            // Arrange
            var controller = new GroupController(Mock.Of<IGroupService>());
            var group = new Group { NAME = "" };
            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = controller.CreateGroup(group, 1);

            // Assert
            var badRequestResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<Group>(badRequestResult.ViewData.Model);
        }

        [TestMethod()]
        public void Groups_ReturnsAViewResult_WithListOfGroups()
        {
            // Arrange
            int courseId = 4;

            _mockGroupService.Setup(repo => repo.GetAll(courseId)).Returns(GetTestGroups());

            // Act
            var result = _controller.Groups(courseId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Group>>(viewResult.ViewData.Model);
            Assert.Equal(GetTestGroups().Count, model.Count());
        }

        private static List<Group> GetTestGroups()
        {
            var Group = new List<Group>
            {
                new Group 
                { 
                    COURSE_ID = 4, 
                    GROUP_ID = 31, 
                    NAME = "fghf" 
                }
            };
            return Group;
        }

        [TestMethod]
        public void GroupsByGroupId_ReturnsAViewResult_WithListOfGroups()
        {
            // Arrange
            int groupId = 31;
            _mockGroupService.Setup(repo => repo.GetAllCourseGroupsByGroupId(groupId)).Returns(GetTestGroups());

            // Act
            var result = _controller.GroupsByGroupId(groupId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Group>>(viewResult.ViewData.Model);
            Assert.Equal(GetTestGroups().Count, model.Count());
            Assert.Equal("Groups", viewResult.ViewName);
        }

        [TestMethod]
        public void EditGroup_Post_WithValidModel_RedirectsToGroups()
        {
            // Arrange
            Group group = new()
            {
                GROUP_ID = 1, 
                COURSE_ID = 2, 
                NAME = "Test Group" 
            };

            _mockGroupService.Setup(x => x.Update(group)).Verifiable();

            // Act
            var result = _controller.UpdateGroup(group) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Groups", result.ActionName);
            Assert.Equal(group.COURSE_ID, result.RouteValues["courseId"]);
            _mockGroupService.Verify();
        }

        [TestMethod]
        public void EditGroup_Post_WithInvalidModel_ReturnsView()
        {
            // Arrange
            Group group = new() 
            { 
                GROUP_ID = 1, 
                COURSE_ID = 2, 
                NAME = "" 
            };

            _controller.ModelState.AddModelError("Name", "The Name field is required.");

            // Act
            var result = _controller.UpdateGroup(group) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(group, result.Model);
            Assert.False(_controller.ModelState.IsValid);
        }
    }
}
