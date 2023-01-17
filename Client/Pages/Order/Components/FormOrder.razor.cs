using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using TakraonlineCRM.Shared.Orders;

namespace TakraonlineCRM.Client.Pages.Order.Components
{
    public partial class FormOrder
    {
        [Parameter] public TakraonlineCRM.Shared.Orders.Order order { get; set; }
        [Parameter] public string ButtonText { get; set; } = "Save";
        [Parameter] public EventCallback OnValidSubmit { get; set; }

        private double Subtotal = 0;
        private double Sum = 0;
        private double Vat = 0;

        private void SetOrderPrice()
        {
            Sum = order.Financial.Price - order.Financial.Discount;
            Vat = (order.Financial.Vat / 100) * Sum;
            Subtotal = Sum + Vat;

            order.Financial.SubTotal = Subtotal;
        }

        protected async override Task OnParametersSetAsync()
        {
            Subtotal = order.Financial.SubTotal;
            await Task.CompletedTask;
        }

        protected void AssignImageUrl( string imgUrl )
        {
            order.TransferReceipt = imgUrl;
        }
        protected void PriceChange( ChangeEventArgs args )
        {
            var value = args.Value.ToString();
            order.Financial.Price = Convert.ToDouble( value );
            SetOrderPrice();
        }
        protected void DiscountChange( ChangeEventArgs args )
        {
            var value = args.Value.ToString();
            order.Financial.Discount = Convert.ToDouble( value );
            SetOrderPrice();
        }
        protected void VatChange( ChangeEventArgs args )
        {
            var value = args.Value.ToString();
            order.Financial.Vat = Convert.ToDouble( value );
            SetOrderPrice();
        }
    }
}
