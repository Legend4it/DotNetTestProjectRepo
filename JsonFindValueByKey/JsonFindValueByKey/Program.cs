using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonFindValueByKey
{
    class Program
    {
        static void Main(string[] args)
        {
            string jsonData = "{\"OrderNr\":\"12345\",\"Amout\":\"100\",\"PartnerId\":\"1\",\"EcrId\":\"EcrA\",\"RequestType\":\"Fail\",\"ResultObject\":{\"EcrId\":\"EcrA\",\"TerminalId\":\"346-742-881\",\"ServiceId\":\"2006427017\",\"TransactionType\":0,\"EcrTransactionId\":{\"Id\":\"12345\",\"Timestamp\":\"2020-11-06T12:39:33.695+01:00\"},\"TerminalTransactionId\":{\"Id\":\"010303467428812020110616046627860000149000001\",\"Timestamp\":\"2020-11-06T12:39:46+01:00\"},\"AcquirerMerchantId\":\"35000074\",\"AcquirerTerminalId\":\"46742881\",\"AcquirerTransactionId\":{\"Id\":\"201106000149\",\"Timestamp\":\"2020-11-06T12:39:46+01:00\"},\"OriginalAcquirerTransactionId\":null,\"TerminalReconciliationId\":null,\"AuthorizedAmount\":100,\"CashbackAmount\":0,\"TipAmount\":0,\"Online\":true,\"AuthenticationMethod\":4,\"ApprovalCode\":\"000211\",\"PaymentInstrumentData\":{\"PaymentInstrumentType\":0,\"CardData\":{\"PaymentBrand\":\"VISA\",\"MaskedPan\":\"476173******0211\",\"EntryMode\":9,\"CardCountryCode\":null,\"SensitiveCardData\":null},\"AlternativePaymentData\":null},\"Receipts\":[{\"DocumentType\":1,\"IntegratedPrint\":false,\"SignatureRequired\":false,\"Content\":{\"Format\":0,\"Text\":{\"FormattedTexts\":[{\"Text\":\"Verifone Sweden AB\r\n\"},{\"Text\":\"Ljusslingan 4\r\n\"},{\"Text\":\"Stockholm\r\n\"},{\"Text\":\"Phone:      123412341234\r\n\"},{\"Text\":\"Bus.Reg.No:     12341234\r\n\"},{\"Text\":\"\r\n\"},{\"Text\":\"2020-11-06         12:39\r\n\"},{\"Text\":\"\r\n\"},{\"Text\":\"PURCHASE      SEK 100.00\r\n\"},{\"Text\":\"PIN USED\r\n\"},{\"Text\":\"VISA DEBIT\r\n\"},{\"Text\":\"VISA CONTACTLESS\r\n\"},{\"Text\":\"XXXX XXXX XXXX 0211\r\n\"},{\"Text\":\"TERM:    46742881-000149\r\n\"},{\"Text\":\"Evry\r\n\"},{\"Text\":\"35000074\r\n\"},{\"Text\":\"KA1\r\n\"},{\"Text\":\"ATC:00362           AED:\r\n\"},{\"Text\":\"AID:      A0000000031010\r\n\"},{\"Text\":\"ARC:00        STATUS:000\r\n\"},{\"Text\":\"AUTH CODE:        000211\r\n\"},{\"Text\":\"REF:000149\r\n\"},{\"Text\":\"Result:       AUTHORIZED\r\n\"},{\"Text\":\"      KEEP RECEIPT\r\n\"},{\"Text\":\"  CARDHOLDER'S RECEIPT\r\n\"},{\"Text\":\"\r\n\"},{\"Text\":\"\r\n\"},{\"Text\":\"\r\n\"}],\"PlainText\":\"Verifone Sweden AB\r\nLjusslingan 4\r\nStockholm\r\nPhone:      123412341234\r\nBus.Reg.No:     12341234\r\n\r\n2020-11-06         12:39\r\n\r\nPURCHASE      SEK 100.00\r\nPIN USED\r\nVISA DEBIT\r\nVISA CONTACTLESS\r\nXXXX XXXX XXXX 0211\r\nTERM:    46742881-000149\r\nEvry\r\n35000074\r\nKA1\r\nATC:00362           AED:\r\nAID:      A0000000031010\r\nARC:00        STATUS:000\r\nAUTH CODE:        000211\r\nREF:000149\r\nResult:       AUTHORIZED\r\n      KEEP RECEIPT\r\n  CARDHOLDER'S RECEIPT\r\n\r\n\r\n\r\n\"}}}],\"Tokens\":[],\"TokenResponses\":[]},\"TransactionFailureErrorType\":null,\"TransactionFailureResult\":{\"EcrId\":\"EcrA\",\"TerminalId\":\"346-742-881\",\"ServiceId\":\"2006427016\",\"Error\":6,\"AdditionalReason\":null,\"TransactionType\":0,\"EcrTransactionId\":{\"Id\":\"12345\",\"Timestamp\":\"2020-11-06T12:38:57.847+01:00\"},\"TerminalTransactionId\":{\"Id\":\"\",\"Timestamp\":\"2020-11-06T12:39:01+01:00\"},\"AcquirerTransactionId\":null,\"OriginalAcquirerTransactionId\":null,\"Online\":false,\"PaymentInstrumentData\":null,\"Receipts\":[],\"Tokens\":[],\"TokenResponses\":[]},\"TransactionResult\":null}";



            var data = (JObject)JsonConvert.DeserializeObject(jsonData);
            List<JToken> tokens = data.Children().ToList();
            //Console.WriteLine(data["ResultObject"].Value<string>());

            //var p = JObject.Parse(jsonData);
            //var p = JObject.Parse(jsonData).Children();
            //List<JToken> token = p.Children().ToList();
            //Console.WriteLine(p["ResultObject"]["Receipts"]);
            //Console.WriteLine(p["ResultObject"]["ApprovalCode"]);
            //Console.WriteLine(p["ResultObject"]["TransactionFailureErrorType"]);

            foreach (var token in tokens)
            {
                var x = token;
            }

            Console.ReadKey();



        }
    }
}
