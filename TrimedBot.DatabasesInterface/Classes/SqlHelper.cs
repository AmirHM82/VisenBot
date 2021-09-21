using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrimedBot.DatabasesInterface.Classes
{
    public class SqlHelper
    {
        private SqlConnection connection;

        public SqlHelper(string connectionString)
        {
            try
            {
                connection = new SqlConnection(connectionString);
            }
            catch (Exception e)
            {
                MessageBox.Show("Connection string error: ", e.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async Task<bool> ConnectionState()
        {
            bool result = false;
            try
            {
                if (connection != null)
                {
                    if (connection.State == System.Data.ConnectionState.Closed)
                        await connection.OpenAsync();
                   result = true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Database connection error ", e.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }
    }
}
