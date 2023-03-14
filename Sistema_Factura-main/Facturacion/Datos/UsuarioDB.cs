using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;

namespace Datos
{
    public class UsuarioDB
    {
        //Creación de CADENA DE CONEXIÓN--
        string cadena = "server=localhost; user=root; database=factura2; password=ranger2718 ";

        //Métodos para poder interactuar con la tabla usuario de la BD/Referenciamos con Entidades
        public Usuario Autenticar(Login login)
        {
            Usuario user = null;
            //Sentencia para capturar errores y evitar que se cierre la apk 
            try
            {
                //sentencias sql,(clase stringBuilder )podemos agregar una sentencia sql en varias lineas
                StringBuilder sql = new StringBuilder();
                //devuelve todos los usuarios registrados en esa tabla /where -especificamente 
                sql.Append("SELECT * FROM usuario WHERE CodigoUsuario= @CodigoUsuario AND Contrasena = @Contrasena; ");


                //using sirve para abrir y automaticamente despues de usar la BD se cerrara 
                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    //abrir concexion
                    _conexion.Open();

                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {
                        //el using .data lo cortamos y pegamos arriba
                        comando.CommandType = CommandType.Text;
                        //pasamos parametros que asignamos
                        comando.Parameters.Add("@CodigoUsuario", MySqlDbType.VarChar, 50).Value = login.CodigoUsuario;
                        comando.Parameters.Add("@Contrasena", MySqlDbType.VarChar, 50).Value = login.Contrasena;

                        //ejecutar mi sentencia

                        MySqlDataReader dr = comando.ExecuteReader();
                        if (dr.Read())
                        {
                            user = new Usuario();

                            //propiedad de usuario/ atributo de BD

                            user.CodigoUsuario = dr["CodigoUsuario"].ToString();
                            user.Nombre = dr["Nombre"].ToString();
                            user.Contrasena = dr["Contrasena"].ToString();
                            user.Correo = dr["Correo"].ToString();
                            user.Rol = dr["Rol"].ToString();
                            user.FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]);
                            user.EstaActivo = Convert.ToBoolean(dr["EstaActivo"]);
                            //casteo de tipo de dato
                            if (dr["Foto"].GetType() != typeof(DBNull))
                            {
                                user.Foto = (byte[])dr["Foto"];
                            }


                        }
                    }

                }

            }
            catch (System.Exception ex)
            {

            }
            return user;

        }


        //Metodo que permite insertar nuevo usuario
        public bool Insertar(Usuario user)
        {
            bool inserto = false;
            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO usuario VALUES ");
                sql.Append("(@CodigoUsuario,@Nombre,@Contrasena,@Correo,@Rol,@Foto,@FechaCreacion,@EstaActivo); ");


                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    //abrir conexion
                    _conexion.Open();

                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {

                        comando.CommandType = CommandType.Text;

                        comando.Parameters.Add("@CodigoUsuario", MySqlDbType.VarChar, 50).Value = user.CodigoUsuario;
                        comando.Parameters.Add("@Nombre", MySqlDbType.VarChar, 50).Value = user.Nombre;
                        comando.Parameters.Add("@Contrasena", MySqlDbType.VarChar, 80).Value = user.Contrasena;
                        comando.Parameters.Add("@Correo", MySqlDbType.VarChar, 45).Value = user.Correo;
                        comando.Parameters.Add("@Rol", MySqlDbType.VarChar, 20).Value = user.Rol;
                        comando.Parameters.Add("@Foto", MySqlDbType.LongBlob).Value = user.Foto;
                        comando.Parameters.Add("@FechaCreacion", MySqlDbType.DateTime).Value = user.FechaCreacion;
                        comando.Parameters.Add("@EstaActivo", MySqlDbType.Bit).Value = user.EstaActivo;
                        comando.ExecuteNonQuery();

                        inserto = true;


                    }
                }
            }


            catch (System.Exception ex)
            {
            }

            return inserto;

        }

        public bool Editar(Usuario user)
        {
            bool edito = false;
            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("UPDATE usuario SET ");
                sql.Append(" Nombre = @Nombre, Contrasena = @Contrasena, Correo = @Correo, Rol = @Rol, Foto = @Foto, EstaActivo = @EstaActivo ");
                //Permitir el cambio si,el codigo de usuario=
                sql.Append("WHERE CodigoUsuario = @CodigoUsuario;");

                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    //abrir conexion
                    _conexion.Open();

                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {

                        comando.CommandType = CommandType.Text;

                        comando.Parameters.Add("@CodigoUsuario", MySqlDbType.VarChar, 50).Value = user.CodigoUsuario;
                        comando.Parameters.Add("@Nombre", MySqlDbType.VarChar, 50).Value = user.Nombre;
                        comando.Parameters.Add("@Contrasena", MySqlDbType.VarChar, 80).Value = user.Contrasena;
                        comando.Parameters.Add("@Correo", MySqlDbType.VarChar, 45).Value = user.Correo;
                        comando.Parameters.Add("@Rol", MySqlDbType.VarChar, 20).Value = user.Rol;
                        comando.Parameters.Add("@Foto", MySqlDbType.LongBlob).Value = user.Foto;
                        comando.Parameters.Add("@EstaActivo", MySqlDbType.Bit).Value = user.EstaActivo;
                        comando.ExecuteNonQuery();

                        edito = true;


                    }
                }
            }


            catch (System.Exception ex)
            {
            }

            return edito;





            //fin
        }

        public bool Eliminar(string CodigoUsuario)
        {
            bool elimino = false;
            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("DELETE FROM usuario ");
                sql.Append("WHERE CodigoUsuario = @CodigoUsuario;");

                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    //abrir conexion
                    _conexion.Open();

                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {

                        comando.CommandType = CommandType.Text;

                        comando.Parameters.Add("@CodigoUsuario", MySqlDbType.VarChar, 50).Value = CodigoUsuario;
                        comando.ExecuteNonQuery();

                        elimino = true;


                    }
                }
            }


            catch (System.Exception ex)
            {
            }

            return elimino;

        }

        //traer lista de Usuarios(cantidad pequeña)
        public DataTable DevolverUsuarios()
        {
            DataTable dt = new DataTable();

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("SELECT * FROM usuario ;");

                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    //abrir conexion
                    _conexion.Open();

                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {

                        comando.CommandType = CommandType.Text;
                        MySqlDataReader dr = comando.ExecuteReader();
                        dt.Load(dr);

                    }
                }
            }


            catch (System.Exception ex)
            {
            }

            return dt;
        }

        //devolver foto
        public byte[] DevolverFoto(string CodigoUsuario)
        {
            byte[] foto = new byte[0];
            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("SELECT Foto FROM usuario WHERE CodigoUsuario= @CodigoUsuario; ");

                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    //abrir conexion
                    _conexion.Open();

                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {

                        comando.CommandType = CommandType.Text;
                        comando.Parameters.Add("@CodigoUsuario", MySqlDbType.VarChar, 50).Value = CodigoUsuario;
                        MySqlDataReader dr = comando.ExecuteReader();
                        if (dr.Read())
                        {
                            foto = (byte[])dr["Foto"];
                        }

                    }
                }
            }


            catch (System.Exception ex)
            {
            }
            return foto;
        }


    }
}

