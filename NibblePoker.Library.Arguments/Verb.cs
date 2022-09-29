namespace NibblePoker.Library.Arguments; 

/// <summary>
/// Class <c>Verb</c> models a verb that can be given in launch arguments to select a specific action or subset of
///  usable launch parameters.
/// </summary>
public class Verb {
	public string Name;
	public string Description;
	
	public List<Option> Options;
	public List<Verb> Verbs;
	public bool WasUsed;
	public Verb? ParentVerb;
	
	public Verb(string? name, string description = "") {
		Name = name ?? "";
		Description = description;

		Options = new List<Option>();
		Verbs = new List<Verb>();
		WasUsed = false;
		ParentVerb = null;
	}
	
	public Verb RegisterVerb(Verb verb) {
		if(string.IsNullOrWhiteSpace(verb.Name)) {
			throw new Exceptions.InvalidVerbNameException("The given verb's name is empty !");
		}
		
		if(Verbs.Contains(verb)) {
			throw new Exceptions.DuplicateVerbException("The given verb '" + verb.Name +
			                                            "'is already registered !");
		}
		
		verb.ParentVerb = this;
		Verbs.Add(verb);
		
		return this;
	}
	
	public Verb RegisterOption(Option option) {
		if(Options.Contains(option)) {
			throw new Exceptions.DuplicateOptionException(
				"The given option '" + option.GetFullName() + "' is already registered !");
		}

		if(option.IsDefault()) {
			foreach(Option existingOption in Options) {
				if(!existingOption.IsDefault()) {
					continue;
				}
				if(existingOption.CanHaveMultipleValue() || existingOption.IsRepeatable()) {
					throw new Exceptions.ExistingDefaultMultipleOptionException(
						"A default option that accepts multiple values/usage is already registered !");
				}
			}
		}
		
		Options.Add(option);
		
		return this;
	}

	public Verb? GetSubVerbByName(string name) {
		foreach(Verb verb in Verbs) {
			if(verb.Name.Equals(name)) {
				return verb;
			}
		}

		return null;
	}

	public Option? GetRelevantDefaultOption() {
		foreach(Option option in Options) {
			if(!option.IsDefault()) {
				continue;
			}
			
			if(option.CanHaveMultipleValue() || option.IsRepeatable()) {
				return option;
			}
			
			if(option.CanHaveValue() && !option.WasUsed()) {
				return option;
			}
		}

		return null;
	}
	
	public Option? GetOptionByToken(char token) {
		foreach(Option option in Options) {
			if(option.HasToken() && option.Token!.Equals(token)) {
				return option;
			}
		}
		
		return null;
	}
	
	public Option? GetOptionByName(string name) {
		foreach(Option option in Options) {
			if(option.HasName() && option.Name!.Equals(name)) {
				return option;
			}
		}
		
		return null;
	}
	
	public bool HasOptionByToken(char token) {
		return GetOptionByToken(token) != null;
	}
	
	public bool HasOptionByName(string name) {
		return GetOptionByName(name) != null;
	}
	
	public void Clear() {
		foreach(Option option in Options) {
			option.Clear();
		}
		
		foreach(Verb verb in Verbs) {
			verb.Clear();
		}
		
		WasUsed = false;
	}
}