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
    public partial class fr_chi_phiphong : Form
    {
        Database dtbasee = new Database();
        int tiennha, tiendien, tiennuoc, tienvesinh, tienphat;
        public fr_chi_phiphong()
        {
            InitializeComponent();
        }
        //Phương thức ẩn hiện các control trong groupBox Chi tiết
        private void HienChiTiet(bool hien)
        {
            cbxmaphong.Enabled = hien;
            cbxthang.Enabled = hien;
            cbxnam.Enabled = hien;
            cbxtiennha.Enabled = hien;
            txttiendien.Enabled = hien;
            txttiennuoc.Enabled = hien;
            txttienvesinh.Enabled = hien;
            txttienphat.Enabled = hien;
            dtpngayHH.Enabled = hien;
            dtpngaydong.Enabled = hien;
            txttongtien.Enabled = hien;
            //Ẩn hiện 2 nút Lưu và Hủy
            btnluu.Enabled = hien;
            btnhuy.Enabled = hien;
            btntinhtien.Enabled = hien;
        }
        void Xoatrangdulieu()
        {
            txttienvesinh.Text = "";
            dtpngaydong.Value = DateTime.Today;
            dtpngayHH.Value = DateTime.Today;
            txttienphat.Text = "";
            txttiennuoc.Text = "";
            cbxtiennha.SelectedIndex = -1;
            txttiendien.Text = "";
            cbxthang.SelectedIndex = -1;
            cbxnam.SelectedIndex = -1;
            txttongtien.Text = ""; 
            cbxmaphong.SelectedIndex = -1;
        }
        void LoadData()
        {
           dgvchiphiphong.DataSource = dtbasee.DataReader("select * from tblChiPhiPhong");
        }
        private void Form14_Load(object sender, EventArgs e)
        {
            cbxmaphong.DataSource = dtbasee.DataReader("Select MaPhong from tblSinhVienPhong");
            cbxmaphong.ValueMember = "MaPhong";
            cbxmaphong.DisplayMember = "MaPhong";
            cbxmaphong.Text = "";

            LoadData();
            dgvchiphiphong.Columns[0].HeaderText = "MÃ PHÒNG";
            dgvchiphiphong.Columns[1].HeaderText = "THÁNG";
            dgvchiphiphong.Columns[2].HeaderText = "NĂM";
            dgvchiphiphong.Columns[3].HeaderText = "TIỀN NHÀ";
            dgvchiphiphong.Columns[4].HeaderText = "TIỀN ĐIỆN";
            dgvchiphiphong.Columns[5].HeaderText = "TIỀN NƯỚC";
            dgvchiphiphong.Columns[6].HeaderText = "TIỀN VỆ SINH";
            dgvchiphiphong.Columns[7].HeaderText = "TIỀN PHẠT";
            dgvchiphiphong.Columns[8].HeaderText = "NGÀY HẾT HẠN";
            dgvchiphiphong.Columns[9].HeaderText = "NGÀY ĐÓNG";
            dgvchiphiphong.Columns[10].HeaderText = "TỔNG TIỀN";
            //Ẩn nút Sửa,xóa      
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            //Ẩn groupBox chi tiết
            HienChiTiet(false);
            LoadData();
            Xoatrangdulieu();
        }

        private void dgvchiphiphong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           try
            {
                cbxmaphong.SelectedValue = dgvchiphiphong.CurrentRow.Cells[0].Value.ToString();
                cbxthang.SelectedItem = dgvchiphiphong.CurrentRow.Cells[1].Value.ToString();
                cbxnam.SelectedItem = dgvchiphiphong.CurrentRow.Cells[2].Value.ToString();
                cbxtiennha.SelectedItem = dgvchiphiphong.CurrentRow.Cells[3].Value.ToString();
                txttiendien.Text = dgvchiphiphong.CurrentRow.Cells[4].Value.ToString();
                txttiennuoc.Text = dgvchiphiphong.CurrentRow.Cells[5].Value.ToString();
                txttienvesinh.Text = dgvchiphiphong.CurrentRow.Cells[6].Value.ToString();
                txttienphat.Text = dgvchiphiphong.CurrentRow.Cells[7].Value.ToString();
                dtpngayHH.Value = (DateTime)dgvchiphiphong.CurrentRow.Cells[8].Value;
                dtpngaydong.Value = (DateTime)dgvchiphiphong.CurrentRow.Cells[9].Value;
                txttongtien.Text = dgvchiphiphong.CurrentRow.Cells[10].Value.ToString();
            }
            catch
            {
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
            if (txtThang.Text.Trim() == "")
            {
                errorProvider1.SetError(txtThang, "Hãy Nhập tháng");
            }
            else
            {
                errorProvider1.Clear();
            }
            //Cấm nút Sửa và Xóa
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            //Viet cau lenh SQL cho tim kiem 
            string sql = "SELECT * FROM tblChiPhiPhong where Thang is not null ";
            //Tim theo mã phòng
            if (txtThang.Text.Trim() != "")
            {
                sql += " and Thang like '%" + txtThang.Text + "%'";
            }
      
            //Load dữ liệu tìm được lên dataGridView
            dgvchiphiphong.DataSource = dtbasee.DataReader(sql);
        }

        private void btnthemmoi_Click(object sender, EventArgs e)
        {
           
            Xoatrangdulieu();
            //Ẩn nút sửa, xoá
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            HienChiTiet(true);
        }
        private void btnsua_Click(object sender, EventArgs e)
        {
            //Ẩn nút thêm, xoá
            btnxoa.Enabled = false;
            btnthemmoi.Enabled = false;
            HienChiTiet(true);
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            // Cảnh báo xoá
            if (MessageBox.Show("Bạn có muốn xoá không?", "Thông báo", MessageBoxButtons.YesNo
                , MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                dtbasee.DataChange("Delete tblChiPhiPhong Where MaPhong = '" + cbxmaphong.SelectedValue.ToString() + "'");
                btnsua.Enabled = false;
                btnthemmoi.Enabled = true;
                btnxoa.Enabled = false;
                Xoatrangdulieu();
                LoadData();
            }
            HienChiTiet(true);  
        }

        private void btnluu_Click(object sender, EventArgs e)
        {
            string sql = "";
            // sử dụng control ErrorProvider để hiển thị lỗi
            //Kiểm tra đã nhập đủ thông tin chưa
            if (cbxmaphong.Text.Trim() == "")
            {
                errorProvider1.SetError(cbxmaphong, "Hãy Chọn Mã Phòng");
                return;
            }
            else
            {
                errorProvider1.Clear();
            }
            
            if (cbxthang.Text.Trim() == "")
            {
                errorProvider1.SetError(cbxthang, "Hãy nhập tháng");
            }
            else
            {
                errorProvider1.Clear();
            }
           
            if (cbxnam.Text.Trim() =="")
            {
                errorProvider1.SetError(cbxnam, "Hãy nhập năm");
            }
            else
            {
                errorProvider1.Clear();
            }
            if(cbxtiennha.Text.Trim() =="")
            {
                errorProvider1.SetError(cbxtiennha, "Hãy nhập tiền nhà");
            }    
            else
            {
                errorProvider1.Clear();
            }
            if(txttiendien.Text.Trim()=="")
            {
                errorProvider1.SetError(txttiendien, "Hãy nhập tiền điện");
            }    
            else
            {
                errorProvider1.Clear();
            }
            if(txttiennuoc.Text.Trim()=="")
            {
                errorProvider1.SetError(txttiennuoc, "Hãy nhập tiền nước");
            }    
            else
            {
                errorProvider1.Clear();
            }
            if(txttienvesinh.Text.Trim()=="")
            {
                errorProvider1.SetError(txttienvesinh, "Hãy nhập tiền vệ sinh");
            }    
            else
            {
                errorProvider1.Clear();
            }
            if(txttienphat.Text.Trim() =="")
            {
                errorProvider1.SetError(txttienphat, "Hãy Nhập tiền phạt");
            }    
            else
            {
                errorProvider1.Clear();
            }
            if(txttongtien.Text.Trim() =="")
            {
                errorProvider1.SetError(txttongtien, "hãy tính tổng tiền");
            }   
            else
            {
                errorProvider1.Clear();
            }
            //Nếu nút Thêm enable = true thì thực hiện thêm  mới
            if (btnthemmoi.Enabled == true)
            {  //Kiểm  tra  xem  ô  nhập   có  bị  trống  không 
                if (cbxmaphong.Text.Trim() == "")
                {
                    errorProvider1.SetError(cbxmaphong, "Hãy chọn mã phòng");
                    return;
                }
                else
                {  //Kiểm tra xem mã phòng đã tồn tại chưa đẻ tránh việc  insert  mới  bị  lỗi  
                    sql = "Select  *  From tblChiPhiPhong Where MaPhong  = '" + cbxmaphong.SelectedValue.ToString() + "'";
                    DataTable dtmp = dtbasee.DataReader(sql);
                    if (dtmp.Rows.Count > 0)
                    {
                        MessageBox.Show("Mã phòng đã tồn tại", "Thông báo");
                        errorProvider1.SetError(cbxmaphong, "Mã phòng đã tồn tại");
                        return;
                    }
                    errorProvider1.Clear();
                }
                 
                //Insert vao CSDL
                 sql = "INSERT INTO tblChiPhiPhong(MaPhong, Thang, Nam, TienNha, TienDien, TienNuoc, TienVeSinh, TienPhat, NgayHetHan, NgayDong,TongTien) VALUES(";
                sql += "'" + cbxmaphong.Text + "','" + cbxthang.Text + "','" + cbxnam.Text + "'" +
                  ",'" +  cbxtiennha.Text + "','" + txttiendien.Text+ "','" + txttiennuoc.Text + "'," +
                "'" + txttienvesinh.Text + "','" + txttienphat.Text + "','" + dtpngayHH.Value.ToString("yyyy/MM/dd") + "'," +
                "'" + dtpngaydong.Value.ToString("yyyy/MM/dd") + "','" + txttongtien.Text + "')";
                MessageBox.Show("Thêm Thành Công", "Thông báo");
            }
             //Nếu nút Sửa enable=true thì thực hiện cập nhật dữ liệu
            if (btnsua.Enabled == true)
            {
                sql = "Update tblChiPhiPhong SET Thang = '" + cbxthang.Text + "',Nam='" + cbxnam.Text + "'" +
                     ",TienNha='" + cbxtiennha.Text + "',TienDien='" + txttiendien.Text + "'," +
                     "TienNuoc='" + txttiennuoc.Text + "',TienVeSinh='" + txttienvesinh.Text +"',TienPhat='" +txttienphat.Text + "'," +
                     "NgayHetHan='" + dtpngayHH.Value.ToString("yyyy/MM/dd") + "',NgayDong ='" + dtpngaydong.Value.ToString("yyyy/MM/dd") + "',TongTien = '" + txttongtien.Text + "' Where MaPhong='" + cbxmaphong.SelectedValue.ToString() + "'";
                dgvchiphiphong.DataSource = dtbasee.DataReader(sql);
                MessageBox.Show("Sửa thành công", "Thông báo");
                LoadData();
            }
            //Nếu nút Xóa enable=true thì thực hiện xóa dữ liệu
            if (btnxoa.Enabled == true)
            {
                sql = "Delete From tblChiPhiPhong Where MaPhong ='" + cbxmaphong.SelectedValue.ToString() + "'";
            }
            dtbasee.DataChange(sql);
            //Cap nhat lai DataGrid
            sql = "Select * from tblChiPhiPhong";
            dgvchiphiphong.DataSource = dtbasee.DataReader(sql);
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

        private void btnthoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Thông Báo",
             MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
        }

        private void btntinhtien_Click(object sender, EventArgs e)
        {
            tiennha = int.Parse(cbxtiennha.Text);
            tiendien = int.Parse(txttiendien.Text);
            tiennuoc = int.Parse(txttiennuoc.Text);
            tienvesinh = int.Parse(txttienvesinh.Text);
            tienphat = int.Parse(txttienphat.Text);
            txttongtien.Text = (tiennha + tiendien + tiennuoc + tienvesinh + tienphat).ToString();
        }
    }
}
