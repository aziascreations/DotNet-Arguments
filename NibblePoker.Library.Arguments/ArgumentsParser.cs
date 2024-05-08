namespace NibblePoker.Library.Arguments {
    /// <summary>
    ///     Static class that contains a function related to parsing launch arguments.
    /// </summary>
    public static class ArgumentsParser {
        /// <summary>
        ///     Parses the given arguments into the given root <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>.
        /// </summary>
        /// <param name="rootVerb">
        ///     The <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>
        ///     for which the given arguments should be processed.
        /// </param>
        /// <param name="arguments">
        ///     Array of launch arguments to parse.
        /// </param>
        /// <returns>
        ///     The last used <see cref="NibblePoker.Library.Arguments.Verb">Verb</see> when parsing.
        /// </returns>
        /// <exception cref="Exceptions.ParserException">
        ///     Extended by all the following exceptions.
        /// </exception>
        /// <exception cref="Exceptions.InvalidArgumentException">
        ///     If given the <c>--</c> token twice, or after reaching the end of options.
        /// </exception>
        /// <exception cref="Exceptions.NoDefaultOptionFoundException">
        ///     If no appropriate <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     with the <see cref="OptionFlags.Default">OptionFlags.Default</see>
        ///     flag could be found when needed.
        /// </exception>
        /// <exception cref="Exceptions.UnknownOptionException">
        ///     If a given <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     couldn't be found when needed.
        /// </exception>
        /// <exception cref="Exceptions.RepeatedSingularOptionException">
        ///     If a given <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     that could only be used once was used twice.
        /// </exception>
        /// <exception cref="Exceptions.OptionValueOverflowException">
        ///     If a given <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     that could only hold one value was made to hold more.
        /// </exception>
        /// <exception cref="Exceptions.NotEnoughArgumentsException">
        ///     If a given <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     needs to have a value, but is the last argument.
        /// </exception>
        /// <exception cref="Exceptions.OptionHasValueAndMoreShortsException">
        ///     If a short <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     with an expected value isn't given at the end of a short options block.
        /// </exception>
        /// <exception cref="Exceptions.MissingRequiredOptionException">
        ///     If an <see cref="NibblePoker.Library.Arguments.Option">Option</see>
        ///     with the <see cref="OptionFlags.Required">OptionFlags.Required</see> flag
        ///     wasn't used after all arguments were parsed.
        /// </exception>
        public static Verb ParseArguments(Verb rootVerb, string[] arguments) {
            Verb currentVerb = rootVerb;

            // Used to skip some checks if no verbs is expected to be parsable after it is set.
            bool hasFinishedParsingVerbs = false;

            // Used to attribute values to an option after '--'
            bool hasReachedEndOfOptions = false;

            Option? relevantOption = null;

            // Parsing arguments
            for (int iArg = 0; iArg < arguments.Length; iArg++) {
                currentVerb.WasUsed = true;

                // Checking for the 'StopsParsing' flags and setting 'relevantOption' to null if not encountered.
                if (relevantOption != null) {
                    if (relevantOption.ShouldStopParsing()) {
                        return currentVerb;
                    }

                    relevantOption = null;
                }

                if (arguments[iArg].StartsWith("--")) {
                    // Long option or end of parameters

                    if (arguments[iArg].Length == 2) {
                        // End of options '--' symbol
                        if (hasReachedEndOfOptions) {
                            throw new Exceptions.InvalidArgumentException("Dual end of options given as '--' !");
                        }

                        hasFinishedParsingVerbs = true;
                        hasReachedEndOfOptions = true;
                        continue;
                    }

                    if (hasReachedEndOfOptions) {
                        // Default option's value that starts with "--" after a '--' token.
                        relevantOption = currentVerb.GetRelevantDefaultOption();

                        if (relevantOption == null) {
                            throw new Exceptions.NoDefaultOptionFoundException(
                                "No relevant option found in the '" + currentVerb.Name + "' verb !");
                        }

                        relevantOption.Occurrences++;
                        relevantOption.Arguments.Add(arguments[iArg]);
                    } else {
                        // Generic option by '--<name>'
                        relevantOption = currentVerb.GetOptionByName(arguments[iArg][2..]);
                        if (relevantOption == null) {
                            throw new Exceptions.UnknownOptionException(
                                "Unable to find the '" + arguments[iArg][2..] + "' option !");
                        }

                        if (relevantOption.WasUsed() && !relevantOption.IsRepeatable()) {
                            throw new Exceptions.RepeatedSingularOptionException(
                                "The option '" + arguments[iArg][2..] + "' was used more than once !");
                        }

                        relevantOption.Occurrences++;

                        if (relevantOption.CanHaveValue()) {
                            if (!relevantOption.CanHaveMultipleValue() && relevantOption.Arguments.Count >= 1) {
                                throw new Exceptions.OptionValueOverflowException(
                                    "The option '" + arguments[iArg][2..] + "' can only have 1 argument !");
                            }

                            if (arguments.Length <= iArg + 1) {
                                throw new Exceptions.NotEnoughArgumentsException(
                                    "Unable to get a value for '" + arguments[iArg][2..] + " !");
                            }

                            relevantOption.Arguments.Add(arguments[iArg + 1]);
                            iArg++;
                        }
                    }

                    // Preventing the parsing of future verbs if the option doesn't explicitly allow it.
                    if (!relevantOption.AllowsVerbsAfter()) {
                        hasFinishedParsingVerbs = true;
                    }
                } else if (arguments[iArg].StartsWith("-")) {
                    // Short option

                    for (int iChar = 1; iChar < arguments[iArg].Length; iChar++) {
                        relevantOption = currentVerb.GetOptionByToken(arguments[iArg][iChar]);

                        if (relevantOption == null) {
                            throw new Exceptions.UnknownOptionException(
                                "Unable to find the '" + arguments[iArg][iChar] + "' short option !");
                        }

                        if (relevantOption.WasUsed() && !relevantOption.IsRepeatable()) {
                            throw new Exceptions.RepeatedSingularOptionException(
                                "The short option '" + arguments[iArg][iChar] + "' was used more than once !");
                        }

                        relevantOption.Occurrences++;

                        if (!relevantOption.AllowsVerbsAfter()) {
                            hasFinishedParsingVerbs = true;
                        }

                        if (!relevantOption.CanHaveValue()) {
                            continue;
                        }

                        if (iChar + 1 < arguments[iArg].Length) {
                            throw new Exceptions.OptionHasValueAndMoreShortsException("The short option '" +
                                arguments[iArg][iChar] + " was given before the end of the argument");
                        }

                        if (!relevantOption.CanHaveMultipleValue() && relevantOption.Arguments.Count >= 1) {
                            // This condition might be redundant ! -> See (used && !repeat) ~15 lines above.
                            throw new Exceptions.OptionValueOverflowException(
                                "The short option '" + arguments[iArg][iChar] + "' can only have 1 argument !");
                        }

                        if (arguments.Length <= iArg + 1) {
                            throw new Exceptions.NotEnoughArgumentsException(
                                "Unable to get a value for '" + arguments[iArg][iChar] + " !");
                        }

                        relevantOption.Arguments.Add(arguments[iArg + 1]);
                        iArg++;
                        break;
                    }
                } else {
                    // Verb or default argument
                    Verb? desiredVerb = null;

                    // TODO: Optimize this condition !
                    if (hasFinishedParsingVerbs) {
                        // Already finished parsing verbs.
                        // We are sure we are parsing option's arguments.
                        relevantOption = currentVerb.GetRelevantDefaultOption();

                        if (relevantOption == null) {
                            throw new Exceptions.NoDefaultOptionFoundException(
                                "No relevant option found in the '" + currentVerb.Name + "' verb !");
                        }
                    } else {
                        // We will have find out if it is a verb or an option's argument.
                        // We should still be at the end of the arguments.

                        // We check for the verb first.
                        desiredVerb = currentVerb.GetSubVerbByName(arguments[iArg]);

                        if (desiredVerb == null) {
                            // We did not find a verb, we will now search for the default option

                            relevantOption = currentVerb.GetRelevantDefaultOption();
                            if (relevantOption == null) {
                                throw new Exceptions.NoDefaultOptionFoundException(
                                    "No relevant verb or option found in the '" + currentVerb.Name + "' verb !");
                            }
                        }
                    }

                    // We now process and set the variables before finishing.
                    if (desiredVerb != null) {
                        desiredVerb.WasUsed = true;
                        currentVerb = desiredVerb;
                        // 'relevantOption' will always be null in this case, there is no need to reset it !
                    } else if (relevantOption != null) {
                        // Not checking for flags and max value count since "GetRelevantDefaultOption" takes care of it !

                        relevantOption.Occurrences++;
                        relevantOption.Arguments.Add(arguments[iArg]);

                        if (!relevantOption.AllowsVerbsAfter()) {
                            hasFinishedParsingVerbs = true;
                        }
                    }
                }
            }

            // Checking the "Required" flag recursively from the final verb back to the root verb.
            Verb? tempReqVerb = currentVerb;
            while (tempReqVerb != null) {
                foreach (Option option in tempReqVerb.Options) {
                    if (option.IsRequired() && !option.WasUsed()) {
                        throw new Exceptions.MissingRequiredOptionException(
                            "The required option '" + option.GetFullName() + "' wasn't given !");
                    }
                }

                tempReqVerb = tempReqVerb.ParentVerb;
            }

            return currentVerb;
        }
    }
}
