using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VPosTR.Api;
using VPosTR.Api.VirtualPos;

namespace Usage
{
    class Program
    {
        static void Main(string[] args)
        {
            VPosContext context = new VPosContext(new GarantiVPos("123123", "PAUHT", "P@ssW0rd", "008871", "htttp://garantisanalpos.com"));

            var request = new PosRequest();
            request.Amount = "1000";
            request.CardNumber = "2039023902909309";
            request.CardOnName = "Oğuzhan YILMAZ";
            request.CurrencyCode = "989";
            request.Cvc2 = "000";
            request.InstallmentCount = 3;
            request.Month = "12";
            request.Year = "14";
            request.UserIp = "192.168.2.1";
            request.UserEmail = "aspsrc@gmail.com";
            request.UserId = Guid.NewGuid().ToString("N");
            
           

            Console.Read();
        }
    }
}
