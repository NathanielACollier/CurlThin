using System;
using System.Collections.Generic;
using System.Linq;
using nac.CurlThin;
using nac.CurlThin.Enums;
using nac.CurlThin.Helpers;
using nac.CurlThin.SafeHandles;

namespace Tests.lib;

public class libCurlTester: IDisposable
{
    private CURLcode global;
    private SafeSlistHandle headers;

    public libCurlTester()
    {
        // curl_global_init() with default flags.
        global = CurlNative.Init();
    }

    public model.CurlResult execute(string url,
                    Dictionary<string,string> headers = null,
                    Dictionary<string,string> postFields = null)
    {
        // curl_easy_init() to create easy handle.
        var easy = CurlNative.Easy.Init();
        try
        {
            var dataCopier = new DataCallbackCopier();
            CurlNative.Easy.SetOpt(easy, CURLoption.URL, url);

            handlePostFields(easy, postFields);
            
            CurlNative.Easy.SetOpt(easy, CURLoption.WRITEFUNCTION, dataCopier.DataHandler);

            addHeaders(easy, headers);

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

    private void handlePostFields(SafeEasyHandle easy, Dictionary<string, string> postFields)
    {
        if (postFields == null)
        {
            return;
        }

        if (postFields.Keys.Count < 1)
        {
            return;
        }
        
        // form the postfields string
        string textPostFields = string.Join('&', postFields.Select(d => $"{d.Key}={d.Value}"));
        
        // This one has to be called before setting COPYPOSTFIELDS.
        CurlNative.Easy.SetOpt(easy, CURLoption.POSTFIELDSIZE,System.Text.Encoding.ASCII.GetByteCount(textPostFields));
        CurlNative.Easy.SetOpt(easy, CURLoption.COPYPOSTFIELDS, textPostFields);
        
        
    }

    private void addHeaders(SafeEasyHandle easy, Dictionary<string, string> dictionary)
    {
        if (dictionary == null)
        {
            return;
        }

        if (dictionary.Keys.Count < 1)
        {
            return;
        }
        
        if (this.headers == null)
        {
            // Initialize HTTP header list with first value.
            var firstHeader = dictionary.First();
            // remove the first header so we don't add it again later
            dictionary.Remove(firstHeader.Key);
            headers = CurlNative.Slist.Append(SafeSlistHandle.Null, $"{firstHeader.Key}: {firstHeader.Value}");
        }
        
        // loop through any other headers and add them
        foreach (var headerEntry in dictionary)
        {
            CurlNative.Slist.Append(headers, $"{headerEntry.Key}: {headerEntry.Value}");
        }
        
        // Configure libcurl easy handle to send HTTP headers we configured.
        CurlNative.Easy.SetOpt(easy, CURLoption.HTTPHEADER, headers.DangerousGetHandle());
    }

    public void Dispose()
    {
        // CurlCode.OK means we successfully init curl so we have stuff to free up
        if (global == CURLcode.OK)
        {
            CurlNative.Cleanup();
        }

        if (headers != null)
        {
            headers.Dispose();
        }
    }
}