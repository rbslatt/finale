using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using MySql.Data.MySqlClient;

namespace backupfinals
{
    public partial class Inventory : Form
    {

        private MySqlConnection connection;
        public Inventory()
        {
            InitializeComponent();
            connection = new MySqlConnection("server=localhost;database=finalsdb;username=root;password=;");
        }
        private void Inventory_Load(object sender, EventArgs e)
        {
            loaddata();
        }
        private void loaddata()
        {
            try
            {
                connection.Open();
                string showallrecords = "SELECT ID, brand, price, flavor FROM user ORDER BY ID ASC";
                MySqlCommand command = new MySqlCommand(showallrecords, connection);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error:" + ex.Message);
            }
            finally
            {

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtID.Text = row.Cells["ID"].Value.ToString();
                txtBrand.Text = row.Cells["brand"].Value.ToString();
                txtPrice.Text = row.Cells["price"].Value.ToString();
                txtFlavor.Text = row.Cells["flavor"].Value.ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string brand = txtBrand.Text;
            string price = txtPrice.Text;
            string flavor = txtFlavor.Text;


            if (string.IsNullOrWhiteSpace(brand) || string.IsNullOrWhiteSpace(price) || string.IsNullOrWhiteSpace(flavor))
            {
                MessageBox.Show("Please enter both brand, price and flavor");
            }
            try
            {
                connection.Open();
                string registerquery = "INSERT INTO user (brand, price, flavor) VALUE (@brand, @price, @flavor)";
                MySqlCommand command = new MySqlCommand(registerquery, connection);
                command.Parameters.AddWithValue("@brand", brand);
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@flavor", flavor);

                int rowaffected = command.ExecuteNonQuery();
                if (rowaffected > 0)
                {
                    MessageBox.Show("Product Successfully Registered");
                }
                else
                {
                    MessageBox.Show("Product Failed to Add!, Please Try Again.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                loaddata();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string brand = txtBrand.Text;
            string price = txtPrice.Text;
            string flavor = txtFlavor.Text;
            string ID = txtID.Text;

            if (string.IsNullOrWhiteSpace(brand) || string.IsNullOrWhiteSpace(price) || string.IsNullOrWhiteSpace(flavor))
            {
                MessageBox.Show("Please enter both brand, price amd flavor");
            }
            try
            {
                connection.Open();
                string registerquery = "UPDATE user SET brand = @brand, price = @price, flavor = @flavor WHERE ID = @id";
                MySqlCommand command = new MySqlCommand(registerquery, connection);
                command.Parameters.AddWithValue("@brand", brand);
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@flavor", flavor);
                command.Parameters.AddWithValue("ID", ID);
                int rowaffected = command.ExecuteNonQuery();
                if (rowaffected > 0)
                {
                    MessageBox.Show("Product Successfully Update");
                }
                else
                {
                    MessageBox.Show("Product Failed to Update!, Please Try Again.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                loaddata();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you really want to delete this record?", "CONFIRMATION", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string ID = txtID.Text;

                if (string.IsNullOrWhiteSpace(ID))
                {
                    MessageBox.Show("No Record Found!");
                    return;
                }
                try
                {
                    connection.Open();
                    string deletequery = "DELETE FROM user WHERE ID = @ID";
                    MySqlCommand command = new MySqlCommand(deletequery, connection);
                    command.Parameters.AddWithValue("@ID", ID);

                    int rowaffected = command.ExecuteNonQuery();
                    if (rowaffected > 0)
                    {
                        MessageBox.Show("Product Successfully Update");
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    loaddata();
                    txtID.Clear();
                    txtBrand.Clear();
                    txtPrice.Clear();
                    txtFlavor.Clear();

                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string search = txtSearch.Text;
                connection.Open();
                string showallrecrods = "SELECT ID, brand, price, flavor FROM user WHERE ID LIKE CONCAT('%', @search, '%') OR brand LIKE CONCAT('%', @search, '%') OR price LIKE CONCAT('%', @search, '%') OR flavor LIKE CONCAT('%', @search, '%') ";
                MySqlCommand command = new MySqlCommand(showallrecrods, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                command.Parameters.AddWithValue("@search", search);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

            }
        }
    }

}
