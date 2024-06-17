# Tax Calculator API
## Keywords
1. **Tax**: An amount to be calculated from the gross income and deducted from it to calculate net income.
2. **Deductible**: An amount to be deducted from the gross income before a tax is applied to it. For example: "**Charity Contribution**". Taxes and Deductibles have a many-to-many relationship. 

## Implemented tax jurisdictions
1. **Default**: Taxes in this jurisdiction will apply to any taxpayer with a numeric SSN.
2. **Lithuania**: Taxes in this jurisdiction will apply to any taxpayer with an SSN prefixed with "**LT**".

The API will not calculate taxes for any taxpayer which does not fall into one of the above jurisdictions and will return HTTP 400 (BadRequest) with a helpful error message.

## How to add a new tax calculator:
1. Create an `appsettings` section that corresponds to the `UniqueName` of the new calculator. Set all parameters required for the calculation.
2. Create a new tax calculator that satisfies the ITaxCalculator interface. Ensure that:
    - The `UniqueName` is unique across the application. Look at existing implementations for naming convention (which is important to maintain for configuration reasons);
    - The `TaxFieldName` matches a `decimal` field in the `Core.Dto.Taxes` object. Otherwise, the calculation will not propagate to the API client;
    - Ensure that for this tax jurisdiction, no other calculator has the same value for `TaxFieldName` as the new calculator.
3. Register the new tax calculator as an implementation of `ITaxCalculator` in the DI container.
4. If there are any deductibles that apply to the new tax calculator, (create them if they do not exist and) ensure that this tax calculator's `UniqueName` is included in its `ApplicableTaxes` configuration.

## API testing gotchas
1. If you get a wrong response after changing an amount e.g. `GrossIncome` in the `TaxPayer` payload, the response is likely a cached response. Restart the API to clear the cache or use a different `SSN` (calculated taxes are cached against `SSN`).


## Potential improvements
1. Add authentication and authorization for security.
2. Use distributed caching instead of in-memory caching to allow for scaling.
3. Move calculation parameters to database to allow updates without redeploying the application.
4. Allow the client to trigger recalculation of cached tax.
5. Instantiate TaxCalculators and DeductibleCalculators during start-up to ensure all calculators have been correctly configured.
6. Validate during start-up that no two calculator implementations share the same `UniqueName`.
7. Use a common test class for the different TaxCalculators which share the same base class to reduce test-case duplication.
8. Add Integration tests to cover the Controllers and other parts not unit tested like Configuration.