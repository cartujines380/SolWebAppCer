using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Configuration;

namespace PP.AccesoDatos
{
    public class OperadorBaseDatos
    {
        private SqlConnection _SqlConn = null;
        public string _CadenaConexion = "";

        private SqlCommand _SqlComm = null;

        private Dictionary<string, string> _Salidas;
        public OperadorBaseDatos(string PI_CadenaConexion)
        {
            _CadenaConexion = PI_CadenaConexion;
            this._SqlConn = new SqlConnection(_CadenaConexion);
            this._SqlComm = new SqlCommand
            {
                Connection = this._SqlConn,
                CommandType = CommandType.StoredProcedure
            };
        }

        public string ProcedimientoAlmacenado { get; set; }

        public void AgregarParametro(string Nombre, SqlDbType Tipo, object Valor)
        {
            SqlParameter Param = new SqlParameter
            {
                ParameterName = Nombre,
                SqlDbType = Tipo,
                Value = Valor
            };
            this._SqlComm.Parameters.Add(Param);
        }

        public void AgregarParametro(string Nombre, SqlDbType Tipo, object Valor, int Tamano)
        {
            SqlParameter Param = new SqlParameter
            {
                ParameterName = Nombre,
                SqlDbType = Tipo,
                Value = Valor,
                Size = Tamano
            };
            this._SqlComm.Parameters.Add(Param);
        }

        public void AgregarParametroDeSalida(string Nombre, SqlDbType Tipo)
        {
            SqlParameter Param = new SqlParameter
            {
                ParameterName = Nombre,
                SqlDbType = Tipo,
                Direction = ParameterDirection.Output,
                Size = 100
            };
            this._SqlComm.Parameters.Add(Param);
        }

        private void LLenarParametrosDeSalida()
        {
            this._Salidas = new Dictionary<string, string>();
            if (this._SqlComm.Parameters != null)
            {
                foreach (SqlParameter p in this._SqlComm.Parameters)
                {
                    if (p.Direction == ParameterDirection.Output | p.Direction == ParameterDirection.InputOutput)
                    {
                        this._Salidas.Add(p.ParameterName, p.Value.ToString());
                    }
                }
            }
        }

        public int Ejecutar()
        {
            int Resultado = 0;
            try
            {
                this._SqlConn.Open();
                this._SqlComm.CommandText = this.ProcedimientoAlmacenado;
                Resultado = this._SqlComm.ExecuteNonQuery();
                this.LLenarParametrosDeSalida();
                this._SqlConn.Close();
            }
            catch (Exception ex)
            {
                Resultado = -1;
                throw new Exception(ex.Message);
            }
            finally
            {
                try
                {
                    this._SqlConn.Close();
                }
                catch { }
            }
            return Resultado;
        }

        public DataSet ConsultarDataSet()
        {
            try
            {
                this._SqlConn.Open();
                this._SqlComm.CommandText = this.ProcedimientoAlmacenado;
                this._SqlComm.CommandTimeout = 0;
                DataSet Resultado = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(this._SqlComm);
                da.Fill(Resultado);
                this.LLenarParametrosDeSalida();
                this._SqlConn.Close();
                return Resultado;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                try
                {
                    this._SqlConn.Close();
                }
                catch { }
            }
        }

        public DataTable ConsultarDataTable()
        {
            try
            {
                DataTable dt = this.ConsultarDataSet().Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                try
                {
                    this._SqlConn.Close();
                }
                catch { }
            }
        }

        public string LeerParametroDeSalida(string Nombre)
        {
            if (this._Salidas == null)
            {
                return string.Empty;
            }
            if (this._Salidas.ContainsKey(Nombre))
            {
                return this._Salidas[Nombre];
            }
            return string.Empty;
        }
    }
}
