using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PerpustakaanAppMVC.Model.Entity;
using PerpustakaanAppMVC.Controller;

namespace PerpustakaanAppMVC.View
{
    public partial class FrmMahasiswa : Form
    {
        // constructor
        private List<Mahasiswa> listOfMahasiswa = new List<Mahasiswa>();
        private MahasiswaController controller;
        public FrmMahasiswa()
        {
            InitializeComponent();
            InisialisasiListView();
            controller = new MahasiswaController();
            LoadDataMahasiswa();
        }
       
        private void InisialisasiListView()
        {
            lvwMahasiswa.View = System.Windows.Forms.View.Details;
            lvwMahasiswa.FullRowSelect = true;
            lvwMahasiswa.GridLines = true;
            lvwMahasiswa.Columns.Add("No.", 35, HorizontalAlignment.Center);
            lvwMahasiswa.Columns.Add("Npm", 91, HorizontalAlignment.Center);
            lvwMahasiswa.Columns.Add("Nama", 350, HorizontalAlignment.Left);
            lvwMahasiswa.Columns.Add("Angkatan", 80, HorizontalAlignment.Center);
        }
        private void LoadDataMahasiswa()
        {
            // kosongkan listview
            lvwMahasiswa.Items.Clear();
            // panggil method ReadAll dan tampung datanya ke dalam collection
            listOfMahasiswa = controller.ReadAll();
            // ekstrak objek mhs dari collection
            foreach (var mhs in listOfMahasiswa)
            {
                var noUrut = lvwMahasiswa.Items.Count + 1;
                var item = new ListViewItem(noUrut.ToString());
                item.SubItems.Add(mhs.Npm);
                item.SubItems.Add(mhs.Nama);
                item.SubItems.Add(mhs.Angkatan);
                // tampilkan data mhs ke listview
                lvwMahasiswa.Items.Add(item);
            }
        }
        private void OnCreateEventHandler(Mahasiswa mhs)
        {
            // tambahkan objek mhs yang baru ke dalam collection
            listOfMahasiswa.Add(mhs);
            int noUrut = lvwMahasiswa.Items.Count + 1;
            // tampilkan data mhs yg baru ke list view
            ListViewItem item = new ListViewItem(noUrut.ToString());
            item.SubItems.Add(mhs.Npm);
            item.SubItems.Add(mhs.Nama);
            item.SubItems.Add(mhs.Angkatan);
            lvwMahasiswa.Items.Add(item);
        }
        // method event handler untuk merespon event OnUpdate,
        private void OnUpdateEventHandler(Mahasiswa mhs)
        {
            // ambil index data mhs yang edit
            int index = lvwMahasiswa.SelectedIndices[0];
            // update informasi mhs di listview
            ListViewItem itemRow = lvwMahasiswa.Items[index];
            itemRow.SubItems[1].Text = mhs.Npm;
            itemRow.SubItems[2].Text = mhs.Nama;
            itemRow.SubItems[3].Text = mhs.Angkatan;
        }


        private void btnTambah_Click(object sender, EventArgs e)
        {
            // buat objek form entry data mahasiswa
            FrmEntryMahasiswa frmEntry = new FrmEntryMahasiswa("Tambah DataMahasiswa", controller);
            // mendaftarkan method event handler untuk merespon event OnCreate frmEntry.OnCreate += OnCreateEventHandler;
            // tampilkan form entry mahasiswa
            frmEntry.ShowDialog();

        }

        private void btnPerbaiki_Click(object sender, EventArgs e)
        {
            if (lvwMahasiswa.SelectedItems.Count > 0)
            {
                // ambil objek mhs yang mau diedit dari collection
                Mahasiswa mhs = listOfMahasiswa[lvwMahasiswa.SelectedIndices[0]];
                // buat objek form entry data mahasiswa
                FrmEntryMahasiswa frmEntry = new FrmEntryMahasiswa("Edit Data Mahasiswa", mhs, controller);
                // mendaftarkan method event handler untuk merespon event OnUpdate frmEntry.OnUpdate += OnUpdateEventHandler;
                // tampilkan form entry mahasiswa
                frmEntry.ShowDialog();
            }
            else // data belum dipilih
            {
                MessageBox.Show("Data belum dipilih", "Peringatan",
               MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
            }

        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (lvwMahasiswa.SelectedItems.Count > 0)
            {
                var konfirmasi = MessageBox.Show("Apakah data mahasiswa ingin dihapus ? ", "Konfirmasi",
               
                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (konfirmasi == DialogResult.Yes)
                {
                    // ambil objek mhs yang mau dihapus dari collection
                    Mahasiswa mhs =
                   listOfMahasiswa[lvwMahasiswa.SelectedIndices[0]];
                    // panggil operasi CRUD
                    var result = controller.Delete(mhs);
                    if (result > 0) LoadDataMahasiswa();
                }
            }
            else // data belum dipilih
            {
                MessageBox.Show("Data mahasiswa belum dipilih !!!", "Peringatan",
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnSelesai_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            // Ambil teks dari kotak teks pencarian
            string keyword = txtNama.Text.Trim();

            // Cek apakah kotak teks tidak kosong
            if (!string.IsNullOrEmpty(keyword))
            {
                // Bersihkan listview
                lvwMahasiswa.Items.Clear();

                // Filter mahasiswa berdasarkan kata kunci
                var searchResults = listOfMahasiswa
                    .Where(mhs => mhs.Npm.Contains(keyword) || mhs.Nama.Contains(keyword) || mhs.Angkatan.Contains(keyword))
                    .ToList();

                // Tampilkan hasil pencarian di listview
                foreach (var mhs in searchResults)
                {
                    var noUrut = lvwMahasiswa.Items.Count + 1;
                    var item = new ListViewItem(noUrut.ToString());
                    item.SubItems.Add(mhs.Npm);
                    item.SubItems.Add(mhs.Nama);
                    item.SubItems.Add(mhs.Angkatan);
                    lvwMahasiswa.Items.Add(item);
                }
            }
            else
            {
                // Jika kotak teks pencarian kosong, tampilkan semua data
                LoadDataMahasiswa();
            }
        }
      

        private void txtNama_TextChanged(object sender, EventArgs e)
        {
            btnCari_Click(sender, e);
        }
    }
}
