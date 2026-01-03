namespace RobloxCS.Types;
public static partial class Enums {
    public enum SubscriptionState {
        NeverSubscribed = 0,
        SubscribedWillRenew = 1,
        SubscribedWillNotRenew = 2,
        SubscribedRenewalPaymentPending = 3,
        Expired = 4,
    }
}
