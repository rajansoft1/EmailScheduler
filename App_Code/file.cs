using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace school
{
    public class file
    {
        public static  int  writefile(string txtfilepath,string text)

        {
            System.IO.StreamWriter fp;

            try
            {
                fp = System.IO.File.CreateText(txtfilepath);
                fp.WriteLine(text );

                fp.Close();
                return 1;
            }
            catch (Exception e1)
            {
                return -1;
            }
        }
        public static  string  getdata(string filepath)
        {
            try
            {

               // string pid = Request.QueryString["page"].ToString();
               // string filepath = Server.MapPath("~/bus" + "\\" + Request.QueryString["page"].ToString() + ".aspx.txt");
                System.IO.StreamReader re = System.IO.File.OpenText(filepath);
                string ret = re.ReadToEnd();
                //TextBox1.Text = Request.QueryString["page"].ToString();
                re.Close();
                return ret;
            }
            catch (Exception e)
            {
                return e.Message;
            }
                
        }
    }
}
