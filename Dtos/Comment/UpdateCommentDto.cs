using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comment
{
    public class UpdateCommentDto
    {
      public int CommentId { get; set; }
      public string Title { get; set; } = string.Empty;
      public string Content { get; set; } = string.Empty;
    }
}
