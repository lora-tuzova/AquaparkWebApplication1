using System;
using System.Collections.Generic;

namespace AquaparkWebApplication1.Models;

public partial class Pool
{
    public int PoolId { get; set; }

    public decimal PoolDepth { get; set; }

    public byte? PoolMinHeight { get; set; }

    public byte WaterType { get; set; }

    public int Hall { get; set; }

    public virtual Hall HallNavigation { get; set; } = null!;
}
