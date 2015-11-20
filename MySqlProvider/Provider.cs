using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;

namespace MySqlProvider
{
    public class Provider: IDisposable
    {
        private MySqlConnection m_conn;
        private string m_connectionString;

        public static Provider CreateProvider(string connectionString="")
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                ConnectionStringSettingsCollection settings =
                ConfigurationManager.ConnectionStrings;

                if (settings != null)
                {
                    foreach (ConnectionStringSettings cs in settings)
                    {
                        if (cs.ProviderName == "MySql.Data.MySqlClient")
                            return new Provider(cs.ConnectionString);
                    }

                }
                throw new ArgumentNullException("connectionString", "Value have to assign");
            }
            return new Provider(connectionString);
        }

        private Provider(string connectionString)
        {
            this.m_connectionString = connectionString;
        }

        public void OpenConnection()
        {
            m_conn = new MySql.Data.MySqlClient.MySqlConnection();
            m_conn.ConnectionString = m_connectionString;
            m_conn.Open();
        }

        public DataTable ExecuteSelectCommand(string commandText)
        {
            if (m_conn.State != System.Data.ConnectionState.Open)
                m_conn.Open();
            MySqlCommand cmd = new MySqlCommand(commandText, m_conn);

            MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
            MyAdapter.SelectCommand = cmd;
            DataTable dTable = new DataTable();
            MyAdapter.Fill(dTable);
            return dTable;
            //MySqlDataReader reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    //Console.WriteLn(reader[0], reader[1]...);
            //}
        }

        public int ExecuteUpdateComand(string commandText)
        {
            if (m_conn.State != System.Data.ConnectionState.Open)
                m_conn.Open();
            MySqlCommand cmd = new MySqlCommand(commandText, m_conn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                return reader.RecordsAffected;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                m_conn.Close();
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Provider() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
