using System;

public class DefaultStaminaFilter : StaminaFilter
{
	public int priority
	{
		get
		{
			return 10;
		}
	}

	public void FilterExpense(StaminaExpense expense, Stamina stamina)
	{
		expense.canAfford = expense.cost <= stamina.currentStamina;
	}
}
