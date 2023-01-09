using BookBlazorExample.Models;

namespace BookBlazorExample.States
{
    public class ProductFormState
    {
        public const string NameRequired = "Name is required";
        public const string NameMaxLength = "Can be at most 100 characters long";
        public const string NameUnique = "This name is already taken";
        public const string PriceRequired = "Price is required";
        public const string CategoryRequired = "Category is required";

        #region Form properties
        public bool IsLoading { get; set; }
        public bool ProductDoesNotExist { get; set; }
        public bool HasPendingChanges { get; set; } // aka IsDirty
        #endregion

        /// <summary>
        /// The product's unique ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The product name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The product price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Reflects whether the product is in stock
        /// </summary>
        public bool IsInStock { get; set; }

        /// <summary>
        /// The product category
        /// </summary>
        public int CategoryId { get; set; }

        #region Validation
        /// <summary>
        /// Defines whether state.Name, which is required, is missing. Reevaluated upon each change of state.Name.
        /// </summary>
        public bool NameIsMissing { get; set; }

        /// <summary>
        /// Defines whether state.Name is too long (100 chars max). Reevaluated upon each change of state.Name.
        /// </summary>
        public bool NameIsTooLong { get; set; }

        /// <summary>
        /// Defines whether state.Name, which must be unique, is already in use. Reevaluated upon each change of state.Name.
        /// </summary>
        public bool NameIsInUse { get; set; }

        /// <summary>
        /// Defines whether state.Name is invalid. If it is invalid, InvalidNameErrorMessage is displayed.
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
                    return NameRequired;
                }
                if (NameIsTooLong)
                {
                    return NameMaxLength;
                }
                if (NameIsInUse)
                {
                    return NameUnique;
                }
                return string.Empty;
            }
        }

        public string InvalidPriceErrorMessage { get; } = PriceRequired;

        public string InvalidCategoryErrorMessage { get; } = CategoryRequired;

        public bool PriceIsInvalid { get; set; }

        public bool CategoryIsInvalid { get; set; }

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
