using NUnit.Framework;

namespace NibblePoker.Library.Arguments.Tests;

[TestFixture]
public class TestCombinedOptionRegistration {
    private Verb _rootVerb;
    private Verb _subVerb;

    private Option _option1;
    private Option _option2;

    [SetUp]
    public void Setup() {
        _rootVerb = new Verb(null);
        _subVerb = new Verb("test");

        _option1 = new Option('a', "alice");
        _option2 = new Option('b', "bob");

        _rootVerb.RegisterVerb(_subVerb);
    }

    [Test]
    public void TestRootRegistration() {
        Assert.DoesNotThrow(delegate {
            _rootVerb.RegisterOption(_option1, _option2);
        });

        Assert.Multiple(() => {
            Assert.That(_rootVerb.Options, Does.Contain(_option1));
            Assert.That(_rootVerb.Options, Does.Contain(_option2));

            Assert.That(_subVerb.Options, Does.Not.Contain(_option1));
            Assert.That(_subVerb.Options, Does.Not.Contain(_option2));
        });
    }

    [Test]
    public void TestSubVerbRegistration() {
        Assert.DoesNotThrow(delegate {
            _subVerb.RegisterOption(_option1, _option2);
        });

        Assert.Multiple(() => {
            Assert.That(_rootVerb.Options, Does.Not.Contain(_option1));
            Assert.That(_rootVerb.Options, Does.Not.Contain(_option2));

            Assert.That(_subVerb.Options, Does.Contain(_option1));
            Assert.That(_subVerb.Options, Does.Contain(_option2));
        });
    }

    [Test]
    public void TestRecursiveRegistration() {
        Assert.DoesNotThrow(delegate {
            _rootVerb.RegisterOptionRecursively(_option1, _option2);
        });

        Assert.Multiple(() => {
            Assert.That(_rootVerb.Options, Does.Contain(_option1));
            Assert.That(_rootVerb.Options, Does.Contain(_option2));

            Assert.That(_subVerb.Options, Does.Contain(_option1));
            Assert.That(_subVerb.Options, Does.Contain(_option2));
        });
    }
}
