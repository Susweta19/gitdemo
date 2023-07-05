using System.Collections.Generic;

namespace TxWebTests.HeaderUtils
{
    public class TestData
    {
        public int TestPriority { get; private set; }
        public ICollection<string> TestDependencies { get; private set; }
        public string TestDescription { get; private set; }
        public string TestRunningIssue { get; private set; }
        public int TestBugRef { get; private set; }
        public ICollection<string> TestFuncTimer { get; private set; }

        public TestData(int testPriority, ICollection<string> testDependencies, string testDescription, string testRunningIssue = null, int testBugRef = 0, ICollection<string> testFuncTimer = null)
        {
            TestPriority = testPriority;
            TestDependencies = testDependencies;
            TestDescription = testDescription;
            TestRunningIssue = testRunningIssue;
            TestBugRef = testBugRef;
            TestFuncTimer = testFuncTimer;
        }
    }
}
