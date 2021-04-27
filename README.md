# CurlThin

+ Forked from [CurlThin](https://github.com/stil/CurlThin)

_CurlThin_ is a NET Standard compatible binding library against [libcurl](http://curl.haxx.se/libcurl).
It includes a modern wrapper for `curl_multi` interface which uses polling with [libuv](https://libuv.org/) library instead of using inefficient `select`.

_CurlThin_ has a very thin abstraction layer, which means that writing the code is as close as possible to writing purely in libcurl. libcurl has extensive documentation and relatively strong support of community and not having additional abstraction layer makes it easier to search solutions for your problems.

Using this library is very much like working with cURL's raw C API.

### License
Library is MIT licensed. 

## Installation

### Linux
+ Install curl
	+ After you have installed curl find the libcurl library with these commands
	```bash
	cd /usr/lib
	find . | grep curl
	```
	+ Then create a symbolic link to the right libcurl
	```bash
	sudo ln -s /usr/lib/x86_64-linux-gnu/libcurl.so.4.6.0 /usr/lib/libcurl.so
	```

### Windows
+ Install curl somewhere

  + You can download curl at [this website](https://curl.se/windows/)
  + Unzip the folder somewhere, and add it to your `PATH` system environment variable

+ Then you can do the same kind of symbolic linking that linux did to get this to work

  + **NOTE:** You must be at an `Administrator` command prompt to create symbolic links [see](https://stackoverflow.com/questions/894430/creating-hard-and-soft-links-using-powershell)

+ ```powershell
  New-Item -Path C:\Windows\System32\libcurl.dll -ItemType SymbolicLink -Value C:\programs\curl-7.76.1-win64-mingw\bin\libcurl-x64.dll
  ```

+ 


## Examples

### Easy interface

#### GET request
```csharp
// curl_global_init() with default flags.
var global = CurlNative.Init();

// curl_easy_init() to create easy handle.
var easy = CurlNative.Easy.Init();
try
{
    CurlNative.Easy.SetOpt(easy, CURLoption.URL, "http://httpbin.org/ip");

    var stream = new MemoryStream();
    CurlNative.Easy.SetOpt(easy, CURLoption.WRITEFUNCTION, (data, size, nmemb, user) =>
    {
        var length = (int) size * (int) nmemb;
        var buffer = new byte[length];
        Marshal.Copy(data, buffer, 0, length);
        stream.Write(buffer, 0, length);
        return (UIntPtr) length;
    });

    var result = CurlNative.Easy.Perform(easy);

    Console.WriteLine($"Result code: {result}.");
    Console.WriteLine();
    Console.WriteLine("Response body:");
    Console.WriteLine(Encoding.UTF8.GetString(stream.ToArray()));
}
finally
{
    easy.Dispose();

    if (global == CURLcode.OK)
    {
        CurlNative.Cleanup();
    }
}
```


#### POST request
```csharp
// curl_global_init() with default flags.
var global = CurlNative.Init();

// curl_easy_init() to create easy handle.
var easy = CurlNative.Easy.Init();
try
{
    var postData = "fieldname1=fieldvalue1&fieldname2=fieldvalue2";

    CurlNative.Easy.SetOpt(easy, CURLoption.URL, "http://httpbin.org/post");

    // This one has to be called before setting COPYPOSTFIELDS.
    CurlNative.Easy.SetOpt(easy, CURLoption.POSTFIELDSIZE, Encoding.ASCII.GetByteCount(postData));
    CurlNative.Easy.SetOpt(easy, CURLoption.COPYPOSTFIELDS, postData);
    
    var stream = new MemoryStream();
    CurlNative.Easy.SetOpt(easy, CURLoption.WRITEFUNCTION, (data, size, nmemb, user) =>
    {
        var length = (int) size * (int) nmemb;
        var buffer = new byte[length];
        Marshal.Copy(data, buffer, 0, length);
        stream.Write(buffer, 0, length);
        return (UIntPtr) length;
    });

    var result = CurlNative.Easy.Perform(easy);

    Console.WriteLine($"Result code: {result}.");
    Console.WriteLine();
    Console.WriteLine("Response body:");
    Console.WriteLine(Encoding.UTF8.GetString(stream.ToArray()));
}
finally
{
    easy.Dispose();

    if (global == CURLcode.OK)
    {
        CurlNative.Cleanup();
    }
}
```
