using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Memberships.Entities
{
    //specify the table name as Part. 
    //If we don't do this then the table will get created with name Parts.
    [Table("SubscriptionProduct")]
    public class SubscriptionProduct
    {
        //we have a composite primary key
        [Required]
        [Key, Column(Order = 1)]
        public int ProductId { get; set; }

        [Required]
        [Key, Column(Order = 2)]
        public int SubscriptionId { get; set; }

        [NotMapped]
        public int OldProductId { get; set; }

        [NotMapped]
        public int OldSubscriptionId { get; set; }

    }
}