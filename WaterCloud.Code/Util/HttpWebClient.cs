﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WaterCloud.Code
{
	public class HttpWebClient
    {

        private IHttpClientFactory _httpClientFactory;
        public HttpWebClient(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Get异步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dicHeaders"></param>
        /// <param name="timeoutSecond"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(string url, Dictionary<string, string> dicHeaders, int timeoutSecond = 120)
        {
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            if (dicHeaders != null)
            {
                foreach (var header in dicHeaders)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
            client.Timeout = TimeSpan.FromSeconds(timeoutSecond);
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                throw new CustomerHttpException($"接口请求错误,错误代码{response.StatusCode}，错误原因{response.ReasonPhrase}");

            }
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestString"></param>
        /// <param name="dicHeaders"></param>
        /// <param name="timeoutSecond"></param>
        /// <returns></returns>
        public async Task<string> PostAsync(string url, string requestString, Dictionary<string, string> dicHeaders, int timeoutSecond)
        {
            var client = _httpClientFactory.CreateClient();
            var requestContent = new StringContent(requestString);
            if (dicHeaders != null)
            {
                foreach (var head in dicHeaders)
                {
                    requestContent.Headers.Add(head.Key, head.Value);
                }
            }
            client.Timeout = TimeSpan.FromSeconds(timeoutSecond);
            var response = await client.PostAsync(url, requestContent);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                throw new CustomerHttpException($"接口请求错误,错误代码{response.StatusCode}，错误原因{response.ReasonPhrase}");
            }
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestString"></param>
        /// <param name="dicHeaders"></param>
        /// <param name="timeoutSecond"></param>
        /// <returns></returns>
        public async Task<string> PutAsync(string url, string requestString, Dictionary<string, string> dicHeaders, int timeoutSecond)
        {
            var client = _httpClientFactory.CreateClient();
            var requestContent = new StringContent(requestString);
            if (dicHeaders != null)
            {
                foreach (var head in dicHeaders)
                {
                    requestContent.Headers.Add(head.Key, head.Value);
                }
            }
            client.Timeout = TimeSpan.FromSeconds(timeoutSecond);
            var response = await client.PutAsync(url, requestContent);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                throw new CustomerHttpException($"接口请求错误,错误代码{response.StatusCode}，错误原因{response.ReasonPhrase}");
            }
        }

        /// <summary>
        /// Patch异步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestString"></param>
        /// <param name="dicHeaders"></param>
        /// <param name="timeoutSecond"></param>
        /// <returns></returns>
        public async Task<string> PatchAsync(string url, string requestString, Dictionary<string, string> dicHeaders, int timeoutSecond)
        {
            var client = _httpClientFactory.CreateClient();
            var requestContent = new StringContent(requestString);
            if (dicHeaders != null)
            {
                foreach (var head in dicHeaders)
                {
                    requestContent.Headers.Add(head.Key, head.Value);
                }
            }
            client.Timeout = TimeSpan.FromSeconds(timeoutSecond);
            var response = await client.PatchAsync(url, requestContent);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                throw new CustomerHttpException($"接口请求错误,错误代码{response.StatusCode}，错误原因{response.ReasonPhrase}");
            }
        }
        public async Task<string> DeleteAsync(string url, Dictionary<string, string> dicHeaders, int timeoutSecond)
        {
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            if (dicHeaders != null)
            {
                foreach (var head in dicHeaders)
                {
                    request.Headers.Add(head.Key, head.Value);
                }
            }
            client.Timeout = TimeSpan.FromSeconds(timeoutSecond);
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                throw new CustomerHttpException($"接口请求错误,错误代码{response.StatusCode}，错误原因{response.ReasonPhrase}");
            }
        }
        /// <summary>
        /// 异步请求（通用）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="requestString"></param>
        /// <param name="dicHeaders"></param>
        /// <param name="timeoutSecond"></param>
        /// <returns></returns>
        public async Task<string> ExecuteAsync(string url, HttpMethod method, string requestString, Dictionary<string, string> dicHeaders, int timeoutSecond = 120,
            string accept = "application/json")
        {
            var client = _httpClientFactory.CreateClient(); 
            if (url.IndexOf('?')>-1)
			{
                url += "&v=" + DateTime.Now.ToString("yyyyMMddhhmmss");
            }
			else
			{
                url += "?v=" + DateTime.Now.ToString("yyyyMMddhhmmss");
            }
            var request = new HttpRequestMessage(method, url)
            {
                Content = new StringContent(requestString),
            };
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (dicHeaders != null)
            {
                foreach (var header in dicHeaders)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(accept) { CharSet = "utf-8" });
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                throw new CustomerHttpException($"接口请求错误,错误代码{response.StatusCode}，错误原因{response.ReasonPhrase}");
            }
        }

    }
    public class CustomerHttpException : Exception
    {
        public CustomerHttpException() : base()
        { }
        public CustomerHttpException(string message) : base(message)
        {

        }
    }
}
