using System;
using System.Collections.Generic;

namespace AquaparkWebApplication1.Models;

public partial class Hall
{
    public int HallId { get; set; }

    public decimal PoolsMaxDepth { get; set; }

    public decimal PoolsMinDepth { get; set; }

    public byte HallMaxPeople { get; set; }

    public virtual ICollection<Pool> Pools { get; } = new List<Pool>();

    public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();
}
