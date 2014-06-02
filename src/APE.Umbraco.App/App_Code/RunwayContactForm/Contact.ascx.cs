using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace UmbracoShop.Controls {
    public partial class Contact : System.Web.UI.UserControl {

        private string subject;
        public string Subject {
            get { return subject; }
            set { subject = value; }
        }


        private string yourEmail;
        public string YourEmail {
            get { return yourEmail; }
            set { yourEmail = value; }
        }

        
        protected void Page_Load(object sender, EventArgs e) {

        }

        protected void sendMail(object sender, EventArgs e) {
            Page.Validate();

            if (Page.IsValid) {
                umbraco.library.SendMail(tb_email.Text, yourEmail, subject, tb_msg.Text, false);
                bt_submit.Visible = false;
                lb_success.Visible = true;                
            }
        }
    }
}