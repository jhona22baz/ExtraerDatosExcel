//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExtraerDatosExcel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Colaboradores
    {
        public Colaboradores()
        {
            this.Llaves = new HashSet<Llaves>();
        }
    
        public int ID { get; set; }
        public string Nomina { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public Nullable<int> Estado { get; set; }
    
        public virtual ICollection<Llaves> Llaves { get; set; }
    }
}