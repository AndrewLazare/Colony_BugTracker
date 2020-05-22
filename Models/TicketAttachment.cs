
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COLONY_THE_BUGTRACKER.Models { 
  public class TicketAttachment
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public string FilePath { get; set; }
    public string Description { get; set; }
    public DateTime UploadDate { get; set; }
    public string UserId { get; set; } //guid = global unique id   tags your comp as part of your id

    public virtual ApplicationUser User { get; set; }





}
}