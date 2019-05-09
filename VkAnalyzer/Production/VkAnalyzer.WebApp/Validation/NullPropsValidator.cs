using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

#pragma warning disable 1591

namespace VkAnalyzer.WebApp.Validation
{
	/// <inheritdoc />
	public class NullPropsValidator : IValidator
	{
		/// <summary>
		/// Проверка типов значений на стандартное
		/// </summary>
		public bool CheckValueTypes;

		public ValidationResult Validate(object instance)
		{
			var failures = new List<ValidationFailure>();
			var properties = instance.GetType().GetProperties();
			foreach (var property in properties)
			{
				var isValueType = property.PropertyType.IsValueType;
				var value = property.GetValue(instance);

				if (!isValueType && value == null)
					failures.Add(new ValidationFailure(property.Name, $"{property.Name} не может быть пустым"));

				if (CheckValueTypes && isValueType && value.Equals(Activator.CreateInstance(property.PropertyType)))
					failures.Add(new ValidationFailure(property.Name, $"{property.Name} пустое или заполнено неверно"));
			}
			var result = failures.Any() ? new ValidationResult(failures) : new ValidationResult();
			return result;
		}

		public Task<ValidationResult> ValidateAsync(object instance, CancellationToken cancellation = new CancellationToken())
		{
			throw new NotImplementedException();
		}

		public ValidationResult Validate(ValidationContext context)
		{
			return Validate(context.InstanceToValidate);
		}

		public Task<ValidationResult> ValidateAsync(ValidationContext context, CancellationToken cancellation = new CancellationToken())
		{
			throw new NotImplementedException();
		}

		public IValidatorDescriptor CreateDescriptor()
		{
			throw new NotImplementedException();
		}

		public bool CanValidateInstancesOfType(Type type)
		{
			return true;
		}
	}
}