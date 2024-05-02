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
        _optionMiddle = new Option('i', "input", "", OptionFlags.AllowVerbsAfter);
        _optionFinal = new Option('o', "output");

        _subVerb.RegisterOption(_optionFinal);
        _rootVerb.RegisterVerb(_subVerb);
        _rootVerb.RegisterOption(_optionMiddle);
    }

    [Test]
    public void TestOptionalMiddleFlag() {
        // Setup: app.exe [-i|--input] [alpha|bravo [-o|--output]]
        
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
}
