using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CertManagment
{
    class Program
    {
        static void Main(string[] args)
        {
            X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

            X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
            X509Certificate2Collection fcollection = (X509Certificate2Collection)collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
            foreach (var item in fcollection)
            {
                var x = item;
                Console.Write(x.Issuer);
            }


            X509Certificate2Collection scollection = X509Certificate2UI.SelectFromCollection(fcollection, "Test Certificate Select", "Select a certificate from the following list to get information on that certificate", X509SelectionFlag.MultiSelection);
            Console.WriteLine("Number of certificates: {0}{1}", scollection.Count, Environment.NewLine);
            //foreach (X509Certificate2 x509 in scollection)
            //{
            //    try
            //    {
            //        byte[] rawdata = x509.RawData;
            //        //Console.WriteLine("Content Type: {0}{1}", X509Certificate2.GetCertContentType(rawdata), Environment.NewLine);
            //        //Console.WriteLine("Friendly Name: {0}{1}", x509.FriendlyName, Environment.NewLine);
            //        //Console.WriteLine("Certificate Verified?: {0}{1}", x509.Verify(), Environment.NewLine);
            //        //Console.WriteLine("Simple Name: {0}{1}", x509.GetNameInfo(X509NameType.SimpleName, true), Environment.NewLine);
            //        //Console.WriteLine("Signature Algorithm: {0}{1}", x509.SignatureAlgorithm.FriendlyName, Environment.NewLine);
            //        //Console.WriteLine("Public Key: {0}{1}", x509.PublicKey.Key.ToXmlString(false), Environment.NewLine);
            //        //Console.WriteLine("Certificate Archived?: {0}{1}", x509.Archived, Environment.NewLine);
            //        //Console.WriteLine("Length of Raw Data: {0}{1}", x509.RawData.Length, Environment.NewLine);
            //        Console.WriteLine("Vaidate: {0}{1}", x509.GetExpirationDateString(), Environment.NewLine);
            //        Console.WriteLine("Effective: {0}{1}", x509.GetEffectiveDateString(), Environment.NewLine);
            //        Console.WriteLine("IssuerName: {0}{1}", x509.IssuerName.ToString(), Environment.NewLine);
            //        Console.WriteLine("IssuerName: {0}{1}", x509.GetEffectiveDateString(), Environment.NewLine);
            //        X509Certificate2UI.DisplayCertificate(x509);
            //        x509.Reset();

            //    }
            //    catch (CryptographicException)
            //    {
            //        Console.WriteLine("Information could not be written out for this certificate.");
            //    }
            //}
            //store.Close();

            //Console.WriteLine("\r\nExists Certs Name and Location");
            //Console.WriteLine("------ ----- -------------------------");

            //foreach (StoreLocation storeLocation in (StoreLocation[])
            //    Enum.GetValues(typeof(StoreLocation)))
            //{
            //    foreach (StoreName storeName in (StoreName[])
            //        Enum.GetValues(typeof(StoreName)))
            //    {
            //        X509Store store = new X509Store(storeName, storeLocation);

            //        try
            //        {
            //            store.Open(OpenFlags.OpenExistingOnly);

            //            Console.WriteLine("Yes    {0,4}  {1}, {2}",
            //                store.Certificates.Count, store.Name, store.Location);
            //        }
            //        catch (CryptographicException)
            //        {
            //            Console.WriteLine("No           {0}, {1}",
            //                store.Name, store.Location);
            //        }
            //    }
            //    Console.WriteLine();
            //}
            // The path to the certificate.
            string Certificate = "C:/Dev/Repos/Source/DotNetTestProjectRepo/CertManagment/shop.apak.se-all.pfx";

            // Load the certificate into an X509Certificate object.
            X509Certificate cert = new X509Certificate();

            cert.Import(Certificate);

            // Get the value.
            string resultsTrue = cert.ToString(true);

            // Display the value to the console.
            Console.WriteLine(resultsTrue);

            // Get the value.
            string resultsFalse = cert.ToString(false);

            // Display the value to the console.
            Console.WriteLine(resultsFalse);
            Console.ReadKey();
        }
    }
}
