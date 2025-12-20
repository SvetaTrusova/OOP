using DeliverySystem.Interfaces;
using DeliverySystem.States;

namespace DeliverySystem.Models
{
    public class Order
    {
        public int Id { get; private set; }
        public Customer Customer { get; private set; }
        public List<OrderItem> Items { get; private set; }
        public DateTime OrderTime { get; private set; }
        public IOrderState State { get; private set; }
        public string DeliveryAddress { get; private set; }
        public bool IsFastDelivery { get; private set; }
        public string SpecialPreferences { get; private set; }

        private IOrderMediator Mediator { get; }

        public Order(int id, Customer customer, List<OrderItem> items, string deliveryAddress, IOrderMediator mediator,
                    bool isFastDelivery = false, string specialPreferences = "")
        {
            Id = id;
            Customer = customer;
            Items = items ?? new List<OrderItem>();
            DeliveryAddress = deliveryAddress;
            IsFastDelivery = isFastDelivery;
            SpecialPreferences = specialPreferences;
            OrderTime = DateTime.Now;
            State = new PendingState();

            Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            Mediator.NotifyOrderCreated(this);
        }

        public void ChangeState(IOrderState state)
        {
            var oldState = State;
            State = state;

            Mediator.NotifyOrderStateChanged(this, oldState, state);
        }

        public void Approve() => State.Approve(this);
        public void Cancel(string reason = "") => State.Cancel(this, reason);
        public void CompletePreparation() => State.CompletePreparation(this);
        public void AssignCourier() => State.AssignCourier(this);
        public void StartDelivery() => State.StartDelivery(this);
        public void CompleteDelivery() => State.CompleteDelivery(this);

        public string GetStatus() => State.GetStatus();

        public decimal CalculateSubtotal()
        {
            return Items.Sum(item => item.GetTotalPrice());
        }
    }
}