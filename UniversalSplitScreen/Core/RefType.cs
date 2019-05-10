namespace UniversalSplitScreen.Core
{
	public class RefType<T>
	{
		public readonly string propertyName;

		public RefType (string propertyName)
		{
			this.propertyName = propertyName;

			foreach (var x in typeof(OptionsStructure).GetProperties())
			{
				if (x.Name == propertyName)
					return;
			}

			throw new System.Exception($"No property found in OptionsStructure: {propertyName}");
		}

		public void Set(T value)
		{
			typeof(OptionsStructure).GetProperty(propertyName).SetValue(Options.CurrentOptions, value);
		}

		public static implicit operator T(RefType<T> refType)
		{
			return (T)typeof(OptionsStructure).GetProperty(refType.propertyName).GetValue(Options.CurrentOptions);
		}

		public override string ToString()
		{
			return ((T)this).ToString();
		}
	}
}
