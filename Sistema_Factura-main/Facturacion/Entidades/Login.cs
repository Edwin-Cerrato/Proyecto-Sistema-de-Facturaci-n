namespace Entidades
{
    public class Login
    {
        //Propiedades
        public string CodigoUsuario { get; set; }
        public string Contrasena { get; set; }



        //constructor vacio
        public Login()
        {
        }

        //constructores de propiedades 
        public Login(string codigoUsuario, string contrasena)
        {
            CodigoUsuario = codigoUsuario;
            Contrasena = contrasena;

        }







    }

}
