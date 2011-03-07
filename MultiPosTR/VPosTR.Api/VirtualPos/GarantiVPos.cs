
namespace VPosTR.Api.VirtualPos
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Xml.Serialization;

    public class GarantiVPos : VPos
    {
        private const string GVPVERSION = "v0.0.1";

        private string _terminalId;
        private string _provisionUser;
        private string _merchantId;
        private string _workingMode;
        private string _password;
        private string _hostUrl;

        public GarantiVPos(string TerminalId, string ProvisionUser, string ProvisionPassword, string MerchantId, string hostUrl, string WorkingMode = "TEST")
        {
            _terminalId = TerminalId;
            _provisionUser = ProvisionUser;
            _merchantId = MerchantId;
            _workingMode = WorkingMode;
            _password = ProvisionPassword;
            _hostUrl = hostUrl;
        }

        internal override PosResponse Sales(PosRequest posRequest)
        {
            var response = new PosResponse();

            var gvpRequest = CreateRequest(posRequest);

            response.RequestTextData = this.SerializeObjectToXmlString<GVPSRequest>(gvpRequest);
            response.ResponseTextData = SendHttpRequest(_hostUrl, "Post", response.RequestTextData);

            var responseObject = this.DeSerializeObject<GVPSResponse>(response.ResponseTextData);

            response.OrderId = responseObject.Order.OrderID;
            response.StatusCode = responseObject.Transaction.Response.Code;
            response.Status = responseObject.Transaction.Response.Message.Equals("Approved") ? true : false;
            response.ErrorMsg = responseObject.Transaction.Response.ErrorMsg;

            return response;
        }

        internal override PosResponse Void(PosRequest posRequest)
        {
            throw new NotImplementedException();
        }

        internal override PosResponse ReFund(PosRequest posRequest)
        {
            throw new NotImplementedException();
        }

        #region Private Functions
        private GVPSRequest CreateRequest(PosRequest posRequest)
        {
            string SecurityData = GetSHA1(_password + String.Format("000{0}", _terminalId))
                            .ToUpper(); //_terminalId'nin 9 haneye tamamlanması gerekiyor.

            string _HashData = GetSHA1(posRequest.OrderId + _terminalId + posRequest.CardNumber + posRequest.Amount + SecurityData)
                .ToUpper();

            var gvpRequest = new GVPSRequest();
            gvpRequest.Mode = _workingMode;
            gvpRequest.Version = GVPVERSION;

            gvpRequest.Terminal = new Terminal()
            {
                ID = _terminalId,
                UserID = posRequest.UserId,
                MerchantID = _merchantId,
                ProvUserID = _provisionUser,
                HashData = _HashData
            };

            gvpRequest.Customer = new Customer()
            {
                EmailAddress = posRequest.UserEmail,
                IPAddress = posRequest.UserIp
            };

            gvpRequest.Card = new Card()
            {
                Number = posRequest.CardNumber,
                CVV2 = posRequest.Cvc2,
                ExpireDate = String.Format("{0}{1}", posRequest.Month, posRequest.Year) //MMYY
            };

            gvpRequest.Order = new Order()
            {
                OrderID = posRequest.OrderId,
                GroupID = string.Empty
            };

            gvpRequest.Transaction = new Transaction()
            {
                Type = "sales",
                CurrencyCode = posRequest.CurrencyCode,
                MotoInd = "N",
                InstallmentCnt = posRequest.InstallmentCount == 0 ? String.Empty :
                                            posRequest.InstallmentCount.ToString(),
                // 10,05 -> 1005
                Amount = posRequest.Amount,
                CardholderPresentCode = "0"
            };

            return gvpRequest;
        }

        private string GetSHA1(string SHA1Data)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            string HashedPassword = SHA1Data;
            byte[] hashbytes = Encoding.GetEncoding("ISO-8859-9").GetBytes(HashedPassword);
            byte[] inputbytes = sha.ComputeHash(hashbytes);
            return GetHexaDecimal(inputbytes);
        }

        private string GetHexaDecimal(byte[] bytes)
        {
            StringBuilder s = new StringBuilder();
            int length = bytes.Length;
            for (int n = 0; n <= length - 1; n++)
            {
                s.Append(String.Format("{0,2:x}", bytes[n]).Replace(" ", "0"));
            }

            return s.ToString();
        }
        #endregion
    }

    #region Request
    [Serializable]
    [XmlRoot("GVPSRequest", Namespace = null)]
    internal class GVPSRequest
    {
        [XmlElement]
        public string Mode { get; set; }

        [XmlElement]
        public string Version { get; set; }

        [XmlElement]
        public Terminal Terminal { get; set; }

        [XmlElement]
        public Customer Customer { get; set; }

        [XmlElement]
        public Card Card { get; set; }

        [XmlElement]
        public Order Order { get; set; }

        [XmlElement]
        public Transaction Transaction { get; set; }
    }

    internal class Terminal
    {
        [XmlElement]
        public string ProvUserID { get; set; }

        [XmlElement]
        public string HashData { get; set; }

        [XmlElement]
        public string UserID { get; set; }

        [XmlElement]
        public string ID { get; set; }

        [XmlElement]
        public string MerchantID { get; set; }

    }

    internal class Customer
    {
        [XmlElement]
        public string IPAddress { get; set; }

        [XmlElement]
        public string EmailAddress { get; set; }
    }

    internal class Card
    {

        [XmlElement]
        public string Number { get; set; }

        /// <summary>
        /// must be MMYY format.
        /// </summary>
        [XmlElement]
        public string ExpireDate { get; set; }

        [XmlElement]
        public string CVV2 { get; set; }

    }

    internal class Order
    {
        [XmlElement]
        public string OrderID { get; set; }

        [XmlElement]
        public string GroupID { get; set; }
    }

    internal class Transaction
    {
        [XmlElement]
        public string Type { get; set; }

        [XmlElement]
        public string InstallmentCnt { get; set; }

        [XmlElement]
        public string Amount { get; set; }

        [XmlElement]
        public string CurrencyCode { get; set; }

        [XmlElement]
        public string CardholderPresentCode { get; set; }

        [XmlElement]
        public string MotoInd { get; set; }
    }
    #endregion

    #region Response
    [Serializable]
    [XmlRoot("GVPSResponse", Namespace = null)]
    internal class GVPSResponse
    {
        [XmlElement("Order")]
        public ResponseOrder Order { get; set; }

        [XmlElement(ElementName = "Transaction")]
        public ResponseTransaction Transaction { get; set; }
    }
    internal class ResponseOrder
    {
        [XmlElement]
        public string OrderID { get; set; }
    }

    internal class ResponseTransaction
    {
        [XmlElement]
        public Response Response { get; set; }

        [XmlElement]
        public string RetRefNum { get; set; }

        [XmlElement]
        public string AuthCode { get; set; }

        [XmlElement]
        public string BatchNum { get; set; }

        [XmlElement]
        public string SequenceNum { get; set; }

        [XmlElement]
        public string ProvDate { get; set; }

        [XmlElement]
        public string HostMsgList { get; set; }

        [XmlElement]
        public RewardInqResult RewardInqResult { get; set; }
    }

    internal class Response
    {

        [XmlElement]
        public string Source { get; set; }

        [XmlElement]
        public string Code { get; set; }

        [XmlElement]
        public string ReasonCode { get; set; }

        [XmlElement]
        public string Message { get; set; }

        [XmlElement]
        public string ErrorMsg { get; set; }

        [XmlElement]
        public string SysErrMsg { get; set; }
    }

    internal class RewardInqResult
    {
        [XmlElement]
        public string RewardList { get; set; }

        [XmlElement]
        public string ChequeList { get; set; }
    }
    #endregion
}
