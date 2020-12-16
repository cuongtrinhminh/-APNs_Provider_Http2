using System;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Net.Http;
using System.Net;

namespace APNs_Provider_Http2
{
    class Program
    {
        private const string HOSTNAME = "gateway.sandbox.push.apple.com";
        private const int PORT = 443;

        static void Main(string[] args)
        {
            PushNotification("", "", "", "", 0);
        }
        
        private static void PushNotification(String certificatePath, String certificatePassword, String deviceToken, String notificationContent, int badgeNumber)
        {
            string payload = ((Convert.ToString("{\"aps\":{\"alert\":\"") + "\" " + notificationContent + "\"") + ("\",\"badge\":" + badgeNumber + ",\"sound\":\"default\",\"category\":\"remind.category\""));
            X509Certificate2 clientCertificate = new X509Certificate2(File.ReadAllBytes(certificatePath), certificatePassword, X509KeyStorageFlags.MachineKeySet);

            ByteArrayContent bytePayload = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(payload));

            string url = "https://" + HOSTNAME + ":" + PORT + "/3/device/" + deviceToken;

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(url));
            httpRequestMessage.Version = new Version(2, 0);
            httpRequestMessage.Content = bytePayload;

            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.ClientCertificates.Add(clientCertificate);

            HttpClient httpClient = new HttpClient(httpClientHandler);

            HttpResponseMessage httpResponseMessage = httpClient.SendAsync(httpRequestMessage).Result;
        }
    }
}
