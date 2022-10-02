namespace NibblePoker.Library.Arguments; 

/// <summary>
/// Class <c>Verb</c> models a verb that can be given in launch arguments to select a specific action or subset of
///  usable launch parameters.
/// </summary>
public class Verb {
	/// <summary>
	/// Name used when searching for sub-verbs or rendering the help text.
	/// </summary>
	public readonly string Name;
	
	/// <summary>
	/// Description that is shown in the help text.
	/// </summary>
	public readonly string Description;
	
	/// <summary>
	/// List of registered <c>Option</c> via the 'RegisterOption' function.
	/// </summary>
	public readonly List<Option> Options;
	
	/// <summary>
	/// List of registered sub-<c>Verb</c> via the 'RegisterVerb' function.
	/// </summary>
	public readonly List<Verb> Verbs;
	
	/// <summary>
	/// Flag used to indicate if the verb was used at any point during the parsing process.
	/// </summary>
	public bool WasUsed;
	
	/// <summary>
	/// Reference to a potential <c>Verb</c> into which this one was registered.
	/// </summary>
	public Verb? ParentVerb;
	
	/// <summary>
	/// Common constructor for any <c>Verb</c>.
	/// </summary>
	/// <param name="name">Name used when searching for sub-verbs.</param>
	/// <param name="description">Description that is shown in the help text.</param>
	public Verb(string? name, string description = "") {
		Name = name ?? "";
		Description = description;

		Options = new List<Option>();
		Verbs = new List<Verb>();
		WasUsed = false;
		ParentVerb = null;
	}
	
	/// <summary>
	/// Attempts to register a sub-<c>Verb</c> in the current <c>Verb</c>.
	/// </summary>
	/// <param name="verb">The <c>Verb</c> to register in the current <c>Verb</c>.</param>
	/// <returns>The current <c>Verb</c> for registration daisy-chaining.</returns>
	/// <exception cref="Exceptions.InvalidVerbNameException">If the registered <c>Verb</c>'s name is empty or null.</exception>
	/// <exception cref="Exceptions.DuplicateVerbException">
	/// If the given <c>Verb</c> or one with the same name is already registered
	/// </exception>
	public Verb RegisterVerb(Verb verb) {
		if(string.IsNullOrWhiteSpace(verb.Name)) {
			throw new Exceptions.InvalidVerbNameException("The given verb's name is empty !");
		}
		
		if(Verbs.Contains(verb) || GetSubVerbByName(verb.Name) != null) {
			throw new Exceptions.DuplicateVerbException("The given verb '" + verb.Name +
			                                            "'is already registered !");
		}
		
		verb.ParentVerb = this;
		Verbs.Add(verb);
		
		return this;
	}
	
	/// <summary>
	/// Attempts to register an <c>Option</c> in the current <c>Verb</c>.
	/// </summary>
	/// <param name="option">The <c>Option</c> to register in the current <c>Verb</c>.</param>
	/// <returns>The current <c>Verb</c> for registration daisy-chaining.</returns>
	/// <exception cref="Exceptions.DuplicateOptionException">
	/// If the given <c>Option</c> or one with the same token/name is already registered
	/// </exception>
	/// <exception cref="Exceptions.ExistingDefaultMultipleOptionException">
	/// If the given <c>Option</c> has the "F:NibblePoker.Library.Arguments.OptionFlags.Default" flag and is registered
	///  after one that has the "F:NibblePoker.Library.Arguments.OptionFlags.Default",
	///  "F:NibblePoker.Library.Arguments.OptionFlags.HasValue" and
	///  "F:NibblePoker.Library.Arguments.OptionFlags.Repeatable" flags.
	/// </exception>
	public Verb RegisterOption(Option option) {
		if(Options.Contains(option)) {
			throw new Exceptions.DuplicateOptionException(
				"The given option '" + option.GetFullName() + "' is already registered !");
		}
		
		if(option.Token != null) {
			if(HasOptionByToken((char) option.Token)) {
				throw new Exceptions.DuplicateOptionException(
					"The given option token '" + option.Token+ "' is already used by another option !");
			}
		}
		
		if(option.Name != null) {
			if(HasOptionByName(option.Name)) {
				throw new Exceptions.DuplicateOptionException(
					"The given option name '" + option.Name+ "' is already used by another option !");
			}
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
	
	/// <summary>
	/// Attempts to retrieve a registered <c>Verb</c> using its name.
	/// </summary>
	/// <param name="name">The desired <c>Verb</c>'s name</param>
	/// <returns>The relevant <c>Verb</c>, null otherwise.</returns>
	public Verb? GetSubVerbByName(string name) {
		foreach(Verb verb in Verbs) {
			if(verb.Name.Equals(name)) {
				return verb;
			}
		}

		return null;
	}
	
	/// <summary>
	/// Attempts to grab the default <c>Option</c> that should be used as one during the parsing process based on
	/// whether it has been used before.
	/// </summary>
	/// <returns></returns>
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
	
	/// <summary>
	/// Attempts to retrieve a registered <c>Option</c> using its token.
	/// </summary>
	/// <param name="token">The desired <c>Option</c>'s token</param>
	/// <returns>The relevant <c>Option</c>, null otherwise.</returns>
	public Option? GetOptionByToken(char token) {
		foreach(Option option in Options) {
			if(option.HasToken() && option.Token!.Equals(token)) {
				return option;
			}
		}
		
		return null;
	}
	
	/// <summary>
	/// Attempts to retrieve a registered <c>Option</c> using its name.
	/// </summary>
	/// <param name="name">The desired <c>Option</c>'s name</param>
	/// <returns>The relevant <c>Option</c>, null otherwise.</returns>
	public Option? GetOptionByName(string name) {
		foreach(Option option in Options) {
			if(option.HasName() && option.Name!.Equals(name)) {
				return option;
			}
		}
		
		return null;
	}
	
	/// <summary>
	/// Checks if a given token is used by a registered <c>Option</c>.
	/// </summary>
	/// <param name="token">The desired <c>Option</c>'s token</param>
	/// <returns>True if a matching <c>Option</c> was found, false otherwise.</returns>
	public bool HasOptionByToken(char token) {
		return GetOptionByToken(token) != null;
	}
	
	/// <summary>
	/// Checks if a given name is used by a registered <c>Option</c>.
	/// </summary>
	/// <param name="name">The desired <c>Option</c>'s name</param>
	/// <returns>True if a matching <c>Option</c> was found, false otherwise.</returns>
	public bool HasOptionByName(string name) {
		return GetOptionByName(name) != null;
	}
	
	/// <summary>
	/// Clears any field and registered member's fields that may be modified once the launch arguments are parsed.
	/// </summary>
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