namespace СalculatorWebApplication.Helpers
{
    public class ExpressionProcessor
    {
        public static string ProcessExpression(string expression)
        {
            expression = expression.Replace(" ", "");
            expression = expression.Replace("–", "-");
            return expression;
        }
    }
}
