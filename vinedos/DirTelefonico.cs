using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vinedos
{
    class DirTelefonico
    {
        private int _id;
        private string _nombre;
        private string _numero;
        private string _direccion;
        private string _ciudad;
        private string _estado;
        private string _email;
        private string _web;

        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        public string Nombre
        {
            set { _nombre = value; }
            get { return _nombre; }
        }
        public string Numero
        {
            set { _numero = value; }
            get { return _numero; }
        }
        public string Direccion
        {
            set { _direccion = value; }
            get { return _direccion; }
        }
        public string Ciudad
        {
            set { _ciudad = value; }
            get { return _ciudad; }
        }
        public string Estado
        {
            set { _estado = value; }
            get { return _estado; }
        }
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        public string Web
        {
            set { _web = value; }
            get { return _web; }
        }

        public DirTelefonico(int id, string nombre, string numero, string direccion, string ciudad, string estado, string email, string web)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.Numero = numero;
            this.Direccion = direccion;
            this.Ciudad = ciudad;
            this.Estado = estado;
            this.Email = email;
            this.Web = web;
        }

    }
}
