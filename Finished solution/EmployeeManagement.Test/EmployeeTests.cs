using Xunit;
using EmployeeManagement.DataAccess.Entities;

namespace EmployeeManagement.Test
{
    public class EmployeeTests
    {
        // we testen business logic.  Waar zit die?  Wel, hangt er heel hard 
        // van af hoe je apparchitectuur in elkaar zit.  Soms in business-laag, zoals je 
        // zou denken, maar niet altijd - soms worden validation annotations op entities
        // of viewmodels gebruikt: dan mag je die stukken testen.  Soms zit logica 
        // in je models (entity en/of viewmodel): bvb fullname via een 
        // property getter = firstname + lastname; 

        [Fact]
        public void EmployeeFullNamePropertyGetter_InputFirstNameAndLastName_FullNameIsConcatenation()
        {
            // Arrange
            var employee = new InternalEmployee("Kevin", "Dockx", 0, 2500, false, 1);

            // Act
            employee.FirstName = "Lucia";
            employee.LastName = "SHELTON";

            // Assert
            Assert.Equal("Lucia Shelton", employee.FullName, ignoreCase: true);
        }


        [Fact]
        public void EmployeeFullNamePropertyGetter_InputFirstNameAndLastName_FullNameStartsWithFirstName()
        {
            // Arrange
            var employee = new InternalEmployee("Kevin", "Dockx", 0, 2500, false, 1);

            // Act
            employee.FirstName = "Lucia";
            employee.LastName = "Shelton";

            // Assert
            Assert.StartsWith(employee.FirstName, employee.FullName);
        }

        [Fact]
        public void EmployeeFullNamePropertyGetter_InputFirstNameAndLastName_FullNameEndsWithFirstName()
        {
            // Arrange
            var employee = new InternalEmployee("Kevin", "Dockx", 0, 2500, false, 1);

            // Act
            employee.FirstName = "Lucia";
            employee.LastName = "Shelton";

            // Assert
            Assert.EndsWith(employee.LastName, employee.FullName);
        }

        [Fact]
        public void EmployeeFullNamePropertyGetter_InputFirstNameAndLastName_FullNameContainsPartOfConcatenation()
        {
            // Arrange
            var employee = new InternalEmployee("Kevin", "Dockx", 0, 2500, false, 1);

            // Act
            employee.FirstName = "Lucia";
            employee.LastName = "Shelton";

            // Assert
            Assert.Contains("ia Sh", employee.FullName);
        }

        [Fact]
        public void EmployeeFullNamePropertyGetter_InputFirstNameAndLastName_FullNameSoundsLikeConcatenation()
        {
            // Arrange
            var employee = new InternalEmployee("Kevin", "Dockx", 0, 2500, false, 1);

            // Act
            employee.FirstName = "Lucia";
            employee.LastName = "Shelton";
 
            // Assert
            Assert.Matches("Lu(c|z|s)ia Shel(t|d)on", employee.FullName);
        }


        [Fact]
        public void WhenCreatingEmployee_AndHasFirstNameAndLastName_FullNameMustLetsThinkAboutABetterMethodName()
        {
            // Arrange
            var employee = new InternalEmployee("Kevin", "Dockx", 0, 2500, false, 1);

            // Act
            employee.FirstName = "Sven";
            employee.LastName = "Vercauteren";

            // Assert
            Assert.Equal("Sven Vercauteren", employee.FullName);
        }

        // en die fullname kan dan ook gebruikt worden voor string manipulatin tests: startswith, upperczse, ...
        // CFR:
        //[Fact]
        //public void HaveFullNameStartingWithFirstName()
        //{
        //    _sut.FirstName = "Sarah";
        //    _sut.LastName = "Smith";

        //    Assert.StartsWith("Sarah", _sut.FullName);
        //}

        //[Fact]
        //public void HaveFullNameEndingWithLastName()
        //{
        //    _sut.LastName = "Smith";

        //    Assert.EndsWith("Smith", _sut.FullName);
        //}

        //[Fact]
        //public void CalculateFullName_IgnoreCaseAssertExample()
        //{
        //    _sut.FirstName = "SARAH";
        //    _sut.LastName = "SMITH";

        //    Assert.Equal("Sarah Smith", _sut.FullName, ignoreCase: true);
        //}

        //[Fact]
        //public void CalculateFullName_SubstringAssertExample()
        //{
        //    _sut.FirstName = "Sarah";
        //    _sut.LastName = "Smith";

        //    Assert.Contains("ah Sm", _sut.FullName);
        //}


        //[Fact]
        //public void CalculateFullNameWithTitleCase()
        //{
        //    _sut.FirstName = "Sarah";
        //    _sut.LastName = "Smith";

        //    Assert.Matches("[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+", _sut.FullName);
        //} 
    }
}