//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Models.NorthwindIB.EDMX_2012
{
    using System;
    using System.Collections.Generic;
    
    public partial class InternationalOrder
    {
        public int OrderID { get; set; }
        public string CustomsDescription { get; set; }
        public decimal ExciseTax { get; set; }
        public int RowVersion { get; set; }
    
        public virtual Order Order { get; set; }
    }
}
