using System.Collections.Generic;

// ReSharper disable ArrangeNamespaceBody
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UseCollectionExpression
namespace NibblePoker.Library.Arguments;

/// <summary>
///     Class <c>Option</c> models an option linked to one or more
///     <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>
///     that can be given in launch arguments to pass a value or toggle some behavior/actions.
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
    ///     Will be treated as non-present, or on empty string if left as <c>null</c>.
    /// </summary>
    public readonly string? Description;

    /// <summary>
    ///     Binary flags used to toggle some special behavior and conditions during the parsing process.
    /// </summary>
    /// <seealso cref="NibblePoker.Library.Arguments.OptionFlags" />
    public OptionFlags Flags;

    /// <summary>
    ///     Name string used when searching for the <see cref="NibblePoker.Library.Arguments.Option">Option</see>
    ///     or rendering the help text.<br/>
    ///     May be left as <c>null</c> to indicate that it has no long name.
    /// </summary>
    public readonly string? Name;

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
    ///     Binary flags used to toggle some special behavior and conditions during the parsing process.
    /// </param>
    /// <exception cref="Exceptions.MissingOptionIdentifierException">
    ///     Thrown is a null <c>token</c> and <c>name</c> were given.
    /// </exception>
    /// <exception cref="Exceptions.InvalidFlagsException">
    ///     Thrown if some invalid <see cref="NibblePoker.Library.Arguments.OptionFlags">OptionFlags</see>
    ///     combination was given in <c>flags</c>.
    /// </exception>
    /// <seealso cref="NibblePoker.Library.Arguments.OptionFlags" />
    public Option(char? token, string? name, string? description = "", OptionFlags flags = OptionFlags.None) {
        Token = token;
        Name = name;
        Description = description;
        Flags = flags;

        Arguments = new List<string>();
        Occurrences = 0;

        if (!HasToken() && !HasName()) {
            throw new Exceptions.MissingOptionIdentifierException("The option doesn't have a token or name !");
        }

        if (IsDefault && !CanHaveValue) {
            throw new Exceptions.InvalidFlagsException("A default options must have one or more value !");
        }
    }

    /// <summary>
    ///     Clears any field that may be modified once the launch arguments are parsed.
    /// </summary>
    /// <seealso cref="NibblePoker.Library.Arguments.Option.Clear" />
    public void Clear() {
        Arguments.Clear();
        Occurrences = 0;
    }


    #region Generic Getters

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

    #endregion


    #region Properties for Flags field

    /// <summary>
    ///   Interacts with the <see cref="NibblePoker.Library.Arguments.OptionFlags.Default">OptionFlags.Default</see>
    ///    flag that may have been given in the constructor, or modified through this property.
    /// </summary>
    public bool IsDefault {
#if NET20 || NET30 || NET35
        get => (Flags & OptionFlags.Default) == OptionFlags.Default;
#else
            get => Flags.HasFlag(OptionFlags.Default);
#endif
        set {
            if (value) {
                Flags |= OptionFlags.Default;
            } else {
                Flags &= ~OptionFlags.Default;
            }
        }
    }

    /// <summary>
    ///   Interacts with the <see cref="NibblePoker.Library.Arguments.OptionFlags.HasValue">OptionFlags.HasValue</see>
    ///    flag that may have been given in the constructor, or modified through this property.
    /// </summary>
    public bool CanHaveValue {
#if NET20 || NET30 || NET35
        get => (Flags & OptionFlags.HasValue) == OptionFlags.HasValue;
#else
            get => Flags.HasFlag(OptionFlags.HasValue);
#endif
        set {
            if (value) {
                Flags |= OptionFlags.HasValue;
            } else {
                Flags &= ~OptionFlags.HasValue;
            }
        }
    }

    /// <summary>
    ///   Interacts with the <see cref="NibblePoker.Library.Arguments.OptionFlags.Repeatable">OptionFlags.Repeatable</see>
    ///    flag that may have been given in the constructor, or modified through this property.
    /// </summary>
    public bool IsRepeatable {
#if NET20 || NET30 || NET35
        get => (Flags & OptionFlags.Repeatable) == OptionFlags.Repeatable;
#else
            get => Flags.HasFlag(OptionFlags.Repeatable);
#endif
        set {
            if (value) {
                Flags |= OptionFlags.Repeatable;
            } else {
                Flags &= ~OptionFlags.Repeatable;
            }
        }
    }

    /// <summary>
    ///   Interacts with the <see cref="NibblePoker.Library.Arguments.OptionFlags.Hidden">OptionFlags.Hidden</see>
    ///    flag that may have been given in the constructor, or modified through this property.
    /// </summary>
    public bool IsHidden {
#if NET20 || NET30 || NET35
        get => (Flags & OptionFlags.Hidden) == OptionFlags.Hidden;
#else
            get => Flags.HasFlag(OptionFlags.Hidden);
#endif
        set {
            if (value) {
                Flags |= OptionFlags.Hidden;
            } else {
                Flags &= ~OptionFlags.Hidden;
            }
        }
    }

    /// <summary>
    ///   Interacts with the <see cref="NibblePoker.Library.Arguments.OptionFlags.Required">OptionFlags.Required</see>
    ///    flag that may have been given in the constructor, or modified through this property.
    /// </summary>
    public bool IsRequired {
#if NET20 || NET30 || NET35
        get => (Flags & OptionFlags.Required) == OptionFlags.Required;
#else
            get => Flags.HasFlag(OptionFlags.Required);
#endif
        set {
            if (value) {
                Flags |= OptionFlags.Required;
            } else {
                Flags &= ~OptionFlags.Required;
            }
        }
    }

    /// <summary>
    ///   Interacts with the <see cref="NibblePoker.Library.Arguments.OptionFlags.StopsParsing">OptionFlags.StopsParsing</see>
    ///    flag that may have been given in the constructor, or modified through this property.
    /// </summary>
    public bool ShouldStopParsing {
#if NET20 || NET30 || NET35
        get => (Flags & OptionFlags.StopsParsing) == OptionFlags.StopsParsing;
#else
            get => Flags.HasFlag(OptionFlags.StopsParsing);
#endif
        set {
            if (value) {
                Flags |= OptionFlags.StopsParsing;
            } else {
                Flags &= ~OptionFlags.StopsParsing;
            }
        }
    }

    /// <summary>
    ///   Interacts with the <see cref="NibblePoker.Library.Arguments.OptionFlags.AllowVerbsAfter">OptionFlags.AllowVerbsAfter</see>
    ///    flag that may have been given in the constructor, or modified through this property.
    /// </summary>
    public bool AllowsVerbsAfter {
#if NET20 || NET30 || NET35
        get => (Flags & OptionFlags.AllowVerbsAfter) == OptionFlags.AllowVerbsAfter;
#else
            get => Flags.HasFlag(OptionFlags.AllowVerbsAfter);
#endif
        set {
            if (value) {
                Flags |= OptionFlags.AllowVerbsAfter;
            } else {
                Flags &= ~OptionFlags.AllowVerbsAfter;
            }
        }
    }

    /// <summary>
    ///   Interacts with the <see cref="NibblePoker.Library.Arguments.OptionFlags.SkipsRequiredChecks">OptionFlags.SkipsRequiredChecks</see>
    ///    flag that may have been given in the constructor, or modified through this property.
    /// </summary>
    public bool ShouldSkipParsingRequirementChecks {
#if NET20 || NET30 || NET35
        get => (Flags & OptionFlags.SkipsRequiredChecks) == OptionFlags.SkipsRequiredChecks;
#else
            get => Flags.HasFlag(OptionFlags.SkipsRequiredChecks);
#endif
        set {
            if (value) {
                Flags |= OptionFlags.SkipsRequiredChecks;
            } else {
                Flags &= ~OptionFlags.SkipsRequiredChecks;
            }
        }
    }

    /// <summary>
    ///   Interacts with the <see cref="NibblePoker.Library.Arguments.OptionFlags.HasValue">OptionFlags.HasValue</see>
    ///    and <see cref="NibblePoker.Library.Arguments.OptionFlags.Repeatable">OptionFlags.Repeatable</see>
    ///    flags that may have been given in the constructor, or modified through this property.
    /// </summary>
    public bool CanHaveMultipleValue {
        get => CanHaveValue && IsRepeatable;
        set {
            CanHaveValue = value;
            IsRepeatable = value;
        }
    }

    #endregion

}
