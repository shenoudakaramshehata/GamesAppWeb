using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Gameapp.Models
{
    public partial class PublicNotification
    {

        public PublicNotification()
        {
            PublicNotificationDevice = new HashSet<PublicNotificationDevice>();
        }

        public int PublicNotificationId { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
        public int? SliderTypeId { get; set; }
        public int? EntityId { get; set; }
        [NotMapped]
        public string EntityNameAr { get; set; }

        [NotMapped]
        public string EntityNameEn { get; set; }
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }
        public virtual SliderType SliderType { get; set; }
        public virtual ICollection<PublicNotificationDevice> PublicNotificationDevice { get; set; }

    }
}
