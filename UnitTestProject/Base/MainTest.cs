


// ReSharper disable once CheckNamespace - do not change
namespace UnitTestProject;

public partial class MainTest : IDisposable
{

    public MainTest()
    {
    }

    /// <summary>
    /// Perform initialization before each test runs
    /// </summary>
    /// <returns>Nothing we care about</returns>
    /// <remarks>
    /// For synchronous preparation
    /// * Remove the async modifier
    /// * Remove the line with await Task.Delay(0);
    /// </remarks>
    [TestInitialize]
    public async Task Init()
    {
        if (TestContext.TestName == "TestMethod1")
        {
            await Task.Delay(0);
        }
    }
    [TestCleanup]
    public void TestCleanup()
    {

    }
    /// <summary>
    /// Perform any initialize for the class
    /// </summary>
    /// <param name="testContext"></param>
    [ClassInitialize()]
    public static void MyClassInitialize(TestContext testContext)
    {
        TestResults = new List<TestContext>();
    }
    /// <summary>
    /// Here is where any clean operations are performed for this class
    /// </summary>
    /// <returns></returns>
    [ClassCleanup()]
    public static async Task ClassCleanup()
    {
        await Task.Delay(0);
    }
    /// <summary>
    /// Optional event to clean up any objects that are not managed by the framework
    /// </summary>
    public void Dispose()
    {
    }
}