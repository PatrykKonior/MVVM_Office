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
    
    public partial class Contracts
    {
        public int ContractID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<System.DateTime> ContractDate { get; set; }
        public Nullable<decimal> ContractValueNet { get; set; }
        public Nullable<decimal> VATRate { get; set; }
        public Nullable<decimal> ContractValueGross { get; set; }
        public Nullable<System.DateTime> ClientSignatureDate { get; set; }
        public Nullable<System.DateTime> CompanySignatureDate { get; set; }
    
        public virtual Projects Projects { get; set; }
    }
}
