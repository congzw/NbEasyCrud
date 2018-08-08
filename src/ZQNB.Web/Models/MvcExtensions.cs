using System.Web.Mvc;

namespace ZQNB.Web.Models
{
    /// <summary>
    /// Mvc Extensions
    /// </summary>
    public static class MvcExtensions
    {
        /// <summary>
        /// 获取当前的controller
        /// </summary>
        /// <param name="webPage"></param>
        /// <returns></returns>
        public static string GetControllerName(this WebViewPage webPage)
        {
            var controllerType = webPage.ViewContext.Controller.GetType();
            var controllerName = controllerType.Name.Replace("Controller", "");
            return controllerName;
        }
    }
}
