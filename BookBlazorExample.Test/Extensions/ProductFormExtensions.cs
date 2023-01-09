using BookBlazorExample.Pages;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BookBlazorExample.Test.Extensions
{
    /// <summary>
    /// Helper functions that make the test code better readable by means of abstraction from UI details
    /// </summary>
    public static class ProductFormExtensions
    {
        public static bool HasEnabledElements(this IRenderedComponent<ProductForm> cut, params string[] elementIds)
        {
            var allExistAndAreEnabled = true;
            elementIds.ToList().ForEach(elementId =>
            {
                var element = cut.FindAll($"#{elementId}").SingleOrDefault();
                allExistAndAreEnabled = allExistAndAreEnabled && (element != null && element.GetAttribute("disabled") == null);
                if (!allExistAndAreEnabled)
                {
                    throw new Exception($"Element '{elementId}' is missing or disabled");
                }
            });
            return allExistAndAreEnabled;
        }

        public static bool DoesNotHaveElements(this IRenderedComponent<ProductForm> cut, params string[] elementIds)
        {
            var anyExists = false;
            elementIds.ToList().ForEach(elementId =>
            {
                var element = cut.FindAll($"#{elementId}").SingleOrDefault();
                anyExists = anyExists || (element != null);
                if (anyExists)
                {
                    throw new Exception($"Element '{elementId}' exists");
                }
            });
            return !anyExists;
        }

        public static bool HasDisabledElements(this IRenderedComponent<ProductForm> cut, params string[] elementIds)
        {
            var allExistAndAreDisabled = true;
            elementIds.ToList().ForEach(elementId =>
            {
                var element = cut.FindAll($"#{elementId}").SingleOrDefault();
                allExistAndAreDisabled = allExistAndAreDisabled && (element != null && element.GetAttribute("disabled") != null);
                if (!allExistAndAreDisabled)
                {
                    throw new Exception($"Element '{elementId}' is missing or enabled");
                }
            });
            return allExistAndAreDisabled;
        }

        public static string? GetProductName(this IRenderedComponent<ProductForm> cut)
        {
            var inputElement = cut.Find("input#product-name");
            return inputElement.GetAttribute("value");
        }

        public static void SetProductName(this IRenderedComponent<ProductForm> cut, string name)
        {
            cut.WaitForAssertion(() => cut.FindAll("input#product-name").Count().Should().BeGreaterThan(0));
            var inputElement = cut.Find("input#product-name");
            inputElement.Input(new ChangeEventArgs() { Value = name });
        }

        public static bool HasVisibleValidationErrorMessage(this IRenderedComponent<ProductForm> cut, string message)
        {
            const string validationErrorSelector = "div.invalid-feedback";
            var messageElems = cut.FindAll(validationErrorSelector);
            var messageElement = messageElems.FirstOrDefault(m => m.InnerHtml == message);
            if (messageElement == null)
            {
                return false;
            }
            else
            {
                var styleAttributeValue = messageElement.Attributes?.GetNamedItem("style")?.Value;
                return styleAttributeValue != null && Regex.IsMatch(styleAttributeValue, "display\\s*:\\s*block");
            }
        }
    }
}
