Feature: User Registration
  As a new visitor of Budgetly
  I want to register a new account
  So that I can start using the platform to split my expenses

  @user_registration
  Scenario: Successful user registration
    Given I provide the name "Carlos Perez" and email "carlos@example.com"
    And I choose a password "Password123!" and the role "Admin"
    When I submit the registration
    Then the user should be created with the email "carlos@example.com"
    And the account should be active by default