using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyWPService
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
    
                
        }

        [WebMethod]
        public string getJson() {
            return "{'Name' : 'Marija'}";
        }

        protected void btnPostNoParameters_Click(object sender, EventArgs e)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create("http://localhost/WP_AppsService/WPService.asmx/HelloWorld");
            
            //Add these, as we're doing a POST
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            //We need to count how many bytes we're sending. Post'ed Faked Forms should be name=value&
            
            req.ContentLength = 0;
           
            System.Net.WebResponse resp = req.GetResponse();
            if (resp == null)
                lblText.Text = "NULL";
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            lblText.Text = sr.ReadToEnd().Trim();
        }

        protected void btnPostWithParameters_Click(object sender, EventArgs e)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create("http://localhost/WP_AppsService/WPService.asmx/HelloNumber");
            //req.Proxy = new System.Net.WebProxy(ProxyString, true);
            //Add these, as we're doing a POST
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            //We need to count how many bytes we're sending. Post'ed Faked Forms should be name=value&
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes("broj=3");
            req.ContentLength = bytes.Length;
            System.IO.Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length); //Push it out there
            os.Close();
            System.Net.WebResponse resp = req.GetResponse();
            if (resp == null)
                lblText.Text = "NULL";
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            lblText.Text = sr.ReadToEnd().Trim();
        }


    }
}