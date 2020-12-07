# Cobbler API

## api/budgetplan

  ### GET api/budgetplan

  Returns all budget plan, their allocations, and their histories.

  ### GET api/budgetplan/1 

  Returns a single plan with no additional information

  ### POST api/budgetplan

    {
       "planName": "Cobbler 2",
       "budgetAmount": 15000.00
    }

  ### PUT api/budgetplan/1

    {
        "id": 3,
        "planName": "Cobbler 2",
        "budgetAmount": 1000.0
    }

  ### DELETE api/budgetplan/1


## api/allocation

  ### GET api/allocation

  Returns all allocations 

  ### GET api/allocation?orderBy=Person or api/allocation?orderBy=Project

  Returns allocations by Person or Project name.

  ### GET api/allocation/1 

  Returns a single allocation

  ### POST api/allocation

    {
        "PersonName": "Any Person",
        "ProjectName": "SEO",
        "AllocatedAmount": 2000.0,
        "BudgetPlanId": 1
    }
  
  ### PUT api/allocation/1

    {
        "allocationId": 5,
        "PersonName": "Any Person",
        "ProjectName": "SEO",
        "AllocatedAmount": 2000.0,
        "BudgetPlanId": 1,
        "budgetHistoryId": 1
    }
  
  ### DELETE api/allocation/1
