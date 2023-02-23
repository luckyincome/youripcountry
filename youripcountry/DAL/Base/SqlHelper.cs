using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace youripcountry.DAL.Base
{
    public class SqlHelper
    {
     
        private readonly String  _connectionString;
        public  SqlHelper()
        {

            var configurationBuilder = new ConfigurationBuilder();
           
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);
            var root = configurationBuilder.Build();
            _connectionString = root.GetSection("ConnectionStrings").GetSection("ConnStr").Value;
            //_connectionString = configuration.GetConnectionString("ConnStr");
        }


        public  T GetRecord<T>(string spName, List<ParameterInfo> parameters)
        {
          try {
                T objRecord = default(T);
                using (SqlConnection objConnection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        objConnection.Open();
                    }catch(Exception ex)
                    {
                        string str = ex.Message;
                    }//XXXXXXXXXXXXXXXXXXXXXXX
                    DynamicParameters p = new DynamicParameters();
                    try
                    {
                        if (parameters != null && parameters.Count > 0)
                        {
                            foreach (var param in parameters)
                            {
                                p.Add("@" + param.ParameterName, param.ParameterValue);
                            }
                        }
                    }catch(Exception e)
                    {
                        throw;
                    }
                    p = p.ParameterNames.Count() == 0 ? null : p;
                    try
                    {
                        objRecord = SqlMapper.Query<T>(objConnection, spName, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    }catch(Exception ex)
                    {
                        string str = ex.Message;
                        throw;
                    }
                    finally{
                        objConnection.Close();
                    }
                }
                return objRecord;
            }catch(Exception ex)
            {
                string str = ex.Message;
                throw;
            }
        }


        public  DataTable GetTable(string spName, List<ParameterInfo> parameters)
        {
           
                DataTable table = new DataTable();
                using (var con = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand(spName, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if(parameters != null)
                    {
                        foreach (ParameterInfo param in parameters)
                        {
                            cmd.Parameters.Add(new SqlParameter("@" + param.ParameterName, param.ParameterValue));
                        }
                    }
                

                da.Fill(table); /*XXXXXX*/
                con.Close();
                }
            return table;
            
            
        }

        public  List<T> GetRecords<T>(string spName, List<ParameterInfo> parameters,out int returnVal,bool noReturn)
        {
            try { 
            List<T> recordList = new List<T>();
            using (SqlConnection objConnection = new SqlConnection(_connectionString))
            {
                objConnection.Open();/*XXXX*/
                DynamicParameters p = new DynamicParameters();
                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var param in parameters)
                    {
                        p.Add("@" + param.ParameterName, param.ParameterValue);
                    }
                }
                    if (parameters!=null && parameters.Count > 0 && !noReturn)
                    {
                        p.Add("@totalRows", dbType: DbType.Int32, direction: ParameterDirection.Output);
                        //p.Add("@c", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                    }
                    try { 
                    recordList = SqlMapper.Query<T>(objConnection, spName, p, commandType: CommandType.StoredProcedure).ToList();
                    if (parameters != null && parameters.Count > 0 && !noReturn) 
                    {
                        try
                        {
                            returnVal = p.Get<int>("totalRows");
                        }catch(Exception ex)
                        {
                            string message = ex.Message;
                            returnVal = 0;
                        }
                    }
                    else { returnVal = 0; }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        objConnection.Close();
                    }
            }
            return recordList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public  List<T> GetRecords<T>(string spName, List<ParameterInfo> parameters)
        {
            try
            {
                List<T> recordList = new List<T>();
                using (SqlConnection objConnection = new SqlConnection(_connectionString))
                {
                    objConnection.Open();
                    DynamicParameters p = new DynamicParameters();
                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (var param in parameters)
                        {
                            p.Add("@" + param.ParameterName, param.ParameterValue);
                        }
                    }

                  try { 
                    recordList = SqlMapper.Query<T>(objConnection, spName, p, commandType: CommandType.StoredProcedure).ToList();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        objConnection.Close();
                    }
                }
                return recordList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public  int GetIntRecord<T>(string spName, List<ParameterInfo> parameters)
        {
            try { 
            int intRecord = 0;
            using (SqlConnection objConnection = new SqlConnection(_connectionString))
            {
                objConnection.Open();
               try { 
                DynamicParameters p = new DynamicParameters();
                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var param in parameters)
                    {
                        p.Add("@" + param.ParameterName, param.ParameterValue);
                    }
                }

                using (var reader = SqlMapper.ExecuteReader(objConnection, spName, p, commandType: CommandType.StoredProcedure))
                {
                    if (reader != null && reader.Read())
                    {
                        intRecord = Convert.ToInt32(reader[0].ToString());
                    }
                }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        objConnection.Close();
                    }
                }
            return intRecord;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                return 0;
            }
        }

        public  int ExecuteQuery(string spName, List<ParameterInfo> parameters)
        {
            int index = 0;
            try {
                int success = 0;
                using (SqlConnection objConnection = new SqlConnection(_connectionString))
                {
                    objConnection.Open();
                    DynamicParameters p = new DynamicParameters();
                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (var param in parameters)
                        {
                            p.Add("@" + param.ParameterName, param.ParameterValue);
                            index++;
                        }
                    }
                    try { 
                    success = SqlMapper.Execute(objConnection, spName, p, commandType: CommandType.StoredProcedure);
                    }
                    catch (Exception ex)
                    {
                        index.ToString();
                        throw ex;
                    }
                    finally
                    {
                        objConnection.Close();
                    }
                }
                return System.Math.Abs(success) ;
            }
            catch(Exception ex)
            {
                string str = ex.Message;
                return index;
            }
        }
        public bool ExecuteQueryTransation(SqlCommand sqlCommand)
        {
            try
            {
                using (SqlConnection objConnection = new SqlConnection(_connectionString))
                {
                    objConnection.Open();
                    SqlTransaction transaction = objConnection.BeginTransaction();
                    try
                    {
                        sqlCommand.Connection = objConnection;
                        sqlCommand.Transaction = transaction;
                        sqlCommand.ExecuteNonQuery();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        string str = ex.Message;
                        return false;
                       // throw ex; /*XXXXXX*/
                    }
                    finally
                    {
                        objConnection.Close();
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                return false;
            }
        }
        public  bool ExecuteQueryTransation(List<String> sqlList)
        {
            try
            {
                using (SqlConnection objConnection = new SqlConnection(_connectionString))
                {
                    objConnection.Open();
                    SqlTransaction transaction= objConnection.BeginTransaction();
                    try
                    {
                        int id = 0; //add this
                        foreach (string sql in sqlList)
                        {
                           id =  new SqlCommand(sql, objConnection, transaction).ExecuteNonQuery();
                        }
                        transaction.Commit();
                        if (id > 0) //add this
                            return true;
                        else
                            return false;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        string str = ex.Message;
                        return false; /*XXXXXXX*/
                    }
                    finally
                    {
                        objConnection.Close();
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                return false;
            }
        }
        public  int ExecuteQueryWithIntOutputParam(string spName, List<ParameterInfo> parameters)
        {
            try { 
            int success = 0;
            using (SqlConnection objConnection = new SqlConnection(_connectionString))
            {
                objConnection.Open();
                DynamicParameters p = new DynamicParameters();
                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var param in parameters)
                    {
                        p.Add("@" + param.ParameterName, param.ParameterValue);
                    }
                }
                try { 
                success = SqlMapper.Execute(objConnection, spName, p, commandType: CommandType.StoredProcedure);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        objConnection.Close();
                    }
                }
            return System.Math.Abs(success);
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                return 0;
            }
        }



        public  T GetSqlRecord<T>(string sqlStr, List<ParameterInfo> parameters)
        {
            try
            {
                T objRecord = default(T);
                using (SqlConnection objConnection = new SqlConnection(_connectionString))
                {
                    objConnection.Open();
                    DynamicParameters p = new DynamicParameters();
                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (var param in parameters)
                        {
                            p.Add("@" + param.ParameterName, param.ParameterValue);
                        }
                    }
                    p = p.ParameterNames.Count() == 0 ? null : p;
                    try { 
                    objRecord = SqlMapper.Query<T>(objConnection, sqlStr, p, commandType: CommandType.Text).FirstOrDefault();
                }catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objConnection.Close();
            }
        }
                return objRecord;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public  List<T> GetSqlRecordsByScript<T>(string sqlStr, List<ParameterInfo> parameters)
        {
            try
            {
                List<T> recordList = new List<T>();
                using (SqlConnection objConnection = new SqlConnection(_connectionString))
                {
                    objConnection.Open(); 
                    DynamicParameters p = new DynamicParameters();
                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (var param in parameters)
                        {
                            p.Add("@" + param.ParameterName, param.ParameterValue);
                        }
                    }

                    if (parameters != null && parameters.Count > 0)
                    {
                        p.Add("@totalRows", dbType: DbType.Int32, direction: ParameterDirection.Output);
                        //p.Add("@c", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                    }
                    try
                    {
                        recordList = SqlMapper.Query<T>(objConnection, sqlStr, p, commandType: CommandType.Text).ToList();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        objConnection.Close();
                    }
                }
                return recordList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  int GetSqlIntRecord<T>(string sqlStr, List<ParameterInfo> parameters)
        {
            try
            {
                int intRecord = 0;
               
                using (SqlConnection objConnection = new SqlConnection(_connectionString))
                {
                    objConnection.Open();
                    try { 
                    DynamicParameters p = new DynamicParameters();
                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (var param in parameters)
                        {
                            p.Add("@" + param.ParameterName, param.ParameterValue);
                        }
                    }

                    using (var reader = SqlMapper.ExecuteReader(objConnection, sqlStr, p, commandType: CommandType.Text))
                    {
                        if (reader != null && reader.Read())
                        {
                            intRecord = Convert.ToInt32(reader[0].ToString());
                        }
                    }
                    }
                    catch (Exception ex)
                    {
                        string str = ex.Message;
                        return 0;
                    }
                    finally
                    {
                        objConnection.Close();
                    }
                }
                return intRecord;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                return 0;
            }
        }

        public  int ExecuteSqlQuery(string sqlStr, List<ParameterInfo> parameters)
        {
            try
            {
                int success = 0;
                using (SqlConnection objConnection = new SqlConnection(_connectionString))
                {
                    objConnection.Open();
                    DynamicParameters p = new DynamicParameters();
                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (var param in parameters)
                        {
                            p.Add("@" + param.ParameterName, param.ParameterValue);
                        }
                    }
                    try { 
                    success = SqlMapper.Execute(objConnection, sqlStr, p, commandType: CommandType.Text);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        objConnection.Close();
                    }
                }
                return System.Math.Abs(success);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  int ExecuteSqlQueryWithIntOutputParam(string sqlStr, List<ParameterInfo> parameters)
        {
            try
            {
                int success = 0;
                using (SqlConnection objConnection = new SqlConnection(_connectionString))
                {
                    objConnection.Open();
                    DynamicParameters p = new DynamicParameters();
                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (var param in parameters)
                        {
                            p.Add("@" + param.ParameterName, param.ParameterValue);
                        }
                    }
                    try { 
                    success = SqlMapper.Execute(objConnection, sqlStr, p, commandType: CommandType.Text);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        objConnection.Close();
                    }
                }
                return System.Math.Abs(success);
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                return 0;
            }
        }

    }
}
