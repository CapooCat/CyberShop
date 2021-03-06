//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Invoice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Invoice()
        {
            this.Invoice_Detail = new HashSet<Invoice_Detail>();
        }
    
        public int Id { get; set; }
        public Nullable<int> User_id { get; set; }
        public Nullable<System.DateTime> PurchaseDate { get; set; }
        public string CustomerName { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryPhoneNum { get; set; }
        public string Status { get; set; }
        public Nullable<double> Total { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice_Detail> Invoice_Detail { get; set; }
        public virtual User User { get; set; }
    }
}
