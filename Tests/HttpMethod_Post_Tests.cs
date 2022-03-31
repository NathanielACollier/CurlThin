using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests;

[TestClass]
public class HttpMethod_Post_Tests
{

    [TestMethod]
    public void postFormFieldsTest()
    {
        var libCurl = new lib.libCurlTester();
        var resp = libCurl.execute("http://httpbin.org/post", postFields: new Dictionary<string, string>
        {
            { "fieldname1", "fieldvalue1" },
            {"fieldname2", "fieldvalue2"}
        });
        
        string responseText = System.Text.Encoding.UTF8.GetString(resp.responseBytes);
        
        Assert.IsTrue(responseText.Contains("form", StringComparison.OrdinalIgnoreCase) &&
                      responseText.Contains("fieldname1", StringComparison.OrdinalIgnoreCase) &&
                      responseText.Contains("fieldname2", StringComparison.OrdinalIgnoreCase)
                      );
    }
}