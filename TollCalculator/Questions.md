* It was not stated if Tax will be calculated for one day at a time. 
The comment from the original developers seems the first draft was to calculate for one day at a time but the sampel input data provided
in the assignment description includes data over several days.  Dut to this uncertainity, my solution calculates for several days with each request.

* Due to the length of the query POST has been used instead of GET.
* The bonus Scenario specefies, "...same application should be used in other cities with different tax rules".
  Does this mean only hourley rates are different? And/Or,
  Can the Time intervels be different?
  Mamimum amount per day be different?
  Or how about a completely new rule like tax rate differs based on vehicleType i.e. instead of being Exempted certain vehicle pays a reducced rate.

Due to the number of uncertainety related with such a data-model design i didn't attempt the db solution.

* There is no specefic direction for error handelling. So i am skipping that part.
* Used Nager.Date pkg to get the public holidays even though it was not required. So, calculations are not bound to 2013 only.