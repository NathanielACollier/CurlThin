using nac.CurlThin.Enums;

namespace Tests.model;

public class CurlResult
{
    public CURLcode curlCode { get; set; }
    public byte[] responseBytes { get; set; }
}