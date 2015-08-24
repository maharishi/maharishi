using System;
using System.Web;
using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;
using System.Security.Permissions;
using System.Net;
/// <summary>
/// Summary description for ZipCruncher
/// </summary>
[JsonObject("Zip")]
public class ZipCruncher:IZipCruncher
{

    public enum PathType
    {
        URL,
        ABSOLUTE
    }

    [JsonProperty(PropertyName = "url")]
    public string Url { get; set; }
    [JsonProperty(PropertyName = "filename")]
    public string FileName { get; set; }
    [JsonProperty(PropertyName = "filelink")]
    public string FileLink { get; set; }

    public string BaseServerPath { get; set; }

    /// <summary>
    /// Zips the file
    /// </summary>
    public void Zip()
    {
        byte[] byteArray = null;
        byteArray = GetRawFile(Url);

        //Prepare for compress
        using (MemoryStream ms = new MemoryStream())
        using (GZipStream sw = new GZipStream(ms, CompressionMode.Compress))
        {

            //Compress
            sw.Write(byteArray, 0, byteArray.Length);
            //Close, DO NOT FLUSH cause bytes will go missing...
            sw.Close();

            byteArray = ms.ToArray();
            ByteArrayToFile(string.Format("{0}{1}", BaseServerPath, FileName), byteArray);
        }
        FileLink = VirtualPathUtility.ToAbsolute(System.Configuration.ConfigurationManager.AppSettings["TEMPPATH"]) + FileName;
    }

    /// <summary>
    /// Gets the file name and path of based on the url type
    /// </summary>
    /// <param name="filePath">Full path of the file</param>
    /// <param name="filedir">Directory where the file is located</param>
    /// <param name="pathType">Path type <see cref="PathType"/></param>
    /// <returns>file name</returns>
    [FileIOPermission(SecurityAction.Demand, Unrestricted = true)]
    public string GetFileName(string filePath, out string filedir, PathType pathType)
    {
        string result;
        switch (pathType)
        {
            case ZipCruncher.PathType.URL:
                {
                    string[] arr = filePath.Split(new char[]{'/'});
                    string fileName = arr[arr.Length - 1];
                    arr[arr.Length - 1] = null;
                    filedir = string.Join("/", arr);
                    result = fileName;
                    break;
                }
            case ZipCruncher.PathType.ABSOLUTE:
                {
                    FileInfo fi = new FileInfo(filePath);
                    filedir = fi.DirectoryName;
                    result = fi.Name;
                    break;
                }
            default:
                filedir = filePath;
                result = filePath;
                break;
        }
        return result;
    }

    public byte[] GetRawFile(string filePath)
    {
        byte[] data;
        var fileDir = string.Empty;
        var file = this.GetFileName(filePath, out fileDir, ZipCruncher.PathType.URL);
        using (WebClient wc = new WebClient())
        {
            data = wc.DownloadData(filePath);
        }
        FileName = FileName ?? file;
        return data;
    }

    /// <summary>
    /// Function to save byte array to a file
    /// </summary>
    /// <param name="_FileName">File name to save byte array</param>
    /// <param name="_ByteArray">Byte array to save to external file</param>
    /// <returns>Return true if byte array save successfully, if not return false</returns>
    [FileIOPermission(SecurityAction.Demand, Unrestricted = true)]
    public bool ByteArrayToFile(string _FileName, byte[] _ByteArray)
    {
        try
        {
            // Open file for reading
            FileStream _FileStream = new FileStream(_FileName, FileMode.Create, FileAccess.Write);

            // Writes a block of bytes to this stream using data from a byte array.
            _FileStream.Write(_ByteArray, 0, _ByteArray.Length);

            _FileStream.Flush();

            // close file stream
            _FileStream.Close();

            return true;
        }
        catch (Exception _Exception)
        {
            // Error
            Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
        }

        // error occured, return false
        return false;
    }
}