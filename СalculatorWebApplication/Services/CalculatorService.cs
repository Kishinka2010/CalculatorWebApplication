using System.Data;
using System.Text.RegularExpressions;
using СalculatorWebApplication.Exceptions;
using СalculatorWebApplication.Models;
using СalculatorWebApplication.Services.Calculator;

namespace СalculatorWebApplication.Services
{
    public class CalculatorService : ICalculatorService
    {
        public ResultModelDto CalculateSquareRoot(string value)
        {
            var doubleValue = double.Parse(value);
            var result = Math.Sqrt(doubleValue);
            return new ResultModelDto { Result = result };
        }

        public ResultModelDto CalculateExponentiation(string baseValue, double exponent)
        {
            var doubleValue = double.Parse(baseValue);
            var result = Math.Pow(doubleValue, exponent);
            return new ResultModelDto { Result = result };
        }

        public ResultModelDto CalculateExpression(string expression)
        {
            try
            {
                ProcessNestedExpressions(ref expression);   

                expression = ProcessExponentiationOperation(expression);
                expression = ProcessSqrtOperation(expression);

                var table = new DataTable();
                var result = Convert.ToDouble(table.Compute(expression, ""));

                return new ResultModelDto { Result = result };
            }
            catch (BusinessException ex)
            {
                throw new BusinessException("Calculation error: " + ex.Message);
            }
        }

        private static void ProcessNestedExpressions(ref string expression)
        {
            if (expression.Contains('('))
            {
                var regex = new Regex(@"\(([^()]+)\)");
                var matches = regex.Matches(expression);

                foreach (Match match in matches)
                {
                    var innerExpression = match.Groups[1].Value;
                    innerExpression = ProcessExponentiationOperation(innerExpression);
                    innerExpression = ProcessSqrtOperation(innerExpression);
                    var innerTable = new DataTable();
                    var resultTable = Convert.ToDouble(innerTable.Compute(innerExpression, ""));
                    expression = expression.Replace(match.Value, resultTable.ToString());
                }
            }
        }

            private static string ProcessSqrtOperation(string expression)
        {
            var regex = new Regex(@"(?i)sqrt\{(\d+(\.\d+)?)\}");
            var matches = regex.Matches(expression);

            foreach (Match match in matches)
            {
                var sqrtExpression = match.Groups[1].Value;
                var sqrtResult = Math.Sqrt(double.Parse(sqrtExpression));
                expression = expression.Replace(match.Value, sqrtResult.ToString());
            }

            return expression;
        }

        private static string ProcessExponentiationOperation(string expression)
        {
            // регулярное выражение для поиска выражений вида число^степень
            var regex = new Regex(@"(\d+(\.\d+)?)\s*\^\s*(\d+(\.\d+)?)");
            var matches = regex.Matches(expression);

            if (!matches.Any())
            {        
                expression = ProcessSqrtOperation(expression);

                regex = new Regex(@"(\d+(\.\d+)?)\s*\^\s*(\d+(\.\d+)?)");
                matches = regex.Matches(expression);
            }

            foreach (Match match in matches)
            {
                var baseValue = match.Groups[1].Value;
                var exponent = match.Groups[3].Value;
                var exponentiationResult = Math.Pow(double.Parse(baseValue), double.Parse(exponent));
                expression = expression.Replace(match.Value, exponentiationResult.ToString());
            }

            return expression;
        }

    }
}