using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests;

[TestClass]
public class HttpMethod_Get_Tests
{
    [TestMethod]
    public void getUrlOneTime()
    {
        var libcurl = new lib.libCurlTester();
        var response = libcurl.get("http://httpbin.org/ip");

        string responseText = System.Text.Encoding.UTF8.GetString(response.responseBytes);
        
        Assert.IsTrue(!string.IsNullOrWhiteSpace(responseText));
    }
}