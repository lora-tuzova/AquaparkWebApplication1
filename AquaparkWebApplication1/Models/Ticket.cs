using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AquaparkWebApplication1.Models;


[ValidCapacity] 
public partial class Ticket
{
    [Display(Name = "Ід. номер")]
    public int TicketId { get; set; }

    [Display(Name = "Номер - хол")]
    public byte? LocationHall { get; set; } = null;

    [Display(Name = "Номер - гірка")]
    public byte? LocationSlide { get; set; } = null;

    [Display(Name = "Type локації")]
    public string LocationType { get; set; }

    [Display(Name = "Номер власника")]
    public int? TicketOwner { get; set; }

    [Display(Name = "Статус використання")]
    public byte TicketStatus { get; set; }

    [Display(Name = "Вартість")]
    [Range(10,200,ErrorMessage ="Припустима вартість квитків від 10 до 200 грн")]
    public decimal Price { get; set; }

    public virtual Hall Location { get; set; } = null!;

    public virtual Slide LocationNavigation { get; set; } = null!;

    public virtual Visitor TicketOwnerNavigation { get; set; } = null!;
}
public class ValidCapacityAttribute : ValidationAttribute
{
    public ValidCapacityAttribute()
    {
        ErrorMessage = "Оберіть рівно один тип локації";
    }
    public override bool IsValid(object? value)
    {
        Ticket? t = value as Ticket;
        
        return t != null && t.LocationHall != t.LocationSlide;
    }
}
