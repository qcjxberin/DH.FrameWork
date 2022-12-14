using DH.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;

namespace DH.Web.Framework.Themes
{
    /// <summary>
    /// 指定Microsoft.AspNetCore.Mvc.Razor使用的视图位置扩展器的合同。RazorViewEngine实例来确定视图的搜索路径。
    /// </summary>
    public class ThemeableViewLocationExpander : IViewLocationExpander
    {
        private const string THEME_KEY = "dh.themename";

        /// <summary>
        /// 由Microsoft.AspNetCore.Mvc.Razor调用。RazorViewEngine确定此Microsoft.AspNetCore.Mvc.Razor.IViewLocationExpander实例将使用的值。
        /// 计算值用于确定视图位置自上次定位以来是否已更改。
        /// </summary>
        /// <param name="context">Context</param>
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            // 根本不需要添加可主题化的视图位置，因为管理无论如何都不应该是可主题化
            if (context.AreaName?.Equals(AreaNames.Admin) ?? false)
                return;

            context.Values[THEME_KEY] = EngineContext.Current.Resolve<IThemeContext>().GetWorkingThemeNameAsync().Result;
        }

        /// <summary>
        /// Invoked by a Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine to determine potential locations for a view.
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="viewLocations">View locations</param>
        /// <returns>View locations</returns>
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (context.Values.TryGetValue(THEME_KEY, out string theme))
            {
                viewLocations = new[] {
                        $"/Themes/{theme}/Views/{{1}}/{{0}}.cshtml",
                        $"/Themes/{theme}/Views/Shared/{{0}}.cshtml",
                    }
                    .Concat(viewLocations);
            }


            return viewLocations;
        }
    }
}
