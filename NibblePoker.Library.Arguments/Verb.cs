using System.Collections.Generic;

namespace NibblePoker.Library.Arguments {
    /// <summary>
    ///     Class <c>Verb</c> models a verb that can be given in launch arguments to select a specific action or subset of
    ///     usable launch parameters.
    /// </summary>
    public class Verb {
        /// <summary>
        ///     Description as shown in the help text.
        /// </summary>
        public readonly string Description;

        /// <summary>
        ///     Name used when searching for a sub-<see cref="NibblePoker.Library.Arguments.Verb">Verb</see>
        ///     or rendering the help text.
        /// </summary>
        public readonly string Name;

        /// <summary>
        ///     List of <see cref="NibblePoker.Library.Arguments.Option">Option</see> registered via the
        ///     <see cref="NibblePoker.Library.Arguments.Verb.RegisterOption">Verb.RegisterOption</see> function.
        /// </summary>
        public readonly List<Option> Options;

        /// <summary>
        ///     List of sub-<see cref="NibblePoker.Library.Arguments.Verb">Verb</see> registered via the
        ///     <see cref="NibblePoker.Library.Arguments.Verb.RegisterVerb">Verb.RegisterVerb</see> function.
        /// </summary>
        public readonly List<Verb> Verbs;

        /// <summary>
        ///     Reference to a potential parent <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>
        ///     in which this one was registered.
        /// </summary>
        public Verb? ParentVerb;

        /// <summary>
        ///     Flag used to indicate if the <see cref="NibblePoker.Library.Arguments.Verb">Verb</see> was
        ///     used at any point during the parsing process.
        /// </summary>
        public bool WasUsed;

        /// <summary>
        ///     Constructs an independant <see cref="NibblePoker.Library.Arguments.Verb">Verb</see> with the given parameters.
        /// </summary>
        /// <param name="name">
        ///     Name used when searching for a sub-<see cref="NibblePoker.Library.Arguments.Verb">Verb</see>
        ///     or rendering the help text.
        /// </param>
        /// <param name="description">Description as shown in the help text.</param>
        public Verb(string? name, string description = "") {
            Name = name ?? "";
            Description = description;

            Options = new List<Option>();
            Verbs = new List<Verb>();
            WasUsed = false;
            ParentVerb = null;
        }

        /// <summary>
        ///     Attempts to register a sub-<see cref="NibblePoker.Library.Arguments.Verb">Verb</see>
        ///     in the current <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>.
        /// </summary>
        /// <param name="verb">
        ///     The <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>
        ///     to register in <see cref="NibblePoker.Library.Arguments.Verb.Verbs">Verb.Verbs</see>
        ///     as a sub-<see cref="NibblePoker.Library.Arguments.Verb">Verb</see>.
        /// </param>
        /// <returns>Itself to allow for registration daisy-chaining.</returns>
        /// <exception cref="Exceptions.InvalidVerbNameException">
        ///     If the registered <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>'s name is empty or null.
        /// </exception>
        /// <exception cref="Exceptions.DuplicateVerbException">
        ///     If the given <see cref="NibblePoker.Library.Arguments.Verb">Verb</see> or one with the same name is already
        ///     registered.
        /// </exception>
        public Verb RegisterVerb(Verb verb) {
            if (string.IsNullOrWhiteSpace(verb.Name)) {
                throw new Exceptions.InvalidVerbNameException("The given verb's name is empty !");
            }

            if (Verbs.Contains(verb) || GetSubVerbByName(verb.Name) != null) {
                throw new Exceptions.DuplicateVerbException("The given verb '" + verb.Name +
                                                            "'is already registered !");
            }

            verb.ParentVerb = this;
            Verbs.Add(verb);

            return this;
        }

        /// <summary>
        ///     Attempts to register an <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     in the current <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>.
        /// </summary>
        /// <param name="option">
        ///     The <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     to register in <see cref="NibblePoker.Library.Arguments.Verb.Verbs">Verb.Verbs</see>.
        /// </param>
        /// <returns>Itself to allow for registration daisy-chaining.</returns>
        /// <exception cref="Exceptions.DuplicateOptionException">
        ///     If the given <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     or one with the same token/name is already registered.
        /// </exception>
        /// <exception cref="Exceptions.ExistingDefaultMultipleOptionException">
        ///     If the given <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     has the <see cref="NibblePoker.Library.Arguments.OptionFlags.Default">Default</see>
        ///     flag and is registered after one that also has
        ///     <see cref="NibblePoker.Library.Arguments.OptionFlags.Default">OptionFlags.Default</see>,
        ///     <see cref="NibblePoker.Library.Arguments.OptionFlags.HasValue">OptionFlags.HasValue</see> and
        ///     <see cref="NibblePoker.Library.Arguments.OptionFlags.Repeatable">OptionFlags.Repeatable</see> flags.
        /// </exception>
        public Verb RegisterOption(Option option) {
            if (Options.Contains(option)) {
                throw new Exceptions.DuplicateOptionException(
                    "The given option '" + option.GetFullName() + "' is already registered !");
            }

            if (option.Token != null) {
                if (HasOptionByToken((char) option.Token)) {
                    throw new Exceptions.DuplicateOptionException(
                        "The given option token '" + option.Token + "' is already used by another option !");
                }
            }

            if (option.Name != null) {
                if (HasOptionByName(option.Name)) {
                    throw new Exceptions.DuplicateOptionException(
                        "The given option name '" + option.Name + "' is already used by another option !");
                }
            }

            if (option.IsDefault()) {
                foreach (Option existingOption in Options) {
                    if (!existingOption.IsDefault()) {
                        continue;
                    }

                    if (existingOption.CanHaveMultipleValue() || existingOption.IsRepeatable()) {
                        throw new Exceptions.ExistingDefaultMultipleOptionException(
                            "A default option that accepts multiple values/usage is already registered !");
                    }
                }
            }

            Options.Add(option);

            return this;
        }


        /// <summary>
        ///     Attempts to register an <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     in the current <see cref="NibblePoker.Library.Arguments.Verb">Verb</see> and all its
        ///     <see cref="NibblePoker.Library.Arguments.Verb">sub-Verb</see> in a recursive manner.
        /// </summary>
        /// <param name="option">
        ///     The <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     to register in <see cref="NibblePoker.Library.Arguments.Verb.Verbs">Verb.Verbs</see>.
        /// </param>
        /// <param name="ignoreDuplicates">
        ///     Prevents exceptions from being raised if an <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     with the same token or name is encountered.<br />
        ///     Default: <c>false</c>
        /// </param>
        /// <returns>Itself to allow for registration daisy-chaining.</returns>
        /// <exception cref="Exceptions.DuplicateOptionException">
        ///     If the given <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     or one with the same token/name is already registered.<br />
        ///     Will not be thrown if <c>ignoreDuplicates</c> is set to <c>true</c>.
        /// </exception>
        /// <exception cref="Exceptions.ExistingDefaultMultipleOptionException">
        ///     If the given <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     has the <see cref="NibblePoker.Library.Arguments.OptionFlags.Default">Default</see>
        ///     flag and is registered after one that also has
        ///     <see cref="NibblePoker.Library.Arguments.OptionFlags.Default">OptionFlags.Default</see>,
        ///     <see cref="NibblePoker.Library.Arguments.OptionFlags.HasValue">OptionFlags.HasValue</see> and
        ///     <see cref="NibblePoker.Library.Arguments.OptionFlags.Repeatable">OptionFlags.Repeatable</see> flags.
        /// </exception>
        public Verb RegisterOptionRecursively(Option option, bool ignoreDuplicates = false) {
            try {
                RegisterOption(option);
            } catch (Exceptions.DuplicateOptionException) {
                if (!ignoreDuplicates) {
                    throw;
                }
            }

            foreach (Verb subVerb in Verbs) {
                subVerb.RegisterOptionRecursively(option, ignoreDuplicates);
            }

            return this;
        }

        /// <summary>
        ///     Attempts to retrieve a registered sub-<see cref="NibblePoker.Library.Arguments.Verb">Verb</see>
        ///     using its name.
        /// </summary>
        /// <param name="name">
        ///     The desired <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>'s name
        /// </param>
        /// <returns>
        ///     The relevant <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>, <c>null</c> otherwise.
        /// </returns>
        public Verb? GetSubVerbByName(string name) {
            foreach (Verb verb in Verbs) {
                if (verb.Name.Equals(name)) {
                    return verb;
                }
            }

            return null;
        }

        /// <summary>
        ///     Attempts to retrieve the default <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     that should be used during the parsing process based on whether it has been used before.
        /// </summary>
        /// <returns>
        ///     The relevant <see cref="NibblePoker.Library.Arguments.Option">Option</see>, <c>null</c> otherwise.
        /// </returns>
        public Option? GetRelevantDefaultOption() {
            foreach (Option option in Options) {
                if (!option.IsDefault()) {
                    continue;
                }

                if (option.CanHaveMultipleValue() || option.IsRepeatable()) {
                    return option;
                }

                if (option.CanHaveValue() && !option.WasUsed()) {
                    return option;
                }
            }

            return null;
        }

        /// <summary>
        ///     Attempts to retrieve a registered <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     using its token.
        /// </summary>
        /// <param name="token">
        ///     The desired <see cref="NibblePoker.Library.Arguments.Option">Option</see>'s token
        /// </param>
        /// <returns>
        ///     The relevant <see cref="NibblePoker.Library.Arguments.Option">Option</see>, <c>null</c> otherwise.
        /// </returns>
        public Option? GetOptionByToken(char token) {
            foreach (Option option in Options) {
                if (option.HasToken() && option.Token!.Equals(token)) {
                    return option;
                }
            }

            return null;
        }

        /// <summary>
        ///     Attempts to retrieve a registered <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     using its name.
        /// </summary>
        /// <param name="name">
        ///     The desired <see cref="NibblePoker.Library.Arguments.Option">Option</see>'s name
        /// </param>
        /// <returns>
        ///     The relevant <see cref="NibblePoker.Library.Arguments.Option">Option</see>, <c>null</c> otherwise.
        /// </returns>
        public Option? GetOptionByName(string name) {
            foreach (Option option in Options) {
                if (option.HasName() && option.Name!.Equals(name)) {
                    return option;
                }
            }

            return null;
        }

        /// <summary>
        ///     Checks if a given token is used by a registered <see cref="NibblePoker.Library.Arguments.Option">Option</see>.
        /// </summary>
        /// <param name="token">
        ///     The desired <see cref="NibblePoker.Library.Arguments.Option">Option</see>'s token
        /// </param>
        /// <returns>
        ///     <c>true</c> if a matching <see cref="NibblePoker.Library.Arguments.Option">Option</see> was found,
        ///     <c>false</c> otherwise.
        /// </returns>
        public bool HasOptionByToken(char token) {
            return GetOptionByToken(token) != null;
        }

        /// <summary>
        ///     Checks if a given name is used by a registered <see cref="NibblePoker.Library.Arguments.Option">Option</see>.
        /// </summary>
        /// <param name="name">
        ///     The desired <see cref="NibblePoker.Library.Arguments.Option">Option</see>'s name
        /// </param>
        /// <returns>
        ///     <c>true</c> if a matching <see cref="NibblePoker.Library.Arguments.Option">Option</see> was found,
        ///     <c>false</c> otherwise.
        /// </returns>
        public bool HasOptionByName(string name) {
            return GetOptionByName(name) != null;
        }

        /// <summary>
        ///     Clears any field and registered member's fields that may be modified once the launch arguments are parsed.
        /// </summary>
        /// <seealso cref="NibblePoker.Library.Arguments.Option.Clear" />
        public void Clear() {
            foreach (Option option in Options) {
                option.Clear();
            }

            foreach (Verb verb in Verbs) {
                verb.Clear();
            }

            WasUsed = false;
        }
    }
}
