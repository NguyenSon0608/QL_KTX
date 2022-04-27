using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QLKTX
{
    public partial class fr_quanly_thietbi : Form
    {
        Database Database = new Database();
        public fr_quanly_thietbi()
        {
            InitializeComponent();
        }

        //chỉ cho phép nhập số nguyên
        private void txtSL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                MessageBox.Show("Bạn chỉ được nhập số nguyên");
                e.Handled = true;
            }
        }

        private void Form12_Load(object sender, EventArgs e)
        {
            //nạp dữ liệu vào combobox
            cbxmaphong.DataSource = Database.DataReader("Select MaPhong,TenPhong from tblPhong");
            cbxmaphong.ValueMember = "MaPhong";
            cbxmaphong.DisplayMember = "TenPhong";
            cbxmaphong.Text = "";

            cbxmathietbi.DataSource = Database.DataReader("Select MaThietBi,TenThietBi from tblThietBi");
            cbxmathietbi.ValueMember = "MaThietBi";
            cbxmathietbi.DisplayMember = "TenThietBi";
            cbxmathietbi.Text = "";

            Loaddata();
            dgvthietbi.Columns[0].HeaderText = "MÃ PHÒNG";
            dgvthietbi.Columns[1].HeaderText = "MÃ THIẾT BỊ";
            dgvthietbi.Columns[2].HeaderText = "SỐ LƯỢNG";
            dgvthietbi.Columns[3].HeaderText = "TÌNH TRẠNG";
           
            //ẩn nút sửa,xoá
            btnsua.Enabled = false;
            btnXoa.Enabled = false;
            Loaddata();
            Xoatrangdulieu();
        }
        void Loaddata()
        {
            dgvthietbi.DataSource = Database.DataReader("Select * from tblThietBiPhong");
        }
        void Xoatrangdulieu()
        {
            cbxmaphong.SelectedIndex = -1;
            cbxmathietbi.SelectedIndex = -1;
            cbxtinhtrang.SelectedIndex = -1;
            txtSL.Text = "";
        }

        private void dgvthietbi_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            cbxmaphong.SelectedValue = dgvthietbi.CurrentRow.Cells[0].Value.ToString();
            cbxmathietbi.SelectedValue = dgvthietbi.CurrentRow.Cells[1].Value.ToString();
            txtSL.Text = dgvthietbi.CurrentRow.Cells[2].Value.ToString();
            cbxtinhtrang.SelectedItem = dgvthietbi.CurrentRow.Cells[3].Value.ToString();
            
            //Hiển thị các nút cần thiết
            btnsua.Enabled = true;
            btnXoa.Enabled = true;
            btnThem.Enabled = false;
        }
        private void btnTK_Click(object sender, EventArgs e)
        {
            //Kiểm tra đã nhập thông tin tìm kiếm chưa
            if (txtTKMP.Text.Trim() == "")
            {
                errorProvider1.SetError(txtTKMP, "Hãy Nhập Mã Phòng");
            }
            else
            {
                errorProvider1.Clear();
            }
            //Cấm nút Sửa và Xóa
            btnsua.Enabled = false;
            btnXoa.Enabled = false;
            //Viết câu lệnh tìm kiếm sql 
            string sql = "SELECT * FROM tblThietBiPhong where MaPhong is not null ";
            //Tim theo mã phòng 
            if (txtTKMP.Text.Trim() != "")
            {
                sql += " and MaPhong like '%" + txtTKMP.Text + "%'";
            }
            //Load dữ liệu tìm được lên dataGridView
            dgvthietbi.DataSource = Database.DataReader(sql);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            //kiểm tra đã nhập đủ dữ liệu chưa
            if (cbxmaphong.Text == "" || cbxmathietbi.Text == "" ||
               cbxtinhtrang.Text == "" || txtSL.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập đủ dữ liệu");
                return;
            }

            // chưa check khóa
            //Insert CSDL
             Database.DataChange("Insert into tblThietBiPhong Values('" + cbxmaphong.SelectedValue.ToString() + "', " +
              "'" + cbxmathietbi.SelectedValue.ToString() + "','" + txtSL.Text + "'," +
              "N'" + cbxtinhtrang.Text + "')");
            Xoatrangdulieu();
            Loaddata();
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            //kiểm tra 
            if (cbxmaphong.Text == "" || cbxmathietbi.Text == "" 
                || cbxtinhtrang.Text== ""|| txtSL.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập đủ dữ liệu");
                return;
            }
            //cập nhật lại dữ liệu 
            Database.DataChange("Update tblThietBiPhong set MaThietBi = '" + cbxmathietbi.SelectedValue.ToString() + "'," +
                 "SoLuong='" + txtSL.Text + "',TinhTrang = N'" + cbxtinhtrang.SelectedIndex.ToString() + "'" +
                 " Where MaPhong = '" + cbxmaphong.SelectedValue.ToString() + "'");
            Loaddata();
            //Ẩn hiện các nút phù hợp
            btnsua.Enabled = false;
            btnXoa.Enabled = false;
            btnThem.Enabled = true;
            Xoatrangdulieu();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xoá không?", "Thông Báo"
               , MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                Database.DataChange("Delete tblThietBiPhong Where MaPhong = '" + cbxmaphong.SelectedValue +"'");
                btnsua.Enabled = false;
                btnThem.Enabled = true;
                btnXoa.Enabled = false;
                Xoatrangdulieu();
                Loaddata();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Thông Báo",
              MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
        }

        
    }
}   
