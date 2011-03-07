namespace VPosTR.Api
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public abstract class VPos
    {
        internal abstract PosResponse Sales(PosRequest posRequest);
        internal abstract PosResponse Void(PosRequest posRequest);
        internal abstract PosResponse ReFund(PosRequest posRequest);

        internal string SerializeObjectToXmlString<T>(T TModel)
        {
            string xmlData = String.Empty;

            XmlSerializerNamespaces EmptyNameSpace = new XmlSerializerNamespaces();
            EmptyNameSpace.Add("", "");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter xmlWriter = new XmlTextWriter(memoryStream,
                Encoding.GetEncoding("iso-8859-9"));

            xmlSerializer.Serialize(xmlWriter, TModel, EmptyNameSpace);

            memoryStream = (MemoryStream)xmlWriter.BaseStream;
            xmlData = UTF8ByteArrayToString(memoryStream.ToArray());

            return xmlData;
        }

        internal T DeSerializeObject<T>(string xmlData)
        {
            T deSerializeObject = default(T);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            StringReader stringReader = new StringReader(xmlData);
            XmlReader XR = new XmlTextReader(stringReader);

            if (xmlSerializer.CanDeserialize(XR))
            {
                deSerializeObject = (T)xmlSerializer.Deserialize(XR);
            }

            return deSerializeObject;
        }

        internal string SendHttpRequest(string Host, string Method, string Params)
        {
            //Bozuk SSL Sertifikalarını by-pass için.
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate
                                                                            (object sender, X509Certificate certificate, X509Chain chain,
                                                                            SslPolicyErrors sslPolicyErrors)
            { return true; };

            string GeriDonen = String.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Host);
            request.Timeout = 30000;
            request.Method = Method;

            byte[] bytes = new ASCIIEncoding().GetBytes(Params);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Params.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();

            StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream());
            GeriDonen = sr.ReadToEnd();

            return GeriDonen;
        }

        #region Privates
        private String UTF8ByteArrayToString(Byte[] characters)
        {

            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }
        #endregion
    }
}
