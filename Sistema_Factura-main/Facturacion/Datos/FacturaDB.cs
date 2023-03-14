using Entidades;
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
                sqlFactura.Append("INSERT INTO factura VALUES (@Fecha,@Identidad,@CodigoUsuario,@ISV,@Descuento,@SubTotal,@Total);");
                //capturar el id que se acaba de generar automaticamente
                sqlFactura.Append("SELECT LAST_INSERT_ID();");

                //guardar el detalle

                StringBuilder sqlDetalle = new StringBuilder();
                sqlDetalle.Append("INSERT INTO detallefactura VALUES (@IdFactura,@CodigoProducto,@Precio,@Cantidad,@Total);");

                //rebaja de existencia
                StringBuilder sqlExistencia = new StringBuilder();
                sqlExistencia.Append("UPDATE producto SET Existencia = Existencia - @Cantidad WHERE Codigo = @Codigo; ");



            }
            catch (Exception)
            {
            }
            return inserto;
        }

    }
}
