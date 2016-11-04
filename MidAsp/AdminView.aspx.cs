﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MidAsp
{
    public partial class AdminView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["type"] != null)
            {
                if (Session["type"].ToString().Equals("admin") == false)
                {
                    Response.Redirect("~/Login.aspx");
                }
            }
            else if (Session["type"] == null)
                Response.Redirect("~/Login.aspx");
            
            if (!IsPostBack)
            {
                if (Cache["DATASET"] == null)
                {
                    this.Loaddata();
                }
                else
                {
                    this.LoadFrmCache();
                }
            }
           // Response.Write(ds.Tables["dvd"].Rows[0]["dvdid"].ToString());


        }

         void Loaddata()
        {
            string connStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);
            //string sql = "SELECT dvdid,title,description,actor,type,category,date FROM dvds dvd,casting cas,type tp, category ct where dvd.castingId=cas.id and dvd.typeid=tp.id and dvd.categoryid=ct.id";
            string sql = "select * from dvds";
            string sql2 = "select * from category";
            string sql3 = "select * from casting";
            string sql4 = "select * from type";
             SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
             SqlDataAdapter adapter1 = new SqlDataAdapter(sql2, conn);
             SqlDataAdapter adapter2 = new SqlDataAdapter(sql3, conn);
             SqlDataAdapter adapter3 = new SqlDataAdapter(sql4, conn);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            SqlCommandBuilder builder1 = new SqlCommandBuilder(adapter1);
            SqlCommandBuilder builder2 = new SqlCommandBuilder(adapter2);
            SqlCommandBuilder builder3 = new SqlCommandBuilder(adapter3);
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            DataSet ds3 = new DataSet();
            adapter.Fill(ds, "dvd");
            adapter1.Fill(ds1, "category");
            adapter2.Fill(ds2, "casting");
            adapter3.Fill(ds3, "type");
            ds.Tables["dvd"].PrimaryKey = new DataColumn[] { ds.Tables["dvd"].Columns["dvdid"] };
            ds1.Tables["category"].PrimaryKey = new DataColumn[] { ds1.Tables["category"].Columns["id"] };
            ds2.Tables["casting"].PrimaryKey = new DataColumn[] { ds2.Tables["casting"].Columns["id"] };
            ds3.Tables["type"].PrimaryKey = new DataColumn[] { ds3.Tables["type"].Columns["id"] };
            Cache["DATASET"] = ds;
            Cache["DATASET1"] = ds1;
            Cache["DATASET2"] = ds2;
            Cache["DATASET3"] = ds3;
            Cache["ADAPTER"] = adapter;
            Cache["ADAPTER1"] = adapter1;
            Cache["ADAPTER2"] = adapter2;
            Cache["ADAPTER3"] = adapter3;

            dvd.DataSource = ds.Tables["dvd"];
            category.DataSource = ds1.Tables["category"];
            casting.DataSource = ds2.Tables["casting"];
            type.DataSource = ds3.Tables["type"];
            dvd.DataBind();
            category.DataBind();
            casting.DataBind();
            type.DataBind();
          
        }
         private void LoadFrmCache()
         {
             DataSet ds = (DataSet)Cache["DATASET"];
             DataSet ds1 = (DataSet)Cache["DATASET1"];
             DataSet ds2 = (DataSet)Cache["DATASET2"];
             DataSet ds3 = (DataSet)Cache["DATASET3"];
             dvd.DataSource = ds.Tables["dvd"];
             category.DataSource = ds1.Tables["category"];
             casting.DataSource = ds2.Tables["casting"];
             type.DataSource = ds3.Tables["type"];
             dvd.DataBind();
             category.DataBind();
             casting.DataBind();
             type.DataBind();
         }
       
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataSet ds = (DataSet)Cache["DATASET"];
            DataRow dr = ds.Tables["dvd"].Rows.Find(e.Keys["dvdid"]);
            dr["title"] = e.NewValues["title"];
            dr["description"] = e.NewValues["description"];
            dr["castingId"] = e.NewValues["castingId"];
            dr["typeid"] = e.NewValues["typeid"];
            dr["categoryid"] = e.NewValues["categoryid"];
            dr["Date"] = e.NewValues["Date"];


            Cache["DATASET"] = ds;
            dvd.EditIndex = -1;
            this.LoadFrmCache();
            // DataSet ds = (DataSet)Cache["DATASET"];
            SqlDataAdapter adapter = (SqlDataAdapter)Cache["ADAPTER"];
            adapter.Update(ds.Tables["dvd"]);
        }

      

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataSet ds = (DataSet)Cache["DATASET"];
            DataRow dr = ds.Tables["dvd"].Rows.Find(e.Keys["dvdid"]);
            dr.Delete();
            Cache["DATASET"] = ds;
            this.LoadFrmCache();
            SqlDataAdapter adapter = (SqlDataAdapter)Cache["ADAPTER"];
            adapter.Update(ds.Tables["dvd"]);
        }

     

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            dvd.EditIndex = -1;
            this.LoadFrmCache();
        }

        protected void dvd_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dvd.EditIndex = e.NewEditIndex;
            this.LoadFrmCache(); 
        }

        protected void casting_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataSet ds = (DataSet)Cache["DATASET2"];
            DataRow dr = ds.Tables["casting"].Rows.Find(e.Keys["id"]);
            dr.Delete();
            Cache["DATASET"] = ds;
            this.LoadFrmCache();
            SqlDataAdapter adapter = (SqlDataAdapter)Cache["ADAPTER2"];
            adapter.Update(ds.Tables["casting"]);
        }

        protected void casting_RowEditing(object sender, GridViewEditEventArgs e)
        {
            casting.EditIndex = e.NewEditIndex;
            this.LoadFrmCache(); 
        }

        protected void casting_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            casting.EditIndex = -1;
            this.LoadFrmCache();
        }

        protected void casting_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataSet ds = (DataSet)Cache["DATASET2"];
            DataRow dr = ds.Tables["casting"].Rows.Find(e.Keys["id"]);
            dr["id"] = e.NewValues["id"];
            dr["actor"] = e.NewValues["actor"];
        


            Cache["DATASET2"] = ds;
            casting.EditIndex = -1;
            this.LoadFrmCache();
            // DataSet ds = (DataSet)Cache["DATASET"];
            SqlDataAdapter adapter = (SqlDataAdapter)Cache["ADAPTER2"];
            adapter.Update(ds.Tables["casting"]);
        }
    
        protected void category_RowUpdating1(object sender, GridViewUpdateEventArgs e)
        {
            DataSet ds = (DataSet)Cache["DATASET1"];
            DataRow dr = ds.Tables["category"].Rows.Find(e.Keys["id"]);
            dr["id"] = e.NewValues["id"];
            dr["category"] = e.NewValues["category"];



            Cache["DATASET1"] = ds;
            category.EditIndex = -1;
            this.LoadFrmCache();
            // DataSet ds = (DataSet)Cache["DATASET"];
            SqlDataAdapter adapter = (SqlDataAdapter)Cache["ADAPTER1"];
            adapter.Update(ds.Tables["category"]);
        }

        protected void category_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataSet ds = (DataSet)Cache["DATASET1"];
            DataRow dr = ds.Tables["category"].Rows.Find(e.Keys["id"]);
            dr.Delete();
            Cache["DATASET1"] = ds;
            this.LoadFrmCache();
            SqlDataAdapter adapter = (SqlDataAdapter)Cache["ADAPTER1"];
            adapter.Update(ds.Tables["category"]);
        }

        protected void category_RowEditing(object sender, GridViewEditEventArgs e)
        {
            category.EditIndex = e.NewEditIndex;
            this.LoadFrmCache(); 

        }

        protected void category_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            category.EditIndex = -1;
            this.LoadFrmCache();
        }

        protected void type_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataSet ds = (DataSet)Cache["DATASET3"];
            DataRow dr = ds.Tables["type"].Rows.Find(e.Keys["id"]);
            dr.Delete();
            Cache["DATASET3"] = ds;
            this.LoadFrmCache();
            SqlDataAdapter adapter = (SqlDataAdapter)Cache["ADAPTER3"];
            adapter.Update(ds.Tables["type"]);
        }

        protected void type_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataSet ds = (DataSet)Cache["DATASET3"];
            DataRow dr = ds.Tables["type"].Rows.Find(e.Keys["id"]);
            dr["id"] = e.NewValues["id"];
            dr["type"] = e.NewValues["type"];



            Cache["DATASET3"] = ds;
            type.EditIndex = -1;
            this.LoadFrmCache();
            // DataSet ds = (DataSet)Cache["DATASET"];
            SqlDataAdapter adapter = (SqlDataAdapter)Cache["ADAPTER3"];
            adapter.Update(ds.Tables["type"]);
        }

        protected void type_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            type.EditIndex = -1;
            this.LoadFrmCache();
        }

        protected void type_RowEditing(object sender, GridViewEditEventArgs e)
        {
            type.EditIndex = e.NewEditIndex;
            this.LoadFrmCache(); 
        }

        















        protected void Button1_Click(object sender, EventArgs e)
        {
            Session.Remove("type");
            Response.Redirect("~/Login.aspx");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 1;
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 2;
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 3;
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 4;
        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 1;
        }

        protected void Button7_Click(object sender, EventArgs e)
        {

            MultiView1.ActiveViewIndex = 2;
        }

        protected void Button8_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 3;
        }

        protected void Button9_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 4;
        }

        protected void Button11_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 1;
        }

        protected void Button12_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 2;
        }

        protected void Button13_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 3;
        }

        protected void Button14_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 4;
        }

        protected void Button19_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 4;
        }

        protected void Button18_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 3;
        }

        protected void Button16_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 1;
        }

        protected void Button17_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 2;
        }

        protected void Button24_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 4;
        }

        protected void Button21_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 1;
        }

        protected void Button22_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 2;
        }

        protected void Button23_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 3;
        }

        protected void Button26_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 5;
        }

        protected void Button27_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 5;
        }

        protected void Button28_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 5;
        }

        protected void Button29_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 5;
        }

        protected void Button30_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 5;
        }

        protected void Button31_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 1;
        }

        protected void Button32_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 2;
        }

        protected void Button33_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 3;
        }

        protected void Button34_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 4;
        }

        protected void Button35_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 5;
        }

        protected void Button37_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
        }

        protected void Button38_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
        }

        protected void Button39_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
        }

        protected void Button42_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
        }

        protected void Button41_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
        }

        protected void Button40_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
        }

        protected void casting_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}