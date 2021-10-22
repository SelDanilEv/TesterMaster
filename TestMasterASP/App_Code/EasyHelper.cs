using Microsoft.AspNetCore.Html;

namespace TestMasterASP.App_Code
{
    public static class EasyExtensions
    {
        public static HtmlString EasyHelper()
        {
            var str = @"<div>{0}</div>";
            var x = new HtmlString(str);
            return x;
        }
    }
}