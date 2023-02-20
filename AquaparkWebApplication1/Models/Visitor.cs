using System;
using System.Collections.Generic;

namespace AquaparkWebApplication1.Models;

public partial class Visitor
{
    public string Name { get; set; } = null!;

    public int VisitorId { get; set; }

    public DateTime BirthDate { get; set; }

    public byte Height { get; set; }

    public byte Weight { get; set; }

    public byte Status { get; set; }

    public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();
}
