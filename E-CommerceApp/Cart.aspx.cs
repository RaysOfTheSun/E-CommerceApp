﻿using System;
using System.Web.UI.WebControls;
using System.Globalization;
namespace E_CommerceApp
{
    public partial class FrmCheckout1 : System.Web.UI.Page
    {
        #region Global Variables
        readonly UserCart _cart = UserCart.Instance;
        string _user = "-";
        int _userCartId = -1; 
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            // if the user is logged in, obtain his current cart if it's not zero.
            // otherwise, use the ID stored in the session variable.
            if (Session["currUser"] != null)
            {
                _user = (((string)(Session["currUser"])));

                _userCartId = (DBOps.GetLatestEntry(DBOps.GetUserID(_user)) != 0) ? DBOps.GetLatestEntry(DBOps.GetUserID(_user)) :
                    (Convert.ToInt32(Session["prevID"]));
                _cart.cartID = _userCartId;

                if (Session["checkSync"] != null)
                {
                    _cart.SyncCart((Convert.ToInt32(Session["prevID"])), _user);
                    cartDatasource.Update();
                    Session["prevID"] = _userCartId;
                    Session.Remove("checkSync");
                }
            }
            else
            {
                _user = "-";
                _userCartId = (Convert.ToInt32(Session["prevID"]));
            }

            if (Session["prevID"] == null)
            {
                _userCartId = Convert.ToInt32(Session["prevID"]);
            }
            else
            {
                Session["prevID"] = _userCartId;
            }

            _cart.cartID = _userCartId;
            Session["prevID"] = _userCartId;

            lvw_items.DataSource = DBOps.BuildUserCart(_userCartId);
            lvw_items.DataBind();
            lvw_totals.DataSource = DBOps.BuildUserCartTotals(_userCartId);
            lvw_totals.DataBind();

            Button button = (Button)UpdatePanel1.FindControl("btn_checkout");

            if ((DBOps.BuildUserCart(_userCartId)).Rows.Count == 0)
            {
                button.Enabled = false;
                button.CssClass = "btn btn-outline-secondary btn-block";
            }
            else
            {
                button.Enabled = true;
            }
        }

        protected void lvw_items_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            string[] productDetails = ((String)e.CommandArgument).Split(',');

            try
            {
                _cart.RemoveItem(productDetails[0].Trim(), Convert.ToDecimal(productDetails[1].Trim()), Convert.ToInt32(productDetails[2].Trim()));
                cartDatasource.Update();
            }
            catch (Exception)
            {
                // ignored
            }

            Button button = (Button)UpdatePanel1.FindControl("btn_checkout");
            if (((DBOps.BuildUserCart(_userCartId)).Rows.Count < 1))
            {
                if (_user != "-")
                {
                    //cartDatasource.Delete();
                    //DataOps.reassignUserCart(user);
                }
                button.Enabled = false;
                button.CssClass = "btn btn-outline-secondary btn-block";
            }

            DataPager dp = (DataPager)lvw_items.FindControl("DataPager1");
            if (lvw_items.Items.Count <= 1)
            {
                dp.SetPageProperties(0, dp.MaximumRows, false);
            }

            lvw_items.DataSource = DBOps.BuildUserCart(_userCartId);
            lvw_totals.DataSource = DBOps.BuildUserCartTotals(_userCartId);
            lvw_items.DataBind();
            lvw_totals.DataBind();
        }

        protected void cartDatasource_Updating(object sender, SqlDataSourceCommandEventArgs e)
        {
            e.Command.Parameters["@Id"].Value = _userCartId;
            e.Command.Parameters["@customer"].Value = _user;
            e.Command.Parameters["@items"].Value = _cart.lastInsertedItem;
            e.Command.Parameters["@prices"].Value = _cart.lastInsertedPrice;
            e.Command.Parameters["@quants"].Value = _cart.lastInsertedQuant;
            e.Command.Parameters["@totalCount"].Value = _cart.totalItemQuantity;
            e.Command.Parameters["@totalPrice"].Value = _cart.totalCartPrice;
        }

        protected void tbx_qty_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox1 = (TextBox)sender;
            ListViewDataItem item = (ListViewDataItem)textBox1.NamingContainer;
            TextBox tb = (TextBox)item.FindControl("tbx_qty"); //get the textbox in the proper listview item
            if (((Convert.ToInt32(tb.Text) <= 0) || tb.Text == string.Empty)
                || ((string.IsNullOrEmpty(tb.Text)) || (string.IsNullOrWhiteSpace(tb.Text)))
                || (tb.Text == DBNull.Value.ToString(CultureInfo.InvariantCulture)))
            {
                tb.Text = "1";
            }
            else if ((Convert.ToInt32(tb.Text)) > 99)
            {
                tb.Text = "99";
            }

            Label lblSku = (Label)item.FindControl("lbl_sku");
            Label lblPrice = (Label)item.FindControl("lbl_price");


            try
            {
                _cart.UpdateItem(lblSku.Text, Decimal.Parse(lblPrice.Text, NumberStyles.Currency), Convert.ToInt32(tb.Text));
                cartDatasource.Update();
            }
            catch (Exception)
            {
                // ignored
            }

            lvw_items.DataSource = DBOps.BuildUserCart(_userCartId);
            lvw_totals.DataSource = DBOps.BuildUserCartTotals(_userCartId);
            lvw_items.DataBind();
            lvw_totals.DataBind();

            //string cleanMessage = "AAAAAAAAAAAAAAAAAAA " + tb.Text;
            //string script = string.Format("alert('{0}');", cleanMessage);
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "alert", script, true);

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (((string)(Session["currUser"])) == null)
            {
                Session["loginRedirect"] = "yes";
                Session["refNum"] = _userCartId;
                //Response.Redirect(@"~/frm_payment.aspx?mode=unlogged");
            }
            else
            {
                Session["refNum"] = _userCartId;
                //Response.Redirect(@"~/frm_payment.aspx?mode=logged");
            }

            Response.Redirect(@"~/Payment.aspx");

        }

        protected void lvw_items_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            // Prevent the pager from showing an empty page
            // in case the user deletes all the items in a
            // single page
            DataPager dp = (DataPager)lvw_items.FindControl("DataPager1");
            dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            lvw_items.DataSource = DBOps.BuildUserCart(_userCartId);
            lvw_items.DataBind();
        }

        protected void lvw_items_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {

        }

        protected void lvw_items_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void cartDatasource_Deleting(object sender, SqlDataSourceCommandEventArgs e)
        {
            e.Command.Parameters["@Id"].Value = _userCartId;
        }

        protected void btn_goRefCart_Click(object sender, EventArgs e)
        {
            Session["refNum"] = tbx_refNum.Text;
            Response.Redirect("~/RefCart.aspx");
        }
    }
}