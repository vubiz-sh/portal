﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace portal
{
  public class Memb
  {
    #region Fields
    // memb db fields
    public int
      membNo = 0,
      membLevel = 2,
      membNoVisits = 0;

    public string
      membAcctId,
      membId,
      membPwd,
      membFirstName,
      membLastName,
      membEmail,
      membCust,
      membMemo,
      membOrganization,
      membNoHours,
      membOnline,
      membBrowser,
      membPrograms,
      membProgramsAdded,
      membEcomG2Alert,
      membDuration,
      membJobs,
      membAuth,
      membMyWorld,
      membLCMS,
      membChannel,
      membAlteredBy,
      membGuid;

    public DateTime?
      membAlteredOn,
      membFirstVisit,
      membLastVisit,
      membExpires;

    public bool?
      membActive,
      membInternal,
      membManager,
      membEcom;

    // memb misc fields (passwords are site wide)
    public bool
      membEof,
      membExists;

    public string
      // these are no longer used - internal values are embedded in SQL SP: sp6memberInsertSpecialsNew2
      membPassword2 = "VUV5_LRN",
      membPassword3 = "VUV5_FAC",
      membPassword4 = "VUV5_MGR",
      membPassword5 = "VUV5_ADM",

      membGuidTemp; // used to access Portal etc with a tempGuid (managed in apps.dbo.guids)

    #endregion

    #region Functions

    public void init()   // initialize memb before adding new record so old data is not includedd in new updates
    {
      membNo = 0;
      membLevel = 2;
      membNoVisits = 0;
      membActive = true;

      membAcctId = null;
      membId = null;
      membPwd = null;
      membFirstName = null;
      membLastName = null;
      membEmail = null;
      membFirstVisit = null;
      membLastVisit = null;
      membCust = null;
      membMemo = null;
      membOrganization = null;
      membNoHours = null;
      membExpires = null;
      membActive = null;
      membInternal = null;
      membBrowser = null;
      membPrograms = null;
      membProgramsAdded = null;
      membEcomG2Alert = null;
      membDuration = null;
      membJobs = null;
      membAuth = null;
      membMyWorld = null;
      membLCMS = null;
      membEcom = null;
      membChannel = null;
      membManager = null;
      membAlteredOn = null;
      membAlteredBy = null;
    }

    public bool memberByUserName(string membId)
    {
      using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString))
      {
        con.Open();
        using (SqlCommand cmd = new SqlCommand())
        {
          cmd.CommandTimeout = 100;
          cmd.Connection = con;
          cmd.CommandText = "dbo.sp6memberByUserName";
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add(new SqlParameter("@membId", membId));
          SqlDataReader drd = cmd.ExecuteReader();
          membExists = false;
          while (drd.Read())
          {
            membExists = true;
            membNo = (int)drd["Memb_No"];
            membId = drd["Memb_Id"].ToString();
            membFirstName = drd["Memb_FirstName"].ToString();
            membLastName = drd["Memb_LastName"].ToString();
            membEmail = drd["Memb_Email"].ToString();
            membGuid = drd["Memb_Guid"].ToString();
            membLevel = (int)drd["Memb_Level"];
          }
          drd.Close();
        }
      }
      return (membExists);
    }

    public bool memberById(string membAcctId, string membId)
    {
      using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString))
      {
        con.Open();
        using (SqlCommand cmd = new SqlCommand())
        {
          cmd.CommandTimeout = 100;
          cmd.Connection = con;
          cmd.CommandText = "dbo.sp6memberById";
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add(new SqlParameter("@membAcctId", membAcctId));
          cmd.Parameters.Add(new SqlParameter("@membId", membId));
          SqlDataReader drd = cmd.ExecuteReader();
          membExists = false;
          while (drd.Read())
          {
            membExists = true;
            membNo = (int)drd["membNo"];
            this.membId = drd["membId"].ToString();
            membFirstName = drd["membFirstName"].ToString();
            membLastName = drd["membLastName"].ToString();
            membEmail = drd["membEmail"].ToString();
            membGuid = drd["membGuid"].ToString();
            membLevel = (int)drd["membLevel"];
          }
          drd.Close();
        }
      }
      return (membExists);
    }

    public void memberByEmail(string membEmail, out int count, out string custId, out string membId, out string membPwd)
    {
      using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString))
      {
        con.Open();
        using (SqlCommand cmd = new SqlCommand())
        {
          cmd.Connection = con;
          cmd.CommandText = "dbo.sp7credentials";
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add(new SqlParameter("@membEmail", membEmail));

          cmd.Parameters.Add("@count", SqlDbType.Int).Direction = ParameterDirection.Output;
          cmd.Parameters.Add("@custId", SqlDbType.Char, 8).Direction = ParameterDirection.Output;
          cmd.Parameters.Add("@membId", SqlDbType.VarChar, 128).Direction = ParameterDirection.Output;
          cmd.Parameters.Add("@membPwd", SqlDbType.VarChar, 64).Direction = ParameterDirection.Output;

          cmd.ExecuteNonQuery();

          count = (int)cmd.Parameters["@count"].Value;
          custId = cmd.Parameters["@custId"].Value.ToString();
          membId = cmd.Parameters["@membId"].Value.ToString();
          membPwd = cmd.Parameters["@membPwd"].Value.ToString();


        }
      }

    }

    public string memberByGuid(string membGuidTemp)
    {
      string custId = null;

      using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString))
      {
        membExists = false;

        con.Open();
        using (SqlCommand cmd = new SqlCommand())
        {
          cmd.CommandTimeout = 100;
          cmd.Connection = con;
          cmd.CommandText = "dbo.sp6memberByGuid";
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add(new SqlParameter("@membGuid", membGuidTemp));
          SqlDataReader drd = cmd.ExecuteReader();
          // put values into the cust and memb class
          while (drd.Read())
          {
            custId = drd["custId"].ToString();
            membAcctId = drd["membAcctId"].ToString();
            membNo = (int)drd["membNo"];
            membId = drd["membId"].ToString();
            membFirstName = drd["membFirstName"].ToString();
            membLastName = drd["membLastName"].ToString();
            membEmail = drd["membEmail"].ToString();
            membGuid = drd["membGuid"].ToString();
            membLevel = (int)drd["membLevel"];
            membActive = (bool)drd["membActive"];

            membExists = true;
          }
          drd.Close();
        }
      }
      return (custId);
    }

    public string memberGuidTemp(string membGuid) // gets a temp guid so portal can go to NOP
    {
      using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString))
      {
        con.Open();
        using (SqlCommand cmd = new SqlCommand())
        {
          cmd.Connection = con;
          cmd.CommandText = "dbo.sp8guidGet";
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add(new SqlParameter("@guidLive", membGuid));
          cmd.Parameters.Add("@guidTemp", SqlDbType.VarChar, 38).Direction = ParameterDirection.Output;
          cmd.ExecuteNonQuery();
          string guidTemp = cmd.Parameters["@guidTemp"].Value.ToString();
          return (guidTemp);
        }
      }
    }

    public void memberInsertUpdate(string type) // type is either "Insert", "Update" or "InsertUpdate" (if twigging in SQL remember to update all 3 SPs)
    {
      using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString))
      {
        con.Open();
        using (SqlCommand cmd = new SqlCommand())
        {
          cmd.CommandTimeout = 100;
          cmd.Connection = con;
          cmd.CommandText = "dbo.sp6member" + type;
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add(new SqlParameter("@membAcctId", membAcctId));
          cmd.Parameters.Add(new SqlParameter("@membId", membId));
          cmd.Parameters.Add(new SqlParameter("@membPwd", membPwd));
          cmd.Parameters.Add(new SqlParameter("@membFirstName", membFirstName));
          cmd.Parameters.Add(new SqlParameter("@membLastName", membLastName));
          cmd.Parameters.Add(new SqlParameter("@membEmail", membEmail));
          cmd.Parameters.Add(new SqlParameter("@membOrganization", membOrganization));
          cmd.Parameters.Add(new SqlParameter("@membLevel", membLevel));
          cmd.ExecuteReader();
        }
      }
    }

    public void memberUpdate(string membAcctId, int membNo) // similiar to above, used in learnersUpload - needs programs and Memo
    {
      using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString))
      {
        con.Open();
        using (SqlCommand cmd = new SqlCommand())
        {
          cmd.Connection = con;
          cmd.CommandText = "dbo.sp7memberUpload";
          cmd.CommandType = CommandType.StoredProcedure;

          cmd.Parameters.Add(new SqlParameter("@membAcctId", membAcctId));
          cmd.Parameters.Add(new SqlParameter("@membId", membId));
          cmd.Parameters.Add(new SqlParameter("@membPwd", membPwd));
          cmd.Parameters.Add(new SqlParameter("@membFirstName", membFirstName));
          cmd.Parameters.Add(new SqlParameter("@membLastName", membLastName));
          cmd.Parameters.Add(new SqlParameter("@membEmail", membEmail));
          cmd.Parameters.Add(new SqlParameter("@membPrograms", membPrograms));
          cmd.Parameters.Add(new SqlParameter("@membMemo", membMemo));
          cmd.Parameters.Add(new SqlParameter("@membNo", membNo));

          cmd.ExecuteNonQuery();
        }
      }
    }

    public void memberInsertSpecials(string membAcctId, string membId, int membLevel, int membInternal, int membManager, int membEcom) // used to add group internals and big mgr
    {
      using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString))
      {
        con.Open();
        using (SqlCommand cmd = new SqlCommand())
        {
          cmd.CommandTimeout = 100;
          cmd.Connection = con;
          cmd.CommandText = "dbo.sp6memberInsertSpecials";
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add(new SqlParameter("@membAcctId", membAcctId));
          cmd.Parameters.Add(new SqlParameter("@membId", membId));
          cmd.Parameters.Add(new SqlParameter("@membLevel", membLevel));
          cmd.Parameters.Add(new SqlParameter("@membInternal", membInternal));
          cmd.Parameters.Add(new SqlParameter("@membManager", membManager));
          cmd.Parameters.Add(new SqlParameter("@membEcom", membEcom));
          cmd.ExecuteReader();
        }
      }
    }

    public void memberInsertSpecialsNew(string membAcctId, string membId1, string membId2, string membId3, string membId4, string membId5) // revised to pass in all 5 Internals in one SP
    {
      using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString))
      {
        con.Open();
        using (SqlCommand cmd = new SqlCommand())
        {
          cmd.CommandTimeout = 100;
          cmd.Connection = con;
          cmd.CommandText = "dbo.sp6memberInsertSpecialsNew";
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add(new SqlParameter("@membAcctId", membAcctId));
          cmd.Parameters.Add(new SqlParameter("@membId1", membId1));
          cmd.Parameters.Add(new SqlParameter("@membId2", membId2));
          cmd.Parameters.Add(new SqlParameter("@membId3", membId3));
          cmd.Parameters.Add(new SqlParameter("@membId4", membId4));
          cmd.Parameters.Add(new SqlParameter("@membId5", membId5));
          cmd.ExecuteReader();
        }
      }
    }

    public void memberInsertSpecialsNew2(string custId)
    // revised May 28, 2019
    // only need to pass in CustId to create all the new Internals - values are in SP
    // this will delete any existing credentials and add in the new (useful for converting old to new)
    {
      using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString))
      {
        con.Open();
        using (SqlCommand cmd = new SqlCommand())
        {
          cmd.CommandTimeout = 100;
          cmd.Connection = con;
          cmd.CommandText = "dbo.sp6memberInsertSpecialsNew2";
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add(new SqlParameter("@membCustId", custId));
          cmd.ExecuteReader();
        }
      }
    }

    public bool memberIsNop(
      string membId,
      int storeId,
      string nopReturnUrl,
      out string custId,
      out string membPwd,
      out string alias,
      out string profile,
      out string usesPassword) // used in portal signin
    {
      using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString))
      {
        con.Open();
        using (SqlCommand cmd = new SqlCommand())
        {
          cmd.Connection = con;
          cmd.CommandText = "dbo.sp7memberIsNop";
          cmd.CommandType = CommandType.StoredProcedure;

          cmd.Parameters.Add(new SqlParameter("@membId", membId));
          cmd.Parameters.Add(new SqlParameter("@storeId", storeId));
          cmd.Parameters.Add(new SqlParameter("@nopReturnUrl", nopReturnUrl));

          cmd.Parameters.Add("@custId", SqlDbType.Char, 8).Direction = ParameterDirection.Output;
          cmd.Parameters.Add("@membPwd", SqlDbType.VarChar, 64).Direction = ParameterDirection.Output;
          cmd.Parameters.Add("@alias", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
          cmd.Parameters.Add("@profile", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
          cmd.Parameters.Add("@usesPassword", SqlDbType.VarChar, 8).Direction = ParameterDirection.Output;

          cmd.ExecuteNonQuery();

          custId = cmd.Parameters["@custId"].Value.ToString();
          membPwd = cmd.Parameters["@membPwd"].Value.ToString();
          alias = cmd.Parameters["@alias"].Value.ToString();                // changed from profile to the correct "alias" Apr 2019
          profile = cmd.Parameters["@profile"].Value.ToString();            // changed from profile to the correct "alias" Apr 2019
          usesPassword = cmd.Parameters["@usesPassword"].Value.ToString();  // changed from profile to the correct "alias" Apr 2019

          if (custId.Length == 8 && membPwd.Length > 0)
          {
            return (true);
          }
          return (false);
        }
      }
    }

    public bool memberIsVisitor(string ecomId, int storeId, string custId, out string ecomGuid, out string ecomPwd) // used in portal signin if not a registered V8 user
    {
      using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString))
      {
        con.Open();
        using (SqlCommand cmd = new SqlCommand())
        {
          cmd.Connection = con;
          cmd.CommandText = "dbo.sp7memberIsVisitor";
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add(new SqlParameter("@ecomId", ecomId));
          cmd.Parameters.Add(new SqlParameter("@storeId", storeId));
          cmd.Parameters.Add(new SqlParameter("@custId", custId));
          cmd.Parameters.Add("@ecomGuid", SqlDbType.VarChar, 40).Direction = ParameterDirection.Output;
          cmd.Parameters.Add("@ecomPwd", SqlDbType.VarChar, 64).Direction = ParameterDirection.Output;
          cmd.ExecuteNonQuery();

          ecomGuid = cmd.Parameters["@ecomGuid"].Value.ToString();
          ecomPwd = cmd.Parameters["@ecomPwd"].Value.ToString();

          if (ecomGuid.Length > 08 && ecomPwd.Length > 0)
          {
            return (true);
          }
          return (false);

        }
      }
    }

    public void memberNextId(string membAcctId, out string _membId, out int _membNo)  // insert new security membId and update basics
    {
      _membId = Guid.NewGuid().ToString();  // start with a temp membId
      _membNo = 0;

      using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString))
      {
        con.Open();
        using (SqlCommand cmd = new SqlCommand())
        {
          cmd.CommandTimeout = 100;
          cmd.Connection = con;
          cmd.CommandText = "dbo.sp6memberNext";
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add(new SqlParameter("@membAcctId", membAcctId));
          cmd.Parameters.Add(new SqlParameter("@membId", _membId));
          SqlDataReader drd = cmd.ExecuteReader();
          while (drd.Read())
          {
            _membNo = Convert.ToInt32(drd["membNo"]);
          }
          drd.Close();
          cmd.Parameters.Clear();

          _membId = memberSecurityId(membAcctId, _membNo);  //  update with security membId

          cmd.CommandText = "dbo.sp6memberUpdateByNo";
          cmd.Parameters.Add(new SqlParameter("@membNo", _membNo));
          cmd.Parameters.Add(new SqlParameter("@membAcctId", membAcctId));
          cmd.Parameters.Add(new SqlParameter("@membId", _membId));
          cmd.Parameters.Add(new SqlParameter("@membFirstName", membFirstName));
          cmd.Parameters.Add(new SqlParameter("@membLastName", membLastName));
          cmd.Parameters.Add(new SqlParameter("@membEmail", membEmail));
          cmd.Parameters.Add(new SqlParameter("@membOrganization", membOrganization));
          cmd.Parameters.Add(new SqlParameter("@membLevel", membLevel));
          cmd.ExecuteNonQuery();

        }
      }
    }

    public int memberNextNo(string membAcctId, string membId) // insert new membId, update basics, return membNo
    {

      using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString))
      {
        con.Open();
        using (SqlCommand cmd = new SqlCommand())
        {
          cmd.CommandTimeout = 100;
          cmd.Connection = con;
          cmd.CommandText = "dbo.sp6memberNext";
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add(new SqlParameter("@membAcctId", membAcctId));
          cmd.Parameters.Add(new SqlParameter("@membId", membId));
          SqlDataReader drd = cmd.ExecuteReader();
          while (drd.Read())
          {
            membNo = Convert.ToInt32(drd["membNo"]);
          }
          drd.Close();

          cmd.CommandText = "dbo.sp6memberUpdate";
          cmd.Parameters.Clear();
          cmd.Parameters.Add(new SqlParameter("@membAcctId", membAcctId));
          cmd.Parameters.Add(new SqlParameter("@membId", membId));
          cmd.Parameters.Add(new SqlParameter("@membPwd", membPwd));
          cmd.Parameters.Add(new SqlParameter("@membFirstName", membFirstName));
          cmd.Parameters.Add(new SqlParameter("@membLastName", membLastName));
          cmd.Parameters.Add(new SqlParameter("@membEmail", membEmail));
          cmd.Parameters.Add(new SqlParameter("@membOrganization", membOrganization));
          cmd.Parameters.Add(new SqlParameter("@membLevel", membLevel));
          cmd.ExecuteReader();
        }
      }
      return (membNo);
    }

    public string memberChangeId(string membAcctId, string membNewId, string membOldId)
    {
      using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString))
      {
        con.Open();
        using (SqlCommand cmd = new SqlCommand())
        {
          cmd.Connection = con;
          cmd.CommandText = "dbo.sp6memberChangeId";
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add(new SqlParameter("@membAcctId", membAcctId));
          cmd.Parameters.Add(new SqlParameter("@membNewId", membNewId));
          cmd.Parameters.Add(new SqlParameter("@membOldId", membOldId));

          SqlParameter returnValue = new SqlParameter("@Return_Value", DbType.Int32)
          {
            Direction = ParameterDirection.ReturnValue
          };
          cmd.Parameters.Add(returnValue);
          cmd.ExecuteNonQuery();
          // returns 200 is OK else 4XX if errors
          string membStatus = cmd.Parameters["@Return_Value"].Value.ToString();

          con.Close();
          return (membStatus);
        }
      }
    }

    public string memberSecurityId(string membAcctId, int membNo)

    {
      // we no longer use membAcctId when it supported negatives and alpha

      int i, jj;
      string j, k, l = "";
      string alph = "ABCDEFGHXY";

      // vubiz formula for the trailing 4 characters
      double huge = membNo * 2112;
      string four = fn.right("0000" + huge.ToString(), 4);

      // convert the 4 numbers to characters (ie 1234 becines ABCD)
      for (i = 0; i < 4; i++)
      {
        j = four.Substring(i, 1); // get first number as a string char
        jj = Convert.ToInt16(j);
        k = alph.Substring(jj, 1); // get that alpha value
        l += k;
      }

      return (membNo.ToString() + "-" + l);
    }

    public void memberPrograms(int membNo, string membPrograms) // used in assignPrograms
    {
      using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString))
      {
        con.Open();
        using (SqlCommand cmd = new SqlCommand())
        {
          cmd.CommandTimeout = 100;
          cmd.Connection = con;
          cmd.CommandText = "dbo.sp7memberPrograms";
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add(new SqlParameter("@membPrograms", membPrograms));
          cmd.Parameters.Add(new SqlParameter("@membNo", membNo));
          cmd.ExecuteReader();
        }
      }
    }

    public void memberPrograms2(int membNo, string membPrograms) // similiar to above, used in default.aspx - notice panel
    {
      using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString))
      {
        con.Open();
        using (SqlCommand cmd = new SqlCommand())
        {
          cmd.Connection = con;
          cmd.CommandText = "dbo.sp7memberPrograms2";
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add(new SqlParameter("@membNo", membNo));
          cmd.Parameters.Add(new SqlParameter("@membPrograms", membPrograms));
          cmd.ExecuteReader();
        }
      }
    }

    public void memberSignIn(string custId, string membId_inp) // used in v7 to signin
    {
      using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["apps"].ConnectionString))
      {
        con.Open();
        using (SqlCommand cmd = new SqlCommand())
        {
          cmd.CommandTimeout = 100;
          cmd.Connection = con;
          cmd.CommandText = "dbo.sp7signIn";
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add(new SqlParameter("@custId", custId));
          cmd.Parameters.Add(new SqlParameter("@membId", membId_inp));
          SqlDataReader drd = cmd.ExecuteReader();
          membExists = false;
          while (drd.Read())
          {
            membExists = true;
            membNo = (int)drd["Memb_No"];
            membId = drd["Memb_Id"].ToString();
            membFirstName = drd["Memb_FirstName"].ToString();
            membLastName = drd["Memb_LastName"].ToString();
            membEmail = drd["Memb_Email"].ToString();
            membGuid = drd["Memb_Guid"].ToString();
            membLevel = (int)drd["Memb_Level"];
            membJobs = drd["Memb_Jobs"].ToString(); // this is typically null but for managers represents extended access
          }
          drd.Close();

        }
      }
    }

    #endregion
  }
}