using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Memberships.Entities
{
    //specify the table name as ProductLinkText. 
    //If we don't do this then the table will get created with name ProductLinkTexts.
    [Table("ProductLinkText")]
    public class ProductLinkText
    {
        //specify the Id as Identity column with sequence starting from 1
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(25)]
        [Required]
        public string Title { get; set; }
    }
}