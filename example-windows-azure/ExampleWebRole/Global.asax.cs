using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using ExampleWebRole;

namespace ExampleWebRole
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // アプリケーションのスタートアップで実行するコードです
            AuthConfig.RegisterOpenAuth();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        void Application_End(object sender, EventArgs e)
        {
            //  アプリケーションのシャットダウンで実行するコードです

        }

        void Application_Error(object sender, EventArgs e)
        {
            // ハンドルされていないエラーが発生したときに実行するコードです

        }
    }
}
