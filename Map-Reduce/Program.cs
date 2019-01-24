// Import the right namespaces for access to classes and functions
using System;
using System.Collections.Generic;

// The namespace for our program: MapReduce
namespace MapReduce {
    /// <summary>
    /// Class for extension methods
    /// This class will house the methods that are an extension on other classes
    /// An extension method can be recognised by "this" in front of the first parameter
    /// </summary>
    public static class Extensions {
        /// <summary>
        /// We create an extension method called PrintList
        /// This method extends the IEnumerable interface
        /// During execution, this method print the index with their corresponding elements to the console
        /// </summary>
        /// <param name="list">Contains an IEnumerable of which the items shall be printed</param>
        /// <typeparam name="T">The generic type the items in our list</typeparam>
        public static void PrintList<T>(this IEnumerable<T> list) {
            // Print a line
            System.Console.WriteLine( "----------" );

            // Create a counter to keep track of the current index
            int counter = 0;

            // Loop over all the items in the list
            foreach ( T item in list ) {
                // Print the index of the current item with the corresponding value
                System.Console.WriteLine( counter.ToString() + " => " + item.ToString() );

                // Increase the counter by one in order to keep track of the index
                counter = counter + 1;
            }

            // Print a line
            System.Console.WriteLine( "----------" );
        }

        /// <summary>
        /// We create an extension method called Map
        /// This method extends the IEnumerable interface
        /// Map loops over all the items in an IEnumerable and perfoms an action on them
        /// The result of the action is stored in a new list, which is later returned
        /// </summary>
        /// <returns>A List containing the modified items</returns>
        /// <param name="list">The list of which the items that shall be modified</param>
        /// <param name="mapper">A function that modifies the items in the list</param>
        /// <typeparam name="T">The type of the orginal list</typeparam>
        /// <typeparam name="U">The type of the new list</typeparam>
        public static IEnumerable<U> Map<T, U>( this IEnumerable<T> list, Func<T, U> mapper ) {
            // Create a list that will store the modified items
            List<U> result = new List<U>();

            // Loop over the items in the original list
            foreach ( T item in list ) {
                // Add the item to the result list after it has been modified by the function
                result.Add( mapper( item ) );
            }

            // Return the result list
            return result;
        }

        /// <summary>
        /// We create an extension method called Reduce (Also known als Fold)
        /// This method extends the IEnumerable interface
        /// Reduce goes over all the items and combines them into a single value
        /// This can be usefull in order to e.g. the sum of all the numbers in a list
        /// </summary>
        /// <returns>The result of reducing all values using a specific function</returns>
        /// <param name="list">The list of which the items that shall be reduced</param>
        /// <param name="init">The starting point of our reduction, 0 for a sum function</param>
        /// <param name="updater">
        /// A function that updates the result with an item from the list, 
        /// takes the current result and current item
        /// </param>
        /// <typeparam name="T">The type of the items in the orginal list</typeparam>
        /// <typeparam name="U">The type of the reduced element</typeparam>
        public static U Reduce<T, U> ( this IEnumerable<T> list, U init, Func<U, T, U> updater ) {
            // We create a variable called result
            // This variable will be used to accumulate the result
            U result = init;

            // Loop over the items of the list
            foreach ( T item in list ) {
                // Add the current item to the accumulator using the updater function
                result = updater( result, item );
            }

            // Return the result of the reduction
            return result;
        }

        /// <summary>
        /// We create an extension method call Where (Also known as Filter)
        /// This method extends the IEnumerable interface
        /// We check all the items against a condition, and return a list of the items that meet the given condition
        /// </summary>
        /// <returns>Returns a list of items that meet the condition</returns>
        /// <param name="list">The list of which the items that shall be filtered</param>
        /// <param name="condition">A function that check an item against a condition</param>
        /// <typeparam name="T">The type of the items in the list</typeparam>
        public static IEnumerable<T> Where<T>( this IEnumerable<T> list, Func<T, bool> condition ) {
            // Create a list that will store the items that meet the condition
            List<T> result = new List<T>();

            // Loop over all the items in the list
            foreach ( T item in list ) {
                // Check whether the current item meets the condition
                if ( condition( item ) ) {
                    // If it does, add it to the list
                    result.Add( item );    
                }
            }

            // Return the list with the items that meet the condition
            return result;
        }

        /// <summary>
        /// We create the extension method called SimpleJoin
        /// This method extends the IEnumerable interface
        /// This version of join uses foreach loops instead of reduce
        /// Join combines two tables on a certain condition
        /// </summary>
        /// <returns>The joined table</returns>
        /// <param name="leftTable">The left table (Joining from)</param>
        /// <param name="rightTable">The right table (Joining with)</param>
        /// <param name="condition">A function with a condition that has to be met</param>
        /// <typeparam name="T">The type of the items in the left table</typeparam>
        /// <typeparam name="U">The type of the items in the right table</typeparam>
        public static IEnumerable<Tuple<T, U>> SimpleJoin<T, U>(
            this IEnumerable<T> leftTable,
            IEnumerable<U> rightTable,
            Func<T, U, bool> condition ) {

            // Create a list that will store the combinations that meet the condition
            List<Tuple<T, U>> combinations = new List<Tuple<T, U>>();

            // Loop over the items in the left table
            foreach( T leftItem in leftTable ) {

                // Loop over the items in the right table
                foreach( U rightItem in rightTable ) {
                    Console.WriteLine(leftItem + " + " + rightItem);

                    // Check whether the combination of items meets the condition
                    if ( condition( leftItem, rightItem ) ) {
                        Console.WriteLine("True");

                        // If so, add it to the list of conditions
                        combinations.Add( new Tuple<T, U>( leftItem, rightItem ) );
                    }
                }
            }

            return combinations;
        }

        /// <summary>
        /// We create the extension method called Join
        /// This method extends the IEnumerable interface
        /// Join combines two tables on a certain condition
        /// </summary>
        /// <returns>The joined table</returns>
        /// <param name="leftTable">The left table (Joining from)</param>
        /// <param name="rightTable">The right table (Joining with)</param>
        /// <param name="condition">A function with a condition that has to be met</param>
        /// <typeparam name="T">The type of the items in the left table</typeparam>
        /// <typeparam name="U">The type of the items in the right table</typeparam>
        public static IEnumerable<Tuple<T, U>> Join<T, U> (
            this IEnumerable<T> leftTable,
            IEnumerable<U> rightTable,
            Func<T, U, bool> condition ) {

            // Start the join by reducing the left table
            return leftTable.Reduce<T, List<Tuple<T, U>>> (
                // We start bij creating a new list that will store the tuples that meet the requirements
                // This is the init parameter of reduce
                new List<Tuple<T, U>>(),

                // We give a function that combines the item of the left with the item on the right
                // This is the updater parameter of reduce
                ( joinedTable, leftRow ) => {
                    // We create a list that stores the combinations
                    List<Tuple<T, U>> leftRowCombinations =
                        // We call the reduce function on the right side in order to combine the elements into a tuple
                        rightTable.Reduce<U, List<Tuple<T, U>>>(
                            // We start again with a new list (the actual combined elements that meet the condition)
                            // This is the init parameter of reduce
                            new List<Tuple<T, U>>(),

                            // We start a function that check the condition with the items of both table
                            // This is the updater parameter of reduce
                            ( rowCombinations, rightRow ) => {
                                // Check whether the combination of items meet the requirements
                                if ( condition( leftRow, rightRow ) ) {
                                    // If so, we create the combination tuple
                                    Tuple<T, U> combination = new Tuple<T, U>( leftRow, rightRow );

                                    // Add the combination tuple to the accumulator
                                    rowCombinations.Add( combination );
                                }

                                // Return the list of all combination that meet the requirements =
                                return rowCombinations;
                            }
                        );

                    // Add all the items that meet the condition to the final list
                    joinedTable.AddRange( leftRowCombinations );

                    // Return the final list
                    return joinedTable;
                }
            );
        }
    }

    // The main clas of our program
    class Program {
        // The starting point of our application
        static void Main(string[] args) {
            #region PrintList
            // Print a title
            Console.WriteLine("PRINTLIST");

            // We create a list with even numbers
            List<int> evenNumber = new List<int>() { 0, 2, 4, 6, 8 };   

            // We check out the content of the list using the printList function
            evenNumber.PrintList();

            // Make space
            Console.WriteLine( "\n\n" );
            #endregion

            #region Map
            // Print a title
            Console.WriteLine( "MAP" );

            // We create a lambda function that multiplies the input
            Func<int, int> Multiplier = (input) => input * 2;

            // We print all the numbers in evenNumbers multiplied by two
            evenNumber
                .Map( Multiplier )
                .PrintList();

            // Make space
            Console.WriteLine( "\n\n" );
            #endregion

            #region Reduce
            // Print a title
            Console.WriteLine( "REDUCE" );

            // We create a method call summer, which adds the value of an item to an accumulator
            Func<int, int, int> Summer = ( result, item ) => result + item;

            // We ask for the sum of evenNumbers
            int sum = evenNumber.Reduce( 0, Summer );

            // We print the result of the reduce function
            System.Console.WriteLine( sum );

            // Make space
            Console.WriteLine( "\n\n" );
            #endregion

            #region Where
            // Print a title
            Console.WriteLine( "WHERE" );

            // Create a list with multiplications of three
            List<int> threeMultiplications = new List<int>() { 0, 3, 6, 9 };

            // We create a function that checks whether an item is uneven
            Func<int, bool> isUneven = ( number ) => number % 2 != 0;

            // We print the list of uneven items in evenNumber
            evenNumber
                .Where( isUneven )
                .PrintList();

            // We print the list of uneven items in threeMultiplications
            threeMultiplications
                .Where( isUneven )
                .PrintList();

            // Make space
            Console.WriteLine( "\n\n" );
            #endregion

            #region Join
            // Print a title
            Console.WriteLine( "JOIN" );

            // We join the evenNumber list with the threeMultiplications list using SimpleJoin
            // We want the combinations where the sum of both numbers is even
            evenNumber
                .SimpleJoin( threeMultiplications, ( left, right ) => ( ( left + right ) % 2 ) == 0 )
                .PrintList();

            // We join the evenNumber list with the threeMultiplications list using Join
            // Because of the lambda that always returns true, we will receive the cartesian product
            evenNumber
                .Join( threeMultiplications, ( left, right ) => true )
                .PrintList();
            #endregion
        }
    }
}
