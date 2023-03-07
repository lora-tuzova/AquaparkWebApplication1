using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AquaparkWebApplication1.Models;

//[ValidDepthInRangeAttribute]
public partial class Pool
{
    [Display(Name = "Ід. номер")]
    public byte PoolId { get; set; }

    [Display(Name = "Глибина")]
    [Range(0.3, 3, ErrorMessage = "Припустима глибина басейнів від 0.3 до 3м")]
    public decimal PoolDepth { get; set; }

    [Display(Name = "Мін. дозволений зріст")]
    [Range(100, 160, ErrorMessage = "Обмеження на зріст від 1 до 1.6м")]
    public byte? PoolMinHeight { get; set; }

    [Display(Name = "Тип води")]
    [Range(0, 1, ErrorMessage = "Тип води приймає лише значення 0 (прісна вода) та 1 (солона вода)")]
    public byte WaterType { get; set; }

    [Display(Name = "Номер холу")]
    public byte? Hall { get; set; }

    public virtual Hall HallNavigation { get; set; } //= null!;
}

/*public class ValidDepthInRangeAttribute : ValidationAttribute
{
    public ValidDepthInRangeAttribute()
    {
        ErrorMessage = "Глибина басейну має знаходитись в межах, визначених його холом";
    }
    public override bool IsValid(object? value)
    {
        Pool? t = value as Pool;
        return t != null && t.HallNavigation.PoolsMaxDepth <= t.PoolDepth && t.HallNavigation.PoolsMinDepth >= t.PoolDepth;
    }
}*/
