using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AquaparkWebApplication1.Models;

public partial class Visitor
{
    [Display(Name = "Ім'я")]
    [StringLength(50, MinimumLength = 8, ErrorMessage = "Довжина імені від 8 до 50 символів")]
    public string Name { get; set; } = null!;

    [Display(Name = "Ід. номер")]
    public int VisitorId { get; set; }

    [Display(Name = "Дата народження")]
    public DateTime BirthDate { get; set; }

    [Display(Name = "Зріст")]
    [Range(100,200, ErrorMessage = "Зріст від 1 до 2м")]
    public byte Height { get; set; }

    [Display(Name = "Вага")]
    [Range(0,256, ErrorMessage = "Вага від 0 до 256кг")]
    public byte Weight { get; set; }

    [Display(Name = "Статус відвідування")]
    [Range(0,1, ErrorMessage = "Статус приймає тільки значення 0 або 1")]
    public byte Status { get; set; }

    public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();
}
