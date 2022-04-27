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
    public partial class fr_lop : Form
    {
        Database dtbase = new Database();
        public fr_lop()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            LoadData();
            //thiết lập các thuộc tính cho dgv
            dgvlop.Columns[0].HeaderText = "MÃ LỚP";
            dgvlop.Columns[1].HeaderText = "TÊN LỚP";
            dgvlop.Columns[0].Width = 200;
            dgvlop.Columns[1].Width = 200;
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
        }
        void Xoatrangdulieu()
        {
            txtmalop.Text = "";
            txttenlop.Text = "";
        }
        void LoadData()
        {
            DataTable dtLop = new DataTable();
            dtLop = dtbase.DataReader("select * from tblLop");
            dgvlop.DataSource = dtLop; //Gán dữ liệu vào dgv
        }

        private void dgvlop_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnsua.Enabled = true;
            btnxoa.Enabled = true;
            try
            {
                txtmalop.Text = dgvlop.CurrentRow.Cells[0].Value.ToString();
                txttenlop.Text = dgvlop.CurrentRow.Cells[1].Value.ToString();
            }
            catch
            {
            }
            LoadData();
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            if (txtmalop.Text.Trim() == "" || txttenlop.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập đủ dữ liệu");
                return;
            }
            //Kiểm tra xem mã có trùng trước khi thêm vào csdl
            string malop = txtmalop.Text;
            DataTable dtlop = dtbase.DataReader("Select * from tblLop where MaLop='" + malop + "'");
            if (dtlop.Rows.Count > 0)
            {
                MessageBox.Show("Đã có lớp với mã " + malop + ", Bạn hãy nhập mã khác");
                txtmalop.Focus();
                return;
            }
            //Tạo câu lệnh sql
            string sqlInsertLop = "insert into tblLop values('" + txtmalop.Text + "',N'" + txttenlop.Text + "')";
            dtbase.DataChange(sqlInsertLop);
            LoadData();
            MessageBox.Show("Thêm thành công", "Thông báo");
            Xoatrangdulieu();
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            if (txtmalop.Text.Trim() == "" || txttenlop.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập đủ dữ liệu");
                return;
            }
            dtbase.DataChange("Update tblLop set MaLop = '" + txtmalop.Text + "'," +
                "TenLop = N'" + txttenlop.Text + "'Where MaLop='" + txtmalop.Text + "'");
            LoadData();
            MessageBox.Show("Sửa thành công", "Thông báo");
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            btnthem.Enabled = true;
            Xoatrangdulieu();
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa lớp có mã " + txtmalop.Text + " không?",
                "TB", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dtbase.DataChange("delete tblLop where MaLop='" + txtmalop.Text + "'");
                btnsua.Enabled = false;
                btnxoa.Enabled = false;
                btnthem.Enabled = true;
                LoadData();
                Xoatrangdulieu();
            }
        }

        private void btnthoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Thông báo",
          MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
        }
    }
}
