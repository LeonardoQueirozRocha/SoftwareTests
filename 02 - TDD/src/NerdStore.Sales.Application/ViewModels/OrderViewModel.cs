namespace NerdStore.Sales.Application.ViewModels;

public class OrderViewModel
{
    public Guid Id { get; set; }
    public int Code { get; set; }
    public decimal TotalValue { get; set; }
    public DateTime RegistrationDate { get; set; }
    public int OrderStatus { get; set; }
}