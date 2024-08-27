using API.Migrations;
using API.Models;
using Stripe;
using PaymentIntent = Stripe.PaymentIntent;

namespace API.Services;

public class PaymentService
{
    private readonly IConfiguration _config;
    public PaymentService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<PaymentIntent> CreateOrUpdatePyamentIntent(Cart cart)
    {
        StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];
        var service = new PaymentIntentService();
        var intent = new PaymentIntent();
        var subtotal = cart.Items.Sum(item => item.Quantity * item.Product.Price);
        var deliveryFee = subtotal > 10000 ? 0 : 500;

        if (string.IsNullOrEmpty(cart.PaymentIntentId))
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = subtotal + deliveryFee,
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" }
            };
            intent = await service.CreateAsync(options);
        }
        else
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = subtotal + deliveryFee
            };
            await service.UpdateAsync(cart.PaymentIntentId, options);
        }

        return intent;
    }
}