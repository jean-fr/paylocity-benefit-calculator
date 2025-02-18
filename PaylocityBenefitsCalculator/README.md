## How to run the api and test
# Via Swagger UI
- In Visual Studio 2nd top bar, select the Api project and click the green play button
- VS will build and open the browser with Swagger UI, ready to be used
- From Swagger, all the enpoints can be called too see data
# Via Console
- Open a Console and navigate to the project directory `...\PaylocityBenefitsCalculator\Api\`
- run the command `dotnet build`
- Open another tab in the Console window and navigate to `...\PaylocityBenefitsCalculator\Api\bin\Debug\net6.0\`
- run the command `dotnet Api.dll`
    - At this stage client tools like Postman (make sure to disable `Enable SSL certificate verification` in the `settings`) could be used to call each endpoint as GET using the following (replace `{id}` by the suitable integer value):
        - `https://localhost:5001/api/v1/dependents`
        - `https://localhost:5001/api/v1/dependents/{id}`
        - `https://localhost:5001/api/v1/employees`
        - `https://localhost:5001/api/v1/employees/{id}`
        - `https://localhost:5001/api/v1/employees/{id}/paycheck`

- Open another tab in the Console window and navigate to `...\PaylocityBenefitsCalculator\ApiTests\`
- run the command `dotnet test` or alternatively run the test via Visual Studio 