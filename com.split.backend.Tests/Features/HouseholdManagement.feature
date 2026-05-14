Feature: Household Management
  As a software engineering student
  I want to create a new household in Budgetly
  So that I can start managing shared expenses with my team

  @household_creation
  Scenario: Create a household successfully
    Given I am a registered user with ID 1
    When I request to create a household named "Apartamento 402" with a limit of 5 members
    Then the system should generate the household aggregate successfully
    And the household name should be "Apartamento 402"
    And I should be assigned as the representative