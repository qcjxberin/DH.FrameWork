﻿using PluginCore.Interfaces;

namespace PluginCore.AspNetCore.Middlewares
{
    /// <summary>
    /// 一定在 PluginCore 添加的中间件中 最后一个
    /// </summary>
    public class PluginHttpEndFilterMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly IPluginFinder _pluginFinder;

        public PluginHttpEndFilterMiddleware(
            RequestDelegate next,
            IWebHostEnvironment hostingEnv,
            ILoggerFactory loggerFactory,
            IPluginFinder pluginFinder)
        {
            _next = next;
            _pluginFinder = pluginFinder;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            var httpMethod = httpContext.Request.Method;
            var path = httpContext.Request.Path.Value;

            // 在请求下一个 middleware 前过滤

            // Call the next delegate/middleware in the pipeline
            await _next(httpContext);

            // middleware 回退时 过滤
            await Filter(httpContext);
        }

        private async Task Filter(HttpContext httpContext)
        {
            var plugins = this._pluginFinder.EnablePlugins<PluginCore.IPlugins.IHttpFilterPlugin>().ToList();

            foreach (var item in plugins)
            {
                // 调用
                await item?.HttpEndFilter(httpContext);
            }
        }



    }
}
