using EmployeeManagement.Business;
using EmployeeManagement.Business.EventArguments;
using EmployeeManagement.Business.Exceptions;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.Services.Test;
using EmployeeManagement.Test.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EmployeeManagement.Test
{
    [Collection("EmployeeServiceCollection")]
    public class EmployeeServiceTests //: IClassFixture<EmployeeServiceFixture>
    {
        private readonly EmployeeServiceFixture _employeeServiceFixture;
        private readonly ITestOutputHelper _testOutputHelper;

        public EmployeeServiceTests(EmployeeServiceFixture employeeServiceFixture, 
            ITestOutputHelper testOutputHelper)
        {
            _employeeServiceFixture = employeeServiceFixture;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedFirstObligatoryCourse_WithObject()
        {
            // Arrange

            var obligatoryCourse = _employeeServiceFixture
                .EmployeeManagementTestDataRepository
                .GetCourse(Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));

            // Act
            var internalEmployee = _employeeServiceFixture
                .EmployeeService.CreateInternalEmployee("Brooklyn", "Cannon");

            // Assert
            Assert.Contains(obligatoryCourse, internalEmployee.AttendedCourses);
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedFirstObligatoryCourse_WithPredicate()
        {
            // Arrange 

            // Act
            var internalEmployee = _employeeServiceFixture.EmployeeService.CreateInternalEmployee("Brooklyn", "Cannon");

            // Assert
            Assert.Contains(internalEmployee.AttendedCourses,
                course => course.Id == Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedSecondObligatoryCourse_WithPredicate()
        {
            // Arrange 

            // Act
            var internalEmployee = _employeeServiceFixture.EmployeeService.CreateInternalEmployee("Brooklyn", "Cannon");

            // Assert
            Assert.Contains(internalEmployee.AttendedCourses,
                course => course.Id == Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));
        }
         

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustMatchObligatoryCourses()
        {
            // Arrange 

            var obligatoryCourses = _employeeServiceFixture.EmployeeManagementTestDataRepository
                .GetCourses(
                    Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"), 
                    Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));

            // Act
            var internalEmployee = _employeeServiceFixture.EmployeeService.CreateInternalEmployee("Brooklyn", "Cannon");

            _testOutputHelper.WriteLine($"Employee after Act: " +
                $"{internalEmployee.FirstName} {internalEmployee.LastName}");
            internalEmployee.AttendedCourses
                .ForEach(c => _testOutputHelper.WriteLine($"Attended course: {c.Id} {c.Title}"));

            // Assert
            Assert.Equal(obligatoryCourses, internalEmployee.AttendedCourses);
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustNotBeNew()
        {
            // Arrange 

            // Act
            var internalEmployee = _employeeServiceFixture.EmployeeService
                .CreateInternalEmployee("Brooklyn", "Cannon");

            //internalEmployee.AttendedCourses[0].IsNew = true;
            // Assert
            //foreach (var course in internalEmployee.AttendedCourses)
            //{
            //    Assert.False(course.IsNew);
            //} 
            Assert.All(internalEmployee.AttendedCourses, course => Assert.False(course.IsNew));
        }


        [Fact]
        public async Task CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustMatchObligatoryCourses_Async()
        {
            // Arrange 
            var obligatoryCourses = await _employeeServiceFixture.EmployeeManagementTestDataRepository
                .GetCoursesAsync(
                    Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
                    Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));

            // Act
            var internalEmployee = await _employeeServiceFixture.EmployeeService
                .CreateInternalEmployeeAsync("Brooklyn", "Cannon");

            // Assert
            Assert.Equal(obligatoryCourses, internalEmployee.AttendedCourses);
        }

        [Fact]
        public async Task GiveRaise_RaiseBelowMinimumGiven_EmployeeInvalidRaiseExceptionMustBeThrown()
        {
            // Arrange  
            var internalEmployee = new InternalEmployee("Brooklyn", "Cannon", 5, 3000, false, 1);

            // Act & Assert
            await Assert.ThrowsAsync<EmployeeInvalidRaiseException>(async () =>
                {
                    await _employeeServiceFixture.EmployeeService.GiveRaiseAsync(internalEmployee, 50);
                });
        }

        [Fact]
        public void GiveRaise_RaiseBelowMinimumGiven_EmployeeInvalidRaiseExceptionMustBeThrown_Mistake()
        {
            // Arrange  
            var internalEmployee = new InternalEmployee("Brooklyn", "Cannon", 5, 3000, false, 1);

            // Act & Assert
            Assert.ThrowsAsync<EmployeeInvalidRaiseException>(async () =>
            {
                await _employeeServiceFixture.EmployeeService.GiveRaiseAsync(internalEmployee, 50);
            });
             
        }


        [Fact]
        public void NotifyOfAbsence_EmployeeIsAbsent_OnEmployeeIsAbsentMustBeTriggered()
        {
            // Arrange  
            var internalEmployee = new InternalEmployee("Brooklyn", "Cannon", 5, 3000, false, 1);

            // Act & Assert
            Assert.Raises<EmployeeIsAbsentEventArgs>(
               handler => _employeeServiceFixture.EmployeeService.EmployeeIsAbsent += handler,
               handler => _employeeServiceFixture.EmployeeService.EmployeeIsAbsent -= handler,
               () => _employeeServiceFixture.EmployeeService.NotifyOfAbsence(internalEmployee));              
        }



        // inline data thingies  

        //[Fact]
        //public void WhenCreatingEmployee_SalaryMustBeDefaultSalary()
        //{
        //    // Arrange
        //    var employeeService = new EmployeeManagement.Business.EmployeeService(
        //        new EmployeeManagementTestDataRepository());

        //    var employee = employeeService.CreateInternalEmployee("Amber", "Garcia");

        //    // Act

        //    // Assert
        //    Assert.Equal(2500, employee.Salary);
        //}


        //[Fact]
        //public void WhenCreatingEmployee_SuggestedBonusMustBeBlabalbalba()
        //{
        //    // Arrange
        //    var employeeService = new EmployeeManagement.Business.EmployeeService(
        //         new EmployeeManagementTestDataRepository());

        //    var employee = employeeService.CreateInternalEmployee("Amber", "Garcia");

        //    // Act
        //    // ?

        //    int correctSuggestedBonus;

        //    if (employee.YearsInService == 0)
        //    {
        //        correctSuggestedBonus = employee.AttendedCourses.Count * 100;
        //    }
        //    else
        //    {
        //        correctSuggestedBonus = employee.AttendedCourses.Count
        //            * employee.YearsInService * 100;
        //    }

        //    // Assert
        //    Assert.Equal(correctSuggestedBonus, employee.SuggestedBonus);
        //}


        // gebruiken op course collection na employee creation:
        // "customer blablablab course" mag er niet bij zijn
        [Fact]
        public void WhenCreatingEmployee_MustNotHaveThisCourse()
        {
            //Assert.DoesNotContain("Staff Of Wonder", _sut.Weapons);
        }

        //  geburiken op course collection: moet sowieso 1 van de 2 basiscursussen hebben
        // na creatie
        [Fact]
        public void WhenCreatingEmployee_MustAtLeastHaveThisCourse()
        {
            //Assert.Contains(_sut.Weapons, weapon => weapon.Contains("Sword"));
        }

        // nog een collection check: AttendCourseAsync => wanneer course reeds bestaat
        // voor gebruiker wordt 'm niet nog eens toegevoegd.  



        // bij inline data: ik kan 2x die WhenCreatingEmployee_MustAtLeastHaveThisCourse 
        // aanroepen met verschillende parameters zodat 'k op all courses uitkom
        // ==> neen, gaat niet, inline data moet vast ding zijn.  Dan kan ik deze
        // waarschijlijk wel gebruiken voor met een testdataclass ofzo te werken, dan
        // // kan 'k 9/10 data uit een repo halen en als object doorgeven enzo!
        //[Fact]
        //[InlineData(new Course("bla"))]
        //[InlineData(new Course("bla"))]
        //public void WhenCreatingEmployee_MustAtLeastHaveThisCourse_InlineData(Course course)
        //{
        //    //Assert.Contains(_sut.Weapons, weapon => weapon.Contains("Sword"));
        //}

        [Theory]
        [InlineData("37e03ca7-c730-4351-834c-b66f280cdb01")]
        [InlineData("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e")]
        public void WhenCreatingEmployee_MustAtLeastHaveThisCourse_InlineData(string courseId)
        {
            //Assert.Contains(_sut.Weapons, weapon => weapon.Contains("Sword"));
        }


        // geburiken op course collection: moet de 2 basiscursussen altidj hebben 
        // na creatie
        [Fact]
        public void WhenCreatingEmployee_MustHaveAllTheseCourses()
        {
            //var expectedWeapons = new[]
            //{
            //    "Long Bow",
            //    "Short Bow",
            //    "Short Sword"
            //};

            //Assert.Equal(expectedWeapons, _sut.Weapons);
        }

        //// 
        //[Fact]
        //public async Task WhenTryingToGiveARaiseBelowMinium_MustThrowEmployeeInvalidRaiseException()
        //{
        //    // Arrange
        //    var employeeService = new EmployeeManagement.Business.EmployeeService(
        //        new EmployeeManagementTestDataRepository());

        //    var employee = employeeService.CreateInternalEmployee("Amber", "Garcia");

        //    // Act
        //    await employeeService.GiveMinimumRaise(employee);

        //    // Assert (check of dit werkt, zowel valid als invalid!)
        //    await Assert.ThrowsAsync<EmployeeInvalidRaiseException>(
        //        async () => { await employeeService.GiveRaise(employee, 20); });
        //}

        [Fact]
        public async Task GiveRaise()
        {
            // todo: test dat salary wordt verhoogd
        }

        [Fact]
        public async Task GiveMinimumRaise()
        {
            // todo: test dat minimumveldje true wordt
        }

        [Theory]
        [InlineData(100, true)]
        [InlineData(200, false)]
        public async Task GiveRaiseWithInlineData(int raise, bool expectedMinimumRaiseValue)
        {
            // todo: test op expectedMinimumRaiseValue
        }

        // bovenste 2 vervangen door 1 met inline data: 
        // NEEN: iets beter vindne.  Inline data is voor cases waarin 
        // de test hetzelfde blijft, maar de business logica afhankelijk 
        // is van de input.  In dit geval blijft de test niet hetzelfde.  
        // Check: inlinedata sample in "Game" (=> damage + player health params)
        // ==> damage heeft effect op hoe de health wordt, dus je geeft 
        // als waarden door wat je wijzigt EN wat je verwacht als resultaat!
        // ==> 


    }
}
