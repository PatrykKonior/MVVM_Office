//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVVMFirma.Models.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProjectMaterials
    {
        public int ProjectMaterialID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<int> MaterialID { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> VATAmount { get; set; }
    
        public virtual Materials Materials { get; set; }
        public virtual Projects Projects { get; set; }
    }
}
