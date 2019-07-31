public interface StaminaFilter
{
	int priority { get; }

	void FilterExpense(StaminaExpense expense, Stamina stamina);
}
