using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Net;
using System.IO;

namespace THOK.PDA.Util
{
    public class HttpUtil
    {
        public string GetDataFromServer(string parameter)
        {

            string url = SystemCache.HttpConnectionStr;
                //"http://10.82.211.240:8090/Task/";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded";
            request.AllowWriteStreamBuffering = true;
            request.Method = "POST";

            byte[] buffer = Encoding.Default.GetBytes(parameter);
            request.ContentLength = buffer.Length;
            Stream requestStream = request.GetRequestStream();

            requestStream.Write(buffer, 0, buffer.Length);
            requestStream.Flush();
            requestStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            parameter = reader.ReadToEnd();


            reader.Close();
            responseStream.Close();
            request.Abort();

            return parameter;
        }
    }
}
