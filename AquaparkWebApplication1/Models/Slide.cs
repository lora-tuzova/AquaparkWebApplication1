using System;
using System.Collections.Generic;

namespace AquaparkWebApplication1.Models;

public partial class Slide
{
    public int SlideId { get; set; }

    public byte? SlideMinHeight { get; set; }

    public byte? SlideMaxHeight { get; set; }

    public byte? SlideMaxWeight { get; set; }

    public byte SlideMaxPeople { get; set; }

    public byte? SlideMinAge { get; set; }

    public byte SlideHighestPoint { get; set; }

    public string? SlideName { get; set; }

    public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();
}
