using Extensions;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Windows;

namespace SlickControls
{
	public class IconManager
	{
		private static readonly Dictionary<string, Dictionary<int, Bitmap>> _iconLibrary;

		static IconManager()
		{
			var list = new List<KeyAndIcon>();

			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (!assembly.IsDynamic)
				{
					list.AddRange(GetIconNames(assembly));
				}
			}

			_iconLibrary = new Dictionary<string, Dictionary<int, Bitmap>>();

			foreach (var item in list)
			{
				var match = Regex.Match(item.Name, @"^(I_[A-Za-z]+(?:_[A-Za-z]+)*)?_([0-9]+)$");

				if (match.Success)
				{

					if (!_iconLibrary.ContainsKey(match.Groups[1].Value))
					{
						_iconLibrary[match.Groups[1].Value] = new Dictionary<int, Bitmap>();
					}

					_iconLibrary[match.Groups[1].Value][match.Groups[2].Value.SmartParse(16)] = item.Icon;
				}
			}
		}

		public static Bitmap GetIcon(string name)
		{
			return GetIcon(name, GetNormalScale());
		}

		public static Bitmap GetSmallIcon(string name)
		{
			return GetIcon(name, GetSmallScale());
		}

		public static Bitmap GetLargeIcon(string name)
		{
			return GetIcon(name, GetLargeScale());
		}

		public static int GetNormalScale() => (int)(-27.42857 + 52.57143 * UI.FontScale - 9.142857 * Math.Pow(UI.FontScale, 2));
		public static int GetSmallScale() => (int)(8.964706 + 5.388235 * UI.FontScale + 1.647059 * Math.Pow(UI.FontScale, 2));
		public static int GetLargeScale() => (int)(24 * UI.FontScale);

		public static Bitmap GetIcon(string name, int preferredSize)
		{
			if (name == null || !_iconLibrary.ContainsKey(name))
			{
				return null;
			}

			var key = _iconLibrary[name].Keys.Where(x => x <= preferredSize).DefaultIfEmpty(_iconLibrary[name].Keys.Min()).Max();

			return new Bitmap(_iconLibrary[name][key]);
		}

		public static Dictionary<int, Bitmap> GetIcons(string name)
		{
			if (name == null || !_iconLibrary.ContainsKey(name))
			{
				return new Dictionary<int, Bitmap>();
			}

			return _iconLibrary[name];
		}

		private static IEnumerable<KeyAndIcon> GetIconNames(Assembly appAssembly)
		{
			ResourceSet entries;
			try
			{
				var resourceManager = new ResourceManager(appAssembly.GetName().Name + ".Properties.Resources", appAssembly);
				entries = resourceManager.GetResourceSet(new CultureInfo(""), true, false);
			}
			catch
			{
				yield break;
			}

			if (entries == null)
			{
				yield break;
			}

			foreach (DictionaryEntry entry in entries)
			{
				if (entry.Value is Bitmap bitmap)
				{
					yield return new KeyAndIcon((string)entry.Key, bitmap);
				}
			}
		}

		public class IconConverter : TypeConverter
		{
			public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
			{
				return true;
			}

			public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				return new StandardValuesCollection(_iconLibrary?.Keys.OrderBy(x => x).Select(x => new DynamicIcon(x)).ToArray());
			}

			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return destinationType == typeof(string);
			}

			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string);
			}

			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (value is DynamicIcon icon)
				{
					return icon.Name;
				}

				return null;
			}

			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				if (value is string str && !string.IsNullOrEmpty(str))
				{
					return new DynamicIcon(str);
				}

				return null;
			}
		}

		private struct KeyAndIcon
		{
			public string Name;
			public Bitmap Icon;

			public KeyAndIcon(string name, Bitmap icon)
			{
				Name = name;
				Icon = icon;
			}
		}
	}

	public class DynamicIcon
	{
		public DynamicIcon()
		{

		}

		public DynamicIcon(string name)
		{
			Name = name;
		}

		public string Name { get; set; }

		public Bitmap Small => IconManager.GetSmallIcon(Name);
		public Bitmap Large => IconManager.GetLargeIcon(Name);
		public Bitmap Default => IconManager.GetIcon(Name);

		public static implicit operator DynamicIcon(string name) => name == null ? null : new DynamicIcon(name);

		public static implicit operator Bitmap(DynamicIcon icon) => icon == null ? null : IconManager.GetIcon(icon.Name);

		public static implicit operator Image(DynamicIcon icon) => icon == null ? null : IconManager.GetIcon(icon.Name);

		public Bitmap Get(int preferredSize) => IconManager.GetIcon(Name, preferredSize);

		public override string ToString()
		{
			return Name;
		}
	}
}
