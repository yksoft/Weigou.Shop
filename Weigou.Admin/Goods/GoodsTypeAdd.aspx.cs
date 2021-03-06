﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Weigou.Service;
using System.Data;
using Weigou.Common;

namespace Weigou.Admin.Goods
{
    public partial class GoodsTypeAdd : AdminPage
    {
        /// <summary>
        /// 商品分类ID
        /// </summary>
        private int _ID
        {
            get { return GetRequest("ID", 0); }
        }

        private int _IsAdd
        {
            get { return GetRequest("IsAdd", 1); }
        }
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGoodsType(this.ddlParentType, "-根节点-");
                if (_IsAdd == 2)
                {
                    BindInfo();
                }
                else
                {
                    ddlParentType.SelectedValue = Convert.ToString(_ID);
                }
            }
        }

        /// <summary>
        /// 绑定商户类别信息
        /// </summary>
        public void BindInfo()
        {
            if (_ID == 0)
                return;
            DataTable dt = goodsService.GetDataByKey("T_GoodsType", "ID", _ID);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                this.txtName.Text = dr["Name"].ToString();
                this.txtSpell.Text = dr["Spell"].ToString();
                this.ddlParentType.SelectedValue = dr["ParentID"].ToString();
                this.txtRemark.Text = dr["Remark"].ToString();
            }
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (_IsAdd == 1)
            {
                dt = goodsService.GetDataByKey("T_GoodsType", "ID", 0);
            }
            else
            {
                dt = goodsService.GetDataByKey("T_GoodsType", "ID", _ID);
            }
            DataRow dr;
            if (dt.Rows.Count == 0)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
                dr["CreateBy"] = UserInfo.ID;
                dr["CreateTime"] = DateTime.Now;
            }
            else
            {
                dr = dt.Rows[0];
            }
            dr["Name"] = this.txtName.Text;
            dr["Spell"] = this.txtSpell.Text;
            dr["ParentID"] = this.ddlParentType.SelectedValue == "" ? "0" : this.ddlParentType.SelectedValue;
            dr["Remark"] = this.txtRemark.Text;

            int res = goodsService.UpdateDataTable(dt);
            if (res > 0)
                RegistScript("CloseWin('操作成功！',parent.ReLoad);");
            else
                RegistScript("ShowMsg('操作失败');");
        }
    }
}
