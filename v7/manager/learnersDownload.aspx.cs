﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace portal.v7.facilitator
{
  public partial class learnersDownload : FormBase
  {
    private Cust cu = new Cust();
    private Memb me = new Memb();
    private Sess se = new Sess();
    private int membCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
      se.localize();

      if (!IsPostBack)
      {
        // get count of learners on file
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString))
        {
          con.Open();
          using (SqlCommand cmd = new SqlCommand())
          {
            cmd.Connection = con;
            cmd.CommandText = "dbo.sp7learnersExcelCount2";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@StrParm0", se.custId));
            cmd.Parameters.Add("@count", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();

            membCount = (int)cmd.Parameters["@count"].Value;
          }
        }
      }
      labCount.Text = membCount.ToString();
      if (membCount == 0)
      {
        butBegin.Visible = false;
      }
    }

    protected void butBegin_Click(object sender, EventArgs e)
    {
      Excel ex = new Excel(); // instantiate the excelwriter

      ex.DocName = Session["custId"].ToString() + "_Learners";
      ex.WsName = "Learners";                                     // this is the worksheet name which shows on the first tab at the bottom
      ex.SpName = "[apps].[dbo].[sp7learnersExcel2]";             // this is the stored proc that drives this - must be "excelWriter" friendly    

      ex.StrParms[0] = Session["custId"].ToString();

      ex.excelWriter();
    }

    protected void exit_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
      Response.Redirect("/portal/v7/default.aspx");
    }

  }
}