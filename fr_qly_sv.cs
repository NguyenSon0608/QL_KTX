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
    public partial class fr_qly_sv : Form
    {
        Database database = new Database();
        string imageName;
        public fr_qly_sv()
        {
            InitializeComponent();
        }

        //Phương thức ẩn hiện các control trong groupBox Chi tiết
        private void HienChiTiet(bool hien)
        {
            txtMSV.Enabled = hien;
            txtHoten.Enabled = hien;
            dtpngaysinh.Enabled = hien;
            rdonam.Enabled = hien;
            rdonu.Enabled = hien;
            cbxmaque.Enabled = hien;
            cbxmakhoa.Enabled = hien;
            cbxmalop.Enabled = hien;
            picanh.Enabled = hien;
            //Ẩn nút lưu,huỷ,ảnh
            btnluu.Enabled = hien;
            btnhuy.Enabled = hien;
            btnanh.Enabled = hien;
        }
        private void Form4_Load(object sender, EventArgs e)
        {
            //đưa dữ liệu vào combobox
            cbxmaque.DataSource = database.DataReader("Select MaQue, TenQue from tblQue");
            cbxmaque.ValueMember = "MaQue";
            cbxmaque.DisplayMember = "TenQue";
            cbxmaque.Text = "";

            cbxmakhoa.DataSource = database.DataReader("select MaKhoa,TenKhoa from tblKhoa");
            cbxmakhoa.ValueMember = "MaKhoa";
            cbxmakhoa.DisplayMember = "TenKhoa";
            cbxmakhoa.Text = "";

            cbxmalop.DataSource = database.DataReader("Select MaLop,TenLop from tblLop");
            cbxmalop.ValueMember = "MaLop";
            cbxmalop.DisplayMember = "TenLop";
            cbxmalop.Text = "";

            LoadData();
            dgvsinhvien.Columns[0].HeaderText = "MÃ SINH VIÊN";
            dgvsinhvien.Columns[1].HeaderText = "TÊN SINH VIÊN";
            dgvsinhvien.Columns[2].HeaderText = "NGÀY SINH";
            dgvsinhvien.Columns[3].HeaderText = "GIỚI TÍNH";
            dgvsinhvien.Columns[4].HeaderText = "MÃ QUÊ";
            dgvsinhvien.Columns[5].HeaderText = "MÃ KHOA";
            dgvsinhvien.Columns[6].HeaderText = "MÃ LỚP";
            dgvsinhvien.Columns[7].HeaderText = "ẢNH";
            //Ẩn nút Sửa,xóa      
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            //Ẩn groupBox chi tiết
            HienChiTiet(false);
            LoadData();
        }
        void Xoatrangdulieu()
        {
            txtMSV.Text = "";
            txtHoten.Text = "";
            dtpngaysinh.Value = DateTime.Today;
            rdonam.Checked = false;
            rdonu.Checked = false;
            cbxmaque.SelectedIndex = -1;
            cbxmakhoa.SelectedIndex = -1;
            cbxmalop.SelectedIndex = -1;
            picanh.Image = null;
        }
        void LoadData()
        {
            dgvsinhvien.DataSource = database.DataReader("select * from tblSinhVien");
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMSV.Text = dgvsinhvien.CurrentRow.Cells[0].Value.ToString();
            txtHoten.Text = dgvsinhvien.CurrentRow.Cells[1].Value.ToString();
            dtpngaysinh.Text = dgvsinhvien.CurrentRow.Cells[2].Value.ToString();
            if (dgvsinhvien.CurrentRow.Cells[3].Value.ToString() == "Nam")
            {
                rdonam.Checked = true;
            }
            else
            {
                rdonu.Checked = true;
            }
            cbxmakhoa.SelectedValue = dgvsinhvien.CurrentRow.Cells[5].Value.ToString();
            cbxmaque.SelectedValue = dgvsinhvien.CurrentRow.Cells[4].Value.ToString();
            cbxmalop.SelectedValue = dgvsinhvien.CurrentRow.Cells[6].Value.ToString();
           
            try
            {
                imageName = dgvsinhvien.CurrentRow.Cells[7].Value.ToString();
                picanh.Image = Image.FromFile(Application.StartupPath.ToString() + "\\Image\\AnhSV\\" + imageName);
            }
            catch
            {
                picanh.Image = null;
            }
            //Hiển thị nút sửa,xoá
            btnsua.Enabled = true;
            btnxoa.Enabled = true;
            btnthemmoi.Enabled = false;
            HienChiTiet(false);
        }
        private void btntimkiem_Click(object sender, EventArgs e)
        {
            //Kiểm tra đã nhập thông tin tìm kiếm chưa
            if (txtTKMSV.Text.Trim() == "")
            {
                errorProvider1.SetError(txtTKMSV, "Hãy Nhập Mã Sinh Viên");
            }
            else
            {
                errorProvider1.Clear();
            }
            if(txtTKHoten.Text.Trim() =="")
            {
                errorProvider1.SetError(txtTKHoten, "Hãy Nhập Tên Sinh Viên");
            }    
            else
            {
                errorProvider1.Clear();
            }    
            //Cấm nút Sửa và Xóa
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            //Viet cau lenh SQL cho tim kiem 
            string sql = "SELECT * FROM tblSinhVien where MaSV is not null ";
            //Tim theo mã sinh viên 
            if (txtTKMSV.Text.Trim() != "")
            {
                sql += " and MaSV like '%" + txtTKMSV.Text + "%'";
            }
            //Tìm theo tên sinh viên
            if (txtTKHoten.Text.Trim() != "")
            {
                sql += " and TenSinhVien like N'%" + txtTKHoten.Text + "%'";
            }
            //Load dữ liệu tìm được lên dataGridView
            dgvsinhvien.DataSource = database.DataReader(sql);

        }

        private void btnthemmoi_Click(object sender, EventArgs e)
        {
           
            Xoatrangdulieu();
            //Cấm nút sửa, xoá
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            //Hiện GroupBox Chi tiết
            HienChiTiet(true);
           
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            
            //Ẩn hai nút Thêm và Xóa
            btnthemmoi.Enabled = false;
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
                database.DataChange("Delete tblSinhVien Where MaSV = '" + txtMSV.Text + "'");
                btnsua.Enabled = false;
                btnthemmoi.Enabled = true;
                btnxoa.Enabled = false;
                Xoatrangdulieu();
                LoadData();
            }
            //Hiện gropbox chi tiết
            HienChiTiet(true);
        }

        private void btnluu_Click(object sender, EventArgs e)
        {
            string sql = "";
            // sử dụng control ErrorProvider để hiển thị lỗi
            //Kiểm tra đã nhập mã sinh viên chưa
            if (txtMSV.Text.Trim() == "")
            {
                errorProvider1.SetError(txtMSV, "Hãy Nhập Mã Sinh Viên!");
                return;
            }
            else
            {
                errorProvider1.Clear();
            }
            //Kiểm tra đã nhập tên sinh viên chưa  
            if (txtHoten.Text.Trim() == "")
            {
                errorProvider1.SetError(txtHoten, "Bạn phải nhập tên sinh viên!");
            }
            else
            {
                errorProvider1.Clear();
            }
            //Kiểm  tra các cbx bị thiếu
            if (cbxmaque.Text.Trim() == "" || cbxmakhoa.Text.Trim() == "" ||
                (rdonam.Checked == false && rdonu.Checked == false|| cbxmalop.Text == ""))
            {
                errorProvider1.SetError(cbxmakhoa, "Bạn phải chọn mã khoa");
                errorProvider1.SetError(cbxmaque, "Bạn phải chọn mã quê");
                errorProvider1.SetError(cbxmalop, "Bạn phải chọn mã lớp");
            }
            else
            {
                errorProvider1.Clear();
            }
            //Nếu nút Thêm enable = true thì thực hiện thêm  mới
            if (btnthemmoi.Enabled == true)
            {  //Kiểm  tra  xem  ô  nhập  MaSV  có  bị  trống  không 
                if (txtMSV.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtMSV, "Bạn không để trống mã sinh viên trường này!");
                    return;
                }
                else
                {  //Kiểm tra xem mã sinh viên đã tồn tại chưa đẻ tránh việc  insert  mới  bị  lỗi  
                    sql = "Select  *  From tblSinhVien Where MaSV  = '" + txtMSV.Text + "'";
                    DataTable dtSP = database.DataReader(sql);
                    if (dtSP.Rows.Count > 0)
                    {
                        errorProvider1.SetError(txtMSV, "Mã sinh viên bị trùng");
                        return;
                    }
                    errorProvider1.Clear();
                }
                string GioiTinh = "";
                if (rdonam.Checked == true)
                    GioiTinh = "Nam";
                if (rdonu.Checked == true)
                    GioiTinh = "Nữ";
                //
                //Insert vao CSDL
                sql = "INSERT INTO tblSinhVien(MaSV, TenSinhVien, NgaySinh, GioiTinh, MaQue, MaKhoa, MaLop, Anh) VALUES(";
                sql += "'" + txtMSV.Text + "',N'" + txtHoten.Text + "','" + dtpngaysinh.Value.ToString("MM/dd/yyyy") + "'" +
                    ",N'" + GioiTinh + "','" + cbxmaque.SelectedValue.ToString() + "','" + cbxmakhoa.SelectedValue.ToString() + "','" + cbxmalop.SelectedValue.ToString() + "','" + imageName + "')";
                MessageBox.Show("Thêm thành công", "Thông báo");
            }
            //Nếu nút Sửa enable=true thì thực hiện cập nhật dữ liệu
            string Gioitinh = "";
            if (rdonam.Checked == true)
                Gioitinh = "Nam";
            if (rdonu.Checked == true)
                Gioitinh = "Nữ";

            if (btnsua.Enabled == true)
            {
                sql = "Update tblSinhVien SET TenSinhVien =N'" + txtHoten.Text + "',NgaySinh='" + dtpngaysinh.Value.ToString("MM/dd/yyyy") + "'" +
                     ",GioiTinh=N'" + Gioitinh + "',MaQue='" + cbxmaque.SelectedValue.ToString() + "'," +
                     "MaKhoa='" + cbxmakhoa.SelectedValue.ToString() + "',MaLop='" + cbxmalop.SelectedValue.ToString() + "',Anh='" + imageName + "' Where MaSV='" + txtMSV.Text + "'";
                dgvsinhvien.DataSource = database.DataReader(sql);
                LoadData();
                MessageBox.Show("Sửa thành công", "Thông báo");
            }
            //Nếu nút Xóa enable=true thì thực hiện xóa dữ liệu
            if (btnxoa.Enabled == true)
            {
                sql = "Delete From tblSinhVien Where MaSV ='" + txtMSV.Text + "'";
            }
            database.DataChange(sql);
            //Cap nhat lai DataGrid
            sql = "Select * from tblSinhVien";
            dgvsinhvien.DataSource = database.DataReader(sql);
            //Ẩn hiện các nút phù hợp chức năng
            Xoatrangdulieu();
            LoadData();
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
        }

        private void btnhuy_Click(object sender, EventArgs e)
        {
            //Thiết lập lại các nút như ban đầu
            btnxoa.Enabled = false;
            btnsua.Enabled = false;
            btnthemmoi.Enabled = true;
            //xoá trắng chi tiết
            Xoatrangdulieu();
            //không cho nhập vào groupBox chi tiết
            HienChiTiet(false);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Thông Báo",
             MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
        }

        //chỉ cho phép nhập số nguyên
        private void txtMSV_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                MessageBox.Show("Bạn chỉ được nhập số nguyên");
                e.Handled = true;
            }

        }

        private void btnanh_Click(object sender, EventArgs e)
        {
            string[] pathAnh;
            OpenFileDialog dlgOpen = new OpenFileDialog();
            dlgOpen.Filter = "JPEG Images|*.jpg|All Fies|*.*";
            dlgOpen.InitialDirectory = Application.StartupPath.ToString() + "\\Image\\AnhSV ";
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                picanh.Image = Image.FromFile(dlgOpen.FileName);
                pathAnh = dlgOpen.FileName.Split('\\');
                imageName = pathAnh[pathAnh.Length - 1];

            }
        }

        private void bt_refesh_Click(object sender, EventArgs e)
        {
            btnthemmoi.Enabled = true;
            LoadData();
        }
    }  }
