using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsDTO
{
    public class Hashtag
    {

        public int HashTagId { get; set; }

        public string TagName { get; set; }

        public string UserId { get; set; }

        public int Count { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}