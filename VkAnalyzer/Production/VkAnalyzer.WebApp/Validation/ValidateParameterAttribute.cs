using System;
using FluentValidation;

#pragma warning disable 1591

namespace VkAnalyzer.WebApp.Validation
{
	/// <inheritdoc />
	/// <summary>
	/// Атрибут оповещает о требовании обязательной валидации параметра
	/// <example>[ValidateParameter(DefaultValidators.NullProps)]</example>
	/// </summary>
	public class ValidateParameterAttribute : Attribute
	{
		private IValidator _customValidator;
		private readonly Type _customValidatorType;

		public IValidator CustomValidator =>
			_customValidator ?? (_customValidatorType != null
				? _customValidator = (IValidator)Activator.CreateInstance(_customValidatorType)
				: null);

		public ValidateParameterAttribute()
		{
			_customValidator = null;
			_customValidatorType = null;
		}

		public ValidateParameterAttribute(Type customValidatorType)
		{
			_customValidatorType = customValidatorType;
		}

		public ValidateParameterAttribute(IValidator customValidator = null)
		{
			_customValidator = customValidator;
		}

		public ValidateParameterAttribute(DefaultValidators defaultValidator)
		{
			switch (defaultValidator)
			{
				case DefaultValidators.NullProps:
					_customValidator = new NullPropsValidator();
					break;
				case DefaultValidators.NullPropsNoDefault:
					_customValidator = new NullPropsValidator { CheckValueTypes = true };
					break;
				default:
					_customValidator = null;
					break;
			}
		}
	}

	public enum DefaultValidators
	{
		/// <summary>
		/// Проверка ссылочных типов на Null
		/// </summary>
		NullProps,

		/// <summary>
		/// Проверка ссылочных типов на Null и типов значений на default
		/// </summary>
		NullPropsNoDefault
	}

	/// <inheritdoc />
	/// <summary>
	/// Атрибут оповещает о требовании обязательной проверки параметра на Null
	/// </summary>
	public class NullCheckAttribute : Attribute
	{
	}
}