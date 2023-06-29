namespace WebUI.Models;

public class IpModel
{
    public string ipString { get; set; }
    public int ipNumeric { get; set; }
    public string ipType { get; set; }
    public bool isBehindProxy { get; set; }
    public string device { get; set; }
    public string os { get; set; }
    public string userAgent { get; set; }
    public string family { get; set; }
    public string versionMajor { get; set; }
    public string versionMinor { get; set; }
    public string versionPatch { get; set; }
    public bool isSpider { get; set; }
    public bool isMobile { get; set; }
    public string userAgentDisplay { get; set; }
    public string userAgentRaw { get; set; }

}
