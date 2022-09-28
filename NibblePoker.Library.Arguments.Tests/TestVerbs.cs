using System.Diagnostics;

namespace NibblePoker.Library.Arguments.Tests; 

[TestFixture]
public class TestVerbs {
	// Default verb
	private Verb _rootVerb = null!;
	
	// Valid sub verbs
	private Verb _validSubVerb1 = null!;
	private Verb _validSubVerb2 = null!;
	
	// Will throw an exception when registered into '_rootVerb'.
	// Verbs with spaces in their name aren't tested since they're allowed !
	private Verb _invalidSubVerb1 = null!; 
	private Verb _invalidSubVerb2 = null!;
	
	//private Option _longOption = null!;
	//private Option _mixedOption = null!;
	
	[SetUp]
	public void Setup() {
		_rootVerb = new Verb(null);
		
		_validSubVerb1 = new Verb("create");
		_validSubVerb2 = new Verb("delete");
		
		_invalidSubVerb1 = new Verb(null);
		_invalidSubVerb2 = new Verb("  ");
	}

	[Test]
	public void TestValidSubVerbs() {
        Assert.DoesNotThrow(() => {
			_rootVerb.RegisterVerb(_validSubVerb1).RegisterVerb(_validSubVerb2);
		});
		
		Assert.Throws<Exceptions.DuplicateVerbException>(delegate { _rootVerb.RegisterVerb(_validSubVerb1); });
		Assert.Throws<Exceptions.DuplicateVerbException>(delegate { _rootVerb.RegisterVerb(_validSubVerb2); });
		
		Debug.Assert(_validSubVerb1.ParentVerb != null, "_validSubVerb1.ParentVerb != null");
        Debug.Assert(_validSubVerb2.ParentVerb != null, "_validSubVerb2.ParentVerb != null");
        Assert.Multiple(() => {
            Assert.That(_validSubVerb1.ParentVerb, Is.EqualTo(_rootVerb));
            Assert.That(_validSubVerb2.ParentVerb, Is.EqualTo(_rootVerb));
        });
        
        // TODO: Test sub-verb getter.
    }

    [Test]
	public void TestInvalidSubVerbs() {
		Assert.Throws<Exceptions.InvalidVerbNameException>(delegate { _rootVerb.RegisterVerb(_invalidSubVerb1); });
		Assert.Throws<Exceptions.InvalidVerbNameException>(delegate { _rootVerb.RegisterVerb(_invalidSubVerb2); });
	}
}