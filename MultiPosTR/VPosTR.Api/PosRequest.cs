namespace VPosTR.Api
{
    public class PosRequest
    {
        public string OrderId { get; set; }

        public string CardNumber { get; set; }
        public string CardOnName { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Cvc2 { get; set; }
        public string CurrencyCode { get; set; }
        public string UserId { get; set; }
        public string UserIp { get; set; }
        public string UserEmail { get; set; }
        public string Amount { get; set; }
        public int InstallmentCount { get; set; } //Taksit Sayısı
    }
}
