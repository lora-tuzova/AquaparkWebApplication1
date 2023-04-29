using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AquaparkWebApplication1.Models;

[ValidDepth]
public partial class Hall
{
    [Display(Name = "№")]
    //[Range(200, 256, ErrorMessage = "Діапазон припустимих номерів від 200 до 256")]
    public byte HallId { get; set; }

    [Display(Name = "Макс. глибина басейнів, м")]
    [Range(0.30, 3.00, ErrorMessage = "Припустима глибина басейнів від 0.3 до 3м")]
    public decimal PoolsMaxDepth { get; set; }

    [Display(Name = "Мін. глибина басейнів, м")]
    [Range(0.3, 3, ErrorMessage = "Припустима глибина басейнів від 0.3 до 3м")]
    public decimal PoolsMinDepth { get; set; }

    [Display(Name = "Макс. к-сть відвідувачів")]
    [Range(30, 150, ErrorMessage = "Можливі обмеження від 30 до 150 відвідувачів")]
    public byte HallMaxPeople { get; set; }

    [Display(Name = "Ціна квитка, грн")]
    //[Range(10,500, ErrorMessage = "Припустима вартість квитків від 10 до 500 грн")]
    public decimal HallPrice { get; set; }

    public virtual ICollection<Pool> Pools { get; } = new List<Pool>();

    public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();
}

public class ValidDepthAttribute : ValidationAttribute
{
    public ValidDepthAttribute()
    {
        ErrorMessage = "Мінімальна глибина басейну не може перевищувати максимальну глибину";
    }
    public override bool IsValid(object value)
    {
        if (value == null) { return true; }
        Hall hall = value as Hall;
        return hall != null && hall.PoolsMinDepth <= hall.PoolsMaxDepth;
    }
}