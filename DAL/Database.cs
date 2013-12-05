using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.Common;
using System.Data;

namespace FestivalApp.Utilities
{
    class Database
    {
        private static ConnectionStringSettings ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionString"];
            }
        }

        private static DbConnection GetConnection()
        {
            DbConnection connection = DbProviderFactories.GetFactory(ConnectionString.ProviderName).CreateConnection();
            connection.ConnectionString = ConnectionString.ConnectionString;
            connection.Open();

            return connection;
        }

        public static void ReleaseConnection(DbConnection connection)
        {
            if (connection != null)
            {
                connection.Close();
                connection = null;
            }
        }

        private static DbCommand BuildCommand(string query, params DbParameter[] parameters)
        {
            DbCommand command = GetConnection().CreateCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = query;

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            return command;
        }

        public static DbParameter CreateParameter(string name, object value)
        {
            DbParameter param = DbProviderFactories.GetFactory(ConnectionString.ProviderName).CreateParameter();
            param.ParameterName = name;
            param.Value = value;

            return param;
        }

        public static DbDataReader GetData(string query, params DbParameter[] parameters)
        {
            DbCommand command = null;
            DbDataReader reader = null;

            try
            {
                command = BuildCommand(query, parameters);
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                return reader;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (reader != null) reader.Close();
                if (command != null) ReleaseConnection(command.Connection);

                throw e;
            }
        }

        public static int ModifyData(string query, params DbParameter[] parameters)
        {
            DbCommand command = null;

            try
            {
                command = BuildCommand(query, parameters);
                int affectedRows = command.ExecuteNonQuery();

                ReleaseConnection(command.Connection);

                return affectedRows;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (command != null) ReleaseConnection(command.Connection);

                throw e;
            }
        }

        public static DbTransaction BeginTransaction()
        {
            DbConnection connection = null;

            try
            {
                connection = GetConnection();
                return connection.BeginTransaction();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (connection != null) ReleaseConnection(connection);

                throw e;
            }
        }

        private static DbCommand BuildCommand(DbTransaction transaction, string query, params DbParameter[] parameters)
        {
            DbCommand command = transaction.Connection.CreateCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = query;
            Console.WriteLine(command.Transaction);

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            return command;
        }

        public static DbDataReader GetData(DbTransaction transaction, string query, params DbParameter[] parameters)
        {
            DbCommand command = null;
            DbDataReader reader = null;

            try
            {
                command = BuildCommand(transaction, query, parameters);
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                return reader;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (reader != null) reader.Close();
                if (command != null) ReleaseConnection(command.Connection);

                throw e;
            }
        }

        public static int ModifyData(DbTransaction transaction, string query, params DbParameter[] parameters)
        {
            DbCommand command = null;

            try
            {
                command = BuildCommand(transaction, query, parameters);
                int affectedRows = command.ExecuteNonQuery();

                ReleaseConnection(command.Connection);

                return affectedRows;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (command != null) ReleaseConnection(command.Connection);

                throw e;
            }
        }
    }
}
