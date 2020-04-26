using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Memberships.Entities
{
    //specify the table name as Item. 
    //If we don't do this then the table will get created with name Items.
    [Table("Item")]
    public class Item
    {
        //specify the Id as Identity column with sequence starting from 1
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(255)]
        [Required]
        public string Title { get; set; }

        [MaxLength(2048)]
        public string Description { get; set; }

        [MaxLength(1024)]
        public string Url { get; set; }

        [MaxLength(1024)]
        [DisplayName("Image Url")]
        public string ImageUrl { get; set; }

        [AllowHtml]
        public string HTML { get; set; }

        //only getter
        public string HTMLShort => HTML == null || HTML.Length < 50 ? HTML : HTML.Substring(0, 50);

        [DefaultValue(0)]
        [DisplayName("Wait Days")]
        public int WaitDays { get; set; }

        public int ProductId { get; set; }

        public int ItemTypeId { get; set; }

        public int SectionId { get; set; }

        public int PartId { get; set; }

        public bool IsFree { get; set; }

        public ICollection<ItemType> ItemTypes { get; set; }

        [DisplayName("Sections")]
        public ICollection<Section> Sections { get; set; }

        [DisplayName("Parts")]
        public ICollection<Part> Parts { get; set; }
    }
}