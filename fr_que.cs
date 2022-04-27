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
    public partial class fr_que : Form
    {
        Database dtbase = new Database();
        public fr_que()
        {
            InitializeComponent();
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            LoadData();
            dgvque.Columns[0].HeaderText = "MÃ QUÊ";
            dgvque.Columns[1].HeaderText = "TÊN QUÊ";
            dgvque.Columns[0].Width = 250;
            dgvque.Columns[1].Width = 250;
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
        }
    
    void LoadData()
    {
        DataTable dtQue = new DataTable();
        dtQue = dtbase.DataReader("select * from tblQue");
        dgvque.DataSource = dtQue; //Gán dữ liệu vào dgv
    }
    void Xoatrangdulieu()
    {
        txtmaque.Text = "";
        txttenque.Text = "";
    }

        private void dgvque_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnsua.Enabled = true;
            btnxoa.Enabled = true;
            try
            {
                txtmaque.Text = dgvque.CurrentRow.Cells[0].Value.ToString();
                txttenque.Text = dgvque.CurrentRow.Cells[1].Value.ToString();
            }
            catch
            {
            }
            LoadData();
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            if (txtmaque.Text.Trim() == "" || txttenque.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập đủ dữ liệu");
                return;
            }
            //Kiểm tra xem mã có trùng trước khi thêm vào csdl
            string maque = txtmaque.Text;
            DataTable dtQue = dtbase.DataReader("Select * from tblQue where MaQue='" + maque + "'");
            if (dtQue.Rows.Count > 0)
            {
                MessageBox.Show("Đã có quê với mã " + maque + ", Bạn hãy nhập mã khác");
                txtmaque.Focus();
                return;
            }
            //Tạo câu lệnh sql
            string sqlInsertQue = "insert into tblQue values('" + txtmaque.Text + "'," +
                "N'" + txttenque.Text + "')";
            dtbase.DataChange(sqlInsertQue);
            LoadData();
            Xoatrangdulieu();
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            
            if (txtmaque.Text.Trim() == "" || txttenque.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập đủ dữ liệu");
                return;
            }
            dtbase.DataChange("Update tblQue set MaQue = '" + txtmaque.Text + "'," +
                "TenQue = N'" + txttenque.Text + "'Where MaQue='"+ txtmaque.Text +"'");
            LoadData();
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            btnthem.Enabled = true;
            Xoatrangdulieu();
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa quê có mã " + txtmaque.Text + " không?",
"TB", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dtbase.DataChange("delete tblQue where MaQue='" + txtmaque.Text + "'");
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

