# Expressions for Query and Schema

Under construction...


## NULL Checks

In SQL, `x == NULL`and `x IS NULL` is not the same! `TRUE == NULL` would evalute to `NULL` and not to `FALSE`. 

If you want to do an explicit NULL check, use `.Filter(x => x.Email.IsNull())`.

If you have a query like `x.Email1 == x.Email2` and one of them is NULL, it would evaluate to NULL on the database level.
We don't even have a chance to change that behavior.

Note that you cannot use an expression like `.Filter(x => x.Email == null)`. This would throw you a compile time error. 
There are two reasons why this is not supported: 

- To avoid confusion between `==` and `IS NULL` operations.
- And internally, we can't make the above C# syntax work, or it would be very hard at least (due to CS8625). 

