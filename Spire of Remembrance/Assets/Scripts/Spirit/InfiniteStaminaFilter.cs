using System;

public class InfiniteStaminaFilter : StaminaFilter
{
	public int priority
	{
		get { return 0; }
	}

	public void FilterExpense(StaminaExpense expense, Stamina stamina)
	{
		expense.cost = 0;
	}
}
