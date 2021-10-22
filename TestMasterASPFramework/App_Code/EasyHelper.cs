using System.Web;

namespace TestMasterASPFramework.App_Code
{
    public static class EasyExtensions
    {
        public static IHtmlString EasyHelper()
        {
            var str = @"<div>{0}</div>";
            var x = new HtmlString(str);
            return x;
        }
    }
}