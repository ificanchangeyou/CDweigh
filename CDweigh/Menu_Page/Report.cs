using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CDweigh.Helper;
using CDweigh.Class;
namespace CDweigh.Menu_Page
{
    public partial class Report : Form
    {
        public int Order = 0;
        public Report(int Order)
        {
            this.Order = Order;
            InitializeComponent();
        }

        private void Report_Load(object sender, EventArgs e)
        {
            /*************初始化数据****************/
            //1.添加查询时间列表
            Time_Combo.Items.Add(new KeyValuePair<string, string>("订单创建", "BillCreateTime"));
            Time_Combo.Items.Add(new KeyValuePair<string, string>("订单关闭", "BillEndTime"));
            Time_Combo.Items.Add(new KeyValuePair<string, string>("一次称重", "FirstWeightTime"));
            Time_Combo.Items.Add(new KeyValuePair<string, string>("二次称重", "SecondWeightTime"));
            Time_Combo.Items.Add(new KeyValuePair<string, string>("装车开始", "LoadStartTime"));
            Time_Combo.Items.Add(new KeyValuePair<string, string>("装车结束", "LoadEndTime"));
            Time_Combo.SelectedIndex = 0;
            //2.定义日历
            StartTimePicker.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            StartTimePicker.Value = DateTime.Now.Date;
            EndTimePicker.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            EndTimePicker.Value = DateTime.Now.AddDays(1).Date;
            //3.定义下拉框
            //3.1车辆下拉框
            Vehicle_Combo.IntegralHeight = false;//车辆选项内不支持在线查找，允许模糊查找
            Vehicle_Combo.DropDownStyle = ComboBoxStyle.DropDown;
            //3.2物料下拉框
            Material_Combo.DropDownStyle = ComboBoxStyle.DropDownList; //物料设置为不允许用户输入，只能在下拉框内选择
            try
            {
                string SqlStr = "SELECT  DISTINCT  MaterialName FROM Material";
                using (DataTable MarterialTable = SqlHelper.QueryDataTable(Global.Sqldatabase, SqlStr))
                {
                    foreach (DataRow marRow in MarterialTable.Rows)
                    {
                        Material_Combo.Items.Add(marRow[0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                //log error
                throw;
            }

            //3.3鹤位下拉框
            Load_Combo.DropDownStyle = ComboBoxStyle.DropDownList; //鹤位设置为不允许用户输入，只能在下拉框内选择
            try
            {
                string SqlStr = "SELECT  DISTINCT Tag FROM Material";
                using (DataTable MarterialTable = SqlHelper.QueryDataTable(Global.Sqldatabase, SqlStr))
                {
                    foreach (DataRow marRow in MarterialTable.Rows)
                    {
                        Load_Combo.Items.Add(marRow[0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                //log error
                throw;
            }

            //3.4客户下拉框
            Customer_Combo.DropDownStyle = ComboBoxStyle.DropDownList; //鹤位设置为不允许用户输入，只能在下拉框内选择
            try
            {
                string SqlStr = "SELECT  DISTINCT TOP 30 CustomerName   FROM Customer  "; //支持30条
                using (DataTable MarterialTable = SqlHelper.QueryDataTable(Global.Sqldatabase, SqlStr))
                {
                    foreach (DataRow marRow in MarterialTable.Rows)
                    {
                        Customer_Combo.Items.Add(marRow[0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                //log error
                throw;
            }

            dataGridView1.HorizontalScrollingOffset = 10;
        }

        private void Select_But_Click(object sender, EventArgs e)
        {
            try
            {

                //查询用户
                string sql = @"SELECT BillNum AS 提单号,BillType AS 提单类型,VehicleCode AS 车牌,IdNum AS 卡号, MaterialName AS 物料,PreSet AS 预装量,Postion AS 鹤位,
                                   CustomerName AS 客户, ";
                string sql_con = "";
                switch (Order)
                {
                    case 0://创建的订单
                        sql += @" BillCreateTime AS 订单创建时间 ,Remarks AS 备注  FROM Inbill WHERE BillStatus < 30 ";
                        break;
                    case 1://尚未过磅订单
                        sql += @" BillCreateTime AS 订单创建时间 ,Remarks AS 备注 FROM Inbill WHERE BillStatus BETWEEN 20 AND 30 ";
                        break;
                    case 2://已过一次地衡的订单
                        sql += @" FirstWeight AS 一次称重,FirstWeightTime AS 一次称重时间,FirstWeighbridgeNum AS 一次称重地衡, 
                                BillCreateTime AS 订单创建时间 ,Remarks AS 备注 FROM Inbill WHERE BillStatus BETWEEN 30 AND 60 ";
                        break;
                    case 3://已过二次地衡的订单
                        sql += @" FirstWeight AS 一次称重,FirstWeightTime AS 一次称重时间,FirstWeighbridgeNum AS 一次称重地衡, 
                                SecondWeight AS 二次称重,SecondWeightTime AS 二次称重时间, SecondWeighbridgeNum AS 二次称重地衡,
                                    WeightNet AS 净重, RealLoad AS 实装量,LoadStartTime AS 开始作业时间, LoadEndTime AS 结束作业时间,
                                        CreateOperator AS 订单创建者,FirstWeightOperator AS 一次过磅操作员,SecondWeightOperator AS 二次过磅操作员,
                                            BillCreateTime AS 订单创建时间 ,BillEndTime AS 订单结束时间,SealNum AS 铅封,Remarks AS 备注 
                                                FROM Outbill WHERE BillStatus = 100  ";
                        break;
                    default:
                        sql = "";
                        break;
                }
                if (Time_Combo.SelectedItem != null)
                {
                    IDictionary<string, Object> parmater = new Dictionary<string, Object>();

                    KeyValuePair<string, string> selectTime = (KeyValuePair<string, string>)Time_Combo.SelectedItem;
                    sql += " AND " + selectTime.Value.ToString() + " BETWEEN @StartTIme AND @EndTime";
                    parmater.Add("@StartTIme", StartTimePicker.Value);
                    parmater.Add("@EndTime", EndTimePicker.Value);
                    if (Vehicle_Combo.Text!= "")
                    {
                        sql += " AND VehicleCode LIKE @vehicle ";
                        parmater.Add("@vehicle", "%"+Vehicle_Combo.Text+"%");
                    }
                    if(Material_Combo.SelectedIndex != -1)
                    {
                        sql += " AND MaterialName = @MaterialName ";
                        parmater.Add("@MaterialName", Material_Combo.Text);
                    }
                    
                    if (Load_Combo.SelectedIndex != -1)
                    {
                        sql += " AND Postion = @Postion ";
                        parmater.Add("@Postion", Load_Combo.Text);
                    }
                    if (Customer_Combo.SelectedIndex != -1)
                    {
                        sql += " AND CustomerName = @CustomerName ";
                        parmater.Add("@CustomerName", Customer_Combo.Text);
                    }

                    DataTable table=   SqlHelper.QueryDataTable(Global.Sqldatabase, sql,CommandType.Text, parmater);
                    dataGridView1.DataSource = table;
                }
                else
                {
                    MessageBox.Show("未选择查询的时间段");
                }



            }

            catch (Exception)
            {

                throw;
            }

        }


    }
}
