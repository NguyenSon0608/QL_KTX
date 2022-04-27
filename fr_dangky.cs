using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLKTX
{
    public partial class fr_dangky : Form
    {
        int id_rold = fr_login.id_rold;
        Database dtbase  = new Database();
        DataTable dt = new DataTable();
        public fr_dangky()
        {
            InitializeComponent();
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            txt_holot.Text = "";
            txt_holot.Focus();
            txt_ten.Text = "";
            txt_password.Text = "";
            txt_sdt.Text = "";
            txt_usename.Text = "";
            
        }

        public void bt_dky_Click(object sender, EventArgs e)
        {
            string pass = txt_password.Text.Trim();
            string pass_md5=FunctionMD5.Create_md5(pass);
            string username= txt_usename.Text.Trim();
            DateTime brithday = date_brithday.Value;
            string holot= txt_holot.Text.Trim();
            string ten= txt_ten.Text.Trim();    
            string sdt = txt_sdt.Text.Trim();
            string masv= cb_masv.Text.Trim();
            string nganhhoc = cb_nganhhoc.ValueMember;
            string diachi=txt_dchi.Text.Trim();
            string gioitinh = "";
            if(check_box_nam.Checked==true)
            {
                gioitinh = "Nam";
            }
            else if (checkbox_nu.Checked==true)
            {
                gioitinh = "Nữ";
            }
          

            if (txt_usename.Text.Trim() == "" && txt_password.Text.Trim() == "" && txt_ten.Text.Trim()=="")
            {
                MessageBox.Show("Bạn phải username, password, tên không được bỏ trống");
                return;
            }
            //Kiểm tra xem mã 
           
            DataTable dtlop = dtbase.DataReader("Select * from TaiKhoan where Username=N'" + username + "'");
            if (dtlop.Rows.Count > 0)
            {
                MessageBox.Show("Đã có username  " +username + ", Bạn hãy nhập username khác");
                txt_usename.Focus();
                return;
            }
            DataTable dt_maSV = dtbase.DataReader("Select * from tbl_sinhvien where masv=N'" + masv + "'");
            if (dt_maSV.Rows.Count > 0)
            {
                MessageBox.Show("Mã sinh viên  " + masv + ", Bạn hãy nhập mã khác khác");
                cb_masv.Focus();
                return;
            }
            //Tạo câu lệnh sql
            string sqlInserSV = "insert into tbl_Sinhvien(MaSV,Holot,Ten,ngaysinh,SĐT,Điachi, gioitinh) values" +
               "(N'" + masv + "',N'" + holot + "',N'" + ten + "','" + brithday + "','" + sdt + "',N'" + diachi + "',N'" + gioitinh + "')";
            dtbase.DataChange(sqlInserSV);
            int id_rold2 = 3;
            string sql_tk = "insert into taikhoan(username,password,id_rold,maSV) values(N'" + username + "',N'" + pass_md5 + "','" + id_rold2 + "', N'" + masv + "')";
            dtbase.DataChange(sql_tk);
            MessageBox.Show("Thêm thành công", "Thông báo");
            fr_login fr_login = new fr_login();
            fr_login.Show();
            
            if (id_rold == 1)
            {
                label_loaitk.Visible = true;
                cb_loaitk.Visible=true;
                if (cb_loaitk.Text == "Sinh viên")
                {
                   dtbase.DataChange(sqlInserSV);
                     dtbase.DataChange(sql_tk);
                    MessageBox.Show("Thêm thành công", "Thông báo");
                  
                    fr_login.Show();
                }
                else if(cb_loaitk.Text=="Quản lý")
                {
                    string sqlInserTK_Quanly = "insert into thongtin_tk(Holot,Ten,ngaysinh,SĐT) values" +
                  "(N'" + holot + "',N'" + ten + "','" + brithday + "','" + sdt + "')";
                    dtbase.DataChange(sqlInserTK_Quanly);
                   DataTable dt_id = dtbase.DataReader("select max(id) as id from thongtin_tk");
                    int id = 0;
                    foreach (DataRow data1 in dt_id.Rows)
                    {
                        id = int.Parse(data1["id"].ToString());

                    }
                    int id_rold_ql = 2;
                    string sql_tk_1 = "insert into taikhoan(username,password,id_rold,id) values(N'" + username + "',N'" + pass_md5 + "','" + id_rold_ql + "', '" + id + "')";
                    dtbase.DataChange(sql_tk_1);
                    MessageBox.Show("Thêm thành công", "Thông báo");
                 
                    fr_login.Show();
                }


            }
            
        }

        private void fr_dangky_Load(object sender, EventArgs e)
        {
            if(id_rold == 1)
            {
                label_loaitk.Visible = true;
                cb_loaitk.Visible = true;
            }
            load_data_to_cbox();
            cb_loaitk.SelectedIndex = 0;

        }

        private void check_box_nam_CheckedChanged(object sender, EventArgs e)
        {
            if(check_box_nam.Checked == true)
            {
                checkbox_nu.Checked = false;
            }
        }

        private void checkbox_nu_CheckedChanged(object sender, EventArgs e)
        {
            if (checkbox_nu.Checked == true)
            {

                check_box_nam.Checked = false;
            }
        }
        public void load_data_to_cbox()
        {
            string sql_load_masv = "select MaSV from tbl_sinhvien";
            cb_masv.DataSource = dtbase.DataReader(sql_load_masv);
            cb_masv.DisplayMember = "MaSV";
            cb_masv.ValueMember = "MaSV";
            string sql_load_nganh = "select TenNganh, manganh from tbl_NganhHoc";
            cb_nganhhoc.DataSource = dtbase.DataReader(sql_load_nganh);
            cb_nganhhoc.DisplayMember = "TenNganh";
            cb_nganhhoc.ValueMember = "MaNganh";
            label_loaitk.Visible=false;
            cb_loaitk.Visible = false;
        }
    }
}
