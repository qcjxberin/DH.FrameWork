using DH.Core;
using DH.Core.Caching;
using DH.Core.Configuration;
using DH.Core.Events;
using DH.Core.Infrastructure;
using DH.Services.Common;
using DH.Services.Configuration;
using DH.Services.Customers;
using DH.Services.Events;
using DH.Services.Helpers;
using DH.Services.Localization;
using DH.Services.Plugins;
using DH.Services.Plugins.Marketplace;
using DH.Services.ScheduleTasks;
using DH.Services.Seo;
using DH.Services.Themes;
using DH.VirtualFileSystem;
using DH.Web.Framework.Mvc.Routing;
using DH.Web.Framework.Themes;
using DH.Web.Framework.UI;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using NewLife.Caching;

namespace DH.Web.Framework.Infrastructure;

/// <summary>
/// 表示应用程序启动时注册服务
/// </summary>
public partial class DHStartup : IDHStartup
{
    /// <summary>
    /// 文件提供程序
    /// </summary>
    /// <param name="application">用于配置应用程序的请求管道的生成器</param>
    /// <param name="typeFinder">类型处理器</param>
    public void Configure(IApplicationBuilder application, ITypeFinder typeFinder)
    {
    }

    /// <summary>
    /// 添加并配置任何中间件
    /// </summary>
    /// <param name="services">服务描述符集合</param>
    /// <param name="configuration">应用程序的配置</param>
    /// <param name="startups">查找到的IDHStartup集合</param>
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration, IEnumerable<IDHStartup> startups, IWebHostEnvironment webHostEnvironment)
    {
        // 文件提供程序
        services.AddScoped<IDHFileProvider, DHFileProvider>();

        // web助手
        services.AddScoped<IWebHelper, WebHelper>();

        //UserAgent帮助
        services.AddScoped<IUserAgentHelper, UserAgentHelper>();

        // 插件
        services.AddScoped<IPluginService, PluginService>();
        services.AddScoped<OfficialFeedManager>();

        // 静态缓存管理器
        var appSettings = Singleton<AppSettings>.Instance;
        var distributedCacheConfig = appSettings.Get<DistributedCacheConfig>();
        if (distributedCacheConfig.Enabled)
        {
            switch (distributedCacheConfig.DistributedCacheType)
            {
                case DistributedCacheType.Memory:
                    services.AddScoped<ILocker, MemoryDistributedCacheManager>();
                    services.AddScoped<IStaticCacheManager, MemoryDistributedCacheManager>();
                    break;
                case DistributedCacheType.SqlServer:
                    services.AddScoped<ILocker, MsSqlServerCacheManager>();
                    services.AddScoped<IStaticCacheManager, MsSqlServerCacheManager>();
                    break;
                case DistributedCacheType.Redis:
                    services.AddScoped<ILocker, RedisCacheManager>();
                    services.AddScoped<IStaticCacheManager, RedisCacheManager>();
                    break;
            }
        }
        else
        {
            services.AddSingleton<ILocker, MemoryCacheManager>();
            services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();
        }

        // 工作上下文
        services.AddScoped<IWorkContext, WebWorkContext>();

        // 站点上下文
        services.AddScoped<IStoreContext, WebStoreContext>();



        // 系统应用缓存，小于10000数据的可以考虑直接使用Cache.Default
        if (UtilSetting.Current.RedisEnabled)
        {
            var redisConn = new FullRedis(UtilSetting.Current.RedisConnectionString, UtilSetting.Current.RedisPassWord, UtilSetting.Current.RedisDatabaseId);

            services.TryAddSingleton<ICache>(redisConn);
            services.TryAddSingleton(redisConn);
        }
        else
        {
            services.TryAddSingleton<ICache>(Cache.Default);
        }

        services.AddScoped<IGenericAttributeService, GenericAttributeService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IDHHtmlHelper, DHHtmlHelper>();
        services.AddScoped<ILocalizationService, LocalizationService>();
        services.AddScoped<IUrlRecordService, UrlRecordService>();
        services.AddScoped<IThemeProvider, ThemeProvider>();
        services.AddScoped<IThemeContext, ThemeContext>();
        services.AddSingleton<IRoutePublisher, RoutePublisher>();
        services.AddSingleton<IEventPublisher, EventPublisher>();
        services.AddScoped<ISettingService, SettingService>();


        //plugin managers
        services.AddScoped(typeof(IPluginManager<>), typeof(PluginManager<>));


        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        // 注册所有设置
        var typeFinder = Singleton<ITypeFinder>.Instance;

        var settings = typeFinder.FindClassesOfType(typeof(ISettings), false).ToList();
        foreach (var setting in settings)
        {
            services.AddScoped(setting, serviceProvider =>
            {
                var storeId = DHSetting.Current.IsInstalled
                    ? serviceProvider.GetRequiredService<IStoreContext>().GetCurrentStore()?.Id ?? 0
                    : 0;

                return serviceProvider.GetRequiredService<ISettingService>().LoadSetting(setting, storeId);
            });
        }


        // 分段路由处理
        if (DHSetting.Current.IsInstalled)
            services.AddScoped<SlugRouteTransformer>();

        // 计划任务
        services.AddSingleton<ITaskScheduler, DH.Services.ScheduleTasks.TaskScheduler>();
        services.AddTransient<IScheduleTaskRunner, ScheduleTaskRunner>();

        // 事件消费者
        var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
        foreach (var consumer in consumers)
            foreach (var findInterface in consumer.FindInterfaces((type, criteria) =>
            {
                var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                return isMatch;
            }, typeof(IConsumer<>)))
                services.AddScoped(findInterface, consumer);

    }

    /// <summary>
    /// 配置虚拟文件系统
    /// </summary>
    /// <param name="options">虚拟文件配置</param>
    public void ConfigureVirtualFileSystem(DHVirtualFileSystemOptions options)
    {
    }

    /// <summary>
    /// 注册路由
    /// </summary>
    /// <param name="endpoints">路由生成器</param>
    public void UseDHEndpoints(IEndpointRouteBuilder endpoints)
    {
    }

    /// <summary>
    /// 将区域路由写入数据库
    /// </summary>
    public void ConfigureArea()
    {

    }

    /// <summary>
    /// 调整菜单
    /// </summary>
    public void ChangeMenu()
    {

    }

    /// <summary>
    /// 升级处理逻辑
    /// </summary>
    public void Update()
    {

    }

    /// <summary>
    /// 获取此启动配置实现的顺序
    /// </summary>
    public int Order => 2000;
}
