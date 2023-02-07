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
        var (success, exception ) = DataOperations
            .ValidateUserLogin(userName, password!.ToSecureString()!);

        // assert
        success.ShouldBeTrue();
    }

    [TestMethod]
    [TestTraits(Trait.PasswordCheck)]
    public void InvalidLoginTest()
    {
        // arrange
        var userName = "payneoregon";
        var password = "!firstonmonday"; // wrong password

        // act
        var (success, _ ) = DataOperations.ValidateUserLogin(userName, password!.ToSecureString()!);

        // assert
        success.ShouldBeFalse();
    }
}