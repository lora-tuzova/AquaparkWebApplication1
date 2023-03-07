using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AquaparkWebApplication1.Models;

[ValidDepth]
public partial class Hall
{
    [Display(Name = "Ід. номер")]
    [Range(200, 256, ErrorMessage = "Діапазон припустимих номерів від 200 до 256")]
    public byte HallId { get; set; }

    [Display(Name = "Макс. глибина басейнів")]
    [Range(0.30, 3.00, ErrorMessage = "Припустима глибина басейнів від 0.3 до 3м")]
    public decimal PoolsMaxDepth { get; set; }

    [Display(Name = "Мін. глибина басейнів")]
    [Range(0.3, 3, ErrorMessage = "Припустима глибина басейнів від 0.3 до 3м")]
    public decimal PoolsMinDepth { get; set; }

    [Display(Name = "Максимальна к-сть відвідувачів")]
    [Range(30, 150, ErrorMessage = "Можливі обмеження від 30 до 150 відвідувачів")]
    public byte HallMaxPeople { get; set; }

    public virtual ICollection<Pool> Pools { get; } = new List<Pool>();

    public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();
}

public class ValidDepthAttribute : ValidationAttribute
{
    public ValidDepthAttribute()
    {
        ErrorMessage = "Мінімальна глибина басейну не може перевищувати максимальну глибину";
    }
    public override bool IsValid(object? value)
    {
        Hall? t = value as Hall;
        return t != null && t.PoolsMinDepth <= t.PoolsMaxDepth;
    }
}