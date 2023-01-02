using Shouldly;
using SqlServerUtilities.Classes;
using UnitTestProject.Base;

namespace UnitTestProject;

[TestClass]
public partial class MainTest : TestBase
{
    [TestMethod]
    [TestTraits(Trait.PasswordCheck)]
    public void ValidLoginTest()
    {
        // arrange
        var userName = "payneoregon";
        var password = "!FirstOnMonday";

        // act
        var result = DataOperations.ValidateUser1(userName, password!.ToSecureString()!);

        // assert
        result.ShouldBe(true);
    }

    [TestMethod]
    [TestTraits(Trait.PasswordCheck)]
    public void InvalidLoginTest()
    {
        // arrange
        var userName = "payneoregon";
        var password = "!firstonmonday"; // wrong password

        // act
        var result = DataOperations.ValidateUser1(userName, password!.ToSecureString()!);

        // assert
        result.ShouldBe(false);
    }
}