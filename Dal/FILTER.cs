//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tol.Dal
{
    using System;
    using System.Collections.Generic;
    
    public partial class FILTER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FILTER()
        {
            this.BUBBLE = new HashSet<BUBBLE>();
            this.FILTER1 = new HashSet<FILTER>();
            this.LINK_ITEM_FILTER = new HashSet<LINK_ITEM_FILTER>();
            this.STAT = new HashSet<STAT>();
        }
    
        public int ID_FILTER { get; set; }
        public Nullable<int> ID_PARENT_FILTER { get; set; }
        public string VALUE { get; set; }
        public string COLOR { get; set; }
        public Nullable<bool> ACTIF { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BUBBLE> BUBBLE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FILTER> FILTER1 { get; set; }
        public virtual FILTER FILTER2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LINK_ITEM_FILTER> LINK_ITEM_FILTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STAT> STAT { get; set; }
    }
}