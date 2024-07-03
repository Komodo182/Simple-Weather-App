namespace Simple_Weather_App
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestLogin1()
        {
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            Program.logIn("test", "test");
            var result = sw.ToString().Trim();
            Assert.That(result == "user test has logged in successfully!");
        }
    }
}