using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberShop.Models
{
    public class PCViewModel
    {
        public int id { get; set; }
        public int product_id { get; set; }
        public Nullable<int> cpu_id { get; set; }
        public Nullable<int> main_id { get; set; }
        public Nullable<int> ram_id { get; set; }
        public Nullable<int> hdd_id { get; set; }
        public Nullable<int> ssd_id { get; set; }
        public Nullable<int> vga_int { get; set; }
        public Nullable<int> power_id { get; set; }
        public Nullable<int> cooler_id { get; set; }
        public Nullable<int> monitor_id { get; set; }
        public Nullable<int> keyboard_id { get; set; }
        public Nullable<int> mouse_id { get; set; }
        public Nullable<int> headphone_id { get; set; }
        public Nullable<int> speaker_id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string CreateBy { get; set; }
    }
}