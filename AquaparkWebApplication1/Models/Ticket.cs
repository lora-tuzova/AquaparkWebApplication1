using System;
using System.Collections.Generic;

namespace AquaparkWebApplication1.Models;

public partial class Ticket
{
    public int TicketId { get; set; }

    public byte? LocationHall { get; set; }

    public byte? LocationSlide { get; set; }

    public int LocationId { get; set; }

    public int TicketOwner { get; set; }

    public byte TicketStatus { get; set; }

    public decimal Price { get; set; }

    public virtual Hall Location { get; set; } = null!;

    public virtual Slide LocationNavigation { get; set; } = null!;

    public virtual Visitor TicketOwnerNavigation { get; set; } = null!;
}
