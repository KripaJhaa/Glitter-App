using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsDTO
{
    public class Like
    {
        public int LikeId { get; set; }

        public int TweetId { get; set; }

        public string UserId { get; set; }

        public bool IsLiked { get; set; } = true;

        [ForeignKey("TweetId")]
        public virtual Tweet Tweet { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}