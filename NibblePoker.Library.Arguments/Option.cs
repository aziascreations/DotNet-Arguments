using System.Collections.Generic;

namespace NibblePoker.Library.Arguments {
    /// <summary>
    ///     Class <c>Option</c> models an option linked to one or more
    ///     <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>
    ///     that can be given in launch arguments to pass a value or toggle some behaviour/actions.
    /// </summary>
    public class Option {
        /// <summary>
        ///     List of values given to the <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     if it is allowed to have any.<br />
        ///     If it isn't allowed to hold any, the list will just be instantiated and empty.
        /// </summary>
        /// <seealso cref="NibblePoker.Library.Arguments.OptionFlags.HasValue" />
        /// <seealso cref="NibblePoker.Library.Arguments.OptionFlags.HasMultipleValue" />
        public readonly List<string> Arguments;

        /// <summary>
        ///     Description as shown in the help text.<br/>
        ///     May be left as <c>null</c>.
        /// </summary>
        public readonly string Description;

        /// <summary>
        ///     Binary flags used to toggle some special behaviour and conditions during the parsing process.
        /// </summary>
        /// <seealso cref="NibblePoker.Library.Arguments.OptionFlags" />
        public readonly OptionFlags Flags;

        /// <summary>
        ///     Name string used when searching for the <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     or rendering the help text.<br/>
        ///     May be left as <c>null</c> to indicate that it has no long name.
        /// </summary>
        public readonly string Name;

        /// <summary>
        ///     Token character used when searching for the <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     or rendering the help text.<br/>
        ///     May be left as <c>null</c> to indicate that it has no token.
        /// </summary>
        public readonly char? Token;

        /// <summary>
        ///     Counter used to indicate how many times the <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     was used during the parsing process.
        /// </summary>
        /// <seealso cref="NibblePoker.Library.Arguments.OptionFlags.Repeatable" />
        public int Occurrences;

        /// <summary>
        ///     Common constructor for any <see cref="NibblePoker.Library.Arguments.Option">Option</see>.
        /// </summary>
        /// <param name="token">
        ///     Nullable token character used when searching for it in a <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>.
        /// </param>
        /// <param name="name">
        ///     Nullable name string used when searching for it in a <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>.
        /// </param>
        /// <param name="description">
        ///     Description as shown in the help text.
        /// </param>
        /// <param name="flags">
        ///     Binary flags used to toggle some special behaviour and conditions during the parsing process.
        /// </param>
        /// <exception cref="Exceptions.MissingOptionIdentifierException">
        ///     Thrown is a null <c>token</c> and <c>name</c> were given.
        /// </exception>
        /// <exception cref="Exceptions.InvalidFlagsException">
        ///     Thrown if some invalid <see cref="NibblePoker.Library.Arguments.OptionFlags">OptionFlags</see>
        ///     combination was given in <c>flags</c>.
        /// </exception>
        /// <seealso cref="NibblePoker.Library.Arguments.OptionFlags" />
        public Option(char? token, string name, string description = "", OptionFlags flags = OptionFlags.None) {
            Token = token;
            Name = name;
            Description = description;
            Flags = flags;

            Arguments = new List<string>();
            Occurrences = 0;

            if (!HasToken() && !HasName()) {
                throw new Exceptions.MissingOptionIdentifierException("The option doesn't have a token or name !");
            }

            if (IsDefault() && !CanHaveValue()) {
                throw new Exceptions.InvalidFlagsException("A default options must have one or more value !");
            }
        }

        /// <summary>
        ///     Checks if the <see cref="NibblePoker.Library.Arguments.Option">Option</see> has a token character.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if it has a token character, <c>false</c> otherwise.
        /// </returns>
        public bool HasToken() {
            return Token != null;
        }

        /// <summary>
        ///     Checks if the <see cref="NibblePoker.Library.Arguments.Option">Option</see> has a name string.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if it has a name string, <c>false</c> otherwise.
        /// </returns>
        public bool HasName() {
            return Name != null;
        }

        /// <summary>
        ///     Retrieve a proper and safe to print name representation for the
        ///     <see cref="NibblePoker.Library.Arguments.Option">Option</see>.
        /// </summary>
        /// <param name="addTicks">
        ///     Adds the <c>-</c> and <c>--</c> prefixes to the output as needed.
        /// </param>
        /// <returns>
        ///     The name in the <c>t</c>, <c>token</c> or <c>t|token</c> format.
        /// </returns>
        /// <seealso cref="NibblePoker.Library.Arguments.HelpText" />
        public string GetFullName(bool addTicks = false) {
            return (HasToken() ? (addTicks ? "-" : "") + Token : "") +
                   (HasToken() && HasName() ? "|" : "") +
                   (HasName() ? (addTicks ? "--" : "") + Name : "");
        }

        /// <summary>
        ///     Checks if the <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     was given the <see cref="NibblePoker.Library.Arguments.OptionFlags.Default">OptionFlags.Default</see>
        ///     flag in its constructor.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if the flag was given, <c>false</c> otherwise.
        /// </returns>
        public bool IsDefault() {
            return Flags.HasFlag(OptionFlags.Default);
        }

        /// <summary>
        ///     Checks if the <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     was given the <see cref="NibblePoker.Library.Arguments.OptionFlags.HasValue">OptionFlags.HasValue</see>
        ///     flag in its constructor.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if the flag was given, <c>false</c> otherwise.
        /// </returns>
        public bool CanHaveValue() {
            return Flags.HasFlag(OptionFlags.HasValue);
        }

        /// <summary>
        ///     Checks if the <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     was given the <see cref="NibblePoker.Library.Arguments.OptionFlags.Repeatable">OptionFlags.Repeatable</see>
        ///     flag in its constructor.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if the flag was given, <c>false</c> otherwise.
        /// </returns>
        public bool IsRepeatable() {
            return Flags.HasFlag(OptionFlags.Repeatable);
        }

        /// <summary>
        ///     Checks if the <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     was given the <see cref="NibblePoker.Library.Arguments.OptionFlags.HasValue">OptionFlags.HasValue</see>
        ///     and <see cref="NibblePoker.Library.Arguments.OptionFlags.Repeatable">OptionFlags.Repeatable</see>
        ///     flags in its constructor.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if the flags were both given, <c>false</c> otherwise.
        /// </returns>
        public bool CanHaveMultipleValue() {
            return CanHaveValue() && IsRepeatable();
        }

        /// <summary>
        ///     Checks if the <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     was given the <see cref="NibblePoker.Library.Arguments.OptionFlags.Hidden">OptionFlags.Hidden</see>
        ///     flag in its constructor.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if the flag was given, <c>false</c> otherwise.
        /// </returns>
        public bool IsHidden() {
            return Flags.HasFlag(OptionFlags.Hidden);
        }

        /// <summary>
        ///     Checks if the <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     was given the <see cref="NibblePoker.Library.Arguments.OptionFlags.Required">OptionFlags.Required</see>
        ///     flag in its constructor.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if the flag was given, <c>false</c> otherwise.
        /// </returns>
        public bool IsRequired() {
            return Flags.HasFlag(OptionFlags.Required);
        }

        /// <summary>
        ///     Checks if the <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     was given the <see cref="NibblePoker.Library.Arguments.OptionFlags.StopsParsing">OptionFlags.StopsParsing</see>
        ///     flag in its constructor.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if the flag was given, <c>false</c> otherwise.
        /// </returns>
        public bool ShouldStopParsing() {
            return Flags.HasFlag(OptionFlags.StopsParsing);
        }

        /// <summary>
        ///     Checks if the <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     was given the
        ///     <see cref="NibblePoker.Library.Arguments.OptionFlags.AllowVerbsAfter">OptionFlags.AllowVerbsAfter</see>
        ///     flag in its constructor.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if the flag was given, <c>false</c> otherwise.
        /// </returns>
        public bool AllowsVerbsAfter() {
            return Flags.HasFlag(OptionFlags.AllowVerbsAfter);
        }

        /// <summary>
        ///     Checks if the <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     was used and received a value during the parsing process.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if it has one or more value, <c>false</c> otherwise.
        /// </returns>
        public bool HasValue() {
            return Arguments.Count >= 1;
        }

        /// <summary>
        ///     Checks if the <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     was used in the parsed launch arguments.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if it was used at least once, <c>false</c> otherwise.
        /// </returns>
        public bool WasUsed() {
            return Occurrences >= 1;
        }

        /// <summary>
        ///     Clears any field that may be modified once the launch arguments are parsed.
        /// </summary>
        /// <seealso cref="NibblePoker.Library.Arguments.Option.Clear" />
        public void Clear() {
            Arguments.Clear();
            Occurrences = 0;
        }
    }
}
