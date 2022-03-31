using System;
using System.Collections.Generic;
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

    [TestMethod]
    public void getWithHeaders()
    {
        var libCurl = new lib.libCurlTester();
        var response = libCurl.get(url: "http://httpbin.org/headers",
            headers: new Dictionary<string, string>
            {
                {"X-Foo", "Bar"},
                {"X-Qwerty", "Asdfgh"}
            });
        
        string responseText = System.Text.Encoding.UTF8.GetString(response.responseBytes);
        
        Assert.IsTrue(responseText.Contains("headers", StringComparison.OrdinalIgnoreCase) &&
                      responseText.Contains("X-Foo", StringComparison.OrdinalIgnoreCase) &&
                      responseText.Contains("X-Qwerty", StringComparison.OrdinalIgnoreCase)
                      );
    }
}