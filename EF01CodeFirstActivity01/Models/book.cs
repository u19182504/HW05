//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClassActivity.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public book()
        {
            this.borrows = new HashSet<borrow>();
        }
    
        public int bookId { get; set; }
        public string name { get; set; }
        public Nullable<int> pagecount { get; set; }
        public Nullable<int> point { get; set; }
        public Nullable<int> authorId { get; set; }
        public Nullable<int> typeId { get; set; }
    
        public virtual author author { get; set; }
        public virtual type type { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<borrow> borrows { get; set; }
    }
}
