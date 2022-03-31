using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests;

[TestClass]
public class HttpMethod_Get_Tests
{
    [TestMethod]
    public void getUrlOneTime()
    {
        var libcurl = new lib.libCurlTester();
        var response = libcurl.execute("http://httpbin.org/ip");

        string responseText = System.Text.Encoding.UTF8.GetString(response.responseBytes);
        
        Assert.IsTrue(!string.IsNullOrWhiteSpace(responseText));
    }

    [TestMethod]
    public void getWithHeaders()
    {
        var libCurl = new lib.libCurlTester();
        var response = libCurl.execute(url: "http://httpbin.org/headers",
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


    [TestMethod]
    public void getResuseCurlInstance()
    {
        var libcurl = new lib.libCurlTester();
        for (int i = 0; i < 20; ++i)
        {
            var resp = libcurl.execute("http://httpbin.org/ip");
            string responseText = System.Text.Encoding.UTF8.GetString(resp.responseBytes);
            
            Assert.IsTrue(responseText.Length > 0);
            System.Diagnostics.Debug.WriteLine($"Iteration[{i}]; Response: {responseText}");
            Thread.Sleep(1000 * 5);
        }
    }
    
    
    
}