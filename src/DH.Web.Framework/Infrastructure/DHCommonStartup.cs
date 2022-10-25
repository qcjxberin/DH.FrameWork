﻿using DH.Core.Infrastructure;
using DH.Web.Framework.Infrastructure.Extensions;
using DH.Web.Framework.Mvc.Routing;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DH.Web.Framework.Infrastructure
{
    /// <summary>
    /// 表示用于在应用程序启动时配置公共功能和中间件的对象
    /// </summary>
    public partial class DHCommonStartup : IDHStartup
    {
        /// <summary>
        /// 添加并配置任何中间件
        /// </summary>
        /// <param name="services">服务描述符集合</param>
        /// <param name="configuration">应用程序的配置</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // 添加选项功能
            services.AddOptions();

            // 添加分布式缓存
            services.AddDistributedCache();

            // 添加HTTP会话状态功能
            services.AddHttpSession();

            // 添加默认HTTP客户端
            services.AddDHHttpClients();

            // 添加防伪
            services.AddAntiForgery();

            // 添加主题支持
            services.AddThemes();

            // 添加路由
            services.AddRouting(options =>
            {
                // 为语言添加约束键
                options.ConstraintMap[DHRoutingDefaults.LanguageParameterTransformer] = typeof(LanguageParameterTransformer);
            });
        }

        /// <summary>
        /// 配置添加的中间件的使用
        /// </summary>
        /// <param name="application">用于配置应用程序的请求管道的生成器</param>
        public void Configure(IApplicationBuilder application)
        {
            // 检查请求的页面是否为保持活动页面
            application.UseKeepAlive();

            // 检查数据库是否已安装
            application.UseInstallUrl();

            // 使用HTTP会话
            application.UseSession();

            // 使用请求本地化
            application.UseDHRequestLocalization();
        }

        /// <summary>
        /// 获取此启动配置实现的顺序
        /// </summary>
        public int Order => 100; // 应在错误处理程序之后加载公共服务
    }
}
