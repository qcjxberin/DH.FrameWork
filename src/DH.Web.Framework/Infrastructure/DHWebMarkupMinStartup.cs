﻿using DH.Core.Infrastructure;
using DH.Web.Framework.Infrastructure.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DH.Web.Framework.Infrastructure
{
    /// <summary>
    /// 表示用于在应用程序启动时配置WebMarkupMin服务的对象
    /// </summary>
    public partial class DHWebMarkupMinStartup : IDHStartup
    {
        /// <summary>
        /// 添加并配置任何中间件
        /// </summary>
        /// <param name="services">服务描述符集合</param>
        /// <param name="configuration">应用程序的配置</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // 将WebMarkupMin服务添加到服务容器
            services.AddDHWebMarkupMin();
        }

        /// <summary>
        /// 配置添加的中间件的使用
        /// </summary>
        /// <param name="application">用于配置应用程序的请求管道的生成器</param>
        public void Configure(IApplicationBuilder application)
        {
            // 使用WebMarkupMin
            application.UseDHWebMarkupMin();
        }

        /// <summary>
        /// 获取此启动配置实现的顺序
        /// </summary>
        public int Order => 300; // 确保在"UseRouting"之前调用"UseDHWebMarkupMin"方法。否则，HTML缩小将无法工作
    }
}