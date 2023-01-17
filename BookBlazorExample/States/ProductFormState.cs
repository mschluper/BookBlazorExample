using BookBlazorExample.Models;

namespace BookBlazorExample.States
{
    /// <summary>
    /// Represents the state of a Product Form
    /// </summary>
    public class ProductFormState
    {
        /// <summary>
        /// Error message indicating that the product's name must be supplied.
        /// </summary>
        public const string NameIsRequired = "Name is required";

        /// <summary>
        /// Error message indicating that the product's name's maximum length has been exceeded.
        /// </summary>
        public const string NameHasMaxLength = "Can be at most 100 characters long";

        /// <summary>
        /// Error message indicating that the product's name is not unque.
        /// </summary>
        public const string NameNotUnique = "This name is already taken";

        /// <summary>
        /// Error message indicating that the product's price must be supplied.
        /// </summary>
        public const string PriceRequired = "Price is required";

        /// <summary>
        /// Error message indicating that the product's category must be supplied.
        /// </summary>
        public const string CategoryRequired = "Category is required";

        #region Form properties
        public bool IsLoading { get; set; }
        public bool ProductDoesNotExist { get; set; }
        public bool HasPendingChanges { get; set; } // aka IsDirty
        #endregion

        /// <summary>
        /// The product's unique ID
        /// </summary>
        /// <remarks>
        /// The state pertains to a single product - the one with this ID
        /// </remarks>
        public int Id { get; set; }

        /// <summary>
        /// The product's name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The product's price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Reflects whether the product is in stock
        /// </summary>
        public bool IsInStock { get; set; }

        /// <summary>
        /// Identifies the product's category
        /// </summary>
        public int CategoryId { get; set; }

        #region Validation
        /// <summary>
        /// Defines whether state.Name, which is required, is missing. 
        /// </summary>
        public bool NameIsMissing { get; set; }

        /// <summary>
        /// Defines whether state.Name is too long (100 chars max). 
        /// </summary>
        public bool NameIsTooLong { get; set; }

        /// <summary>
        /// Defines whether state.Name, which must be unique, is already in use. 
        /// </summary>
        public bool NameIsInUse { get; set; }

        /// <summary>
        /// Defines whether state.Name is invalid. If it is invalid, InvalidNameErrorMessage has a value indicating why.
        /// </summary>
        public bool NameIsInvalid
        {
            get
            {
                return NameIsMissing || NameIsTooLong || NameIsInUse;
            }
        }

        /// <summary>
        /// Defines the validation error message in case state.NameIsInvalid equals true. 
        /// </summary>
        public string InvalidNameErrorMessage
        {
            get
            {
                if (NameIsMissing)
                {
                    return NameIsRequired;
                }
                if (NameIsTooLong)
                {
                    return NameHasMaxLength;
                }
                if (NameIsInUse)
                {
                    return NameNotUnique;
                }
                return string.Empty;
            }
        }


        /// <summary>
        /// Defines whether state.Price is invalid. If it is invalid, InvalidPriceErrorMessage has a value indicating why.
        /// </summary>
        public bool PriceIsInvalid { get; set; }

        /// <summary>
        /// Defines the validation error message in case state.PriceIsInvalid equals true. 
        /// </summary>
        public string InvalidPriceErrorMessage { get; } = PriceRequired;

        /// <summary>
        /// Defines whether state.CategoryId is invalid. If it is invalid, InvalidCategoryErrorMessage has a value indicating why.
        /// </summary>
        public bool CategoryIsInvalid { get; set; }

        /// <summary>
        /// Defines the validation error message in case state.CategoryIsInvalid equals true. 
        /// </summary>
        public string InvalidCategoryErrorMessage { get; } = CategoryRequired;


        public List<string> ErrorSummary { get; set; } = new();

        /// <summary>
        /// Defines whether the product is invalid, i.e. whether any of its properties is invalid.
        /// If it is invalid, submits are disabled and the ErrorSummary is displayed.
        /// </summary>
        public bool ProductIsInvalid
        {
            get
            {
                return NameIsInvalid || PriceIsInvalid || CategoryIsInvalid;
            }
        }
        #endregion

        /// <summary>
        /// Existing product names (all not null and not empty)
        /// </summary>
        public List<string> ExistingProductNames { get; set; } = new();

        public List<ProductCategory> ProductCategories { get; set; } = new();
    }
}
