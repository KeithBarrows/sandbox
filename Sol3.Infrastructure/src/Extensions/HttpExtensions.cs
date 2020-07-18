using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Newtonsoft.Json;

namespace Sol3.Infrastructure.Extensions
{
    public static class HttpExtensions
    {
        /// <summary>
        /// Posts to queue as text.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="payload">The payload.</param>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="mediaType">OPTIONAL:  Defaults to 'text/plain', override with a different media type if so desired</param>
        public static void PostToQueueAsText<T>(this T payload, string requestUri, string mediaType = "text/plain") where T : class
        {
            // Create HttpCient and make a request to endpoint...
            var client = new HttpClient();
            var jsonPayload = JsonConvert.SerializeObject(payload);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            var httpContent = new StringContent(jsonPayload, System.Text.Encoding.UTF8, mediaType);

            ThreadPool.QueueUserWorkItem(callback => client.PostAsync(requestUri, httpContent));
        }
        //public static async Task<string> ExecuteHttpRequest<T>(this T values, string requestUri, string mediaType = "text/plain") where T : Dictionary<string, string>
        //{
        //    var content = new FormUrlEncodedContent(values);

        //    using (var client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
        //        var httpResponseMessage = await client.PostAsync(requestUri, content);

        //        if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
        //        {
        //            var results = await httpResponseMessage.Content.ReadAsStringAsync();
        //            return await new Task<string>(() => results);
        //        }
        //        throw new Exception($"httpResponseMessage.StatusCode = {httpResponseMessage.StatusCode}");
        //    }
        //}
        public static string ExecuteHttpRequest<T>(this T values, string requestUri, string mediaType = "text/plain") where T : Dictionary<string, string>
        {
            var uri = requestUri;
            var token = string.Empty;
            foreach (var kvp in values)
            {
                uri = $"{uri}{token}{kvp.Key}={kvp.Value}";
                token = "&";
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
                var httpResponseMessage = client.PostAsync(uri, null).Result;

                if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"httpResponseMessage.StatusCode = {httpResponseMessage.StatusCode}");
                var results = httpResponseMessage.Content.ReadAsStringAsync().Result;
                return results;
            }
        }
    }
}
