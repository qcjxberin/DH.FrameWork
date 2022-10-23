﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DH.Core.Infrastructure
{
    /// <summary>
    /// 表示应用程序启动时配置服务和中间件的对象
    /// </summary>
    public partial interface IDHStartup
    {
        /// <summary>
        /// 添加并配置任何中间件
        /// </summary>
        /// <param name="services">服务描述符集合</param>
        /// <param name="configuration">应用程序的配置</param>
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);

        /// <summary>
        /// 配置添加的中间件的使用
        /// </summary>
        /// <param name="application">用于配置应用程序的请求管道的生成器</param>
        void Configure(IApplicationBuilder application);

        /// <summary>
        /// 获取此启动配置实现的顺序
        /// </summary>
        int Order { get; }
    }
}