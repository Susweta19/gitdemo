namespace TxWebTests.HeaderUtils
{
    public class Header
    {
        public TestData TestData { get; private set; }
        public TeexmaData TeexmaData { get; private set; }

        public Header(TestData testData, TeexmaData teexmaData)
        {
            TestData = testData;
            TeexmaData = teexmaData;
        }
    }
}
