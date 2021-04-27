namespace CurlThin.Enums
{
    public enum CURLAUTH : int
    {
        NONE = 0,
        BASIC = (1 << 0), /* Basic (default) */
        DIGEST = (1 << 1), /* Digest */
        GSSNEGOTIATE = (1 << 2), /* GSS-Negotiate */
        NTLM = (1 << 3), /* NTLM */
        ANY = ~0, /* all types set */
        ANYSAFE = (~CURLAUTH.BASIC)
    }
}