namespace VPosTR.Api
{
    public class PosResponse
    {
        public bool Status { get; set; }

        public string OrderId { get; set; }
        public string StatusCode { get; set; }
        public string Message { get; set; }
        public string ErrorMsg { get; set; }
        public string RequestTextData { get; set; }
        public string ResponseTextData { get; set; }
    }
}
