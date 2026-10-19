#if MYSQL56
namespace Quib.MySql56;
#elif MYSQL57
namespace Quib.MySql57;
#elif MYSQL80
namespace Quib.MySql80;
#elif MYSQL84
namespace Quib.MySql84;
#elif POSTGRES18
namespace Quib.Postgres18;
#endif


public static partial class ExpressionExtensions
{

    extension(NotNullExpression<DbText> value)
    {

        /// <summary>
        /// Text contains the search term (using a LIKE expression). Define the match type to control case and accent sensitivity.
        /// </summary>
        public NotNullExpression<DbBool> Contains(string term, TextMatch match = TextMatch.ColumnDefault)
        {
            ArgumentNullException.ThrowIfNull(value);

            return new NotNullExpression<DbBool>(Like.Contains(value.SQL, term, match));
        }

        /// <summary>
        /// Text starts with the search term (using a LIKE expression). Define the match type to control case and accent sensitivity.
        /// </summary>
        public NotNullExpression<DbBool> StartsWith(string term, TextMatch match = TextMatch.ColumnDefault)
        {
            ArgumentNullException.ThrowIfNull(value);

            return new NotNullExpression<DbBool>(Like.StartsWith(value.SQL, term, match));
        }

        /// <summary>
        /// Text ends with the search term (using a LIKE expression). Define the match type to control case and accent sensitivity.
        /// </summary>
        public NotNullExpression<DbBool> EndsWith(string term, TextMatch match = TextMatch.ColumnDefault)
        {
            ArgumentNullException.ThrowIfNull(value);

            return new NotNullExpression<DbBool>(Like.EndsWith(value.SQL, term, match));
        }

        /// <summary>
        /// Text equals the other text. Define the match type to control case and accent sensitivity.
        /// </summary>
        public NotNullExpression<DbBool> Equals(string text, TextMatch match)
        {
            // Match is required, as Equals(object) would be called otherwise.
            ArgumentNullException.ThrowIfNull(value);

            return new NotNullExpression<DbBool>(new BinaryOperator(Collate.Apply(value.SQL, match), "=", SQL.Parameter(text).SQL));
        }

        public static NotNullExpression<DbBool> operator ==(NotNullExpression<DbText> left, string right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "=", SQL.Parameter(right).SQL));
        }

        public static NotNullExpression<DbBool> operator !=(NotNullExpression<DbText> left, string right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "!=", SQL.Parameter(right).SQL));
        }

        public static NotNullExpression<DbBool> operator <(NotNullExpression<DbText> left, NotNullExpression<DbText> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "<", right.SQL));
        }

        public static NotNullExpression<DbBool> operator <(NotNullExpression<DbText> left, string right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return left < SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator >(NotNullExpression<DbText> left, NotNullExpression<DbText> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, ">", right.SQL));
        }

        public static NotNullExpression<DbBool> operator >(NotNullExpression<DbText> left, string right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return left > SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator <=(NotNullExpression<DbText> left, NotNullExpression<DbText> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "<=", right.SQL));
        }

        public static NotNullExpression<DbBool> operator <=(NotNullExpression<DbText> left, string right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return left <= SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator >=(NotNullExpression<DbText> left, NotNullExpression<DbText> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, ">=", right.SQL));
        }

        public static NotNullExpression<DbBool> operator >=(NotNullExpression<DbText> left, string right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return left >= SQL.Parameter(right);
        }

    }

    extension(NullableExpression<DbText> value)
    {

        /// <summary>
        /// Text contains the search term (using a LIKE expression). Define the match type to control case and accent sensitivity.
        /// </summary>
        public NullableExpression<DbBool> Contains(string term, TextMatch match = TextMatch.ColumnDefault)
        {
            ArgumentNullException.ThrowIfNull(value);

            return new NullableExpression<DbBool>(Like.Contains(value.SQL, term, match));
        }

        /// <summary>
        /// Text starts with the search term (using a LIKE expression). Define the match type to control case and accent sensitivity.
        /// </summary>
        public NullableExpression<DbBool> StartsWith(string term, TextMatch match = TextMatch.ColumnDefault)
        {
            ArgumentNullException.ThrowIfNull(value);

            return new NullableExpression<DbBool>(Like.StartsWith(value.SQL, term, match));
        }

        /// <summary>
        /// Text ends with the search term (using a LIKE expression). Define the match type to control case and accent sensitivity.
        /// </summary>
        public NullableExpression<DbBool> EndsWith(string term, TextMatch match = TextMatch.ColumnDefault)
        {
            ArgumentNullException.ThrowIfNull(value);

            return new NullableExpression<DbBool>(Like.EndsWith(value.SQL, term, match));
        }

        /// <summary>
        /// Text equals the other text.
        /// </summary>
        public NullableExpression<DbBool> Equals(string text, TextMatch match)
        {
            // Match is required, as Equals(object) would be called otherwise. Define the match type to control case and accent sensitivity.
            ArgumentNullException.ThrowIfNull(value);

            return new NullableExpression<DbBool>(new BinaryOperator(Collate.Apply(value.SQL, match), "=", SQL.Parameter(text).SQL));
        }

        public static NullableExpression<DbBool> operator ==(NullableExpression<DbText> left, string right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "=", SQL.Parameter(right).SQL));
        }

        public static NullableExpression<DbBool> operator !=(NullableExpression<DbText> left, string right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "!=", SQL.Parameter(right).SQL));
        }

        public static NullableExpression<DbBool> operator <(NullableExpression<DbText> left, NullableExpression<DbText> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "<", right.SQL));
        }

        public static NullableExpression<DbBool> operator <(NullableExpression<DbText> left, string right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return left < SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator >(NullableExpression<DbText> left, NullableExpression<DbText> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, ">", right.SQL));
        }

        public static NullableExpression<DbBool> operator >(NullableExpression<DbText> left, string right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return left > SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator <=(NullableExpression<DbText> left, NullableExpression<DbText> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "<=", right.SQL));
        }

        public static NullableExpression<DbBool> operator <=(NullableExpression<DbText> left, string right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return left <= SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator >=(NullableExpression<DbText> left, NullableExpression<DbText> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, ">=", right.SQL));
        }

        public static NullableExpression<DbBool> operator >=(NullableExpression<DbText> left, string right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return left >= SQL.Parameter(right);
        }

    }

}
