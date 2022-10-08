namespace FPTBOOK_STORE.Models;
using System.ComponentModel.DataAnnotations;

public class User{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Gmail { get; set; }
    public string? Password { get; set; }
    [DataType(DataType.Date)]
    public DateTime BirthDate { get; set; }
    public string? Address { get; set; }
    public string? Role { get; set; }
    public string? Phone { get; set; }
    public ICollection<Order>? Orders { get; set; }
}