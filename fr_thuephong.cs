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
    public partial class fr_thuephong : Form
    {
        Database Database = new Database();
        public fr_thuephong()
        {
            InitializeComponent();
        }
        private void Form11_Load(object sender, EventArgs e)
        {
            //nạp dữ liệu vào combobox
            cbxmsv.DataSource = Database.DataReader("Select MaSV from tblSinhVien");
            cbxmsv.ValueMember = "MaSV";
            cbxmsv.DisplayMember = "MaSV";
            cbxmsv.Text = "";
            
            cbxmaphong.DataSource = Database.DataReader("Select MaPhong from tblPhong");
            cbxmaphong.ValueMember = "MaPhong";
            cbxmaphong.DisplayMember = "MaPhong";
            cbxmaphong.Text = "";

            Loaddata();
            dgvsvphong.Columns[0].HeaderText = "MÃ SỐ THUÊ";
            dgvsvphong.Columns[1].HeaderText = "MÃ SINH VIÊN";
            dgvsvphong.Columns[2].HeaderText = "MÃ PHÒNG";
            dgvsvphong.Columns[3].HeaderText = "NGÀY BẮT ĐẦU";
            dgvsvphong.Columns[4].HeaderText = "NGÀY KẾT THÚC";
            dgvsvphong.Columns[5].HeaderText = "GHI CHÚ";
            //ẩn nút sửa,xoá
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            HienChiTiet(false);
            Loaddata();
        }
        //Phương thức ẩn hiện các control trong groupBox Chi tiết
        private void HienChiTiet(bool hien)
        {
            txtMST.Enabled = hien;
            cbxmsv.Enabled = hien;
            cbxmaphong.Enabled = hien;
            dtpbatdau.Enabled = hien;
            dtpKetthuc.Enabled = hien;
            txtghichu.Enabled = hien;
            //Ẩn hiện 2 nút Lưu và Hủy
            btnluu.Enabled = hien;
            btnhuy.Enabled = hien;
        }
        void Loaddata()
        {
            dgvsvphong.DataSource = Database.DataReader("Select * from tblSinhVienPhong");
        }
        void Xoatrangdulieu()
        {
            txtMST.Text = "";
            cbxmsv.SelectedIndex = -1;
            cbxmaphong.SelectedIndex = -1 ;
            dtpbatdau.Value = DateTime.Today;
            dtpKetthuc.Value = DateTime.Today;
            txtghichu.Text = "";
        }
        private void dgvphong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMST.Text = dgvsvphong.CurrentRow.Cells[0].Value.ToString();
            cbxmsv.SelectedValue = dgvsvphong.CurrentRow.Cells[1].Value.ToString();
            cbxmaphong.SelectedValue = dgvsvphong.CurrentRow.Cells[2].Value.ToString();
            dtpbatdau.Text = dgvsvphong.CurrentRow.Cells[3].Value.ToString();
            dtpKetthuc.Text = dgvsvphong.CurrentRow.Cells[4].Value.ToString();
            txtghichu.Text = dgvsvphong.CurrentRow.Cells[5].Value.ToString();
            //Hiển thị các nút cần thiết
            btnsua.Enabled = true;
            btnxoa.Enabled = true;
            btnhuy.Enabled = true;
            btnthem.Enabled = false;
            
        }

        private void btnTK_Click(object sender, EventArgs e)
        {
            //Kiểm tra đã nhập thông tin tìm kiếm chưa
            if (txttkmst.Text.Trim() == "")
            {
                errorProvider1.SetError(txttkmst, "Hãy Nhập Mã Sinh Viên");
                errorProvider1.SetError(txtTKMSV, "Hãy Nhập Tên Sinh Viên");
            }
            else
            {
                errorProvider1.Clear();

            }
            //Viet cau lenh SQL cho tim kiem 
            string sql = "SELECT * FROM tblSinhVienPhong where MaSoThue is not null ";
            //Tim theo mã số thuê 
            if (txttkmst.Text.Trim() != "")
            {
                sql += " and MaSoThue like '%" + txttkmst.Text + "%'";
            }
            //Tìm theo mã sinh viên
            if (txtTKMSV.Text.Trim() != "")
            {
                sql += " and MaSV like N'%" + txtTKMSV.Text + "%'";
            }
            //Load dữ liệu tìm được lên dataGridView
            dgvsvphong.DataSource = Database.DataReader(sql);
            
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            Xoatrangdulieu();
            //Cấm nút sửa, xoá
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            //Hiện GroupBox Chi tiết
            HienChiTiet(true);
        }

        private void btnluu_Click(object sender, EventArgs e)
        {
            string sql = "";
            // sử dụng control ErrorProvider để hiển thị lỗi
            //Kiểm tra đã nhập mã số thuê
            if (txtMST.Text.Trim() == "")
            {
                errorProvider1.SetError(txtMST, "Hãy Nhập Mã Số Thuê!");
                return;
            }
            else
            {
                errorProvider1.Clear();
            } 
            //Kiểm  tra các cbx bị thiếu
            if (cbxmsv.Text.Trim() == "" || cbxmaphong.Text.Trim() == "" )
            {
                errorProvider1.SetError(cbxmsv, "Bạn phải chọn mã sinh viên");
                errorProvider1.SetError(cbxmaphong, "Bạn phải chọn mã phòng");
            }
            else
            {
                errorProvider1.Clear();
            }
            //Kiểm tra ngày kết thúc xem có lớn hơn ngày bắt đầu không
            if (dtpKetthuc.Value < dtpbatdau.Value)
            {
                errorProvider1.SetError(dtpKetthuc, "Ngày không hợp lệ");
                return;
            }
            else
            {
                errorProvider1.Clear();
            }
            //Nếu nút Thêm enable thì thực hiện thêm  mới
            if (btnthem.Enabled == true)
            {  //Kiểm  tra  xem  ô  nhập  mã số thuê  có  bị  trống  không 
                if (txtMST.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtMST, "Hãy nhập mã số thuê");
                    return;
                }
                else
                {  //Kiểm tra xem mã số thuê đã tồn tại chưa đẻ tránh việc  insert  mới  bị  lỗi  
                    sql = "Select  *  From tblSinhVienPhong Where MaSoThue  = '" + txtMST.Text + "'";
                    DataTable dtSvp = Database.DataReader(sql);
                    if (dtSvp.Rows.Count > 0)
                    {
                        errorProvider1.SetError(txtMST, "Mã số thuê bị trùng");
                        return;
                    }
                    errorProvider1.Clear();
                }
                //Insert vao CSDL
                sql = "INSERT INTO tblSinhVienPhong(MaSoThue, MaSV, MaPhong, NgayBatDau, NgayKetThuc, GhiChu) VALUES(";
                sql += "'" + txtMST.Text + "','" + cbxmsv.SelectedValue.ToString() + "','" + cbxmaphong.SelectedValue.ToString() + "','" + dtpbatdau.Value.ToString("MM/dd/yyyy") + "'" +
                    ",'" + dtpKetthuc.Value.ToString("MM/dd/yyyy") + "',N'" + txtghichu.Text + "')";
                
            }
            //Nếu nút Sửa enable=true thì thực hiện cập nhật dữ liệu
            if (btnsua.Enabled == true)
            {
                sql = "Update tblSinhVienPhong SET MaSV ='" + cbxmsv.SelectedValue.ToString() + "',MaPhong='" + cbxmaphong.SelectedValue.ToString() + "'" +
                   ",NgayBatDau='" + dtpbatdau.Value.ToString("MM/dd/yyyy") + "',NgayKetThuc='" + dtpKetthuc.Value.ToString("MM/dd/yyyy") + "'," +
                   "GhiChu=N'" + txtghichu.Text + "' Where MaSoThue='" + txtMST.Text + "'";
                    //cập nhật data
                dgvsvphong.DataSource= Database.DataReader(sql);
                Loaddata();
            }
            //Nếu nút Xóa enable=true thì thực hiện xóa dữ liệu
            if (btnxoa.Enabled == true)
            {
                sql = "Delete From tblSinhVienPhong Where MaSoThue ='" + txtMST.Text + "'";
            }
            Database.DataChange(sql);
            //Cap nhat lai DataGrid
            sql = "Select * from tblSinhVienPhong ";
            dgvsvphong.DataSource = Database.DataReader(sql);
            //Ẩn hiện các nút phù hợp chức năng
            HienChiTiet(false);
            Loaddata();
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
        }

        private void btnhuy_Click(object sender, EventArgs e)
        {
            //Thiết lập lại các nút như ban đầu
            btnxoa.Enabled = false;
            btnsua.Enabled = false;
            btnthem.Enabled = true;
            //xoá trắng chi tiết
            Xoatrangdulieu();
            //không cho nhập vào groupBox chi tiết
            HienChiTiet(false);
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            //Ẩn hai nút Thêm và Xóa
            btnthem.Enabled = false;
            btnxoa.Enabled = false;
            //Hiện gropbox chi tiết
            HienChiTiet(true);
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            //Bật Message Box cảnh báo người sử dụng
            if (MessageBox.Show("Bạn có muốn xoá không?", "Thông Báo"
               , MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                Database.DataChange("Delete tblSinhVienPhong Where MaSoThue = '" + txtMST.Text + "'");
                btnsua.Enabled = false;
                btnthem.Enabled = true;
                btnxoa.Enabled = false;
                Xoatrangdulieu();
                Loaddata();
            }
            //Hiện gropbox chi tiết
            HienChiTiet(true);
        }

        private void btnthoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Thông Báo",
           MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
        }
    }
}
