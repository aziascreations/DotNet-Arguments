namespace NibblePoker.Library.Arguments.Tests; 

[TestFixture]
public class TestOptions {
	private Option _shortOption = null!;
	private Option _longOption = null!;
	private Option _mixedOption = null!;
	
	[SetUp]
	public void Setup() {
		_shortOption = new Option('a', null);  // Implies: "", OptionFlags.None
		_longOption = new Option(null, "bravo", "", OptionFlags.All);
		_mixedOption = new Option('c', "charlie", "", OptionFlags.HasValue);
	}

	[Test]
	public void TestHasTokenName() {
        Assert.Multiple(() => {
	        Assert.That(_shortOption.HasToken(), Is.True);
	        Assert.That(_shortOption.HasName(), Is.False);
	        
	        Assert.That(_longOption.HasToken(), Is.False);
	        Assert.That(_longOption.HasName(), Is.True);
	        
	        Assert.That(_mixedOption.HasToken(), Is.True);
	        Assert.That(_mixedOption.HasName(), Is.True);
        });
    }

	[Test]
	public void TestFlags() {
		Assert.Multiple(() => {
			Assert.That(_shortOption.IsDefault(), Is.False);
			Assert.That(_shortOption.CanHaveValue(), Is.False);
			Assert.That(_shortOption.IsRepeatable, Is.False);
			Assert.That(_shortOption.CanHaveMultipleValue(), Is.False);
			Assert.That(_shortOption.IsHidden(), Is.False);
			
			Assert.That(_longOption.IsDefault(), Is.True);
			Assert.That(_longOption.CanHaveValue(), Is.True);
			Assert.That(_longOption.IsRepeatable, Is.True);
			Assert.That(_longOption.CanHaveMultipleValue(), Is.True);
			Assert.That(_longOption.IsHidden(), Is.True);
			
			// Making sure that 'CanHaveMultipleValue()' doesn't return 'true' if only 1 of 2 flags is set !
			Assert.That(_mixedOption.IsDefault(), Is.False);
			Assert.That(_mixedOption.CanHaveValue(), Is.True);
			Assert.That(_mixedOption.IsRepeatable, Is.False);
			Assert.That(_mixedOption.CanHaveMultipleValue(), Is.False);
			Assert.That(_mixedOption.IsHidden(), Is.False);
		});
		
		Assert.DoesNotThrow(() => {
			Option unused = new Option('a', "alpha", "",
				OptionFlags.Default | OptionFlags.HasValue);
		});
		Assert.DoesNotThrow(() => {
			Option unused = new Option('a', "alpha", "",
				OptionFlags.Default | OptionFlags.HasMultipleValue);
		});
		Assert.Throws<Exceptions.InvalidFlagsException>(delegate {
			Option unused = new Option('a', "alpha", "", OptionFlags.Default);
		});
	}
}