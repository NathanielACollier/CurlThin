using System;
using nac.CurlThin;
using nac.CurlThin.Enums;
using nac.CurlThin.Helpers;
using nac.CurlThin.SafeHandles;

namespace Tests.lib;

public class libCurlTester: IDisposable
{
    private CURLcode global;

    public libCurlTester()
    {
        // curl_global_init() with default flags.
        global = CurlNative.Init();
    }

    public model.CurlResult get(string url)
    {
        // curl_easy_init() to create easy handle.
        var easy = CurlNative.Easy.Init();
        try
        {
            var dataCopier = new DataCallbackCopier();
            CurlNative.Easy.SetOpt(easy, CURLoption.URL, "http://httpbin.org/ip");
            CurlNative.Easy.SetOpt(easy, CURLoption.WRITEFUNCTION, dataCopier.DataHandler);

            var result = CurlNative.Easy.Perform(easy);
            
            return new model.CurlResult
            {
                curlCode = result,
                responseBytes = dataCopier.Stream.ToArray()
            };
        }
        finally
        {
            easy.Dispose();
        }
    }
    
    public void Dispose()
    {
        if (global == CURLcode.OK)
        {
            CurlNative.Cleanup();
        }
    }
}