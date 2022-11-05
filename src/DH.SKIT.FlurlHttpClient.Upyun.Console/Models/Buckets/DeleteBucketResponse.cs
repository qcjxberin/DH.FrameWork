﻿namespace SKIT.FlurlHttpClient.Upyun.Console.Models
{
    /// <summary>
    /// <para>表示 [POST] /buckets/delete 接口的响应。</para>
    /// </summary>
    public class DeleteBucketResponse : UpyunConsoleResponse
    {
        /// <summary>
        /// 获取或设置操作结果。
        /// </summary>
        [Newtonsoft.Json.JsonProperty("result")]
        [System.Text.Json.Serialization.JsonPropertyName("result")]
        public bool Result { get; set; }
    }
}