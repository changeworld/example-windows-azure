using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.UI.WebControls;

using Microsoft.AspNet.Membership.OpenAuth;

namespace ExampleWebRole.Account
{
    public partial class Manage : System.Web.UI.Page
    {
        protected string SuccessMessage
        {
            get;
            private set;
        }

        protected bool CanRemoveExternalLogins
        {
            get;
            private set;
        }

        protected void Page_Load()
        {
            if (!IsPostBack)
            {
                // レンダリングするセクションを判別します
                var hasLocalPassword = OpenAuth.HasLocalPassword(User.Identity.Name);
                setPassword.Visible = !hasLocalPassword;
                changePassword.Visible = hasLocalPassword;

                CanRemoveExternalLogins = hasLocalPassword;

                // 成功メッセージをレンダリングします
                var message = Request.QueryString["m"];
                if (message != null)
                {
                    // アクションからクエリ文字列を削除します
                    Form.Action = ResolveUrl("~/Account/Manage");

                    SuccessMessage =
                        message == "ChangePwdSuccess" ? "パスワードが変更されました。"
                        : message == "SetPwdSuccess" ? "パスワードが設定されました。"
                        : message == "RemoveLoginSuccess" ? "外部ログインが削除されました。"
                        : String.Empty;
                    successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
                }
            }


            // 外部アカウントの一覧をデータバインドします
            var accounts = OpenAuth.GetAccountsForUser(User.Identity.Name);
            CanRemoveExternalLogins = CanRemoveExternalLogins || accounts.Count() > 1;
            externalLoginsList.DataSource = accounts;
            externalLoginsList.DataBind();

        }

        protected void setPassword_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                var result = OpenAuth.AddLocalPassword(User.Identity.Name, password.Text);
                if (result.IsSuccessful)
                {
                    Response.Redirect("~/Account/Manage?m=SetPwdSuccess");
                }
                else
                {

                    newPasswordMessage.Text = result.ErrorMessage;

                }
            }
        }


        protected void externalLoginsList_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            var providerName = (string)e.Keys["ProviderName"];
            var providerUserId = (string)e.Keys["ProviderUserId"];
            var m = OpenAuth.DeleteAccount(User.Identity.Name, providerName, providerUserId)
                ? "?m=RemoveLoginSuccess"
                : String.Empty;
            Response.Redirect("~/Account/Manage" + m);
        }

        protected T Item<T>() where T : class
        {
            return GetDataItem() as T ?? default(T);
        }


        protected static string ConvertToDisplayDateTime(DateTime? utcDateTime)
        {
            // このメソッドを変更すると、UTC の日付と時刻を必要な表示のオフセットと形式に
            // 変換できます。ここでは、現在のスレッド カルチャを使用して、短い日付および長い時間の文字列として、
            // それをサーバーのタイムゾーンと書式設定に変換しています。
            return utcDateTime.HasValue ? utcDateTime.Value.ToLocalTime().ToString("G") : "[しない]";
        }
    }
}