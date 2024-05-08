using System;
using NUnit.Framework;

namespace NibblePoker.Library.Arguments.Tests; 

[TestFixture]
public class TestParserMiddleOption {
    private Verb _rootVerb = null!;
    private Verb _subVerb = null!;
    private Option _optionMiddle = null!;
    private Option _optionFinal = null!;
    
    [SetUp]
    public void Setup() {
        _rootVerb = new Verb("_root");
        _subVerb = new Verb("alpha");
        _optionFinal = new Option('o', "output");

        _subVerb.RegisterOption(_optionFinal);
        _rootVerb.RegisterVerb(_subVerb);
    }

    [Test]
    public void TestOptionalMiddleFlag() {
        // Setup: app.exe [-i|--input] [alpha|bravo [-o|--output]]
        _optionMiddle = new Option('i', "input", "", OptionFlags.AllowVerbsAfter);
        _rootVerb.RegisterOption(_optionMiddle);
        
        // Args: []
        _rootVerb.Clear();
        Assert.DoesNotThrow(() => {
            Verb returnedVerb = ArgumentsParser.ParseArguments(_rootVerb, Array.Empty<string>());
            Assert.That(returnedVerb, Is.EqualTo(_rootVerb));
        });
        Assert.Multiple(() => {
            Assert.That(_subVerb.WasUsed, Is.EqualTo(false));
            Assert.That(_optionMiddle.WasUsed, Is.EqualTo(false));
            Assert.That(_optionFinal.WasUsed, Is.EqualTo(false));
        });

        // Args: ['-i']
        _rootVerb.Clear();
        Assert.DoesNotThrow(() => {
            Verb returnedVerb = ArgumentsParser.ParseArguments(_rootVerb, new[]{"-i"});
            Assert.That(returnedVerb, Is.EqualTo(_rootVerb));
        });
        Assert.Multiple(() => {
            Assert.That(_subVerb.WasUsed, Is.EqualTo(false));
            Assert.That(_optionMiddle.WasUsed(), Is.EqualTo(true));
            Assert.That(_optionFinal.WasUsed(), Is.EqualTo(false));
        });

        // Args: ['alpha']
        _rootVerb.Clear();
        Assert.DoesNotThrow(() => {
            Verb returnedVerb = ArgumentsParser.ParseArguments(_rootVerb, new[]{"alpha"});
            Assert.That(returnedVerb, Is.EqualTo(_subVerb));
        });
        Assert.Multiple(() => {
            Assert.That(_subVerb.WasUsed, Is.EqualTo(true));
            Assert.That(_optionMiddle.WasUsed(), Is.EqualTo(false));
            Assert.That(_optionFinal.WasUsed(), Is.EqualTo(false));
        });

        // Args: ['-i', 'alpha']
        _rootVerb.Clear();
        Assert.DoesNotThrow(() => {
            Verb returnedVerb = ArgumentsParser.ParseArguments(_rootVerb, new[]{"-i", "alpha"});
            Assert.That(returnedVerb, Is.EqualTo(_subVerb));
        });
        Assert.Multiple(() => {
            Assert.That(_subVerb.WasUsed, Is.EqualTo(true));
            Assert.That(_optionMiddle.WasUsed(), Is.EqualTo(true));
            Assert.That(_optionFinal.WasUsed(), Is.EqualTo(false));
        });
    }

    [Test]
    public void TestOptionalMiddleOptionalSingleValue() {
        // Setup: app.exe [-i|--input <INPUT>] [alpha|bravo [-o|--output]]
        _optionMiddle = new Option('i', "input", "",
            OptionFlags.AllowVerbsAfter | OptionFlags.HasValue);
        _rootVerb.RegisterOption(_optionMiddle);
        
        // Args: []
        _rootVerb.Clear();
        Assert.DoesNotThrow(() => {
            Verb returnedVerb = ArgumentsParser.ParseArguments(_rootVerb, Array.Empty<string>());
            Assert.That(returnedVerb, Is.EqualTo(_rootVerb));
        });
        Assert.Multiple(() => {
            Assert.That(_subVerb.WasUsed, Is.EqualTo(false));
            Assert.That(_optionMiddle.WasUsed, Is.EqualTo(false));
            Assert.That(_optionFinal.WasUsed, Is.EqualTo(false));
        });

        // Args: ['-i', 'alpha']
        _rootVerb.Clear();
        Assert.DoesNotThrow(() => {
            Verb returnedVerb = ArgumentsParser.ParseArguments(_rootVerb, new[]{"-i", "alpha"});
            Assert.That(returnedVerb, Is.EqualTo(_rootVerb));
        });
        Assert.Multiple(() => {
            Assert.That(_subVerb.WasUsed, Is.EqualTo(false));
            Assert.That(_optionMiddle.WasUsed(), Is.EqualTo(true));
            Assert.That(_optionMiddle.HasValue(), Is.EqualTo(true));
            Assert.That(_optionMiddle.Arguments, Has.Count.EqualTo(1));
            Assert.That(_optionMiddle.Arguments[0], Is.EqualTo("alpha"));
            Assert.That(_optionFinal.WasUsed(), Is.EqualTo(false));
        });

        // Args: ['alpha']
        _rootVerb.Clear();
        Assert.DoesNotThrow(() => {
            Verb returnedVerb = ArgumentsParser.ParseArguments(_rootVerb, new[]{"alpha"});
            Assert.That(returnedVerb, Is.EqualTo(_subVerb));
        });
        Assert.Multiple(() => {
            Assert.That(_subVerb.WasUsed, Is.EqualTo(true));
            Assert.That(_optionMiddle.WasUsed(), Is.EqualTo(false));
            Assert.That(_optionFinal.WasUsed(), Is.EqualTo(false));
        });

        // Args: ['-i', 'alpha', 'alpha']
        _rootVerb.Clear();
        Assert.DoesNotThrow(() => {
            Verb returnedVerb = ArgumentsParser.ParseArguments(_rootVerb, new[]{"-i", "alpha", "alpha"});
            Assert.That(returnedVerb, Is.EqualTo(_subVerb));
        });
        Assert.Multiple(() => {
            Assert.That(_subVerb.WasUsed, Is.EqualTo(true));
            Assert.That(_optionMiddle.WasUsed(), Is.EqualTo(true));
            Assert.That(_optionMiddle.HasValue(), Is.EqualTo(true));
            Assert.That(_optionMiddle.Arguments, Has.Count.EqualTo(1));
            Assert.That(_optionMiddle.Arguments[0], Is.EqualTo("alpha"));
            Assert.That(_optionFinal.WasUsed(), Is.EqualTo(false));
        });
    }

    [Test]
    public void TestOptionalMiddleDefaultOptionalSingleValue() {
        // Setup: app.exe <[-i|--input] <INPUT>> [alpha|bravo [-o|--output]]
        _optionMiddle = new Option('i', "input", "",
            OptionFlags.AllowVerbsAfter | OptionFlags.HasValue | OptionFlags.Default);
        _rootVerb.RegisterOption(_optionMiddle);
        
        // Args: []
        _rootVerb.Clear();
        Assert.DoesNotThrow(() => {
            Verb returnedVerb = ArgumentsParser.ParseArguments(_rootVerb, Array.Empty<string>());
            Assert.That(returnedVerb, Is.EqualTo(_rootVerb));
        });
        Assert.Multiple(() => {
            Assert.That(_subVerb.WasUsed, Is.EqualTo(false));
            Assert.That(_optionMiddle.WasUsed, Is.EqualTo(false));
            Assert.That(_optionFinal.WasUsed, Is.EqualTo(false));
        });

        // Args: ['alpha']
        _rootVerb.Clear();
        Assert.DoesNotThrow(() => {
            Verb returnedVerb = ArgumentsParser.ParseArguments(_rootVerb, new[]{"alpha"});
            Assert.That(returnedVerb, Is.EqualTo(_subVerb));
        });
        Assert.Multiple(() => {
            Assert.That(_subVerb.WasUsed, Is.EqualTo(true));
            Assert.That(_optionMiddle.WasUsed(), Is.EqualTo(false));
            Assert.That(_optionFinal.WasUsed(), Is.EqualTo(false));
        });

        // Args: ['-i', 'alpha', 'alpha']
        _rootVerb.Clear();
        Assert.DoesNotThrow(() => {
            Verb returnedVerb = ArgumentsParser.ParseArguments(_rootVerb, new[]{"-i", "alpha", "alpha"});
            Assert.That(returnedVerb, Is.EqualTo(_subVerb));
        });
        Assert.Multiple(() => {
            Assert.That(_subVerb.WasUsed, Is.EqualTo(true));
            Assert.That(_optionMiddle.WasUsed(), Is.EqualTo(true));
            Assert.That(_optionMiddle.HasValue(), Is.EqualTo(true));
            Assert.That(_optionMiddle.Arguments, Has.Count.EqualTo(1));
            Assert.That(_optionMiddle.Arguments[0], Is.EqualTo("alpha"));
            Assert.That(_optionFinal.WasUsed(), Is.EqualTo(false));
        });
    }

    [Test]
    public void TestOptionalMiddleDefaultRequiredSingleValue() {
        // Setup: app.exe <[-i|--input] <INPUT>> [alpha|bravo [-o|--output]]
        _optionMiddle = new Option('i', "input", "",
            OptionFlags.AllowVerbsAfter | OptionFlags.HasValue | OptionFlags.Default | OptionFlags.Required);
        _rootVerb.RegisterOption(_optionMiddle);
        
        // Args: []
        _rootVerb.Clear();
        Assert.Throws<Exceptions.MissingRequiredOptionException>(delegate {
            ArgumentsParser.ParseArguments(_rootVerb, Array.Empty<string>());
        });

        // Args: ['alpha']
        // Assumes the sub-verb was requested for safety, which leads to this exception.
        // This is thrown by the loop at the end of `ArgumentsParser.ParseArguments`.
        _rootVerb.Clear();
        Assert.Throws<Exceptions.MissingRequiredOptionException>(delegate {
            ArgumentsParser.ParseArguments(_rootVerb, new[]{"alpha"});
        });
        
        // Args: ['-i', 'alpha']
        // We override the previous behaviour by explicitly stating the option.
        _rootVerb.Clear();
        Assert.DoesNotThrow(() => {
            Verb returnedVerb = ArgumentsParser.ParseArguments(_rootVerb, new[]{"-i", "alpha"});
            Assert.That(returnedVerb, Is.EqualTo(_rootVerb));
        });
        Assert.Multiple(() => {
            Assert.That(_subVerb.WasUsed, Is.EqualTo(false));
            Assert.That(_optionMiddle.WasUsed(), Is.EqualTo(true));
            Assert.That(_optionMiddle.HasValue(), Is.EqualTo(true));
            Assert.That(_optionMiddle.Arguments, Has.Count.EqualTo(1));
            Assert.That(_optionMiddle.Arguments[0], Is.EqualTo("alpha"));
            Assert.That(_optionFinal.WasUsed(), Is.EqualTo(false));
        });
        
        // Args: ['zulu']
        // Since 'zulu' isn't a sub-verb, it is assumed to be the required value.
        _rootVerb.Clear();
        Assert.DoesNotThrow(() => {
            Verb returnedVerb = ArgumentsParser.ParseArguments(_rootVerb, new[]{"zulu"});
            Assert.That(returnedVerb, Is.EqualTo(_rootVerb));
        });
        Assert.Multiple(() => {
            Assert.That(_subVerb.WasUsed, Is.EqualTo(false));
            Assert.That(_optionMiddle.WasUsed(), Is.EqualTo(true));
            Assert.That(_optionMiddle.HasValue(), Is.EqualTo(true));
            Assert.That(_optionMiddle.Arguments, Has.Count.EqualTo(1));
            Assert.That(_optionMiddle.Arguments[0], Is.EqualTo("zulu"));
            Assert.That(_optionFinal.WasUsed(), Is.EqualTo(false));
        });
        
        // Args: ['zulu', 'alpha']
        _rootVerb.Clear();
        Assert.DoesNotThrow(() => {
            Verb returnedVerb = ArgumentsParser.ParseArguments(_rootVerb, new[]{"zulu", "alpha"});
            Assert.That(returnedVerb, Is.EqualTo(_subVerb));
        });
        Assert.Multiple(() => {
            Assert.That(_subVerb.WasUsed, Is.EqualTo(true));
            Assert.That(_optionMiddle.WasUsed(), Is.EqualTo(true));
            Assert.That(_optionMiddle.HasValue(), Is.EqualTo(true));
            Assert.That(_optionMiddle.Arguments, Has.Count.EqualTo(1));
            Assert.That(_optionMiddle.Arguments[0], Is.EqualTo("zulu"));
            Assert.That(_optionFinal.WasUsed(), Is.EqualTo(false));
        });

        // Args: ['-i', 'alpha', 'alpha']
        _rootVerb.Clear();
        Assert.DoesNotThrow(() => {
            Verb returnedVerb = ArgumentsParser.ParseArguments(_rootVerb, new[]{"-i", "alpha", "alpha"});
            Assert.That(returnedVerb, Is.EqualTo(_subVerb));
        });
        Assert.Multiple(() => {
            Assert.That(_subVerb.WasUsed, Is.EqualTo(true));
            Assert.That(_optionMiddle.WasUsed(), Is.EqualTo(true));
            Assert.That(_optionMiddle.HasValue(), Is.EqualTo(true));
            Assert.That(_optionMiddle.Arguments, Has.Count.EqualTo(1));
            Assert.That(_optionMiddle.Arguments[0], Is.EqualTo("alpha"));
            Assert.That(_optionFinal.WasUsed(), Is.EqualTo(false));
        });
    }
}
