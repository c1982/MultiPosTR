using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace VPosTR.Api.VirtualPos
{
    public class EstVPos : VPos
    {
        private string _clientId, 
                       _name, 
                       _password, 
                       _hosturl;        

        public EstVPos(string clientId, string Name, string Password, string hostUrl)
        {

        }


        internal override PosResponse Sales(PosRequest posRequest)
        {
            throw new NotImplementedException();
        }

        internal override PosResponse Void(PosRequest posRequest)
        {
            throw new NotImplementedException();
        }

        internal override PosResponse ReFund(PosRequest posRequest)
        {
            throw new NotImplementedException();
        }
    }


    #region Request
    [Serializable]
    [XmlRoot("CC5Request", Namespace = null)]
    public class CC5Request
    {
        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public string Password { get; set; }

        [XmlElement]
        public string ClientId { get; set; }

        [XmlElement]
        public string IPAddress { get; set; }

        [XmlElement]
        public string Email { get; set; }

        [XmlElement]
        public string Mode { get; set; }

        [XmlElement]
        public string OrderId { get; set; }

        [XmlElement]
        public string GroupId { get; set; }

        [XmlElement]
        public string TransId { get; set; }

        [XmlElement]
        public string UserId { get; set; }

        [XmlElement]
        public string Type { get; set; }

        [XmlElement]
        public string Number { get; set; }

        [XmlElement]
        public string Expires { get; set; }

        [XmlElement]
        public string Cvv2Val { get; set; }

        [XmlElement]
        public string Total { get; set; }

        [XmlElement]
        public string Currency { get; set; }

        [XmlElement(IsNullable = true)]
        public string Taksit { get; set; }

        [XmlElement(ElementName = "BillTo")]
        public BillTo BillTo_ { get; set; }

        [XmlElement(ElementName = "ShipTo")]
        public ShipTo ShipTo_ { get; set; }

        [XmlElement(ElementName = "Extra")]
        public Extra Extra_ { get; set; }
    }

    public class BillTo
    {
        [XmlElement(IsNullable = true)]
        public string Street1 { get; set; }

        [XmlElement(IsNullable = true)]
        public string Street2 { get; set; }

        [XmlElement(IsNullable = true)]
        public string Street3 { get; set; }

        [XmlElement(IsNullable = true)]
        public string City { get; set; }

        [XmlElement(IsNullable = true)]
        public string StateProv { get; set; }

        [XmlElement(IsNullable = true)]
        public string PostalCode { get; set; }

        [XmlElement(IsNullable = true)]
        public string Country { get; set; }

        [XmlElement(IsNullable = true)]
        public string Company { get; set; }

        [XmlElement(IsNullable = true)]
        public string TelVoice { get; set; }
    }

    public class ShipTo
    {
        [XmlElement(IsNullable = true)]
        public string Name { get; set; }

        [XmlElement(IsNullable = true)]
        public string Street1 { get; set; }

        [XmlElement(IsNullable = true)]
        public string Street2 { get; set; }

        [XmlElement(IsNullable = true)]
        public string Street3 { get; set; }

        [XmlElement(IsNullable = true)]
        public string City { get; set; }

        [XmlElement(IsNullable = true)]
        public string StateProv { get; set; }

        [XmlElement(IsNullable = true)]
        public string PostalCode { get; set; }

        [XmlElement(IsNullable = true)]
        public string Country { get; set; }
    }
    #endregion


    #region Response
    [Serializable]
    [XmlRoot("CC5Response", Namespace = null)]
    public class CC5Response
    {
        [XmlElement]
        public string OrderId { get; set; }

        [XmlElement]
        public string GroupId { get; set; }

        [XmlElement]
        public string Response { get; set; }

        [XmlElement]
        public string AuthCode { get; set; }

        [XmlElement]
        public string HostRefNum { get; set; }

        [XmlElement]
        public string ProcReturnCode { get; set; }

        [XmlElement]
        public string TransId { get; set; }

        [XmlElement]
        public string ErrMsg { get; set; }

        [XmlElement(ElementName = "Extra")]
        public Extra Extra_ { get; set; }

    }

    public class Extra
    {
        [XmlElement]
        public string KAMPANYAKOD;

        [XmlElement]
        public string taksit;

        [XmlElement]
        public string HOSTMSG { get; set; }

        [XmlElement]
        public string NUMCODE { get; set; }
    }
    #endregion

}
