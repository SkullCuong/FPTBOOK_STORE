namespace FPTBOOK_STORE.Models;
using System.ComponentModel.DataAnnotations;
public class Category{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Status { get; set; }
    public ICollection<Book>? Books { get; set; }
}