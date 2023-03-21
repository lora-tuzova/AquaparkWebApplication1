using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AquaparkWebApplication1.Models;


//[ValidLocationType]
public partial class Ticket
{
    [Display(Name = "Ід. номер")]
    public int TicketId { get; set; }

    [Display(Name = "Тип - хол")]
    //[Range(0, 1, ErrorMessage = "Неприпустиме значення")]
    public byte? LocationHall { get; set; }

    [Display(Name = "Тип - гірка")]
    //[Range(0, 1, ErrorMessage = "Неприпустиме значення")]
    public byte? LocationSlide { get; set; }

    [Display(Name = "Type локації")]
    public string LocationType { get; set; }

    [Display(Name = "Номер власника")]
    public int? TicketOwner { get; set; }

    [Display(Name = "Статус використання")]
    public byte TicketStatus { get; set; }

    [Display(Name = "Вартість")]
    public decimal Price { get; set; }

    //[Display(Name = "Номер холу")]
    public virtual Hall Location { get; set; } = null!;

    //[Display(Name = "Номер гірки")]
    public virtual Slide LocationNavigation { get; set; } = null!;

    public virtual Visitor TicketOwnerNavigation { get; set; } = null!;
}
/*public class ValidLocationTypeAttribute : ValidationAttribute
{
    public ValidLocationTypeAttribute()
    {
        ErrorMessage = "Оберіть рівно один тип локації";
    }
    public override bool IsValid(object? value)
    {
        Ticket? t = value as Ticket;
        return t != null && t.LocationHall != t.LocationSlide;
    }
}*/
