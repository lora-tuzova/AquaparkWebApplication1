using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AquaparkWebApplication1.Models;

public partial class Slide
{
    [Display(Name = "№")]
    [Range(100,199, ErrorMessage = "Діапазон припустимих номерів: 100-199")]
    public byte SlideId { get; set; }

    [Display(Name = "Мін. зріст, см")]
    [Range(100,200, ErrorMessage = "Припустимий зріст від 1 до 2м")]
    public byte? SlideMinHeight { get; set; }

    [Display(Name = "Макс. зріст, см")]
    [Range(100, 200, ErrorMessage = "Припустимий зріст від 1 до 2м")]
    public byte? SlideMaxHeight { get; set; }

    [Display(Name = "Макс. вага, кг")]
    [Range(1, 256, ErrorMessage = "Вага від 1 до 256кг")]
    public byte? SlideMaxWeight { get; set; }

    [Display(Name = "Макс. к-сть відвідувачів")]
    [Range(3, 10, ErrorMessage = "Одночасна кількість відвідувачів від 3 до 10")]
    public byte SlideMaxPeople { get; set; }

    [Display(Name = "Мін. дозволений вік")]
    [Range(3, 18, ErrorMessage = "Вікові обмеження від 3 до 18 років")]
    public byte? SlideMinAge { get; set; }

    [Display(Name = "Найвища точка, м")]
    [Range(1, 10, ErrorMessage = "Припустимі гірки від 1 до 10м заввишки")]
    public byte SlideHighestPoint { get; set; }

    [Display(Name ="Назва гірки")]
    [StringLength(50, ErrorMessage = "Довжина назви до 50 символів")]
    public string? SlideName { get; set; }
    
    [Display(Name = "Ціна квитка, грн")]
    //[Range(10, 500, ErrorMessage = "Припустима вартість квитків від 10 до 500 грн")]
    public decimal SlidePrice { get; set; }

    public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();
}
