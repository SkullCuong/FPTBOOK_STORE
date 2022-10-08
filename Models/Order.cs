namespace FPTBOOK_STORE.Models;
using System.ComponentModel.DataAnnotations;

public class Order{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public int Status { get; set; }
    public int UserID { get; set; }
    public User? User { get; set; }
    public ICollection<OrderDetail>? OrderDetails { get; set; }
}