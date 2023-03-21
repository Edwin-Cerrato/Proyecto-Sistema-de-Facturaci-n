using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class FacturaDB
    {
        //cadena de Conexion
        string cadena = "server=localhost; user=root; database=factura2; password=ranger2718 ";



        public bool Guardar(Factura factura, List<DetalleFactura> detalles)
        {
            bool inserto = false;
            //var que captura el id de la factura y la guarda
            int idfactura = 0;
            try
            {
                //stringBuilder evita las sentencias largas
                StringBuilder sqlFactura = new StringBuilder();
                sqlFactura.Append("INSERT INTO factura VALUES (@Fecha,@IdentidadCliente,@CodigoUsuario,@ISV,@Descuento,@SubTotal,@Total);");
                //capturar el id que se acaba de generar automaticamente
                sqlFactura.Append("SELECT LAST_INSERT_ID();");

                //guardar el detalle

                StringBuilder sqlDetalle = new StringBuilder();
                sqlDetalle.Append("INSERT INTO detallefactura VALUES (@IdFactura,@CodigoProducto,@Precio,@Cantidad,@Total);");

                //rebaja de existencia
                StringBuilder sqlExistencia = new StringBuilder();
                sqlExistencia.Append("UPDATE producto SET Existencia = Existencia - @Cantidad WHERE Codigo = @Codigo; ");


                //abrimos la conexion
                using (MySqlConnection con = new MySqlConnection(cadena))
                {
                    con.Open();

                    //Se ejecuta transacciones

                    //readcomitted bloquea la tabla pero si deja la lectura disponible
                    MySqlTransaction transaction = con.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

                    try
                    {
                        using (MySqlCommand cmd1 = new MySqlCommand(sqlFactura.ToString(), con, transaction))
                        {
                            cmd1.CommandType = System.Data.CommandType.Text;
                            //pasamos parametros de factura
                            cmd1.Parameters.Add("@Fecha", MySqlDbType.DateTime).Value = factura.Fecha;
                            cmd1.Parameters.Add("@IdentidadCliente", MySqlDbType.VarChar, 25).Value = factura.IdentidadCliente;
                            cmd1.Parameters.Add("@CodigoUsuario", MySqlDbType.VarChar, 50).Value = factura.CodigoUsuario;
                            cmd1.Parameters.Add("@ISV", MySqlDbType.Decimal).Value = factura.ISV;
                            cmd1.Parameters.Add("@Descuento", MySqlDbType.Decimal).Value = factura.Descuento;
                            cmd1.Parameters.Add("@SubTotal", MySqlDbType.Decimal).Value = factura.SubTotal;
                            cmd1.Parameters.Add("@Total", MySqlDbType.Decimal).Value = factura.Total;

                            //capturamos el id de factura
                            cmd1.ExecuteScalar();
                            idfactura = Convert.ToInt32(cmd1.ExecuteScalar());

                        }

                        //Uso de for each
                        foreach (DetalleFactura detalle in detalles)
                        {
                            using (MySqlCommand cmd2 = new MySqlCommand(sqlDetalle.ToString(), con, transaction))
                            {
                                cmd2.CommandType = System.Data.CommandType.Text;
                                //pasamos parametros de factura
                                cmd2.Parameters.Add("@IdFactura", MySqlDbType.Int32).Value = idfactura;
                                cmd2.Parameters.Add("@CodigoFactura", MySqlDbType.VarChar, 50).Value = detalle.CodigoProducto;
                                cmd2.Parameters.Add("@Precio", MySqlDbType.Decimal).Value = detalle.Precio;
                                cmd2.Parameters.Add("@Cantidad", MySqlDbType.Decimal).Value = detalle.Cantidad;
                                cmd2.Parameters.Add("@Total", MySqlDbType.Decimal).Value = detalle.Total;
                                cmd2.ExecuteNonQuery();
                            }



                            //Actualizar existencia producto

                            using (MySqlCommand cmd3 = new MySqlCommand(sqlExistencia.ToString(), con, transaction))
                            {
                                cmd3.CommandType = System.Data.CommandType.Text;
                                //pasamos parametros de factura
                                cmd3.Parameters.Add("@Cantidad", MySqlDbType.Decimal).Value = detalle.Cantidad;
                                cmd3.Parameters.Add("@Codigo", MySqlDbType.VarChar, 80).Value = detalle.CodigoProducto;
                                cmd3.ExecuteNonQuery();
                            }

                        }

                        transaction.Commit();
                        inserto = true;



                    }
                    catch (Exception)
                    {
                        inserto = false;
                        transaction.Rollback();
                    }
                }

            }
            catch (Exception)
            {
            }
            return inserto;
        }

    }
}
