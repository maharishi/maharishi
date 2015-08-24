<%@ WebHandler Language="C#" Class="CompressFileHandler" %>

using System;
using System.Web;

public class CompressFileHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        var oSR = new System.IO.StreamReader(context.Request.InputStream);
        var zip = Newtonsoft.Json.JsonConvert.DeserializeObject<ZipCruncher>(oSR.ReadToEnd());
        zip.BaseServerPath = context.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["TEMPPATH"]);
        zip.Zip();
        context.Response.ContentType = "text/json";
        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(zip));
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }
}