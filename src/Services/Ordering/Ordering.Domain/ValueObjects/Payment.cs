namespace Ordering.Domain.ValueObjects
{
    public record Payment
    {
        public string? CardName { get; } = default!;
        public string CardNumber { get; } = default!;
        public string Expiration { get; } = default!;
        public string CVV { get; } = default!;
        public int PaymentMethod { get; } = default!;

        protected Payment() { }

        private Payment(string cardName, string cardNumber, string expiration, string cvv, int paymentMethod)
        {
            this.CardName = cardName;
            this.CardNumber = cardNumber;
            this.PaymentMethod = paymentMethod;
            this.Expiration = expiration;
            this.CVV = cvv;
        }

        public static Payment Of(string cardName, string cardNumber, string expiration, string cvv, int paymentMethod)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(cardName);
            ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber);
            ArgumentException.ThrowIfNullOrWhiteSpace(cvv);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 3);
            return new Payment(cardName, cardNumber, expiration, cvv, paymentMethod);
        }
    }
}
