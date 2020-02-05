namespace EventStoreBackup.Infrastructure
{
    using System;
    using System.Linq.Expressions;

    public static class Validate
    {
        public static void NotNull(params Expression<Func<object>>[] expressions)
        {
            foreach (var expression in expressions)
            {
                var property = (MemberExpression) expression.Body;
                var value = expression.Compile()();

                if (value == null)
                    throw new ArgumentNullException(property.Member.Name);
            }
        }

        public static void NotEmpty(params Expression<Func<string>>[] expressions)
        {
            foreach (var expression in expressions)
            {
                var property = (MemberExpression) expression.Body;
                var value = expression.Compile()();

                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(property.Member.Name);
            }
        }

        public static void NotZero(params Expression<Func<uint>>[] expressions)
        {
            foreach (var expression in expressions)
            {
                var property = (MemberExpression) expression.Body;
                var value = expression.Compile()();

                if (value == 0)
                    throw new ArgumentException($"{property.Member.Name} should not be 0");
            }
        }
    }
}
