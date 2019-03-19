using System;
using Xunit;
using NSubstitute;
using System.Collections.Generic;
using System.Web;
using Mosaic.Controllers;
using Mosaic.Models;
using Mosaic.Services;
using Mosaic.Repository;
using System.Linq;
using Moq;
using Microsoft.EntityFrameworkCore;

namespace Mosaic.Test
{
    public class TestStudentsController
    {
        IQueryable<Student> students;
        IQueryable<Class> classes;
        Mock<MosaicContext> mockContext;

        public TestStudentsController()
        {
            List<Student> liststudents = new List<Student>();
            students = liststudents.AsQueryable();
            var fakeStudents = new Mock<DbSet<Student>>();
            fakeStudents.As<IQueryable<Student>>().Setup(m => m.Provider).Returns(students.Provider);
            fakeStudents.As<IQueryable<Student>>().Setup(m => m.Expression).Returns(students.Expression);
            fakeStudents.As<IQueryable<Student>>().Setup(m => m.ElementType).Returns(students.ElementType);
            fakeStudents.As<IQueryable<Student>>().Setup(m => m.GetEnumerator()).Returns(students.GetEnumerator());
            fakeStudents.Setup(set => set.Add(It.IsAny<Student>())).Callback<Student>(liststudents.Add);

            classes = new List<Class>().AsQueryable();
            var fakeClasses = new Mock<DbSet<Class>>();
            fakeClasses.As<IQueryable<Class>>().Setup(m => m.Provider).Returns(classes.Provider);
            fakeClasses.As<IQueryable<Class>>().Setup(m => m.Expression).Returns(classes.Expression);
            fakeClasses.As<IQueryable<Class>>().Setup(m => m.ElementType).Returns(classes.ElementType);
            fakeClasses.As<IQueryable<Class>>().Setup(m => m.GetEnumerator()).Returns(classes.GetEnumerator());

            mockContext = new Mock<MosaicContext>();
            mockContext.Setup(x => x.Student).Returns(fakeStudents.Object);
            mockContext.Setup(x => x.Class).Returns(fakeClasses.Object);
        }

        [Fact]
        public void TestCreateStudent()
        {
            StudentRepository sRepo = new StudentRepository(mockContext.Object);
            StudentAuthentication sAuth = new StudentAuthentication(mockContext.Object);
            Student kael = new Student
            {
                Username = "testUser",
                Password = sAuth.EncryptPassword("password"),
                FirstName = "Test",
                LastName = "User",
                ClassOne = "",
                ClassTwo = ""
            };

            mockContext.Object.Student.Add(kael);
            var student = mockContext.Object.Student.SingleOrDefault(m => m.Username == kael.Username);
            Assert.True(student != null);
        }

        /*
        [Fact]
        public void TestEnrollInClass()
        {

        }

        [Fact]
        public void TestDropClass()
        {

        }

        [Fact]
        public void TestChangePassword() {

        }

        [Fact]
        public void TestLoginStudent()
        {

        }
        */
    }
}
