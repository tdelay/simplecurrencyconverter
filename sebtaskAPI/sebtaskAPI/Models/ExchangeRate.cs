using System.ComponentModel.DataAnnotations;

namespace sebtaskAPI.Models
{
  public class ExchangeRate
  {
    [Key]
    public int Id { get;set;}
    public decimal Rate { get;set;}

    public string Currency { get;set;} = string.Empty;
  }
}
