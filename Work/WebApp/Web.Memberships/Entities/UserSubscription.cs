using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Memberships.Entities
{
    //specify the table name as UserSubscription. 
    //If we don't do this then the table will get created with name UserSubscription.
    public class UserSubscription
    {
        //we have a composite primary key
        [Required]
        [Key, Column(Order = 1)]
        public int SubscriptionId { get; set; }

        [Required]
        [Key, Column(Order = 2)]
        [MaxLength(128)]
        public string UserId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}