namespace SKIT.FlurlHttpClient.ByteDance.DouyinOpen.Models
{
    /// <summary>
    /// <para>表示 [POST] /goodlife/v1/poi/poi/claim 接口的响应。</para>
    /// </summary>
    public class GoodLifePOIClaimV1Response : DouyinOpenResponse<GoodLifePOIClaimV1Response.Types.Data>
    {
        public static class Types
        {
            public class Data : DouyinOpenResponseData
            {
                public static class Types
                {
                    public class Task : GoodLifePOISyncV1Response.Types.Data.Types.Task
                    {
                    }
                }

                /// <summary>
                /// 获取或设置任务列表。
                /// </summary>
                [Newtonsoft.Json.JsonProperty("tasks")]
                [System.Text.Json.Serialization.JsonPropertyName("tasks")]
                public Types.Task[] TaskList { get; set; } = default!;
            }
        }
    }
}
